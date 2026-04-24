using openCV;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace final_project
{
    public partial class NegativeForm : Form
    {
        IplImage original; // The clean original
        IplImage result;   // The negative version

        // Update constructor to accept the image from Form1
        public NegativeForm(IplImage img)
        {
            InitializeComponent();
            this.original = img;

            // Create the result container
            result = cvlib.CvCreateImage(new CvSize(original.width, original.height), original.depth, original.nChannels);

            // Run the filter immediately
            ApplyNegative();
        }

        private void ApplyNegative()
        {
            int srcAdd = original.imageData.ToInt32();
            int dstAdd = result.imageData.ToInt32();

            unsafe
            {
                int totalBytes = original.width * original.height * original.nChannels;
                for (int i = 0; i < totalBytes; i++)
                {
                    // The Negative Math: Invert the byte value
                    byte currentVal = *(byte*)(srcAdd + i);
                    *(byte*)(dstAdd + i) = (byte)(255 - currentVal);
                }
            }
            DisplayPreview(result);
        }

        private void DisplayPreview(IplImage img)
        {
            if (pictureBox1.Width <= 0 || pictureBox1.Height <= 0) return;

            CvSize size = new CvSize(pictureBox1.Width, pictureBox1.Height);
            IplImage resized = cvlib.CvCreateImage(size, img.depth, img.nChannels);
            cvlib.CvResize(ref img, ref resized, cvlib.CV_INTER_LINEAR);

            pictureBox1.BackgroundImage = (System.Drawing.Image)resized;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
        }

        // Cancel Button (button1)
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Confirm Button (button2)
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Public method to send the result back to Form1
        public IplImage GetResult()
        {
            return result;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Leave empty
        }
    }
}