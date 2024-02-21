using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ImageViewer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SizeChanged += MainWindow_SizeChanged;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
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

                if (bitmap.PixelWidth <= ActualWidth || bitmap.PixelHeight <= ActualHeight)
                {
                    ImageViewer.Width = bitmap.PixelWidth;
                    ImageViewer.Height = bitmap.PixelHeight;
                }
                else
                {
                    double scaleX = ActualWidth / bitmap.PixelWidth;
                    double scaleY = ActualHeight / bitmap.PixelHeight;
                    double scale = Math.Min(scaleX, scaleY);

                    ImageViewer.Width = scale * bitmap.PixelWidth;
                    ImageViewer.Height = scale * bitmap.PixelHeight;

                }
                ImageViewer.Source = bitmap;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ImageViewer.Source != null)
            {
                BitmapImage bitmap = (BitmapImage)ImageViewer.Source;
                double imagePixelWidth = bitmap.PixelWidth;
                double imagePixelHeight = bitmap.PixelHeight;
                double windowWidth = ActualWidth;
                double windowHeight = ActualHeight;

                if (windowWidth < imagePixelWidth || windowHeight < imagePixelHeight)
                {
                    double scaleX = windowWidth / imagePixelWidth;
                    double scaleY = windowHeight / imagePixelHeight;
                    double scale = Math.Min(scaleX, scaleY);

                    ImageViewer.Width = scale * imagePixelWidth;
                    ImageViewer.Height = scale * imagePixelHeight;
                }
                else
                {
                    ImageViewer.Width = imagePixelWidth;
                    ImageViewer.Height = imagePixelHeight;
                }
            }
        }

    }
}
