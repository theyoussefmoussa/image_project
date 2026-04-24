using openCV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace final_project
{
    public partial class Form1 : Form
    {
        IplImage image1; // Original loaded image
        IplImage img;    // Processed image (after color filtering)
        Bitmap bmp;
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Reset file dialog path
            openFileDialog1.FileName = " ";

            // Set allowed image formats
            openFileDialog1.Filter = "JPEG|*JPG|Bitmap|*.bmp|All|*.*";

            // Open file dialog and check if user selected a file
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Load image using OpenCV library (color mode)
                    image1 = cvlib.CvLoadImage(
                        openFileDialog1.FileName,
                        cvlib.CV_LOAD_IMAGE_COLOR
                    );

                    // Define target size based on PictureBox dimensions
                    CvSize size = new CvSize(
                        pictureBox1.Width,
                        pictureBox1.Height
                    );

                    // Create a resized image container
                    IplImage resized_image = cvlib.CvCreateImage(
                        size,
                        image1.depth,
                        image1.nChannels
                    );

                    // Resize original image into the new container
                    cvlib.CvResize(
                        ref image1,
                        ref resized_image,
                        cvlib.CV_INTER_LINEAR
                    );

                    // Display resized image in PictureBox
                    pictureBox1.BackgroundImage = (System.Drawing.Image)resized_image;
                }
                catch (Exception ex)
                {
                    // Show error message if image loading fails
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // =========================
        // Processing Functions
        // =========================

        private void ShowRGBChannels()
        {
            IplImage redImg = cvlib.CvCreateImage(
                new CvSize(image1.width, image1.height),
                image1.depth, image1.nChannels);

            IplImage greenImg = cvlib.CvCreateImage(
                new CvSize(image1.width, image1.height),
                image1.depth, image1.nChannels);

            IplImage blueImg = cvlib.CvCreateImage(
                new CvSize(image1.width, image1.height),
                image1.depth, image1.nChannels);

            int srcAdd = image1.imageData.ToInt32();
            int rAdd = redImg.imageData.ToInt32();
            int gAdd = greenImg.imageData.ToInt32();
            int bAdd = blueImg.imageData.ToInt32();

            unsafe
            {
                int index;

                for (int r = 0; r < image1.height; r++)
                {
                    for (int c = 0; c < image1.width; c++)
                    {
                        index = (image1.width * r * image1.nChannels) + (c * image1.nChannels);

                        byte blue = *(byte*)(srcAdd + index + 0);
                        byte green = *(byte*)(srcAdd + index + 1);
                        byte red = *(byte*)(srcAdd + index + 2);

                        // Red image
                        *(byte*)(rAdd + index + 0) = 0;
                        *(byte*)(rAdd + index + 1) = 0;
                        *(byte*)(rAdd + index + 2) = red;

                        // Green image
                        *(byte*)(gAdd + index + 0) = 0;
                        *(byte*)(gAdd + index + 1) = green;
                        *(byte*)(gAdd + index + 2) = 0;

                        // Blue image
                        *(byte*)(bAdd + index + 0) = blue;
                        *(byte*)(bAdd + index + 1) = 0;
                        *(byte*)(bAdd + index + 2) = 0;
                    }
                }
            }

            DisplayImage(redImg, pictureBox2);
            DisplayImage(greenImg, pictureBox3);
            DisplayImage(blueImg, pictureBox4);
        }

        private void DisplayImage(IplImage src, PictureBox pb)
        {
            CvSize size = new CvSize(pb.Width, pb.Height);
            IplImage resized = cvlib.CvCreateImage(size, src.depth, src.nChannels);

            cvlib.CvResize(ref src, ref resized, cvlib.CV_INTER_LINEAR);

            pb.BackgroundImage = (System.Drawing.Image)resized;
        }

        private void toRGBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowRGBChannels();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void toGreyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image1.ptr == IntPtr.Zero)
            {
                MessageBox.Show("Please load an image first.");
                return;
            }

            // Initialize the new form and pass the current image
            FormGrey greyForm = new FormGrey(image1);

            // Show the form
            greyForm.Show();
        }
    }
}
