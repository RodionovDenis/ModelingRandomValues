using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelirovanieVelichin
{
    class Point
    {
        public float x = 0; // координата по х
        public float y = 0; //координата по у
        public Random random = new Random(); //случайное число

        public bool CheckCondition(float a) //проверка на принадлежность точки треугольнику
        {
            //y=kx+b
            float k = 3f / (float)(Math.Sqrt(3));
            float b = a / (float)(Math.Sqrt(3));
            if (y <= k * x + b && y <= -k * x + b)
                return true;
            return false;
        }
        public void GetRandomPoint(float a) //а - сторона треугольника
        {

            //рандом берется в прямоугольнике, сторона которого совпадает с горизонтальной стороной треугольника

            float xmax = 0.5f * a;
            float xmin = -xmax;
            float ymax = a / (float)(Math.Sqrt(3));
            float ymin = -ymax * 0.5f;
            x = (float)random.NextDouble() * (xmax - xmin) + xmin;
            y = (float)random.NextDouble() * (ymax - ymin) + ymin;

            if (!CheckCondition(a)) //проверка на принадлежность треугольнику
                this.GetRandomPoint(a);
        }
        public float RandonVariable()
        {
            return (float)(Math.Sqrt(x * x + y * y)); //теорема пифагора, начало координат в центре
        }
        public float F(float x, float a)
        {
            if (x <= 0)
                return 0;
            else if (x <= a / (2f * Math.Sqrt(3)))
                return (float)((4f * Math.PI / Math.Sqrt(3)) * (x / a) * (x / a));
            else if (x <= a / (Math.Sqrt(3)))
            {
                float b = (float)(Math.Sqrt(x * x - 1f / 12f * a * a));
                return (float)((4f / (a * a * Math.Sqrt(3))) * (x * x * (Math.PI - 3 * Math.Asin(b / x)) +
                    Math.Sqrt(3) / 2f * a * b));
            }
            else
                return 1;
        }
        public float f(float x, float a)
        {
            if (x <= 0)
                return 0;
            else if (x <= a / (2f * Math.Sqrt(3)))
                return (float)(4f * Math.PI * 2f * x / (Math.Sqrt(3) * a * a));
            else if (x <= a / (Math.Sqrt(3)))
            {
                float b = (float)(Math.Sqrt(x * x - 1f / 12f * a * a));
                float s1 = (float)(Math.PI - 3 * Math.Asin(b / x));
                float s2 = (float)(3 * a * a / (12 * Math.Sqrt(1 - b * b / (x * x)) * b));
                float s3 = (float)(Math.Sqrt(3) * a * x / (2f * b));
                return (float)(4f / (a * a * Math.Sqrt(3)) * (2 * x * s1 - s2 + s3));
            }
            else
                return 0;
        }
        public float F_(float x, float a, float[] xvalue) //выборочная функция распределения
        {
            int n = xvalue.Length;
            //считаем, что значения xvalue - отсортированные
            if (x <= 0)
                return 0;
            else if (x > a / Math.Sqrt(3))
                return 1;
            for (int i = 0; i < n; i++)
                if (xvalue[i] >= x)
                    return i/ (float)n;
            return 1;
        }
        public float D(float[] xvalue, float a) // величина d
        {
            int n = xvalue.Length;
            //считаем, что значения xvalue - отсортированные
            float dvalue; float dleft; float dright; float dvaluemax = 0;
            for (int j = 0; j < n; j++)
            {
                dleft = (j + 1) / (float)n - F(xvalue[j], a);
                dright = F(xvalue[j], a) - j / (float)(n);
                dvalue = (dleft >= dright) ? dleft : dright;
                if (dvalue > dvaluemax)
                    dvaluemax = dvalue;
            }
            return dvaluemax;
        }
        public float Histogram(float x, float[] xvalue, float[] histogram) //построение гистограммы
        {
            //xvalue и histogram отсортированны
            //находим границу в массиве histogram, которому принадлежит x
            int i; int count = 0;
            for(i=0; i < histogram.Length; i++)
            {
                if (histogram[i] > x)
                    break;
            }
            // i - правая граница x, i - 1 левая(если существует)
            if (i == 0)
                return 0; //x не принадлежит никакому из указанных промежутков
            else
            {
                //если принадлежит, найдем число n_j
                for(int j = 0; j < xvalue.Length; j++)
                {
                    if (xvalue[j] < histogram[i - 1])
                        continue;
                    if (xvalue[j] > histogram[i])
                        break;
                    count++; //это то самое нужное n_j
                }
                return count / (xvalue.Length * (histogram[i] - histogram[i - 1])); 
            }
        }
        private int countX(float[] xvalue, float a, float b)
        {
            int count = 0;
            for (int i = 0; i < xvalue.Length; i++)
            {
                if (xvalue[i] >= a && xvalue[i] <= b)
                    count++;
            }
            return count;
        }
        public float R0(float [] xvalue, float[] z, float[] q)
        {
            float R0 = 0;
            //все значения отсортированны
            for (int i = 0; i < z.Length - 1; i++)
            {
                float countx = countX(xvalue, z[i], z[i + 1]);
                float value = xvalue.Length * q[i];
                R0 += (float)(Math.Pow(countx - value, 2) / value);
            }
            return R0;
        }
        private int Factorial(int n)
        {
            int factorial = 1;   
            for (int i = 2; i <= n; i++) 
                factorial = factorial * i;
            return factorial;
        }
        private float Gamma(int k)
        {
            float gamma = 1;
            if (k % 2 == 0)
                return Factorial(k / 2 - 1);
            else
            {
                if (k == 1)
                    return (float)Math.Sqrt(Math.PI);
                gamma = (k*0.5f-1f)*Gamma(k - 2);
            }
            return gamma;
        }
        private float f_x2(float x, int k)
        {
            if (x <= 0)
                return 0;
            else
            {
                float value = (float)(Math.Pow(2, -k * 0.5f) * Math.Pow(Gamma(k), -1) * Math.Pow(x, k * 0.5f - 1) 
                    * Math.Exp(-x * 0.5));
                return value;
            }
        }
        public float FR0(float R0, int k)
        {
            float h = R0 / 500;

            float FR0 = 0;
            for (float i = 0; i < R0; i += h)
                FR0 += h * f_x2(i + h / 2, k);
            return 1 - FR0;
        }
    }
}