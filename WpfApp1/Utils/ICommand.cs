namespace WpfGraphicsApp
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}