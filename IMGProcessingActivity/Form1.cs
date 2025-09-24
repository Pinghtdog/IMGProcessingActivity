using System;
using System.Drawing;
using System.Drawing.Imaging; // Required for ImageLockMode, PixelFormat, BitmapData, and saving with specific formats
using System.Windows.Forms; // Required for OpenFileDialog, SaveFileDialog, PictureBox, etc.
using AForge.Video;
using AForge.Video.DirectShow;

namespace IMGProcessingActivity
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage; // Stores the image loaded by the user
        private Bitmap processedImage; // Stores the image after a filter has been applied
        private Bitmap originalImageB; // Stores Image B (The Background)
        private Bitmap originalImageA; // Stores Image A (The Foreground/Current)
        private Bitmap resultImage;    // Stores the Resulting Difference Image
        private const int DefaultThreshold = 25; // Default value for threshold
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private bool isWebcamRunning = false;
        private const int WEBCAM_FPS = 30;

        public Form1()
        {
            InitializeComponent();
            // Initialize dialogs for better control
            openFileDialog1.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif|All Files|*.*";
            saveFileDialog1.Filter = "PNG Image|*.png|JPEG Image|*.jpg;*.jpeg|BMP Image|*.bmp|GIF Image|*.gif";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Any initialization on form load can go here, though for this app, constructor is usually enough.
        }

        // --- File Menu Handlers ---

        private void loadImageToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Dispose previous images if they exist to free up memory
                    if (originalImage != null) originalImage.Dispose();
                    if (processedImage != null) processedImage.Dispose();

                    originalImage = new Bitmap(openFileDialog1.FileName);
                    pictureBoxOriginal.Image = originalImage;
                    pictureBoxProcessed.Image = null; // Clear processed image display
                    processedImage = null;            // Clear processed image data

                    // Optional: Display file info or status
                    this.Text = "Image Processor - " + System.IO.Path.GetFileName(openFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveImageToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (processedImage == null)
            {
                MessageBox.Show("There is no processed image to save.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Determine the image format based on the selected filter index
                    ImageFormat format = ImageFormat.Png; // Default to PNG

                    switch (saveFileDialog1.FilterIndex)
                    {
                        case 1: // PNG
                            format = ImageFormat.Png;
                            break;
                        case 2: // JPEG
                            format = ImageFormat.Jpeg;
                            break;
                        case 3: // BMP
                            format = ImageFormat.Bmp;
                            break;
                        case 4: // GIF
                            format = ImageFormat.Gif;
                            break;
                    }

                    processedImage.Save(saveFileDialog1.FileName, format);
                    MessageBox.Show("Image saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- Filter Menu Handlers ---

        private void basicCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyFilter(BasicCopy); // Use a helper to apply and update
        }

        private void greyScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyFilter(GrayscaleFast);
            // If you want to use the faster grayscale, change to:
            // ApplyFilter(GrayscaleFast);
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyFilter(ColorInversion);
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyFilter(Sepia);
        }

        private bool IsImageLoaded()
        {
            if (originalImage == null)
            {
                MessageBox.Show("Please load an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Note: Creating a graphical histogram display is complex for a simple example.
            // This implementation will perform a simple **histogram equalization** for visual effect,
            // which enhances contrast, as a common processing step related to histograms.
            // If you only needed the raw histogram data (counts), the code would be simpler.

            if (!IsImageLoaded()) return;

            // For simplicity, we'll implement a basic **Brightness Histogram** (greyscale conversion first)
            // and perform **Histogram Equalization** for visual effect.

            // Step 1: Create a Greyscale version and compute the brightness histogram
            int[] histogram = new int[256];
            Bitmap grayBitmap = new Bitmap(originalImage.Width, originalImage.Height);
            int totalPixels = originalImage.Width * originalImage.Height;

            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color pixel = originalImage.GetPixel(x, y);
                    int grayValue = (int)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);
                    grayBitmap.SetPixel(x, y, Color.FromArgb(grayValue, grayValue, grayValue));
                    histogram[grayValue]++; // Populate the histogram
                }
            }

            // Step 2: Calculate the Cumulative Distribution Function (CDF)
            int[] cdf = new int[256];
            cdf[0] = histogram[0];
            for (int i = 1; i < 256; i++)
            {
                cdf[i] = cdf[i - 1] + histogram[i];
            }

            // Find the minimum non-zero CDF value (needed for equalization formula)
            int cdfMin = 0;
            for (int i = 0; i < 256; i++)
            {
                if (cdf[i] > 0)
                {
                    cdfMin = cdf[i];
                    break;
                }
            }

            // Step 3: Perform Histogram Equalization (generate the new image)
            processedImage = new Bitmap(originalImage.Width, originalImage.Height);
            float scaleFactor = 255.0f / (totalPixels - cdfMin);

            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color pixel = grayBitmap.GetPixel(x, y);
                    int oldGray = pixel.R; // R, G, and B are the same in greyscale

                    // Equalization formula: new_value = round(((cdf[old_value] - cdf_min) / (total_pixels - cdf_min)) * 255)
                    int newGray = (int)Math.Round((cdf[oldGray] - cdfMin) * scaleFactor);

                    // Clamp to [0, 255] just in case
                    newGray = Math.Max(0, Math.Min(255, newGray));

                    processedImage.SetPixel(x, y, Color.FromArgb(newGray, newGray, newGray));
                }
            }

            MessageBox.Show("Histogram Equalization Applied (Enhances Contrast on Greyscale Image).", "Histogram", MessageBoxButtons.OK, MessageBoxIcon.Information);
            pictureBoxProcessed.Image = processedImage;
        }

        // --- Helper method to apply filters and update UI ---
        private void ApplyFilter(Func<Bitmap, Bitmap> filterFunction)
        {
            if (originalImage == null)
            {
                MessageBox.Show("Please load an image first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Dispose previous processed image if it exists
            if (processedImage != null) processedImage.Dispose();

            processedImage = filterFunction(originalImage);
            pictureBoxProcessed.Image = processedImage;
        }


        // --- Image Processing Functions ---

        private Bitmap BasicCopy(Bitmap sourceBitmap)
        {
            // Creates a new bitmap and draws the source onto it, effectively a deep copy.
            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, sourceBitmap.PixelFormat);
            using (Graphics g = Graphics.FromImage(resultBitmap))
            {
                g.DrawImage(sourceBitmap, new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height));
            }
            return resultBitmap;
        }

        private Bitmap Grayscale(Bitmap sourceBitmap)
        {
            Bitmap grayscaleBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, sourceBitmap.PixelFormat);

            for (int x = 0; x < sourceBitmap.Width; x++)
            {
                for (int y = 0; y < sourceBitmap.Height; y++)
                {
                    Color originalColor = sourceBitmap.GetPixel(x, y);
                    // Calculate grayscale value using luminosity method
                    int gray = (int)(originalColor.R * 0.299 + originalColor.G * 0.587 + originalColor.B * 0.114);
                    // Ensure the alpha channel is preserved if it exists
                    Color grayColor = Color.FromArgb(originalColor.A, gray, gray, gray);
                    grayscaleBitmap.SetPixel(x, y, grayColor);
                }
            }
            return grayscaleBitmap;
        }

        private Bitmap ColorInversion(Bitmap sourceBitmap)
        {
            Bitmap invertedBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, sourceBitmap.PixelFormat);

            for (int x = 0; x < sourceBitmap.Width; x++)
            {
                for (int y = 0; y < sourceBitmap.Height; y++)
                {
                    Color originalColor = sourceBitmap.GetPixel(x, y);
                    // Invert R, G, B channels, preserve Alpha
                    Color invertedColor = Color.FromArgb(originalColor.A, 255 - originalColor.R, 255 - originalColor.G, 255 - originalColor.B);
                    invertedBitmap.SetPixel(x, y, invertedColor);
                }
            }
            return invertedBitmap;
        }

        private Bitmap Sepia(Bitmap sourceBitmap)
        {
            Bitmap sepiaBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, sourceBitmap.PixelFormat);

            for (int x = 0; x < sourceBitmap.Width; x++)
            {
                for (int y = 0; y < sourceBitmap.Height; y++)
                {
                    Color originalColor = sourceBitmap.GetPixel(x, y);

                    // Sepia tone formula
                    int tr = (int)(0.393 * originalColor.R + 0.769 * originalColor.G + 0.189 * originalColor.B);
                    int tg = (int)(0.349 * originalColor.R + 0.686 * originalColor.G + 0.168 * originalColor.B);
                    int tb = (int)(0.272 * originalColor.R + 0.534 * originalColor.G + 0.131 * originalColor.B);

                    // Clamp values to 0-255
                    tr = Math.Min(255, tr);
                    tg = Math.Min(255, tg);
                    tb = Math.Min(255, tb);

                    // Preserve Alpha channel
                    sepiaBitmap.SetPixel(x, y, Color.FromArgb(originalColor.A, tr, tg, tb));
                }
            }
            return sepiaBitmap;
        }

        // --- Fast Pixel Access (using LockBits) for potential performance ---
        // Remember to enable unsafe code in your project properties (Build tab -> Allow unsafe code checkbox).
        // This is an example for Grayscale, you could adapt it for other filters.
        private Bitmap GrayscaleFast(Bitmap sourceBitmap)
        {
            // Ensure the image format is 32bppArgb or 24bppRgb for direct byte manipulation
            // If not, you might need to convert it first or handle different pixel formats
            if (sourceBitmap.PixelFormat != PixelFormat.Format32bppArgb &&
                sourceBitmap.PixelFormat != PixelFormat.Format24bppRgb)
            {
                // For simplicity, we'll convert to 32bppArgb if not already.
                // In a production app, you might want to handle various formats more robustly.
                Bitmap temp = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(temp))
                {
                    g.DrawImage(sourceBitmap, new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height));
                }
                sourceBitmap = temp; // Use the converted bitmap
            }

            Bitmap grayscaleBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, sourceBitmap.PixelFormat);

            Rectangle rect = new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height);
            BitmapData originalData = sourceBitmap.LockBits(rect, ImageLockMode.ReadOnly, sourceBitmap.PixelFormat);
            BitmapData grayscaleData = grayscaleBitmap.LockBits(rect, ImageLockMode.WriteOnly, sourceBitmap.PixelFormat);

            int bytesPerPixel = Image.GetPixelFormatSize(sourceBitmap.PixelFormat) / 8;
            int heightInPixels = originalData.Height;

            unsafe // Requires 'Allow unsafe code' in Project Properties -> Build
            {
                byte* ptrOriginal = (byte*)originalData.Scan0;
                byte* ptrGrayscale = (byte*)grayscaleData.Scan0;

                for (int y = 0; y < heightInPixels; y++)
                {
                    byte* currentLineOriginal = ptrOriginal + (y * originalData.Stride);
                    byte* currentLineGrayscale = ptrGrayscale + (y * grayscaleData.Stride);

                    for (int x = 0; x < originalData.Width * bytesPerPixel; x += bytesPerPixel)
                    {
                        byte b = currentLineOriginal[x];
                        byte g = currentLineOriginal[x + 1];
                        byte r = currentLineOriginal[x + 2];

                        byte gray = (byte)(r * 0.299 + g * 0.587 + b * 0.114);

                        currentLineGrayscale[x] = gray;     // Blue channel
                        currentLineGrayscale[x + 1] = gray; // Green channel
                        currentLineGrayscale[x + 2] = gray; // Red channel
                        if (bytesPerPixel == 4) // For ARGB, copy alpha channel
                        {
                            currentLineGrayscale[x + 3] = currentLineOriginal[x + 3]; // Alpha channel
                        }
                    }
                }
            }

            sourceBitmap.UnlockBits(originalData);
            grayscaleBitmap.UnlockBits(grayscaleData);

            return grayscaleBitmap;
        }

        // It's good practice to dispose of Bitmaps when no longer needed to free memory.
        // This is especially important for image processing apps.
        // You can override the FormClosing event to dispose of images.
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (originalImage != null)
            {
                originalImage.Dispose();
                originalImage = null;
            }
            if (processedImage != null)
            {
                processedImage.Dispose();
                processedImage = null;
            }
      
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (originalImageA != null) originalImageA.Dispose();
                    originalImageA = new Bitmap(openFileDialog1.FileName);
                    // The second picturebox displays Image A
                    pictureBoxProcessed.Image = originalImageA;
                    MessageBox.Show("Image A (Foreground) Loaded Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Image A: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
      
        private void btnLoadImageB_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (originalImageB != null) originalImageB.Dispose();
                    originalImageB = new Bitmap(openFileDialog1.FileName);
                    // The first picturebox displays Image B
                    pictureBoxOriginal.Image = originalImageB;
                    MessageBox.Show("Image B (Reference) Loaded Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Image B: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        /// <summary>
        /// Converts a Color to a single 1-byte grayscale value (0-255).
        /// </summary>
        private byte ColorToGrayscaleByte(Color color)
        {
            // Luminosity method: Gray = 0.299*R + 0.587*G + 0.114*B
            int gray = (int)(color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
            // Clamp to 0-255 just in case, though it should be within range
            return (byte)Math.Max(0, Math.Min(255, gray));
        }

            

        private void pictureBoxOriginal_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBoxProcessed_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            WebcamSubtractForm newForm = new WebcamSubtractForm();

            // 2. Show the new form (non-modal)
            newForm.Show();
           this.Hide();
        }
    }


}