using openCV;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace final_project
{
    public partial class RGBForm : Form
    {
        IplImage sourceImage;

        // Default constructor (Required for the Designer to load)
        public RGBForm()
        {
            InitializeComponent();
        }

        // Custom constructor to receive the image from Form1
        public RGBForm(IplImage img) : this()
        {
            this.sourceImage = img;
        }

        private void RGBForm_Load(object sender, EventArgs e)
        {
            if (sourceImage.ptr != IntPtr.Zero)
            {
                ProcessAndDisplayChannels();
            }
        }

        private void ProcessAndDisplayChannels()
        {
            // Create headers for the R, G, and B images
            CvSize size = new CvSize(sourceImage.width, sourceImage.height);
            IplImage redImg = cvlib.CvCreateImage(size, sourceImage.depth, sourceImage.nChannels);
            IplImage greenImg = cvlib.CvCreateImage(size, sourceImage.depth, sourceImage.nChannels);
            IplImage blueImg = cvlib.CvCreateImage(size, sourceImage.depth, sourceImage.nChannels);

            int srcAdd = sourceImage.imageData.ToInt32();
            int rAdd = redImg.imageData.ToInt32();
            int gAdd = greenImg.imageData.ToInt32();
            int bAdd = blueImg.imageData.ToInt32();

            unsafe
            {
                for (int r = 0; r < sourceImage.height; r++)
                {
                    for (int c = 0; c < sourceImage.width; c++)
                    {
                        // Calculate pixel offset (BGR format)
                        int index = (sourceImage.width * r * sourceImage.nChannels) + (c * sourceImage.nChannels);

                        byte b = *(byte*)(srcAdd + index + 0);
                        byte g = *(byte*)(srcAdd + index + 1);
                        byte rVal = *(byte*)(srcAdd + index + 2);

                        // Fill Red channel image (only Red byte preserved)
                        *(byte*)(rAdd + index + 2) = rVal;

                        // Fill Green channel image (only Green byte preserved)
                        *(byte*)(gAdd + index + 1) = g;

                        // Fill Blue channel image (only Blue byte preserved)
                        *(byte*)(bAdd + index + 0) = b;
                    }
                }
            }

            // Display to the local picture boxes
            DisplayResized(redImg, pictureBox1);
            DisplayResized(greenImg, pictureBox2);
            DisplayResized(blueImg, pictureBox3);
        }

        private void DisplayResized(IplImage img, PictureBox pb)
        {
            CvSize size = new CvSize(pb.Width, pb.Height);
            IplImage resized = cvlib.CvCreateImage(size, img.depth, img.nChannels);
            cvlib.CvResize(ref img, ref resized, cvlib.CV_INTER_LINEAR);

            pb.BackgroundImage = (System.Drawing.Image)resized;
            pb.BackgroundImageLayout = ImageLayout.Stretch;
        }
    }
}