using openCV;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace final_project
{
    public partial class BrightnessForm : Form
    {
        IplImage original;
        IplImage result;

        // Added parameterless constructor for Designer compatibility
        public BrightnessForm()
        {
            InitializeComponent();
        }

        public BrightnessForm(IplImage img) : this()
        {
            this.original = img;
            this.result = cvlib.CvCreateImage(new CvSize(original.width, original.height), original.depth, original.nChannels);

            // TrackBar setup
            trackBar1.Minimum = -255;
            trackBar1.Maximum = 255;
            trackBar1.Value = 0;

            ApplyBrightness(0);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            ApplyBrightness(trackBar1.Value);
        }

        private void ApplyBrightness(int amount)
        {
            int srcAdd = original.imageData.ToInt32();
            int dstAdd = result.imageData.ToInt32();

            unsafe
            {
                int totalBytes = original.width * original.height * original.nChannels;

                for (int i = 0; i < totalBytes; i++)
                {
                    int val = *(byte*)(srcAdd + i) + amount;

                    // Clamping to stay in 0-255 range
                    if (val > 255) val = 255;
                    if (val < 0) val = 0;

                    *(byte*)(dstAdd + i) = (byte)val;
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

        // button2 is your "Confirm" button
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK; // Signals success to Form1
            this.Close();
        }

        // button1 is your "Cancel" button
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; // Signals failure/ignore to Form1
            this.Close();
        }

        public IplImage GetResult()
        {
            return result;
        }

        private void pictureBox1_Click(object sender, EventArgs e) { }
    }
}