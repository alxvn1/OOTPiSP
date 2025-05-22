using System.Windows.Media;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace WpfGraphicsApp.Shapes
{
    public abstract class ShapeBase
    {
        [JsonIgnore]
        public Brush Stroke { get; set; } = Brushes.Black;
        
        [JsonIgnore]
        public Brush Fill { get; set; } = Brushes.Transparent;
        
        public double StrokeThickness { get; set; } = 1;

        // Сериализуемые свойства для цвета
        public string StrokeColor
        {
            get => (Stroke as SolidColorBrush)?.Color.ToString();
            set => Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
        }

        public string FillColor
        {
            get => (Fill as SolidColorBrush)?.Color.ToString();
            set => Fill = string.IsNullOrEmpty(value) 
                ? Brushes.Transparent 
                : new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
        }

        public abstract Shape Draw();
        public abstract string GetShapeType();
    }
}