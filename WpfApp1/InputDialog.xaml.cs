using System;
using System.Windows;

namespace WpfGraphicsApp
{
    public partial class PolygonInputDialog : Window
    {
        public int NumberOfSides { get; private set; } = 3; // Значение по умолчанию

        public PolygonInputDialog()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(InputTextBox.Text, out int sides) && sides >= 3)
            {
                NumberOfSides = sides;
                DialogResult = true; // Закрыть окно с результатом true
            }
            else
            {
                MessageBox.Show("Некорректное количество углов. Введите число больше или равное 3.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Закрыть окно с результатом false
        }
    }
}