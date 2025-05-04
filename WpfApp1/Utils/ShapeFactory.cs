using System;
using System.Collections.Generic;
using WpfGraphicsApp.Shapes;

namespace WpfGraphicsApp.Factories
{
    public class ShapeFactory
    {
        private readonly Dictionary<string, Func<ShapeBase>> _shapeCreators = 
            new Dictionary<string, Func<ShapeBase>>();

        public void RegisterShape(string shapeType, Func<ShapeBase> creator)
        {
            _shapeCreators[shapeType] = creator;
        }

        public ShapeBase CreateShape(string shapeType)
        {
            if (_shapeCreators.TryGetValue(shapeType, out var creator))
            {
                return creator();
            }
            throw new ArgumentException($"Unknown shape type: {shapeType}");
        }

        public IEnumerable<string> AvailableShapes => _shapeCreators.Keys;
    }
}