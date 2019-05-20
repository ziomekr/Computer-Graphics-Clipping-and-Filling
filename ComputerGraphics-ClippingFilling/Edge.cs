using System.Windows;

namespace ComputerGraphics_ClippingFilling
{
    internal class Edge
    {
        public Edge(Point p1, Point p2)
        {
            if (p1.Y > p2.Y)
                ymax = (int)p1.Y;
            else
                ymax = (int)p2.Y;
            x = p1.X;
            if (p1.X != p2.X && p2.Y != p1.Y)
            {
                invM = 1 / ((p2.Y - p1.Y) / (p2.X - p1.X));
            }
            else
                invM = 0;
        }

        public double invM { get; set; }
        public double x { get; set; }
        public int ymax { get; set; }
    }
}