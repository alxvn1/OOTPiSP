using System.Windows.Shapes;

namespace WpfGraphicsApp.Shapes
{
    internal class LineShape : ShapeBase
    {
        public double X1 { get; set; } = 100;
        public double Y1 { get; set; } = 100;
        public double X2 { get; set; } = 300;
        public double Y2 { get; set; } = 200;

        public override System.Windows.Shapes.Shape Draw()
        {
            return new Line
            {
                X1 = X1,
                Y1 = Y1,
                X2 = X2,
                Y2 = Y2,
                Stroke = Stroke,
                StrokeThickness = StrokeThickness
            };
        }

        public override string GetShapeType()
        {
            return "Line"; // Реализуем абстрактный метод
        }
    }
}