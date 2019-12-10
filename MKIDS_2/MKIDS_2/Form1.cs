using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MKIDS_2
{
    public partial class Form1 : Form
    {
        static int t0 = 0;
        static double t;
        static double h = 0.001;
        static int N = 1000;
        static double a0;
        static double b0;
        //static int tN = 10;
        static double[] a_f = new double[N];
        static double[] b_f = new double[N];
        static double[] c_f = new double[N];

        static double[] a_g = new double[N];
        static double[] b_g = new double[N];
        static double[] c_g = new double[N];
        static double[] comp = new double[3];

        static int n = 30, m = 30;

        static Bitmap canvas;
        static Graphics g;

        public Form1()
        {
            InitializeComponent();
        }

        static double F(double x, double y)
        {
            return ((2 * x - y) * (2 * x - y) - 9);
        }
        static double G(double x, double y)
        {
            return ((x - 2 * y) * (x - 2 * y) - 9);
        }
        static void fill_ak()
        {
            a_f[0] = a0;
            a_g[0] = b0;
            for (int i = 1; i < N; i++)
            {

            }
        }
        static void fill_bk()
        {
            b_f[0] = a0;
            b_g[0] = b0;
            t = t0;
            for (int i = 1; i < N; i++)
            {
                b_f[i] = b_f[i - 1] + h * F(t, b_f[i - 1]);
                b_g[i] = b_g[i - 1] + h * G(t, b_g[i - 1]);
                t += h;
            }
        }
        static void fill_ck()
        {
            c_f[0] = a0;
            c_g[0] = b0;
            double[] c_tmp_f = new double[N];
            double[] c_tmp_g = new double[N];
            c_tmp_f[0] = c_f[0] + h / 2 * F(t0, c_f[0]);
            c_tmp_g[0] = c_g[0] + h / 2 * G(t0, c_f[0]);
            double t_tmp = t0 + h / 2;
            t = t0;
            for (int i = 1; i < N; i++)
            {
                c_f[i] = c_f[i - 1] + h * F(t_tmp, c_tmp_f[i - 1]);
                c_g[i] = c_g[i - 1] + h * G(t_tmp, c_tmp_g[i - 1]);
                t += h;
                c_tmp_f[i] = c_f[i] + h / 2 * F(t, c_f[i - 1]);
                c_tmp_g[i] = c_g[i] + h / 2 * G(t, c_g[i - 1]);
                t_tmp = t + h / 2;
            }
        }

        void print()
        {
            listBox1.Items.Clear();
            string str = "    {b_x}      {b_y}      {c_x}      {c_y}";
            listBox1.Items.Add(str);

            for (int i = 0; i < N; i++)
            {
                str = "";
                str += String.Format("{0,10}", Math.Round(b_f[i], 5));
                str += String.Format("{0,10}", Math.Round(b_g[i], 5));
                str += String.Format("{0,10}", Math.Round(c_f[i], 5));
                str += String.Format("{0,10}", Math.Round(c_g[i], 5));
                this.listBox1.Items.Add(str);
            }
            //str = "max|ak-bk|=";
            //str += Math.Round(comp[0], 5);
            //listBox1.Items.Add(str);

            //str = "max|ak-ck|=";
            //str += Math.Round(comp[1], 5);
            //listBox1.Items.Add(str);

            //str = "max|ak-dk|=";
            //str += Math.Round(comp[2], 5);
            //listBox1.Items.Add(str);

        }
        float Dx(double x, double y)
        {
            double f = F(x, y);
            double g = G(x, y);
            return (float)(f / Math.Sqrt((f) * (f) + (g) * (g)));
        }
        float Dy(double x, double y)
        {
            double f = F(x, y);
            double g = G(x, y);
            return (float)(g / Math.Sqrt((f) * (f) + (g) * (g)));
        }
        void draw_vector_field()
        {
            var W = pictureBox1.Width;
            var H = pictureBox1.Height;
            var pen = new Pen(Color.Black);
            float dW = W / n;
            float dH = H / m;
            float x0 = W / 2;
            float y0 = H / 2;
            float x1, x2, y1, y2;
            float scale = 10;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    x1 = i * dW;
                    y1 = j * dH;
                    x2 = Dx((x1 - x0) / W * scale, -(y1 - y0) / H * scale) * scale + x1;
                    y2 = -Dy((x1 - x0) / W * scale, -(y1 - y0) / H * scale) * scale + y1;
                    g.DrawLine(pen, x1, y1, x2, y2);
                    g.DrawEllipse(pen, x2 - 1, y2 - 1, 2, 2);
                }
            }
            pictureBox1.Image = canvas;
        }
        void draw_trajectories()
        {
            var W = pictureBox1.Width;
            var H = pictureBox1.Height;

            var R = 50;
            var G = 150;
            var B = 0;

            var pen = new Pen(Color.FromArgb(R, G, B));
            float dW = W / n;
            float dH = H / m;
            float x0 = W / 2;
            float y0 = H / 2;
            float scale = 10;
            float x1, y1, x2, y2;
            for (int i = 0; i < n; i++)
            {
                if (B < 230)
                {
                    B += 10;
                }
                if (B > 150 && R < 200)
                {
                    R += 20;
                    G -= 5;
                }
                for (int j = 0; j < m; j++)
                {
                    x2 = (i * dW - x0) / W * scale;
                    y2 = -(j * dH - y0) / H * scale;
                    for (int k = 0; k < 100; k++)
                    {
                        pen.Color = Color.FromArgb(R, G, B);
                        x1 = x2;
                        y1 = y2;
                        x2 = x1 + (float)Dx((double)x1, (double)y1) * 0.05f;
                        y2 = y1 + (float)Dy((double)x1, (double)y1) * 0.05f;
                        g.DrawLine(pen, x1 * W / scale + x0, y0 - y1 * H / scale, x2 * W / scale + x0, y0 - y2 * H / scale);
                    }
                    x2 = (i * dW - x0) / W * scale;
                    y2 = -(j * dH - y0) / H * scale;
                }
            }
            pictureBox1.Image = canvas;
        }
        
        private void button1_Click_1(object sender, EventArgs e)
        {
            t = t0 + h;

            fill_ak();
            fill_bk();
            fill_ck();

            print();

            canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(canvas);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            draw_trajectories();
            draw_vector_field();

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                a0 = double.Parse(textBox1.Text, System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0)
            {
                b0 = double.Parse(textBox2.Text, System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}