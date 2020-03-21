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
            float k = (1.0f + 2.0f * a) / (a * (float)(Math.Sqrt(3)));
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
   }
}