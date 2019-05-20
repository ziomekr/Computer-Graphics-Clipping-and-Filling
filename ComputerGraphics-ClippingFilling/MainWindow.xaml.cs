using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ComputerGraphics_ClippingFilling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Rect clippingRectangle;
        private Point endPoint;
        private bool hasStartPoint = false;
        private List<Point> polygonVertices = new List<Point>();
        private Point startPoint;

        public MainWindow()
        {
            InitializeComponent();
            InitializeClippingRectangle();
        }

        private void ClippingCanvasClick(object sender, MouseButtonEventArgs e)
        {
            if (!hasStartPoint)
            {
                startPoint = e.GetPosition(ClippingCanvas);
                hasStartPoint = true;
            }
            else
            {
                endPoint = e.GetPosition(ClippingCanvas);
                LiangBarskyClipping clip = new LiangBarskyClipping(ClippingCanvas, clippingRectangle);
                clip.drawLine(startPoint, endPoint, Colors.Green);
                clip.LiangBarsky(startPoint, endPoint);
                hasStartPoint = false;
            }
        }

        private void ClippingReset(object sender, MouseButtonEventArgs e)
        {
            ClippingCanvas.Children.Clear();
            InitializeClippingRectangle();
        }

        private void drawLine(Point p1, Point p2, Color color, Canvas Canvas)
        {
            Line l = new Line();
            l.X1 = p1.X;
            l.Y1 = p1.Y;
            l.X2 = p2.X;
            l.Y2 = p2.Y;
            SolidColorBrush brush = new SolidColorBrush(color);
            l.StrokeThickness = 2;
            l.Stroke = brush;
            Canvas.Children.Add(l);
        }

        private void FillButtonClick(object sender, RoutedEventArgs e)
        {
            ScanlineFillVertexSort filler = new ScanlineFillVertexSort(polygonVertices, FillingCanvas);
            filler.fillPolygon();
        }

        private void FillingCanvasClick(object sender, MouseButtonEventArgs e)
        {
            if (polygonVertices.Count < VertexSlider.Value)
            {
                Point temp = e.GetPosition(FillingCanvas);
                polygonVertices.Add(new Point((int)temp.X, (int)temp.Y));
                if (polygonVertices.Count > 1)
                {
                    drawLine(polygonVertices[polygonVertices.Count - 1], polygonVertices[polygonVertices.Count - 2], Colors.Black, FillingCanvas);
                    if (polygonVertices.Count == VertexSlider.Value)
                    {
                        drawLine(polygonVertices[polygonVertices.Count - 1], polygonVertices[0], Colors.Black, FillingCanvas);
                        showVerticesInListView();
                        FillButton.IsEnabled = true;
                    }
                }
            }
        }

        private void InitializeClippingRectangle()
        {
            Path myPath1 = new Path();
            myPath1.Stroke = Brushes.Black;
            myPath1.StrokeThickness = 2;

            Rect myRect1 = new Rect();
            myRect1.X = 150;
            myRect1.Y = 100;
            myRect1.Width = 300;
            myRect1.Height = 200;
            RectangleGeometry myRectangleGeometry1 = new RectangleGeometry();
            myRectangleGeometry1.Rect = myRect1;

            GeometryGroup myGeometryGroup1 = new GeometryGroup();
            myGeometryGroup1.Children.Add(myRectangleGeometry1);

            myPath1.Data = myGeometryGroup1;
            clippingRectangle = myRect1;
            ClippingCanvas.Children.Add(myPath1);
        }

        private void showVerticesInListView()
        {
            foreach (Point p in polygonVertices)
            {
                ListViewItem it = new ListViewItem { Content = "X: " + (int)p.X + " Y: " + (int)p.Y };
                VerticesListView.Items.Add(it);
            }
        }

        private void VerticesChanged(object sender, TextChangedEventArgs e)
        {
            polygonVertices.Clear();
            FillingCanvas.Children.Clear();
            VerticesListView.Items.Clear();
            FillButton.IsEnabled = false;
        }
    }
}