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


namespace Task3
{

    public static class Constants
    {
        public const double CanvasWidth = 250;
        public const double CanvasHeight = 600;
        public static readonly Brush CanvasBrush = Brushes.Black;
        public const double CanvasLeft = 0;
        public const double CanvasTop = 0;
        public const double Pix = 14;
    }
    public partial class MainWindow : Window
    {
        private CircleDrawer circleDrawer = new CircleDrawer();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindowLoaded;
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            InitCanvas();
            canvas.MouseLeftButtonDown += (sender, e) => circleDrawer.CanvasMouseLeftButtonDown(canvas, sender, e);
        }


        private void InitCanvas()
        {
            Rectangle whiteSquare = new Rectangle
            {
                Width = Constants.CanvasWidth,
                Height = Constants.CanvasHeight,
                Fill = Constants.CanvasBrush
            };
            Canvas.SetLeft(whiteSquare, Constants.CanvasLeft);
            Canvas.SetTop(whiteSquare, Constants.CanvasTop);
            canvas.Children.Add(whiteSquare);
        }
    }
}
