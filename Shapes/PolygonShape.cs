using System;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Collections.Generic;

namespace WpfGraphicsApp.Shapes
{
    internal class PolygonShape : ShapeBase
    {
        public int Sides { get; set; } = 3;
        public PointCollection Points { get; set; } = new PointCollection();

        public override Shape Draw()
        {
            Polygon polygon = new Polygon
            {
                Stroke = Stroke,
                Fill = Fill,
                StrokeThickness = StrokeThickness,
                Points = this.Points
            };

            // Если Points пуст, создаем правильный многоугольник
            if (Points.Count == 0)
            {
                double centerX = 200, centerY = 200, radius = 100;
                for (int i = 0; i < Sides; i++)
                {
                    double angle = 2 * Math.PI * i / Sides;
                    double x = centerX + radius * Math.Cos(angle);
                    double y = centerY + radius * Math.Sin(angle);
                    polygon.Points.Add(new Point(x, y));
                }
            }

            return polygon;
        }

        public override string GetShapeType()
        {
            return "Polygon";
        }
    }
}