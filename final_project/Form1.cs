using openCV;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace final_project
{
    public partial class Form1 : Form
    {
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
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // Restored missing toRGB method
        private void toRGBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image1.ptr == IntPtr.Zero) return;
            ShowRGBChannels();
        }

        // Restored missing Click event for the picturebox
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Leave empty or add logic
        }

        private void toGreyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image1.ptr == IntPtr.Zero)
            {
                MessageBox.Show("Please load an image first.");
                return;
            }

            if (greyFormInstance == null || greyFormInstance.IsDisposed)
            {
                greyFormInstance = new FormGrey(image1);
                greyFormInstance.Show();
            }
            else
            {
                greyFormInstance.Focus();
            }
        }

        private void showHistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (greyFormInstance != null && !greyFormInstance.IsDisposed)
            {
                greyFormInstance.CalculateAndShowHistogram();
            }
            else
            {
                MessageBox.Show("Please open the Gray Form first.");
            }
        }

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

            // Note: Ensure your Form1 has pictureBox2, 3, and 4 for this to work
            DisplayImageToMain(redImg, pictureBox2);
            DisplayImageToMain(greenImg, pictureBox3);
            DisplayImageToMain(blueImg, pictureBox4);
        }

        private void DisplayImageToMain(IplImage src, PictureBox pb)
        {
            if (pb == null) return;
            CvSize size = new CvSize(pb.Width, pb.Height);
            IplImage resized = cvlib.CvCreateImage(size, src.depth, src.nChannels);
            cvlib.CvResize(ref src, ref resized, cvlib.CV_INTER_LINEAR);
            pb.BackgroundImage = (System.Drawing.Image)resized;
        }

        private void equalizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image1.ptr != IntPtr.Zero)
            {
                // Pass the original image to the new form
                EqualizedForm eqForm = new EqualizedForm(image1);
                eqForm.Show();
            }
            else
            {
                MessageBox.Show("Please load an image first.");
            }
        }

        private void showHistogramToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}