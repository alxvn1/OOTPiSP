using WpfGraphicsApp.Shapes;

namespace WpfGraphicsApp.UndoRedo
{
    internal class DrawCommand : ICommand
    {
        private readonly ShapeManager _shapeManager;
        private readonly ShapeBase _shape;

        public DrawCommand(ShapeManager shapeManager, ShapeBase shape)
        {
            _shapeManager = shapeManager;
            _shape = shape;
        }

        public void Execute()
        {
            _shapeManager.AddShape(_shape);
        }

        public void Undo()
        {
            _shapeManager.RemoveShape(_shape);
        }
    }
}