using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DrawingApplication
{
    public partial class MainWindow : Window
    {
        private Point? lastPoint = null;
        private bool isDraw;
        public MainWindow()
        {
            InitializeComponent();
            SizeChanged += MainWindowSizeChanged;

            CreateBlankWhiteImage();
        }

        private void CreateBlankWhiteImage()
        {
            int width = 800;
            int height = 600;

            WriteableBitmap writeableBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);

            byte[] pixelData = new byte[width * height * 4];
            for (int i = 0; i < pixelData.Length; i += 4)
            {
                pixelData[i] = 255;
                pixelData[i + 1] = 255;
                pixelData[i + 2] = 255;
            }
            writeableBitmap.WritePixels(new Int32Rect(0, 0, width, height), pixelData, width * 4, 0);

            myImage.Width = width;
            myImage.Height = height;

            myImage.Source = writeableBitmap;
        }


        private void MainWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (myImage.Source != null && myImage.Source is WriteableBitmap)
            {
                WriteableBitmap writeableBitmap = (WriteableBitmap)myImage.Source;
                double imagePixelWidth = writeableBitmap.PixelWidth;
                double imagePixelHeight = writeableBitmap.PixelHeight;
                double windowWidth = ActualWidth;
                double windowHeight = ActualHeight;

                if (windowWidth < imagePixelWidth || windowHeight < imagePixelHeight)
                {
                    double scaleX = windowWidth / imagePixelWidth;
                    double scaleY = windowHeight / imagePixelHeight;
                    double scale = Math.Min(scaleX, scaleY);

                    myImage.Width = scale * imagePixelWidth;
                    myImage.Height = scale * imagePixelHeight;
                }
                else
                {
                    myImage.Width = imagePixelWidth;
                    myImage.Height = imagePixelHeight;
                }
            }
        }

        private void MainImageMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            lastPoint = e.GetPosition(myImage);
            isDraw = true;
        }
        private void MainImageMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            lastPoint = null;
            isDraw = false;
        }
        private void MainImageMouseMove(object sender, MouseEventArgs e)
        {
            if (isDraw && e.LeftButton == MouseButtonState.Pressed && lastPoint != null)
            {
                Point currentPoint = e.GetPosition(myImage);

                Point startPoint = new Point(((Point)lastPoint).X / myImage.ActualWidth * myImage.Source.Width,
                                              ((Point)lastPoint).Y / myImage.ActualHeight * myImage.Source.Height);
                Point endPoint = new Point(currentPoint.X / myImage.ActualWidth * myImage.Source.Width,
                                           currentPoint.Y / myImage.ActualHeight * myImage.Source.Height);

                DrawLine(startPoint, endPoint);
                lastPoint = currentPoint;
            }
        }
        private void DrawLine(Point startPoint, Point endPoint)
        {
            WriteableBitmap writeableBitmap = (WriteableBitmap)myImage.Source;
            int width = (int)writeableBitmap.Width;
            int height = (int)writeableBitmap.Height;
            int bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
            int stride = width * bytesPerPixel;

            int x0 = (int)startPoint.X;
            int y0 = (int)startPoint.Y;
            int x1 = (int)endPoint.X;
            int y1 = (int)endPoint.Y;

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            int bytes = stride * height;
            byte[] pixelData = new byte[bytes];
            writeableBitmap.CopyPixels(pixelData, stride, 0);

            while (true)
            {
                int index = bytesPerPixel * (x0 + y0 * width);
                pixelData[index] = 0; // Blue
                pixelData[index + 1] = 0; // Green
                pixelData[index + 2] = 0; // Red

                if (x0 == x1 && y0 == y1)
                    break;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }

            writeableBitmap.WritePixels(new Int32Rect(0, 0, width, height), pixelData, stride, 0);
            myImage.Source = writeableBitmap;
        }

        private void OpenFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                LoadImage(openFileDialog.FileName);
            }
        }

        private void LoadImage(string filePath)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage(new Uri(filePath));
                WriteableBitmap writeableBitmap = new WriteableBitmap(bitmap);

                if (bitmap.PixelWidth <= ActualWidth || bitmap.PixelHeight <= ActualHeight)
                {
                    myImage.Width = bitmap.PixelWidth;
                    myImage.Height = bitmap.PixelHeight;
                }
                else
                {
                    double scaleX = ActualWidth / bitmap.PixelWidth;
                    double scaleY = ActualHeight / bitmap.PixelHeight;
                    double scale = Math.Min(scaleX, scaleY);

                    myImage.Width = scale * bitmap.PixelWidth;
                    myImage.Height = scale * bitmap.PixelHeight;
                }
                myImage.Source = writeableBitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearImage()
        {
            WriteableBitmap writeableBitmap = (WriteableBitmap)myImage.Source;
            int width = (int)writeableBitmap.Width;
            int height = (int)writeableBitmap.Height;
            int bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
            int stride = width * bytesPerPixel;

            int bytes = stride * height;
            byte[] pixelData = new byte[bytes];

            for (int i = 0; i < pixelData.Length; i += bytesPerPixel)
            {
                pixelData[i] = 255;
                pixelData[i + 1] = 255;
                pixelData[i + 2] = 255;
            }

            writeableBitmap.WritePixels(new Int32Rect(0, 0, width, height), pixelData, stride, 0);
            myImage.Source = writeableBitmap;
        }


        private void NewImageClick(object sender, RoutedEventArgs e)
        {
            ClearImage();
        }
    }
}
