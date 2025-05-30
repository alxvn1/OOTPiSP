using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;
using System.Windows.Shapes;
using System.Windows.Controls;
using WpfGraphicsApp.Shapes;
using WpfGraphicsApp.UndoRedo;
using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using WpfGraphicsApp.Factories;
using WpfGraphicsApp.Tools;
using MessageBox = System.Windows.MessageBox;
using WindowStartupLocation = System.Windows.WindowStartupLocation;
using WpfGraphicsApp.Tools;



namespace WpfGraphicsApp
{
    public partial class MainWindow : Window
    {
        private ShapeManager _shapeManager = new ShapeManager();
        private DrawingTool _currentTool;
        private Shape _previewShape;
        private bool _isDrawing = false;
        private Point _startPoint;
        private Color _fillColor = Colors.Transparent;
        private Color _strokeColor = Colors.Black;
        private double _strokeThickness = 2;
        private readonly List<IShapePlugin> _loadedPlugins = new List<IShapePlugin>();
        private readonly ShapeFactory _shapeFactory = new ShapeFactory();
        

        public MainWindow()
        {
            InitializeComponent();
            _shapeFactory = new ShapeFactory();
            // Подписка на события мыши
            DrawingCanvas.MouseLeftButtonDown += DrawingCanvas_MouseLeftButtonDown;
            DrawingCanvas.MouseMove += DrawingCanvas_MouseMove;
            DrawingCanvas.MouseLeftButtonUp += DrawingCanvas_MouseLeftButtonUp;

            this.Loaded += (s, e) => { this.Focus(); };
        }

        private void DrawingCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(DrawingCanvas);

            if (e.ClickCount == 2)
            {
                if (_currentTool is PolylineTool polylineTool)
                {
                    var shape = polylineTool.CompleteDrawing();
                    if (shape != null)
                    {
                        AddShapeToCanvas(shape);
                        DrawingCanvas.Children.Remove(_previewShape);
                        _previewShape = null;
                    }
                }

                return;
            }

            if (_currentTool == null) return;

            if (_previewShape == null)
            {
                _previewShape = _currentTool.CreatePreviewShape();
                DrawingCanvas.Children.Add(_previewShape);
            }

            _currentTool.OnMouseDown(position);
            _isDrawing = true;
        }


        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDrawing || _currentTool == null || _previewShape == null) return;

            var currentPoint = e.GetPosition(DrawingCanvas);
            _currentTool.OnMouseMove(currentPoint, _previewShape);
        }



        private void DrawingCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Для Polyline и Polygon ничего не делаем
            if (_currentTool is PolylineTool || _currentTool is PolygonTool)
            {
                return;
            }

            if (_isDrawing && _currentTool != null)
            {
                var endPoint = e.GetPosition(DrawingCanvas);
                var shape = _currentTool.OnMouseUp(endPoint);

                if (shape != null)
                {
                    AddShapeToCanvas(shape);
                }

                DrawingCanvas.Children.Remove(_previewShape);
                _previewShape = null;
            }

            _isDrawing = false;
        }

        // Tool selection handlers
        private void SelectLineTool_Click(object sender, RoutedEventArgs e)
        {
            _currentTool = new LineTool();
        }

        private void SelectRectangleTool_Click(object sender, RoutedEventArgs e)
        {
            _currentTool = new RectangleTool();
        }

        private void SelectEllipseTool_Click(object sender, RoutedEventArgs e)
        {
            _currentTool = new EllipseTool();
        }

        private void SelectPolylineTool_Click(object sender, RoutedEventArgs e)
        {
            _currentTool = new PolylineTool();
        }

        private void SelectPolygonTool_Click(object sender, RoutedEventArgs e)
        {
            _currentTool = new PolygonTool();
        }

        // File operations handlers
        // Для сохранения
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                DefaultExt = ".json"
            };

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    ShapeSerializer.SaveShapesToFile(_shapeManager.Shapes.ToList(), saveDialog.FileName);

                }
                catch (Exception ex)
                {

                }
            }
        }

