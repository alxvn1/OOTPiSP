using System.Windows;
using System.Windows.Controls;
using WpfGraphicsApp.Shapes; // Добавьте эту директиву
using System.Windows.Media;

namespace WpfGraphicsApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Очистка Canvas
        private void ClearCanvas()
        {
            DrawingCanvas.Children.Clear();
        }

        // Обработчик для кнопки "Многоугольник"
        private void DrawPolygon_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvas();

            PolygonInputDialog dialog = new PolygonInputDialog { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                var polygon = new PolygonShape { Sides = dialog.NumberOfSides };
                DrawingCanvas.Children.Add(polygon.Draw());
            }
        }

        // Обработчик для кнопки "Квадрат"
        private void DrawSquare_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvas();
            var square = new RectangleShape { Width = 100, Height = 100, Fill = Brushes.LightGreen };
            DrawingCanvas.Children.Add(square.Draw());
        }

        // Обработчик для кнопки "Прямоугольник"
        private void DrawRectangle_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvas();
            var rectangle = new RectangleShape { Width = 150, Height = 100, Fill = Brushes.LightCoral };
            DrawingCanvas.Children.Add(rectangle.Draw());
        }

        // Обработчик для кнопки "Эллипс"
        private void DrawEllipse_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvas();
            var ellipse = new EllipseShape { Width = 150, Height = 100, Fill = Brushes.LightYellow };
            DrawingCanvas.Children.Add(ellipse.Draw());
        }

        // Обработчик для кнопки "Отрезок"
        private void DrawLine_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvas();
            var line = new LineShape();
            DrawingCanvas.Children.Add(line.Draw());
        }

        // Обработчик для кнопки "Ломаная"
        private void DrawPolyline_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvas();
            var polyline = new PolylineShape();
            DrawingCanvas.Children.Add(polyline.Draw());
        }
    }
}