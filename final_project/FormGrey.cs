using openCV;
using System;
using System.Windows.Forms;
namespace final_project
{
    public partial class FormGrey : Form
    {
        IplImage image1;

        public FormGrey(IplImage img)
        {
            InitializeComponent();
            this.image1 = img;
            ShowImages();
        }

        // ONLY ONE VERSION OF THIS SHOULD EXIST
        private void ShowImages()
        {
            if (image1.ptr != IntPtr.Zero)
            {
                // 1. Show original image
                DisplayImage(image1, pictureBox1);

                // 2. Create a temporary 1-channel gray image
                IplImage tempGray = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, 1);
                cvlib.CvCvtColor(ref image1, ref tempGray, cvlib.CV_BGR2GRAY);

                // 3. Create a 3-channel image to hold the gray data (Fixes the Exception crash)
                IplImage displayGray = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, 3);
                cvlib.CvCvtColor(ref tempGray, ref displayGray, cvlib.CV_GRAY2BGR);

                // 4. Show the 3-channel version
                DisplayImage(displayGray, pictureBox2);
            }
        }

        // YOU WERE MISSING THIS FUNCTION IN FORMGREY
        private void DisplayImage(IplImage src, PictureBox pb)
        {
            if (pb == null) return;

            CvSize size = new CvSize(pb.Width, pb.Height);
            IplImage resized = cvlib.CvCreateImage(size, src.depth, src.nChannels);
            cvlib.CvResize(ref src, ref resized, cvlib.CV_INTER_LINEAR);

            // This works now because displayGray is 3-channel
            pb.BackgroundImage = (System.Drawing.Image)resized;
            pb.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}