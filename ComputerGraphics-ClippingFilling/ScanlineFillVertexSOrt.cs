using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ComputerGraphics_ClippingFilling
{
    internal class ScanlineFillVertexSort
    {
        private List<Edge> activeEdgeTable = new List<Edge>();
        private Canvas Canvas;
        private WrappedList polygonVertices;
        private List<int> sortedIndices;

        public ScanlineFillVertexSort(List<Point> points, Canvas c)
        {
            Canvas = c;
            polygonVertices = new WrappedList(points);
            List<Point> temp = points.OrderBy(p => p.Y).ToList();
            this.sortedIndices = new List<int>();
            foreach (Point p in temp)
            {
                sortedIndices.Add(points.IndexOf(p));
            }
        }

        public void drawLine(Point p1, Point p2)
        {
            Line l = new Line();
            l.X1 = p1.X;
            l.Y1 = p1.Y;
            l.X2 = p2.X;
            l.Y2 = p2.Y;
            SolidColorBrush brush = new SolidColorBrush(Colors.Red);
            l.StrokeThickness = 1;
            l.Stroke = brush;
            Canvas.Children.Add(l);
        }

        public void fillPolygon()
        {
            int k = 0;
            int i = sortedIndices[k];
            double y = polygonVertices[i].Y;
            double ymax = polygonVertices[sortedIndices[sortedIndices.Count - 1]].Y;
            while (y < ymax)
            {
                while (polygonVertices[i].Y == y)
                {
                    if (polygonVertices[i - 1].Y > polygonVertices[i].Y)
                    {
                        activeEdgeTable.Add(new Edge(polygonVertices[i], polygonVertices[i - 1]));
                    }
                    if (polygonVertices[i + 1].Y > polygonVertices[i].Y)
                    {
                        activeEdgeTable.Add(new Edge(polygonVertices[i], polygonVertices[i + 1]));
                    }
                    k += 1;
                    i = sortedIndices[k];
                }
                activeEdgeTable = activeEdgeTable.OrderBy(e => e.x).ToList();
                for (int eIdx = 0; eIdx < activeEdgeTable.Count; eIdx += 2)
                {
                    drawLine(new Point(activeEdgeTable[eIdx].x, y), new Point(activeEdgeTable[eIdx + 1].x, y));
                }
                y += 1;
                activeEdgeTable = activeEdgeTable.Where(e => (e.ymax != y)).ToList();
                foreach (Edge e in activeEdgeTable)
                {
                    e.x += e.invM;
                }
            }
        }

        private class WrappedList
        {
            private List<Point> array;

            public WrappedList(List<Point> _arr)
            {
                array = _arr;
            }

            public Point this[int position]
            {
                get
                {
                    if (position > -1 && position < array.Count)
                        return array[position];
                    else if (position < 0)
                        return array[array.Count + position];
                    else
                        return array[position - array.Count];
                }
            }
        }
    }
}