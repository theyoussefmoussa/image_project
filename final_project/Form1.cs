using openCV;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace final_project
{
    public partial class Form1 : Form
    {
        // Only one global variable needed for the current image
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
                    // Load the image directly into our working variable
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
            // 1. Wipe all PictureBoxes
            pictureBox1.BackgroundImage = null;
            if (pictureBox2 != null) pictureBox2.BackgroundImage = null;
            if (pictureBox3 != null) pictureBox3.BackgroundImage = null;
            if (pictureBox4 != null) pictureBox4.BackgroundImage = null;

            // 2. Release Memory safely
            if (image1.ptr != IntPtr.Zero)
            {
                cvlib.CvReleaseImage(ref image1);
                image1.ptr = IntPtr.Zero;
            }

            // 3. Close the secondary Grey form if it exists
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

        // --- Filter Menu Items ---

        private void brightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image1.ptr != IntPtr.Zero)
            {
                BrightnessForm bForm = new BrightnessForm(image1);
                if (bForm.ShowDialog() == DialogResult.OK)
                {
                    image1 = bForm.GetResult();
                    DisplayImageToMain(image1, pictureBox1);
                }
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

        private void equalizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image1.ptr != IntPtr.Zero)
            {
                EqualizedForm eqForm = new EqualizedForm(image1);
                eqForm.Show();
            }
        }

        private void toRGBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image1.ptr != IntPtr.Zero)
            {
                ShowRGBChannels();
            }
        }

        // --- Helper Methods ---

        private void ShowRGBChannels()
        {
            IplImage redImg = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, image1.nChannels);
            IplImage greenImg = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, image1.nChannels);
            IplImage blueImg = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, image1.nChannels);

            int srcAdd = image1.imageData.ToInt32();
            int rAdd = redImg.imageData.ToInt32();
            int gAdd = greenImg.imageData.ToInt32();
            int bAdd = blueImg.imageData.ToInt32();

            unsafe
            {
                for (int r = 0; r < image1.height; r++)
                {
                    for (int c = 0; c < image1.width; c++)
                    {
                        int index = (image1.width * r * image1.nChannels) + (c * image1.nChannels);
                        byte blue = *(byte*)(srcAdd + index + 0);
                        byte green = *(byte*)(srcAdd + index + 1);
                        byte red = *(byte*)(srcAdd + index + 2);

                        *(byte*)(rAdd + index + 2) = red;
                        *(byte*)(gAdd + index + 1) = green;
                        *(byte*)(bAdd + index + 0) = blue;
                    }
                }
            }

            DisplayImageToMain(redImg, pictureBox2);
            DisplayImageToMain(greenImg, pictureBox3);
            DisplayImageToMain(blueImg, pictureBox4);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Placeholder to prevent designer errors
        }
    }
}