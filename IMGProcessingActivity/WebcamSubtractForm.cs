using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Threading;
using System.Drawing.Imaging;
using WebCamLib;

namespace IMGProcessingActivity
{
    public partial class WebcamSubtractForm : Form
    {
        private Device selectedDevice;
        private Device[] videoDevices;
        private Bitmap subjectFrame;
        private Bitmap newBackground;
        private Color chromaKeyColor = Color.Green;
        private bool isCameraRunning = false; // N

        public WebcamSubtractForm()
        {
            InitializeComponent();
         
        }

        private void btnStartWebcam_Click(object sender, EventArgs e)
        {
            selectedDevice.ShowWindow(pictureBoxLiveFeed);

            // 2. Start the processing loop (Timer)
            processTimer.Start();
            isCameraRunning = true;

            // 3. Optional: Resize Background immediately if it exists, using the PictureBox size
            //    This is for scenarios where the background is loaded BEFORE the camera starts.
            if (newBackground != null)
            {
                // Use the control's size, which matches the video stream size
                newBackground = new Bitmap(newBackground, pictureBoxLiveFeed.Size);
            }
        }
        private void processTimer_Tick(object sender, EventArgs e)
        {
            ProcessFrame();
        }


        private void WebcamSubtractForm_Load(object sender, EventArgs e)
        {
            // 1. Get all available devices
            videoDevices = DeviceManager.GetAllDevices(); // Store all devices here

            if (videoDevices.Length > 0)
            {
                // 2. Populate the ComboBox
                foreach (Device device in videoDevices)
                {
                    // The ToString() method of the Device class returns the device name
                    comboBoxDevices.Items.Add(device);
                }

                // 3. Select the first device by default
                comboBoxDevices.SelectedIndex = 0;

                // Note: selectedDevice is now set in the ComboBox's SelectedIndexChanged event (see below)
                this.Text = "Webcam App - Ready to select device";

                selectedDevice = comboBoxDevices.SelectedItem as Device;
                selectedDevice = videoDevices[0]; // Default to the first device
            }
            else
            {
                MessageBox.Show("No webcam devices found.");
                btnStartWebcam.Enabled = false;
                comboBoxDevices.Enabled = false;
            }
        }

        private void captureBtn_Click(object sender, EventArgs e)
        {
            if (selectedDevice != null)
            {
                // 1. Copy the frame from the live stream to the clipboard
                selectedDevice.Sendmessage(); //

                // 2. Retrieve the image from the clipboard
                if (Clipboard.ContainsImage())
                {
                    // Store the captured image as a Bitmap for later processing
                    subjectFrame = Clipboard.GetImage() as Bitmap;

                    // Optional: You can display the captured subject in the Original Image box temporarily
                    // pictureBoxOriginal.Image = subjectFrame; 
                    MessageBox.Show("Subject frame captured and saved in the clipboard.");
                }
            }
            else
            {
                MessageBox.Show("Please start the camera and capture a frame first.");
            }
        }

        private void WebcamSubtractForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (selectedDevice != null)
            {
                selectedDevice.Stop();
            }
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            if (selectedDevice != null)
            {
                // 1. Copy the current frame from the live feed to the clipboard.
                selectedDevice.Sendmessage(); // Correctly copies the current frame to the clipboard

                // Check if an image was successfully placed on the clipboard.
                if (Clipboard.ContainsImage())
                {
                    // Get the image from the clipboard.
                    Image capturedImage = Clipboard.GetImage();

                    // Store the captured image as the subject frame.
                    subjectFrame = new Bitmap(capturedImage);

                    // 2. CORRECT: Display the CAPTURED SNAPSHOT in the Original PictureBox.
                    pictureBoxOriginal.Image = subjectFrame;

                    MessageBox.Show("Subject frame captured and displayed in Original Image box.");
                }
                else
                {
                    MessageBox.Show("Unable to capture image from live feed. Please try again.");
                }
            }
            else
            {
                MessageBox.Show("Please start the webcam first to capture a frame.");
            }
        }

        private void btnLoadBackground_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // 1. Load the original image file into a temporary variable
                    using (Image tempImage = Image.FromFile(ofd.FileName))
                    {
                        // 2. Determine the target size from the control connected to the webcam.
                        //    This should be 640x360 based on your error message.
                        Size targetSize = pictureBoxLiveFeed.Size;

                        // 3. Resize the background image to the target size and store it in newBackground.
                        newBackground = new Bitmap(tempImage, targetSize);
                    }

                    // 4. Display the loaded/resized background in the PROCESSED IMAGE box.
                    //    (Remember, the final output goes into the LIVE FEED box via ProcessFrame)
                    pictureBoxProcessed.Image = newBackground;

