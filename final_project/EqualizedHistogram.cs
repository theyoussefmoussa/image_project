using openCV;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using openCV;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace final_project
{
    public partial class EqualizedForm : Form
    {
        IplImage original;
        IplImage grayImg;

        public EqualizedForm(IplImage img)
        {
            InitializeComponent();
            this.original = img;
            ProcessEqualization();
        }


        private void ProcessEqualization()
        {
            // 1. Convert to Grayscale
            grayImg = cvlib.CvCreateImage(new CvSize(original.width, original.height), original.depth, 1);
            cvlib.CvCvtColor(ref original, ref grayImg, cvlib.CV_BGR2GRAY);
            DisplayImage(original, pictureBox1); // original is 3-channel, this is fine

            // 2. Calculate Original Histogram
            int[] hist = GetHistogram(grayImg);
            pictureBox2.Image = DrawHistogram(hist);

            // 3. Histogram Equalization Logic
            int numPixels = grayImg.width * grayImg.height;
            int[] lut = new int[256];
            long sum = 0;

            for (int i = 0; i < 256; i++)
            {
                sum += hist[i];
                lut[i] = (int)((float)sum * 255 / numPixels);
            }

            // 4. Apply Look-up Table to create 1-channel Equalized Image
            IplImage equalizedImg1Ch = cvlib.CvCreateImage(new CvSize(grayImg.width, grayImg.height), grayImg.depth, 1);
            int srcAdd = grayImg.imageData.ToInt32();
            int dstAdd = equalizedImg1Ch.imageData.ToInt32();

            unsafe
            {
                for (int i = 0; i < (grayImg.width * grayImg.height); i++)
                {
                    byte originalVal = *(byte*)(srcAdd + i);
                    *(byte*)(dstAdd + i) = (byte)lut[originalVal];
                }
            }

            // 5. FIX: Convert 1-channel image to 3-channel for display
            IplImage displayEqualized = cvlib.CvCreateImage(new CvSize(grayImg.width, grayImg.height), grayImg.depth, 3);
            cvlib.CvCvtColor(ref equalizedImg1Ch, ref displayEqualized, cvlib.CV_GRAY2BGR);

            // Use the 3-channel version for the PictureBox
            DisplayImage(displayEqualized, pictureBox3);

            // 6. Display Equalized Histogram (using the 1-channel image for math)
            int[] eqHist = GetHistogram(equalizedImg1Ch);
            pictureBox4.Image = DrawHistogram(eqHist);
        }


        private int[] GetHistogram(IplImage img)
        {
            int[] hist = new int[256];
            int addr = img.imageData.ToInt32();
            unsafe
            {
                for (int i = 0; i < (img.width * img.height); i++)
                {
                    hist[*(byte*)(addr + i)]++;
                }
            }
            return hist;
        }

        private Bitmap DrawHistogram(int[] hist)
        {
            Bitmap bmp = new Bitmap(256, 150);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                int max = hist.Max();
                for (int i = 0; i < 256; i++)
                {
                    int height = (int)(((double)hist[i] / max) * 150);
                    g.DrawLine(Pens.Black, i, 150, i, 150 - height);
                }
            }
            return bmp;
        }

        private void DisplayImage(IplImage src, PictureBox pb)
        {
            CvSize size = new CvSize(pb.Width, pb.Height);
            IplImage resized = cvlib.CvCreateImage(size, src.depth, src.nChannels);
            cvlib.CvResize(ref src, ref resized, cvlib.CV_INTER_LINEAR);
            pb.BackgroundImage = (System.Drawing.Image)resized;
            pb.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Leave empty
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Leave empty
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            // Leave empty
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {

        }
    }
}