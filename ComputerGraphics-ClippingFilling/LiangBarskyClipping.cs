using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ComputerGraphics_ClippingFilling
{
    internal class LiangBarskyClipping
    {
        private Canvas Canvas;
        private Rect Rect;

        public LiangBarskyClipping(Canvas canvas, Rect rect)
        {
            this.Canvas = canvas;
            this.Rect = rect;
        }

        public void drawLine(Point p1, Point p2, Color color)
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

        public void LiangBarsky(Point p1, Point p2)
        {
            float dx = (float)(p2.X - p1.X);
            float dy = (float)(p2.Y - p1.Y);
            float tE = 0, tL = 1;
            if (Clip(-dx, (float)(p1.X - Rect.Left), ref tE, ref tL))
                if (Clip(dx, (float)(Rect.Right - p1.X), ref tE, ref tL))
                    if (Clip(-dy, (float)(p1.Y - Rect.Top), ref tE, ref tL))
                        if (Clip(dy, (float)(Rect.Bottom - p1.Y), ref tE, ref tL))
                        {
                            if (tL < 1)
                            {
                                p2.X = p1.X + dx * tL;
                                p2.Y = p1.Y + dy * tL;
                            }
                            if (tE > 0)
                            {
                                p1.X += dx * tE;
                                p1.Y += dy * tE;
                            }
                            drawLine(p1, p2, Colors.Red);
                        }
        }

        private bool Clip(float denom, float numer, ref float tE, ref float tL)
        {
            if (denom == 0 && numer < 0) return false;
            float t = numer / denom;
            if (denom < 0)
            {
                if (t > tL)
                    return false;
                if (t > tE) tE = t;
            }
            else if (denom > 0)
            {
                if (t < tE)
                    return false;
                if (t < tL) tL = t;
            }
            return true;
        }
    }
}