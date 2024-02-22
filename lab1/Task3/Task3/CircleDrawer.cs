using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Runtime.ConstrainedExecution;
using System.ComponentModel.DataAnnotations;

namespace Task3
{

    internal class CircleDrawer
    {
        public void CanvasMouseLeftButtonDown(Canvas canvas, object sender, MouseButtonEventArgs e)
        {
            Point pointClicked = e.GetPosition(canvas);

            double Xc = pointClicked.X, Yc = pointClicked.Y, Pix = Constants.Pix;
            double R = ParseR(ButtonClick());
            DrawCircle(canvas, Xc, Yc, R, Pix);
        }
        // класс рисующий окружность не должен выводить окон
        private double ParseR(double R)
        {
            double max = Math.Max(Constants.CanvasHeight, Constants.CanvasWidth);

            if (R > max)
            {
                return 0;
            }

            return R;
        }

        private double ButtonClick()
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Введите число:", "Ввод числа", "");

            double result;
            try
            {
                result = Convert.ToDouble(input);
            }
            catch (FormatException)
            {
                MessageBox.Show("Некорректный формат ввода. Введите число.");
                result = 0;
            }
            return result;
        }

        private void DrawCircle(Canvas canvas, double Xc, double Yc, double R, double Pix)
        {
            //DrawBRcircle(canvas, Xc, Yc, R, Pix);
            VMIcirc(canvas, Xc, Yc, R, Pix);
        }
        // рисует неправильно для маленьких значений
        // реализовать миченра и брезенхема, проверить какой болле правильный
        // goto удалтиь
        private void DrawBRcircle(Canvas canvas, double xc, double yc, double r, double pixel)
        {
            double x = 0, y = r, z, Dd = 0;
            while (x < y)
            {

                PixelСircle(canvas, xc, yc, x, y, pixel);
                if (Dd == 0)
                    goto Pd;
                z = 2 * Dd - 1;
                if (Dd > 0)
                {
                    if (z + 2 * x <= 0)
                        goto Pd;
                    else
                        goto Pv;
                }
                if (z + 2 * y > 0)
                    goto Pd;
                Pg:
                ++x;
                Dd = Dd + 2 * x + 1;
                continue;
            Pd:
                ++x;
                --y;
                Dd = Dd + 2 * (x - y + 1);
                continue;
            Pv:
                --y;
                Dd = Dd - 2 * y + 1;
            }
            if (x == y)
                PixelСircle(canvas, xc, yc, x, y, pixel);
        }

        private bool IsPointInsideCanvas(Canvas canvas, double x, double y)
        {
            return x >= 0 && x < Constants.CanvasWidth && y >= 0 && y < Constants.CanvasHeight;
        }

        private void PixelСircle(Canvas canvas, double xc, double yc, double x, double y, double pixel)
        {
            DrawPixel(canvas, xc + x, yc + y, pixel);
            DrawPixel(canvas, xc + y, yc + x, pixel);
            DrawPixel(canvas, xc - x, yc + y, pixel);
            DrawPixel(canvas, xc - y, yc + x, pixel);
            DrawPixel(canvas, xc + x, yc - y, pixel);
            DrawPixel(canvas, xc + y, yc - x, pixel);
            DrawPixel(canvas, xc - x, yc - y, pixel);
            DrawPixel(canvas, xc - y, yc - x, pixel);
        }
        private void DrawPixel(Canvas canvas, double x, double y, double pixel)
        {
            if (!IsPointInsideCanvas(canvas, x, y))
            {
                return;
            }
            Rectangle rect = new Rectangle
            {
                Width = 1,
                Height = 1,
                Fill = new SolidColorBrush(Color.FromRgb(255, 0, 255))
            };
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            canvas.Children.Add(rect);
        }

        void VMIcirc(Canvas canvas, double xc, double yc, double r, double pixel)
        {
            double x = 0, y = r, d = 3 - 2 * r;

            while (x < y)
            {
                PixelСircle(canvas, xc, yc, x, y, pixel);

                if (d < 0)
                {
                    d += 4 * x + 6;
                }
                else
                {
                    d += 4 * (x - y) + 10;
                    y--;
                }

                x++;
            }

            if (x == y)
            {
                PixelСircle(canvas, xc, yc, x, y, pixel);
            }
        }

    }
}