                    MessageBox.Show("Background image loaded and resized to match camera size.");
                }
            }
        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {
            if (subjectFrame == null || newBackground == null)
            {
                MessageBox.Show("Please capture a subject image and load a background image first.");
                return;
            }

            // Check if the background size matches the subject size (needed for pixel iteration)
            if (subjectFrame.Width != newBackground.Width || subjectFrame.Height != newBackground.Height)
            {
                MessageBox.Show("Background image size must match the captured subject size (" + subjectFrame.Width + "x" + subjectFrame.Height + "). Please load a matching image.");
                return;
            }

            // Create the resulting image container
            Bitmap resultImage = new Bitmap(subjectFrame.Width, subjectFrame.Height);

            // Define the tolerance/threshold for green color detection
            // A high green value compared to R and B indicates a green screen.
            int threshold = 60;

            for (int y = 0; y < subjectFrame.Height; y++)
            {
                for (int x = 0; x < subjectFrame.Width; x++)
                {
                    Color subjectPixel = subjectFrame.GetPixel(x, y);

                    // Check for a green screen pixel: Green component is much higher than Red and Blue
                    bool isChromaKey = (subjectPixel.G > subjectPixel.R + threshold) &&
                                       (subjectPixel.G > subjectPixel.B + threshold);

                    if (isChromaKey)
                    {
                        // Replace the green screen pixel with the pixel from the new background
                        Color backgroundPixel = newBackground.GetPixel(x, y);
                        resultImage.SetPixel(x, y, backgroundPixel);
                    }
                    else
                    {
                        // Keep the subject pixel (part of the foreground)
                        resultImage.SetPixel(x, y, subjectPixel);
                    }
                }
            }

            // Display the final result in the ORIGINAL IMAGE PictureBox
            pictureBoxOriginal.Image = resultImage;
        }

        private void ProcessFrame()
        {
            // 1. Capture the subject frame
            selectedDevice.Sendmessage(); // Copy frame to clipboard

            if (!Clipboard.ContainsImage()) return;

            // Retrieve the frame from the clipboard
            // Must use 'new Bitmap()' to create a copy from the clipboard contents
            subjectFrame = Clipboard.GetImage() as Bitmap;

            if (subjectFrame == null) return;

            if (newBackground == null)
            {
                pictureBoxLiveFeed.Image = subjectFrame; // Show raw feed if no background is loaded
                return;
            }

            // FINAL SAFETY RESIZE: Check the background against the actual captured frame size
            if (newBackground.Width != subjectFrame.Width || newBackground.Height != subjectFrame.Height)
            {
                // Resize the background to match the actual captured subjectFrame size
                newBackground = new Bitmap(newBackground, subjectFrame.Size);
            }

            // 2. OPTION 1: RESIZE THE BACKGROUND IMAGE TO MATCH SUBJECT SIZE
            if (newBackground.Width != subjectFrame.Width || newBackground.Height != subjectFrame.Height)
            {
                // Create a temporary Bitmap, draw the original image onto it at the new size
                // and replace newBackground. This ensures all future frames are the right size.
                newBackground = new Bitmap(newBackground, subjectFrame.Size);
            }

            // 3. Perform Subtraction
            Bitmap resultImage = new Bitmap(subjectFrame.Width, subjectFrame.Height);
            int threshold = 60; // Tolerance for green detection

            for (int y = 0; y < subjectFrame.Height; y++)
            {
                for (int x = 0; x < subjectFrame.Width; x++)
                {
                    Color subjectPixel = subjectFrame.GetPixel(x, y);

                    // Chroma Key Check: Green component is substantially higher than Red and Blue
                    bool isChromaKey = (subjectPixel.G > subjectPixel.R + threshold) &&
                                       (subjectPixel.G > subjectPixel.B + threshold);

                    if (isChromaKey)
                    {
                        // Replace with the pixel from the resized background
                        resultImage.SetPixel(x, y, newBackground.GetPixel(x, y));
                    }
                    else
                    {
                        // Keep the subject pixel (foreground)
                        resultImage.SetPixel(x, y, subjectPixel);
                    }
                }
            }

            // 4. Display the result in the processed picture box
            pictureBoxLiveFeed.Image = resultImage;
        }
        
        private void btnStopWebcam_Click_1(object sender, EventArgs e)
        {
            // The 'isCameraRunning' flag is necessary to ensure the loop is running before attempting to stop.
            if (!isCameraRunning) return;

            // 1. Stop the continuous processing timer
            processTimer.Stop();

            // 2. Disconnect the camera driver
            if (selectedDevice != null)
            {
                // This method sends the WM_CAP_DRIVER_DISCONNECT message and destroys the window handle.
                selectedDevice.Stop(); //
            }

            // 3. Clear the PictureBoxes to indicate the feed has stopped
            pictureBoxLiveFeed.Image = null;
            // Clear the processed output as well
            pictureBoxProcessed.Image = null;

            // 4. Update the state and button status
            isCameraRunning = false;
            btnStartWebcam.Enabled = true; // Allow the user to start the camera again
            btnStopWebcam.Enabled = false;
        }

        private void pictureBoxLiveFeed_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }


}
