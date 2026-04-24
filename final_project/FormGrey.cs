using openCV;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace final_project
{
    public partial class FormGrey : Form
    {
        IplImage image1;
        IplImage tempGray;

        public FormGrey(IplImage img)
        {
            InitializeComponent();
            this.image1 = img;
            ShowImages();
            CalculateAndShowHistogram();
        }

        private void ShowImages()
        {
            if (image1.ptr != IntPtr.Zero)
            {
                DisplayImage(image1, pictureBox1);

                tempGray = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, 1);
                cvlib.CvCvtColor(ref image1, ref tempGray, cvlib.CV_BGR2GRAY);

                IplImage displayGray = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, 3);
                cvlib.CvCvtColor(ref tempGray, ref displayGray, cvlib.CV_GRAY2BGR);

                DisplayImage(displayGray, pictureBox2);
            }
        }

        public void CalculateAndShowHistogram()
        {
            if (tempGray.ptr == IntPtr.Zero) return;

            int[] hist = new int[256];
            int srcAdd = tempGray.imageData.ToInt32();

            unsafe
            {
                for (int r = 0; r < tempGray.height; r++)
                {
                    for (int c = 0; c < tempGray.width; c++)
                    {
                        int index = (tempGray.width * r) + c;
                        byte grayValue = *(byte*)(srcAdd + index);
                        hist[grayValue]++;
                    }
                }
            }

            int histWidth = 256;
            int histHeight = pictureBox3.Height;
            Bitmap histBitmap = new Bitmap(histWidth, histHeight);
            using (Graphics g = Graphics.FromImage(histBitmap))
            {
                g.Clear(Color.White);
                int max = hist.Max();
                if (max == 0) max = 1;

                for (int i = 0; i < 256; i++)
                {
                    int barHeight = (int)(((double)hist[i] / max) * histHeight);
                    g.DrawLine(Pens.Black, new Point(i, histHeight), new Point(i, histHeight - barHeight));
                }
            }

            pictureBox3.Image = histBitmap;
        }

        private void DisplayImage(IplImage src, PictureBox pb)
        {
            if (pb == null) return;
            CvSize size = new CvSize(pb.Width, pb.Height);
            IplImage resized = cvlib.CvCreateImage(size, src.depth, src.nChannels);
            cvlib.CvResize(ref src, ref resized, cvlib.CV_INTER_LINEAR);
            pb.BackgroundImage = (System.Drawing.Image)resized;
            pb.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // This can remain empty
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            // This can remain empty
        }
    }
}