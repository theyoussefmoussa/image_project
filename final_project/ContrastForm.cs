using openCV;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace final_project
{
    public partial class ContrastForm : Form
    {
        IplImage original;
        IplImage result;

        // Constructor accepting the image
        public ContrastForm(IplImage img)
        {
            InitializeComponent();
            this.original = img;

            // Initialize result image
            this.result = cvlib.CvCreateImage(new CvSize(original.width, original.height), original.depth, original.nChannels);

            // Set TrackBar defaults: 100 is "Neutral" (factor of 1.0)
            trackBar1.Minimum = 0;
            trackBar1.Maximum = 200;
            trackBar1.Value = 100;

            ApplyContrast(1.0f);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            // Convert 0-200 range to 0.0-2.0 factor
            float factor = trackBar1.Value / 100.0f;
            ApplyContrast(factor);
        }

        private void ApplyContrast(float factor)
        {
            int srcAdd = original.imageData.ToInt32();
            int dstAdd = result.imageData.ToInt32();

            unsafe
            {
                int totalBytes = original.width * original.height * original.nChannels;
                for (int i = 0; i < totalBytes; i++)
                {
                    byte oldVal = *(byte*)(srcAdd + i);

                    // Contrast Formula: (Pixel - Midpoint) * Factor + Midpoint
                    // We use 128 as the midpoint of a 0-255 scale
                    int newVal = (int)((oldVal - 128) * factor + 128);

                    // Clamping: Ensure we stay in 0-255 range
                    if (newVal > 255) newVal = 255;
                    if (newVal < 0) newVal = 0;

                    *(byte*)(dstAdd + i) = (byte)newVal;
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

        // Cancel Button
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Confirm Button
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public IplImage GetResult() => result;

        private void pictureBox1_Click(object sender, EventArgs e) { }
    }
}