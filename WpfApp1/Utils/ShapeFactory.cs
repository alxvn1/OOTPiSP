// using System;
// using System.Collections.Generic;
// using WpfGraphicsApp.Shapes;
// using WpfGraphicsApp.Tools;
//
// namespace WpfGraphicsApp.Factories
// {
//     public class ShapeFactory
//     {
//         private readonly Dictionary<string, Func<ShapeBase>> _shapeCreators = 
//             new Dictionary<string, Func<ShapeBase>>();
//
//         private readonly Dictionary<string, Func<DrawingTool>> _toolCreators =
//             new Dictionary<string, Func<DrawingTool>>();
//
//         public void RegisterShape(string shapeType, Func<ShapeBase> shapeCreator, Func<DrawingTool> toolCreator)
//         {
//             _shapeCreators[shapeType] = shapeCreator;
//             _toolCreators[shapeType] = toolCreator;
//         }
//
//         public ShapeBase CreateShape(string shapeType)
//         {
//             if (_shapeCreators.TryGetValue(shapeType, out var creator))
//             {
//                 return creator();
//             }
//             throw new ArgumentException($"Unknown shape type: {shapeType}");
//         }
//
//         public DrawingTool CreateTool(string shapeType)
//         {
//             if (_toolCreators.TryGetValue(shapeType, out var creator))
//             {
//                 return creator();
//             }
//             throw new ArgumentException($"Unknown shape type: {shapeType}");
//         }
//
//         public IEnumerable<string> AvailableShapes => _shapeCreators.Keys;
//     }
// }
using System;
using System.Collections.Generic;
using WpfGraphicsApp.Shapes;
using WpfGraphicsApp.Tools;

namespace WpfGraphicsApp.Factories
{
    public class ShapeFactory
    {
        private readonly Dictionary<string, Func<ShapeBase>> _shapeCreators = 
            new Dictionary<string, Func<ShapeBase>>();

        private readonly Dictionary<string, Func<DrawingTool>> _toolCreators =
            new Dictionary<string, Func<DrawingTool>>();

        public void RegisterShape(string shapeType, Func<ShapeBase> shapeCreator, Func<DrawingTool> toolCreator)
        {
            _shapeCreators[shapeType] = shapeCreator;
            _toolCreators[shapeType] = toolCreator;
        }

        public ShapeBase CreateShape(string shapeType)
        {
            if (_shapeCreators.TryGetValue(shapeType, out var creator))
            {
                return creator();
            }
            throw new ArgumentException($"Unknown shape type: {shapeType}");
        }

        public DrawingTool CreateTool(string shapeType)
        {
            if (_toolCreators.TryGetValue(shapeType, out var creator))
            {
                return creator();
            }
            throw new ArgumentException($"Unknown shape type: {shapeType}");
        }

        public IEnumerable<string> AvailableShapes => _shapeCreators.Keys;
    }
}