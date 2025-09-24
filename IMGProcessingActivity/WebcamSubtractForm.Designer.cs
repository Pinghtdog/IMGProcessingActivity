namespace IMGProcessingActivity
{
    partial class WebcamSubtractForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBoxProcessed = new System.Windows.Forms.PictureBox();
            this.pictureBoxOriginal = new System.Windows.Forms.PictureBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.btnSubtract = new System.Windows.Forms.Button();
            this.btnLoadBackground = new System.Windows.Forms.Button();
            this.pictureBoxLiveFeed = new System.Windows.Forms.PictureBox();
            this.btnStartWebcam = new System.Windows.Forms.Button();
            this.captureBtn = new System.Windows.Forms.Button();
            this.comboBoxDevices = new System.Windows.Forms.ComboBox();
            this.processTimer = new System.Windows.Forms.Timer(this.components);
            this.btnStopWebcam = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProcessed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLiveFeed)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Upheaval TT (BRK)", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(896, 400);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(125, 18);
            this.textBox2.TabIndex = 12;
            this.textBox2.Text = "Background";
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Upheaval TT (BRK)", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(326, 400);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 18);
            this.textBox1.TabIndex = 11;
            this.textBox1.Text = "Fore Ground";
            // 
            // pictureBoxProcessed
            // 
            this.pictureBoxProcessed.Location = new System.Drawing.Point(652, 12);
            this.pictureBoxProcessed.Name = "pictureBoxProcessed";
            this.pictureBoxProcessed.Size = new System.Drawing.Size(640, 360);
            this.pictureBoxProcessed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxProcessed.TabIndex = 10;
            this.pictureBoxProcessed.TabStop = false;
            // 
            // pictureBoxOriginal
            // 
            this.pictureBoxOriginal.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxOriginal.Name = "pictureBoxOriginal";
            this.pictureBoxOriginal.Size = new System.Drawing.Size(640, 360);
            this.pictureBoxOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxOriginal.TabIndex = 9;
            this.pictureBoxOriginal.TabStop = false;
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Font = new System.Drawing.Font("Upheaval TT (BRK)", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(1535, 383);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(125, 18);
            this.textBox3.TabIndex = 14;
            this.textBox3.Text = "Live Feed";
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.Location = new System.Drawing.Point(514, 472);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(75, 23);
            this.btnLoadImage.TabIndex = 15;
            this.btnLoadImage.Text = "Load Image";
            this.btnLoadImage.UseVisualStyleBackColor = true;
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // btnSubtract
            // 
            this.btnSubtract.AutoSize = true;
            this.btnSubtract.Location = new System.Drawing.Point(778, 472);
            this.btnSubtract.Name = "btnSubtract";
            this.btnSubtract.Size = new System.Drawing.Size(75, 23);
            this.btnSubtract.TabIndex = 16;
            this.btnSubtract.Text = "Subtract";
            this.btnSubtract.UseVisualStyleBackColor = true;
            this.btnSubtract.Click += new System.EventHandler(this.btnSubtract_Click);
            // 
            // btnLoadBackground
            // 
            this.btnLoadBackground.Location = new System.Drawing.Point(638, 472);
            this.btnLoadBackground.Name = "btnLoadBackground";
            this.btnLoadBackground.Size = new System.Drawing.Size(106, 23);
            this.btnLoadBackground.TabIndex = 17;
            this.btnLoadBackground.Text = "Load Background";
            this.btnLoadBackground.UseVisualStyleBackColor = true;
            this.btnLoadBackground.Click += new System.EventHandler(this.btnLoadBackground_Click);
            // 
            // pictureBoxLiveFeed
            // 
            this.pictureBoxLiveFeed.Location = new System.Drawing.Point(1272, 12);
            this.pictureBoxLiveFeed.Name = "pictureBoxLiveFeed";
            this.pictureBoxLiveFeed.Size = new System.Drawing.Size(640, 360);
            this.pictureBoxLiveFeed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLiveFeed.TabIndex = 13;
            this.pictureBoxLiveFeed.TabStop = false;
            this.pictureBoxLiveFeed.Click += new System.EventHandler(this.pictureBoxLiveFeed_Click);
            // 
            // btnStartWebcam
            // 
            this.btnStartWebcam.Location = new System.Drawing.Point(881, 472);
            this.btnStartWebcam.Name = "btnStartWebcam";
            this.btnStartWebcam.Size = new System.Drawing.Size(106, 23);
            this.btnStartWebcam.TabIndex = 18;
            this.btnStartWebcam.Text = "Start Camera";
            this.btnStartWebcam.UseVisualStyleBackColor = true;
            this.btnStartWebcam.Click += new System.EventHandler(this.btnStartWebcam_Click);
            // 
            // captureBtn
            // 
            this.captureBtn.Location = new System.Drawing.Point(1011, 472);
            this.captureBtn.Name = "captureBtn";
            this.captureBtn.Size = new System.Drawing.Size(106, 23);
            this.captureBtn.TabIndex = 19;
            this.captureBtn.Text = "Capture Image";
            this.captureBtn.UseVisualStyleBackColor = true;
            this.captureBtn.Click += new System.EventHandler(this.captureBtn_Click);
            // 
            // comboBoxDevices
            // 
            this.comboBoxDevices.FormattingEnabled = true;
            this.comboBoxDevices.Location = new System.Drawing.Point(1146, 472);
            this.comboBoxDevices.Name = "comboBoxDevices";
            this.comboBoxDevices.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDevices.TabIndex = 20;
            this.comboBoxDevices.Visible = false;
            // 
            // btnStopWebcam
            // 
            this.btnStopWebcam.Location = new System.Drawing.Point(881, 501);
            this.btnStopWebcam.Name = "btnStopWebcam";
            this.btnStopWebcam.Size = new System.Drawing.Size(106, 23);
            this.btnStopWebcam.TabIndex = 21;
            this.btnStopWebcam.Text = "Stop Camera";
            this.btnStopWebcam.UseVisualStyleBackColor = true;
            this.btnStopWebcam.Click += new System.EventHandler(this.btnStopWebcam_Click_1);
            // 
            // WebcamSubtractForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 661);
            this.Controls.Add(this.btnStopWebcam);
            this.Controls.Add(this.comboBoxDevices);
            this.Controls.Add(this.captureBtn);
            this.Controls.Add(this.btnStartWebcam);
            this.Controls.Add(this.btnLoadBackground);
            this.Controls.Add(this.btnSubtract);
            this.Controls.Add(this.btnLoadImage);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.pictureBoxLiveFeed);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.pictureBoxProcessed);
            this.Controls.Add(this.pictureBoxOriginal);
            this.Name = "WebcamSubtractForm";
            this.Text = "WebcamSubtractForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WebcamSubtractForm_FormClosing);
            this.Load += new System.EventHandler(this.WebcamSubtractForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProcessed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLiveFeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBoxProcessed;
        private System.Windows.Forms.PictureBox pictureBoxOriginal;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.Button btnSubtract;
        private System.Windows.Forms.Button btnLoadBackground;
        private System.Windows.Forms.PictureBox pictureBoxLiveFeed;
        private System.Windows.Forms.Button btnStartWebcam;
        private System.Windows.Forms.Button captureBtn;
        private System.Windows.Forms.ComboBox comboBoxDevices;
        private System.Windows.Forms.Timer processTimer;
        private System.Windows.Forms.Button btnStopWebcam;
    }
}