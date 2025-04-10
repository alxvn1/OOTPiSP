using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using WpfGraphicsApp.Shapes;

namespace WpfGraphicsApp
{
    public class ShapeManager
    {
        private readonly List<ShapeBase> _shapes = new();
        private readonly Stack<ICommand> _undoStack = new();
        private readonly Stack<ICommand> _redoStack = new();

        public IReadOnlyList<ShapeBase> Shapes => _shapes;

        public void AddShape(ShapeBase shape) => _shapes.Add(shape);
        public void RemoveShape(ShapeBase shape) => _shapes.Remove(shape);

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();
        }
        
        public void Clear()
        {
            _shapes.Clear();
            _undoStack.Clear();
            _redoStack.Clear();
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

        public void SaveToFile(string path)
        {
            if (path.EndsWith(".json"))
            {
                var json = JsonSerializer.Serialize(_shapes, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
            }
            else if (path.EndsWith(".xml"))
            {
                var serializer = new XmlSerializer(typeof(List<ShapeBase>));
                using var writer = new StreamWriter(path);
                serializer.Serialize(writer, _shapes);
            }
        }

        public void LoadFromFile(string path)
        {
            if (File.Exists(path))
            {
                if (path.EndsWith(".json"))
                {
                    var json = File.ReadAllText(path);
                    var shapes = JsonSerializer.Deserialize<List<ShapeBase>>(json);
                    if (shapes != null)
                    {
                        _shapes.Clear();
                        _shapes.AddRange(shapes);
                    }
                }
                else if (path.EndsWith(".xml"))
                {
                    var serializer = new XmlSerializer(typeof(List<ShapeBase>));
                    using var reader = new StreamReader(path);
                    var shapes = serializer.Deserialize(reader) as List<ShapeBase>;
                    if (shapes != null)
                    {
                        _shapes.Clear();
                        _shapes.AddRange(shapes);
                    }
                }
            }
        }
    }
}
