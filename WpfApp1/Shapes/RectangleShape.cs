using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace WpfGraphicsApp.Shapes
{
    public class RectangleShape : ShapeBase
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        // Сериализуемые свойства цвета
        public string StrokeColor
        {
            get => Stroke?.ToString() ?? Brushes.Black.ToString();
            set => Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
        }

        public string FillColor
        {
            get => Fill?.ToString() ?? Brushes.Transparent.ToString();
            set => Fill = string.IsNullOrEmpty(value) 
                ? Brushes.Transparent 
                : new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
        }

        public override Shape Draw()
        {
            var rect = new Rectangle
            {
                Width = Width,
                Height = Height,
                Stroke = Stroke,
                Fill = Fill,
                StrokeThickness = StrokeThickness
            };
    
            Canvas.SetLeft(rect, X);
            Canvas.SetTop(rect, Y);
    
            return rect;
        }

        public override string GetShapeType() => "Rectangle";
        public override ShapeBase GetInstance()
        {
            throw new NotImplementedException();
        }
    }
}