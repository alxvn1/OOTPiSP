using System.Collections.Generic;
using System.Linq;
using WpfGraphicsApp.Shapes;
using WpfGraphicsApp.UndoRedo;
using System.Windows.Controls; // Для Canvas

namespace WpfGraphicsApp
{
    internal class ClearCommand : ICommand
    {
        private readonly ShapeManager _shapeManager;
        private readonly List<ShapeBase> _clearedShapes;

        public ClearCommand(ShapeManager shapeManager, List<ShapeBase> shapes)
        {
            _shapeManager = shapeManager;
            _clearedShapes = new List<ShapeBase>(shapes);
        }

        public void Execute()
        {
            _shapeManager.Clear();
        }

        
        
        public void Undo()
        {
            foreach (var shape in _clearedShapes)
            {
                _shapeManager.AddShape(shape);
            }
        }
    }

    public class ShapeManager
    {
        private readonly Stack<ICommand> _undoStack = new Stack<ICommand>();
        private readonly Stack<ICommand> _redoStack = new Stack<ICommand>();
        private readonly List<ShapeBase> _shapes = new List<ShapeBase>();

        public IEnumerable<ShapeBase> Shapes => _shapes.AsReadOnly();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear(); // Очищаем стек redo при новом действии
        }

        public void AddShape(ShapeBase shape)
        {
            _shapes.Add(shape);
        }

        public void AddShapeToCanvas(ShapeBase shape, Canvas drawingCanvas)
        {
            var uiElement = shape.Draw(); // Получение UI элемента
            drawingCanvas.Children.Add(uiElement); // Добавление на холст
        }
        
        public void RemoveShape(ShapeBase shape)
        {
            _shapes.Remove(shape);
        }

        public void Undo()
        {
            if (_undoStack.Count > 0)
            {
                var command = _undoStack.Pop();
                command.Undo();
                _redoStack.Push(command);
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                var command = _redoStack.Pop();
                command.Execute();
                _undoStack.Push(command);
            }
        }

        public void Clear()
        {
            _shapes.Clear();
            _undoStack.Clear();
            _redoStack.Clear();
        }

        // Добавить эти новые методы:
        public void SaveToFile(string filePath)
        {
            ShapeSerializer.SaveShapesToFile(_shapes, filePath);
        }

        public void LoadFromFile(string filePath, Canvas drawingCanvas)
        {
            var loadedShapes = ShapeSerializer.LoadShapesFromFile(filePath);
            _shapes.Clear();
            drawingCanvas.Children.Clear(); // Очистите холст перед загрузкой

            foreach (var shape in loadedShapes)
            {
                AddShape(shape); // Добавление в менеджер
                AddShapeToCanvas(shape, drawingCanvas); // Добавление на холст
            }

            _undoStack.Clear();
            _redoStack.Clear();
        }
    }
}