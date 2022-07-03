using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintColab
{
    public class Line
    {
        int color = 0;
        int thickness = 1;
        int x1, x2, y1, y2 = -1;
        public Line(int color, int thickness, int x1, int y1, int x2, int y2)
        {
            this.color = color;
            this.thickness = thickness;
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;
        }

        public int Color { get => color;}
        public int Thickness { get => thickness; }
        public int X1 { get => x1; }
        public int X2 { get => x2; }
        public int Y1 { get => y1; }
        public int Y2 { get => y2; }
    }
}
