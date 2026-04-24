using openCV;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace final_project
{
    public partial class BrightnessForm : Form
    {
        IplImage original; // The clean original to read from
        IplImage result;   // The brightened version to show

        // Modify the constructor to accept the image from Form1
        public BrightnessForm(IplImage img)
        {
            InitializeComponent();

            // 1. Create a copy of the original image
            this.original = img;

            // 2. Initialize the result image with the same specs
            this.result = cvlib.CvCreateImage(new CvSize(original.width, original.height), original.depth, original.nChannels);

            // 3. Set up the TrackBar defaults (if not done in Designer)
            trackBar1.Minimum = -255;
            trackBar1.Maximum = 255;
            trackBar1.Value = 0;

            // Show the initial image
            ApplyBrightness(0);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            // Whenever the slider moves, update the brightness
            ApplyBrightness(trackBar1.Value);
        }

        private void ApplyBrightness(int amount)
        {
            int srcAdd = original.imageData.ToInt32();
            int dstAdd = result.imageData.ToInt32();

            unsafe
            {
                // totalBytes = Width * Height * Channels (3 for RGB)
                int totalBytes = original.width * original.height * original.nChannels;

                for (int i = 0; i < totalBytes; i++)
                {
                    // Calculate new pixel value
                    int val = *(byte*)(srcAdd + i) + amount;

                    // Clamping: Ensure the value stays between 0 and 255
                    if (val > 255) val = 255;
                    if (val < 0) val = 0;

                    *(byte*)(dstAdd + i) = (byte)val;
                }
            }

            // Update the preview
            DisplayPreview(result);
        }

        private void DisplayPreview(IplImage img)
        {
            if (pictureBox1.Width <= 0 || pictureBox1.Height <= 0) return;

            // Resize the result to fit the picture box
            CvSize size = new CvSize(pictureBox1.Width, pictureBox1.Height);
            IplImage resized = cvlib.CvCreateImage(size, img.depth, img.nChannels);
            cvlib.CvResize(ref img, ref resized, cvlib.CV_INTER_LINEAR);

            // Set as background image
            pictureBox1.BackgroundImage = (System.Drawing.Image)resized;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
        }

        // Add an OK button click event to save changes
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Public method so Form1 can get the final brightened image
        public IplImage GetResult()
        {
            return result;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Can stay empty
        }
    }
}