// Для загрузки
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                DefaultExt = ".json"
            };

            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    _shapeManager.LoadFromFile(openDialog.FileName, DrawingCanvas);
                    var shapes = ShapeSerializer.LoadShapesFromFile(openDialog.FileName);
                    _shapeManager.Clear();
                    DrawingCanvas.Children.Clear();

                    foreach (var shape in shapes)
                    {
                        _shapeManager.ExecuteCommand(new DrawCommand(_shapeManager, shape));
                        DrawingCanvas.Children.Add(shape.Draw());
                    }


                }
                catch (Exception ex)
                {

                }
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "JSON File (*.json)|*.json"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var shapes = ShapeSerializer.LoadShapesFromFile(openFileDialog.FileName);
                    _shapeManager.Clear();
                    foreach (var shape in shapes)
                    {
                        _shapeManager.ExecuteCommand(new DrawCommand(_shapeManager, shape));
                    }

                    RedrawCanvas();
                }
                catch (Exception ex)
                {

                }
            }

            RedrawCanvas(); // Явный вызов перерисовки
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _shapeManager.Clear();
            DrawingCanvas.Children.Clear();
        }

        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _strokeThickness = e.NewValue;

            // Обновляем текущий инструмент
            if (_currentTool != null)
            {
                _currentTool.StrokeThickness = _strokeThickness;

                // Если есть активное превью - обновляем его
                if (_previewShape != null)
                {
                    if (_previewShape is Line line)
                        line.StrokeThickness = _strokeThickness;
                    else if (_previewShape is Polyline polyline)
                        polyline.StrokeThickness = _strokeThickness;
                    else if (_previewShape is Polygon polygon)
                        polygon.StrokeThickness = _strokeThickness;
                    else if (_previewShape is Rectangle rect)
                        rect.StrokeThickness = _strokeThickness;
                    else if (_previewShape is Ellipse ellipse)
                        ellipse.StrokeThickness = _strokeThickness;
                }
            }
        }

        // Shape drawing handlers
        private void DrawPolygon_Click(object sender, RoutedEventArgs e)
        {
            var polygon = new PolygonShape();
            // Заменяем PointCollection на List<PointModel>
            polygon.Points = new List<PolygonShape.PointModel>
            {
                new PolygonShape.PointModel { X = 100, Y = 0 },
                new PolygonShape.PointModel { X = 200, Y = 50 },
                new PolygonShape.PointModel { X = 150, Y = 150 },
                new PolygonShape.PointModel { X = 50, Y = 150 },
                new PolygonShape.PointModel { X = 0, Y = 50 }
            };
            AddShapeToCanvas(polygon);
        }

        private void DrawSquare_Click(object sender, RoutedEventArgs e)
        {
            AddShapeToCanvas(new RectangleShape { Width = 100, Height = 100 });
        }

        private void DrawLine_Click(object sender, RoutedEventArgs e)
        {
            AddShapeToCanvas(new LineShape
            {
                X1 = 50,
                Y1 = 50,
                X2 = 200,
                Y2 = 200
            });
        }

        private void DrawPolyline_Click(object sender, RoutedEventArgs e)
        {
            var polyline = new PolylineShape();
            // Заменяем PointCollection на List<PointModel>
            polyline.Points = new List<PolylineShape.PointModel>
            {
                new PolylineShape.PointModel { X = 50, Y = 50 },
                new PolylineShape.PointModel { X = 100, Y = 150 },
                new PolylineShape.PointModel { X = 150, Y = 50 },
                new PolylineShape.PointModel { X = 200, Y = 150 },
                new PolylineShape.PointModel { X = 250, Y = 50 }
            };
            AddShapeToCanvas(polyline);
        }

        // Color selection methods
        private void SelectFillColor_Click(object sender, RoutedEventArgs e)
        {
            ShowColorPickerDialog("Выберите цвет заливки", (color) =>
            {
                _fillColor = color; // Обязательно сохраняем цвет
            }, _fillColor);
        }

        private void SelectStrokeColor_Click(object sender, RoutedEventArgs e)
        {
            ShowColorPickerDialog("Выберите цвет обводки", (color) => _strokeColor = color, _strokeColor);
        }

        private void ShowColorPickerDialog(string title, Action<Color> colorSetter, Color initialColor)
        {
            var dialog = new Window
            {
                Title = title,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };

            var colorPicker = new ColorPicker
            {
                SelectedColor = initialColor,
                ColorMode = ColorMode.ColorPalette,
                Margin = new Thickness(10),
                UsingAlphaChannel = false,
                AvailableColorsSortingMode = ColorSortingMode.HueSaturationBrightness
            };

            var okButton = new Button
            {
                Content = "OK",
                Margin = new Thickness(0, 10, 0, 0),
                Width = 80,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            okButton.Click += (s, args) =>
            {
                if (colorPicker.SelectedColor.HasValue)
                {
                    colorSetter(colorPicker.SelectedColor.Value);
                    dialog.Close();
                }
            };

            var stackPanel = new StackPanel();
            stackPanel.Children.Add(colorPicker);
            stackPanel.Children.Add(okButton);

            dialog.Content = stackPanel;
            dialog.ShowDialog();
        }

        // Helper methods
        //private void AddShapeToCanvas(ShapeBase shape)
        // {
        //     // Устанавливаем общие свойства для всех фигур
        //     shape.Stroke = new SolidColorBrush(_strokeColor);
        //     shape.StrokeThickness = _strokeThickness;
        //
        //     // Особые правила для заливки
        //     if (shape is LineShape || shape is PolylineShape)
        //     {
        //         shape.Fill = Brushes.Transparent; // Без заливки
        //     }
        //     else
        //     {
        //         shape.Fill = new SolidColorBrush(_fillColor); // С заливкой
        //     }
        //
        //     var uiElement = shape.Draw();
        //     DrawingCanvas.Children.Add(uiElement);
        //     _shapeManager.ExecuteCommand(new DrawCommand(_shapeManager, shape));
        // }

        // private void AddShapeToCanvas(ShapeBase shape)
        // {
        //     // Установка стилей фигуры
        //     shape.Stroke = new SolidColorBrush(_strokeColor);
        //     shape.Fill = new SolidColorBrush(_fillColor);
        //     shape.StrokeThickness = _strokeThickness;
        //
        //     // Создание графического элемента
        //     var uiElement = shape.Draw();
        //     if (uiElement == null) return;
        //
        //     // Установка ZIndex на основе количества элементов на Canvas
        //     Panel.SetZIndex(uiElement, DrawingCanvas.Children.Count);
        //
        //     // Добавление на холст
        //     DrawingCanvas.Children.Add(uiElement);
        //
        //     // Создание и выполнение команды
        //     _shapeManager.ExecuteCommand(new DrawCommand(_shapeManager, shape));
        // }
        
        private void AddShapeToCanvas(ShapeBase shape)
        {
            // Убедитесь, что цвета применяются явно
            shape.Stroke = new SolidColorBrush(_strokeColor);
            shape.Fill = new SolidColorBrush(_fillColor);
            shape.StrokeThickness = _strokeThickness;

            var uiElement = shape.Draw();
            if (uiElement == null) return;

            Panel.SetZIndex(uiElement, DrawingCanvas.Children.Count);
            DrawingCanvas.Children.Add(uiElement);
            _shapeManager.ExecuteCommand(new DrawCommand(_shapeManager, shape));
        }

        public void AddFinalShapeToCanvas(ShapeBase shape)
        {
            // Удаляем превью
            DrawingCanvas.Children.Remove(_previewShape);
            _previewShape = null;

            // Добавляем финальную фигуру через менеджер команд
            _shapeManager.ExecuteCommand(new DrawCommand(_shapeManager, shape));

            // Перерисовываем холст
            RedrawCanvas();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (_currentTool is PolylineTool polylineTool)
                {
                    polylineTool.CancelDrawing();
                    DrawingCanvas.Children.Remove(_previewShape);
                    _previewShape = null;
                    _isDrawing = false;
                }
                else if (_currentTool is PolygonTool polygonTool)
                {
                    polygonTool.CancelDrawing();
                    DrawingCanvas.Children.Remove(_previewShape);
                    _previewShape = null;
                    _isDrawing = false;
                }
            }

            base.OnKeyDown(e);
        }

        private void DrawRectangle_Click(object sender, RoutedEventArgs e)
        {
            AddShapeToCanvas(new RectangleShape { Width = 150, Height = 100 });
        }

        private void DrawEllipse_Click(object sender, RoutedEventArgs e)
        {
            AddShapeToCanvas(new EllipseShape { Width = 100, Height = 100 });
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            _shapeManager.Undo();
            RedrawCanvas();
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            _shapeManager.Redo();
            RedrawCanvas();
        }


        private void RedrawCanvas()
        {
            DrawingCanvas.Children.Clear();

            foreach (var shape in _shapeManager.Shapes)
            {
                Console.WriteLine($"Drawing {shape.GetType().Name}");
                var uiElement = shape.Draw();
                DrawingCanvas.Children.Add(uiElement);
            }
        }

        private void SaveDrawing_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                DefaultExt = ".json"
            };

            if (saveDialog.ShowDialog() == true)
            {
                _shapeManager.SaveToFile(saveDialog.FileName);
            }
        }

        private void LoadDrawing_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                DefaultExt = ".json"
            };

            if (openDialog.ShowDialog() == true)
            {
                _shapeManager.LoadFromFile(openDialog.FileName, DrawingCanvas); // Передача DrawingCanvas
                RedrawCanvas();
            }
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Плагины (*.dll)|*.dll"
            };

            if (dialog.ShowDialog() == true)
            {
                LoadShapePlugin(dialog.FileName);
            }
        }

        private void UpdateActiveButton(Button activeButton)
        {
            foreach (var child in ToolPanel.Children)
            {
                if (child is Button btn && btn.Style == (Style)FindResource("RoundedButtonStyle"))
                {
                    btn.Background = Brushes.Transparent;
                    btn.Foreground = Brushes.Black;

                    if (btn == activeButton)
                    {
                        btn.Background = Brushes.DodgerBlue;
                        btn.Foreground = Brushes.White;
                    }
                }
            }
        }

        private void LoadPluginButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Плагины фигур (*.dll)|*.dll",
                Title = "Выберите файл плагина",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                LoadShapePlugin(openFileDialog.FileName);
            }
        }

        private void LoadShapePlugin(string pluginPath)
        {
            try
            {
                Console.WriteLine($"=== Начало загрузки плагина ===");
                Console.WriteLine($"Путь к плагину: {pluginPath}");

                // 1. Загрузка сборки
                var assembly = Assembly.LoadFrom(pluginPath);

                // 2. Вывод всех типов в сборке (диагностика)
                Console.WriteLine("Все типы в сборке:");
                foreach (var type in assembly.GetTypes())
                {
                    Console.WriteLine($"- {type.FullName}");
                }

                // 3. Поиск реализаций IShapePlugin
                var interfaceType = typeof(IShapePlugin);
                var pluginTypes = assembly.GetTypes()
                    .Where(t => t.IsClass &&
                                !t.IsAbstract &&
                                interfaceType.IsAssignableFrom(t))
                    .ToList();

                Console.WriteLine($"Найдено реализаций IShapePlugin: {pluginTypes.Count}");

                if (pluginTypes.Count == 0)
                {
                    MessageBox.Show("Ошибка: плагин не содержит подходящих реализаций интерфейса");
                    return;
                }

                // 4. Создание экземпляров плагинов
                foreach (var type in pluginTypes)
                {
                    try
                    {
                        var plugin = (IShapePlugin)Activator.CreateInstance(type);
                        _loadedPlugins.Add(plugin);
                        Console.WriteLine($"Успешно создан плагин: {type.FullName}");

                        // 5. Добавление кнопки в UI
                        Dispatcher.Invoke(() => AddPluginButton(plugin));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка создания плагина {type.Name}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ФАТАЛЬНАЯ ОШИБКА: {ex.ToString()}");
                MessageBox.Show($"Не удалось загрузить плагин: {ex.Message}");
            }
        }

        private void AddPluginButton(IShapePlugin plugin)
        {
            var button = new Button
            {
                Content = plugin.ShapeName,
                Margin = new Thickness(5),
                Style = (Style)FindResource("RoundedButtonStyle")
            };
            
                button.Click += (s, e) => 
                {
                    _currentTool = plugin.CreateTool();
                    _currentTool.Stroke = new SolidColorBrush(_strokeColor); // Передаем цвет из MainWindow
                    _currentTool.Fill = new SolidColorBrush(_fillColor);
                    _currentTool.StrokeThickness = _strokeThickness;
                    UpdateActiveButton(button);
                };

            ToolPanel.Children.Add(button);
            Console.WriteLine($"Добавлена кнопка: {plugin.ShapeName}");
        }
    }
}
    
