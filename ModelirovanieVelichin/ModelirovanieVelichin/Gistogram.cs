using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModelirovanieVelichin
{
    public partial class Gistogram : Form
    {
        public float[] g;
        public float[] z;
        bool num;
        public Gistogram()
        {
            InitializeComponent();
        }
        public Gistogram(float a, bool numberlab)
        {
            InitializeComponent();
            num = numberlab;
            int N = (int)a;
            g = new float[N];
            z = new float[N];
            for (int i = 0; i < N; i++)
            {
                g[i] = i; z[i] = i;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string s = Convert.ToString(textBox1.Text);
            int n = 1;
            for (int i = 0; i < s.Length; i++)
                if (s[i] == ' ')
                    n++;
            string k = "";
            int count = 0;
            if (num)
            {
                g = new float[n];
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == ' ')
                    {
                        g[count++] = (float)Convert.ToDouble(k);
                        k = "";
                    }
                    else
                        k += s[i];
                }
                g[count++] = (float)Convert.ToDouble(k);
            }
            else
            {
                z = new float[n];
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == ' ')
                    {
                        z[count++] = (float)Convert.ToDouble(k);
                        k = "";
                    }
                    else
                        k += s[i];
                }
                z[count++] = (float)Convert.ToDouble(k);
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int N = Convert.ToInt16(textBox2.Text);
            float A = (float)Convert.ToDouble(textBox3.Text);
            float B = (float)Convert.ToDouble(textBox4.Text);
            if (num)
            {
                g = new float[N];
                for (int i = 0; i < N; i++)
                    g[i] = A + (B - A) / N * i;
            }
            else 
            {
                z = new float[N];
                for (int i = 0; i < N; i++)
                    z[i] = A + (B - A) / N * i;
            }
                Close();
        }

        private void Gistogram_Load(object sender, EventArgs e)
        {

        }
    }
}
