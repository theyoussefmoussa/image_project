using openCV;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace final_project
{
    public partial class Form1 : Form
    {
        // Global variables
        IplImage image1;
        FormGrey greyFormInstance;

        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = " ";
            openFileDialog1.Filter = "JPEG|*JPG|Bitmap|*.bmp|All|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    image1 = cvlib.CvLoadImage(openFileDialog1.FileName, cvlib.CV_LOAD_IMAGE_COLOR);
                    DisplayImageToMain(image1, pictureBox1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message);
                }
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 1. Clear the main display
            pictureBox1.BackgroundImage = null;

            // 2. Release Memory safely
            if (image1.ptr != IntPtr.Zero)
            {
                cvlib.CvReleaseImage(ref image1);
                image1.ptr = IntPtr.Zero;
            }

            // 3. Close the secondary form if it's still open
            if (greyFormInstance != null && !greyFormInstance.IsDisposed)
            {
                greyFormInstance.Close();
            }

            MessageBox.Show("Workspace cleared.");
        }

        private void DisplayImageToMain(IplImage src, PictureBox pb)
        {
            if (pb == null || src.ptr == IntPtr.Zero) return;

            CvSize size = new CvSize(pb.Width, pb.Height);
            IplImage resized = cvlib.CvCreateImage(size, src.depth, src.nChannels);
            cvlib.CvResize(ref src, ref resized, cvlib.CV_INTER_LINEAR);

            pb.BackgroundImage = (System.Drawing.Image)resized;
            pb.BackgroundImageLayout = ImageLayout.Stretch;
        }

        // --- Navigation to New Forms ---

        private void toRGBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image1.ptr != IntPtr.Zero)
            {
                // Create and show the new RGB separation form
                RGBForm rgbWindow = new RGBForm(image1);
                rgbWindow.Show();
            }
            else
            {
                MessageBox.Show("Please load an image first.");
            }
        }

        private void toGreyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image1.ptr == IntPtr.Zero) return;
            if (greyFormInstance == null || greyFormInstance.IsDisposed)
            {
                greyFormInstance = new FormGrey(image1);
                greyFormInstance.Show();
            }
            else greyFormInstance.Focus();
        }

        // --- Filter Menu Items ---

        private void brightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image1.ptr != IntPtr.Zero)
            {
                BrightnessForm bForm = new BrightnessForm(image1);

                // ShowDialog blocks Form1 until BrightnessForm is closed
                // Only update image1 if the user clicked "Confirm" (button2)
                if (bForm.ShowDialog() == DialogResult.OK)
                {
                    image1 = bForm.GetResult();
                    DisplayImageToMain(image1, pictureBox1);
                }
                // If they clicked Cancel (button1), this 'if' is skipped and image1 remains original
            }
        }

        private void negariveFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image1.ptr != IntPtr.Zero)
            {
                NegativeForm nForm = new NegativeForm(image1);
                if (nForm.ShowDialog() == DialogResult.OK)
                {
                    image1 = nForm.GetResult();
                    DisplayImageToMain(image1, pictureBox1);
                }
            }
        }

        private void contrastFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image1.ptr != IntPtr.Zero)
            {
                ContrastForm cForm = new ContrastForm(image1);
                if (cForm.ShowDialog() == DialogResult.OK)
                {
                    image1 = cForm.GetResult();
                    DisplayImageToMain(image1, pictureBox1);
                }
            }
        }

        private void equalizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image1.ptr != IntPtr.Zero)
            {
                EqualizedForm eqForm = new EqualizedForm(image1);
                eqForm.Show();
            }
        }

        // --- Designer Compatibility placeholders ---
        private void resetToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // This is a placeholder to satisfy the Designer
        }
    }
}