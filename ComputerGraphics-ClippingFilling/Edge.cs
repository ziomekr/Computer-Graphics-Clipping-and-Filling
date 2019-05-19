using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerGraphics_ClippingFilling
{
    class Edge
    {
        public int ymax { get; set; }
        public double x { get; set; }
        public double invM { get; set; }
        public Edge(Point p1, Point p2) {
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
    }
}
