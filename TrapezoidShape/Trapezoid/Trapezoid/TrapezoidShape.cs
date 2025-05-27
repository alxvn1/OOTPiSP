
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Newtonsoft.Json;
using WpfGraphicsApp.Shapes;

public class TrapeziumShape : ShapeBase
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double TopWidth { get; set; } = 50;

    // Сериализуемые свойства цвета
    public string StrokeColor
    {
        get => (Stroke as SolidColorBrush)?.Color.ToString() ?? Colors.Black.ToString();
        set => Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
    }

    public string FillColor
    {
        get => (Fill as SolidColorBrush)?.Color.ToString() ?? Colors.Transparent.ToString();
        set => Fill = string.IsNullOrEmpty(value) 
            ? Brushes.Transparent 
            : new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
    }

    public override Shape Draw()
    {
        var polygon = new Polygon
        {
            Stroke = this.Stroke,
            Fill = this.Fill,
            StrokeThickness = this.StrokeThickness
            
        };
    
        double leftOffset = (Width - TopWidth) / 2;
        polygon.Points = new PointCollection
        {
            new Point(X + leftOffset, Y),
            new Point(X + leftOffset + TopWidth, Y),
            new Point(X + Width, Y + Height),
            new Point(X, Y + Height)
        };

        return polygon;
    }


    public override string GetShapeType() => "Trapezium";

    public override ShapeBase GetInstance()
    {
        return new TrapeziumShape
        {
            X = this.X,
            Y = this.Y,
            Width = this.Width,
            Height = this.Height,
            TopWidth = this.TopWidth,
            Stroke = this.Stroke?.Clone(),
            Fill = this.Fill?.Clone(),
            StrokeThickness = this.StrokeThickness
        };
    }
}

// using System.Diagnostics;
// using System.Windows;
// using System.Windows.Media;
// using System.Windows.Shapes;
// using Newtonsoft.Json;
// using WpfGraphicsApp.Shapes;
//
// public class TrapeziumShape : ShapeBase
// {
//     public double X { get; set; }
//     public double Y { get; set; }
//     public double Width { get; set; }
//     public double Height { get; set; }
//     public double TopWidth { get; set; } = 50;
//
//     [JsonIgnore]
//     public Brush Stroke { get; set; } = Brushes.Black;
//     
//     [JsonIgnore]
//     public Brush Fill { get; set; } = Brushes.Transparent;
//     
//     public string StrokeColor
//     {
//         get => (Stroke as SolidColorBrush)?.Color.ToString();
//         set => Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
//     }
//
//     public string FillColor
//     {
//         get => (Fill as SolidColorBrush)?.Color.ToString();
//         set => Fill = string.IsNullOrEmpty(value) 
//             ? Brushes.Transparent 
//             : new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
//     }
//
//     public override Shape Draw()
//     {
//         var polygon = new Polygon
//         {
//             Stroke = this.Stroke,
//             Fill = this.Fill,
//             StrokeThickness = this.StrokeThickness
//         };
//         
//         double leftOffset = (Width - TopWidth) / 2;
//         polygon.Points = new PointCollection
//         {
//             new Point(X + leftOffset, Y),
//             new Point(X + leftOffset + TopWidth, Y),
//             new Point(X + Width, Y + Height),
//             new Point(X, Y + Height)
//         };
//
//         return polygon;
//     }
//
//     public override string GetShapeType() => "Trapezium";
//
//     public override ShapeBase GetInstance()
//     {
//         return new TrapeziumShape
//         {
//             X = this.X,
//             Y = this.Y,
//             Width = this.Width,
//             Height = this.Height,
//             TopWidth = this.TopWidth,
//             Stroke = this.Stroke?.Clone(),
//             Fill = this.Fill?.Clone(),
//             StrokeThickness = this.StrokeThickness
//         };
//     }
// }

