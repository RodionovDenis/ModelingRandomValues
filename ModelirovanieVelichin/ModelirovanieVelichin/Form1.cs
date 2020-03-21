using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace ModelirovanieVelichin
{
    public partial class Form1 : Form
    {
        float[] xvalue; 
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            float q; //вспомагательная переменная
            float a = float.Parse(textBox1.Text);
            int n = int.Parse(textBox2.Text);

            xvalue = new float[n];
            Point point = new Point();
            for(int i = 0; i< n; i++)
            {
                point.GetRandomPoint(a);
                q = point.RandonVariable();
                dataGridView1.Rows.Add(q, i + 1);
                xvalue[i] = q;
            }
            dataGridView1.Sort(dataGridView1.Columns["x"], ListSortDirection.Ascending);
            Array.Sort(xvalue);

            //подсчет значений характеристик

            //выборочное среднее

            float x_ = xvalue.Sum() / (float)n;
            dataGridView2.Rows[0].Cells[1].Value = x_;

            //выборочная дисперсия

            float s2 = 0;
            for (int i = 0; i < n; i++)
                s2 += (xvalue[i] - x_) * (xvalue[i] - x_);
            s2 = s2 / (float)n;
            dataGridView2.Rows[0].Cells[4].Value = s2;

            //размах выборки
            float r = xvalue[n - 1] - xvalue[0];
            dataGridView2.Rows[0].Cells[7].Value = r;

            //выборочная медиана
            float me;
            if (n % 2 == 1)
                me = xvalue[n / 2];
            else
                me = (xvalue[n / 2 - 1] + xvalue[n / 2]) / 2f;
            dataGridView2.Rows[0].Cells[6].Value = me;

            //вычисление мат.ожидания и дисперсии и помощью метода прямоугольников
            float x0 = 0; float xn = a / (float)Math.Sqrt(3); //пределы интегрирования
            float h = (xn - x0) / 200;
            float M = 0; float D = 0; //значение интеграла

            for (float i = x0; i < xn; i += h)
                M += h * (i + h / 2f) * 
                    point.f(i + h / 2f, a); //длина промежутка на значение точки в промежуточной точки
            dataGridView2.Rows[0].Cells[0].Value = M;

            for (float i = x0; i < xn; i += h)
                D += h * (i + h / 2f - M) * (i + h / 2f - M) *
                    point.f(i + h / 2f, a); //длина промежутка на значение точки в промежуточной точки
            dataGridView2.Rows[0].Cells[3].Value = D;

            //вычисление разностей

            dataGridView2.Rows[0].Cells[2].Value = Math.Abs(M - x_);
            dataGridView2.Rows[0].Cells[5].Value = Math.Abs(D - s2);



        }
    }
}
