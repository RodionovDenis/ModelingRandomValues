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
        Gistogram Gistogram = new Gistogram(5, true);
        Gistogram Zsegment = new Gistogram(5, false);
        Point point;
        public Form1()
        {
            InitializeComponent();
            //
            //заполнение таблички
            //
            // 
            dataGridView3.Rows.Add();
            dataGridView3.Rows.Add();
            dataGridView3.Rows[0].Cells[0].Value = "z_j";
            dataGridView3.Rows[1].Cells[0].Value = "f(z_j)";
            dataGridView3.Rows[2].Cells[0].Value = "g(z_j)";
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
            point = new Point();
            for (int i = 0; i < n; i++)
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
            float h = (xn - x0) / 500;
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

            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl2.GraphPane.CurveList.Clear();


            ZedGraph.PointPairList list1 = new ZedGraph.PointPairList(); //функция распределения
            ZedGraph.PointPairList list2 = new ZedGraph.PointPairList(); //выборочная функция
            ZedGraph.PointPairList list3 = new ZedGraph.PointPairList(); //плотность
            ZedGraph.PointPairList list4 = new ZedGraph.PointPairList(); //гистограмма

            float A = a / (float)Math.Sqrt(3) + 0.1f;

            for (float x = 0; x <= A; x += h)
            {
                list1.Add(x, point.F(x, a));
                list2.Add(x, point.F_(x, a, xvalue));
                list3.Add(x, point.f(x, a));
            }
            //цикл для гистограммы
            int count = 0; int k = 0;
            list4.Add(Gistogram.g[0], 0); //начальное построение
            while (k < Gistogram.g.Length - 1)
            {
                if (count % 3 == 0)
                {
                    list4.Add(Gistogram.g[k], point.Histogram(Gistogram.g[k], xvalue, Gistogram.g));
                }
                else if (count % 3 == 1)
                {
                    list4.Add((Gistogram.g[k] + Gistogram.g[k + 1]) / 2, point.Histogram((Gistogram.g[k] + Gistogram.g[k + 1]) / 2, xvalue, Gistogram.g));
                    list4.Add(Gistogram.g[k + 1], point.Histogram((Gistogram.g[k] + Gistogram.g[k + 1]) / 2, xvalue, Gistogram.g));
                    k++;
                }
                else
                {
                    list4.Add(Gistogram.g[k], 0);
                }
                count++;
            }
            list4.Add(Gistogram.g[Gistogram.g.Length - 1], 0); //конечное построение

            ZedGraph.CurveItem curve1 = zedGraphControl1.GraphPane.AddCurve("F(x)", list1, Color.Blue, SymbolType.None);
            ZedGraph.CurveItem curve2 = zedGraphControl1.GraphPane.AddCurve("F_(x)", list2, Color.Red, SymbolType.None);

            ZedGraph.CurveItem curve3 = zedGraphControl2.GraphPane.AddCurve("f(x)", list3, Color.Blue, SymbolType.None);
            ZedGraph.CurveItem curve4 = zedGraphControl2.GraphPane.AddCurve("Гистограмма", list4, Color.Red, SymbolType.None);

            //удаление предыдущего
            int qwerty = dataGridView3.ColumnCount;
            if (dataGridView3.ColumnCount != 1)
                for (int i = 0; i < qwerty - 1; i++)
                    dataGridView3.Columns.RemoveAt(1);

            //заполнение таблицы

            float eps = 0;

            for (int i = 0; i < Gistogram.g.Length - 1; i++)
            {
                float value = (Gistogram.g[i] + Gistogram.g[i + 1]) / 2f;
                dataGridView3.Columns.Add("z_" + Convert.ToString(i + 1), "z_" + Convert.ToString(i + 1));
                dataGridView3.Rows[0].Cells[i + 1].Value = value;
                dataGridView3.Rows[1].Cells[i + 1].Value = point.f(value, a);
                dataGridView3.Rows[2].Cells[i + 1].Value = point.Histogram(value, xvalue, Gistogram.g);
                float error = Math.Abs(point.Histogram(value, xvalue, Gistogram.g) - point.f(value, a));
                if (error > eps)
                    eps = error;
            }

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();

            zedGraphControl2.AxisChange();
            zedGraphControl2.Invalidate();

            pictureBox1.Visible = true;
            label5.Text = " = " + Convert.ToString(point.D(xvalue, a));
            label9.Text = Convert.ToString(eps);

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1_Click_1(sender, e);
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            float a = float.Parse(textBox1.Text);

            Gistogram.ShowDialog();
            zedGraphControl2.GraphPane.CurveList.RemoveAt(1);

            int count = 0; int k = 0;
            ZedGraph.PointPairList list4 = new ZedGraph.PointPairList(); //гистограмма
            list4.Add(Gistogram.g[0], 0); //начальное построение
            while (k < Gistogram.g.Length - 1)
            {
                if (count % 3 == 0)
                {
                    list4.Add(Gistogram.g[k], point.Histogram(Gistogram.g[k], xvalue, Gistogram.g));
                }
                else if (count % 3 == 1)
                {
                    list4.Add((Gistogram.g[k] + Gistogram.g[k + 1]) / 2, point.Histogram((Gistogram.g[k] + Gistogram.g[k + 1]) / 2, xvalue, Gistogram.g));
                    list4.Add(Gistogram.g[k + 1], point.Histogram((Gistogram.g[k] + Gistogram.g[k + 1]) / 2, xvalue, Gistogram.g));
                    k++;
                }
                else
                {
                    list4.Add(Gistogram.g[k], 0);
                }
                count++;
            }
            list4.Add(Gistogram.g[Gistogram.g.Length - 1], 0); //конечное построение

            ZedGraph.CurveItem curve4 = zedGraphControl2.GraphPane.AddCurve("Гистограмма", list4, Color.Red, SymbolType.None);


            //удаление предыдущего
            int qwerty = dataGridView3.ColumnCount;
            if (dataGridView3.ColumnCount != 1)
                for (int i = 0; i < qwerty - 1; i++)
                    dataGridView3.Columns.RemoveAt(1);

            //заполнение таблицы

            float eps = 0;

            for (int i = 0; i < Gistogram.g.Length - 1; i++)
            {
                float value = (Gistogram.g[i] + Gistogram.g[i + 1]) / 2f;
                dataGridView3.Columns.Add("z_" + Convert.ToString(i + 1), "z_" + Convert.ToString(i + 1));
                dataGridView3.Rows[0].Cells[i + 1].Value = value;
                dataGridView3.Rows[1].Cells[i + 1].Value = point.f(value, a);
                dataGridView3.Rows[2].Cells[i + 1].Value = point.Histogram(value, xvalue, Gistogram.g);
                float error = Math.Abs(point.Histogram(value, xvalue, Gistogram.g) - point.f(value, a));
                if (error > eps)
                    eps = error;
            }

            zedGraphControl2.AxisChange();
            zedGraphControl2.Invalidate();

            label9.Text = Convert.ToString(eps);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            button1_Click_1(sender, e);
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            float a = float.Parse(textBox1.Text);
            point = new Point();

            Zsegment.ShowDialog();
                   
        }
        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView4.Rows.Clear();
            float a = float.Parse(textBox1.Text);
            point = new Point();

            int len = Zsegment.z.Length;
            float h = (float)((Zsegment.z[len - 1] - Zsegment.z[0]) / (100 * a));
            float[] q = new float[len - 1];
            //теоретические вероятности
            for (int i = 0; i < len - 1; i++)
            {
                q[i] = point.F(Zsegment.z[i + 1], a) - point.F(Zsegment.z[i], a);
                dataGridView4.Rows.Add(i + 1, q[i]);
            }
            label16.Text = Convert.ToString(point.R0(xvalue, Zsegment.z, q));
            float FR0 = point.FR0((float)Convert.ToDouble(label16.Text), q.Length);
            label17.Text = "= " + Convert.ToString(FR0);
            float alfa = (float)Convert.ToDouble(textBox3.Text);
            if (alfa > FR0)
                label18.Text = "Гипотеза не принята!";
            else
                label18.Text = "Гипотеза принята!";

        }

        private void button7_Click(object sender, EventArgs e)
        {
            button1_Click_1(sender, e);
        }
    }
}