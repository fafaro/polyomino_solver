using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System;
using System.Collections.Generic;

namespace ui
{
    class PieceEditor : UserControl
    {
        public PieceEditor()
        {
            this.DoubleBuffered = true;

            this.grid.Transform = new Matrix();
            this.grid.Transform.Translate(20, 40);
            this.grid.Size = new Size(8, 50);
            this.grid.CellSize = new Size(20, 20);
            this.grid.Highlight = new Point(-1, -1);
            this.grid.HighlightChanged += () => { Invalidate(); };
            this.grid.CellsChanged += () => 
            {
                if (CellsChanged != null) CellsChanged();
                Invalidate();
            };

            var clearButton = new Button();
            clearButton.Size = new Size(20, 20);
            clearButton.Text = "x";
            clearButton.Location = new Point(200, 5);
            clearButton.Click += (sender, args) => { Clear(); };
            this.Controls.Add(clearButton);
        }

        public bool[,] Cells
        {
            get
            {
                return grid.CellStates;
            }
        }

        public void Clear()
        {
            grid.Clear();
        }

        public Size GridSize { get { return grid.Size; } }

        public event Action CellsChanged;

        private class Grid
        {
            private Size size;
            public Matrix Transform { get; set; }
            public Size Size
            {
                get { return size; }
                set
                {
                    size = value;
                    CellStates = new bool[size.Width, size.Height];
                }
            }
            public Size CellSize { get; set; }
            public bool[,] CellStates { get; set; }

            private Point highlight = new Point(-1, -1);
            public Point Highlight
            {
                get { return highlight; }
                set
                {
                    highlight = value;
                    if (HighlightChanged != null) HighlightChanged();
                }
            }
            public event Action HighlightChanged;
            public event Action CellsChanged;

            public Point HitCell(Point p)
            {
                var m = Transform.Clone();
                m.Invert();
                var points = new Point[] { p };
                m.TransformPoints(points);
                var locp = points[0];
                var loc = new Point(locp.X / CellSize.Width, locp.Y / CellSize.Height);
                if (locp.X < 0 || loc.X >= Size.Width || locp.Y < 0 || loc.Y >= Size.Height) return new Point(-1, -1);
                return loc;
            }

            public Rectangle GetCellRectangle(Point p)
            {
                return new Rectangle(p.X * CellSize.Width, p.Y * CellSize.Height, CellSize.Width, CellSize.Height);
            }

            public void SetCell(Point p, bool value = true)
            {
                if (p.X < 0 || p.X >= Size.Width || p.Y < 0 || p.Y >= Size.Height) return;
                if (CellStates[p.X, p.Y] != value)
                {
                    CellStates[p.X, p.Y] = value;
                    if (CellsChanged != null) CellsChanged();
                }
            }

            public void Clear()
            {
                Array.Clear(CellStates, 0, CellStates.Length);
                if (CellsChanged != null) CellsChanged();
            }
        }

        private Grid grid = new Grid();

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            g.DrawString("Piece Editor",
                new Font("Consolas", 8.0f),
                Brushes.White,
                new PointF(10, 10));

            DrawGrid(g);
        }

        private void DrawGrid(Graphics g)
        {
            var origm = g.Transform;
            g.Transform = grid.Transform;
            var gridSize = grid.Size;
            var cellSize = grid.CellSize.Width;
            for (int y = 0; y < gridSize.Height; y++)
                for (int x = 0; x < gridSize.Width; x++)
                {
                    var cell = new Point(x, y);
                    var rect = grid.GetCellRectangle(cell);
                    g.DrawRectangle(Pens.Black, rect);
                    if (grid.CellStates[x, y])
                        g.FillRectangle(new SolidBrush(Color.Brown), rect);
                }

            if (grid.Highlight.X != -1)
                g.FillRectangle(new SolidBrush(Color.FromArgb(128, 255, 255, 0)), grid.GetCellRectangle(grid.Highlight));

            g.Transform = origm;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.grid.Highlight = new Point(-1, -1);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var cell = grid.HitCell(e.Location);
            grid.Highlight = cell;
            if (buttonDown) grid.SetCell(cell);
            if (eraseButtonDown) grid.SetCell(cell, false);           
        }

        private bool buttonDown = false;
        private bool eraseButtonDown = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            switch (e.Button) {
                case MouseButtons.Left:
                    grid.SetCell(grid.HitCell(e.Location));
                    buttonDown = true;
                    break;
                case MouseButtons.Right:
                    grid.SetCell(grid.HitCell(e.Location), false);
                    eraseButtonDown = true;
                    break;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            switch (e.Button)
            {
                case MouseButtons.Left: buttonDown = false;  break;
                case MouseButtons.Right: eraseButtonDown = false; break;
            }
        }
    }

    class BoardViewer : UserControl
    {
        public PuzzleModel Model { get; set; }
        private Matrix Transform { get; set; }
        private int CellSize = 30;

        public BoardViewer()
        {
            this.DoubleBuffered = true;
            this.Transform = new Matrix();
            this.Transform.Translate(100, 100);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (Model == null) return;

            var g = e.Graphics;
            var font = new Font("Consolas", 8.0f);

            var s = string.Format("Board ({0}x{1})", Model.BoardSize.X, Model.BoardSize.Y);
            g.DrawString(s, font, Brushes.White, new PointF(10, 10));

            DrawBoard(g);
            if (Model.Solution != null)
                DrawSolution(g);
        }

        private void DrawBoard(Graphics g)
        {
            var originalTransform = g.Transform;
            g.Transform = Transform;

            for (int y = 0; y < Model.BoardSize.Y; y++)
                for (int x = 0; x < Model.BoardSize.X; x++)
                    g.DrawRectangle(Pens.Black, new Rectangle(x * CellSize, y * CellSize, CellSize, CellSize));

            g.Transform = originalTransform;
        }

        class ColorHelper
        {
            public static Color FromHSV(double h, double s, double v)
            {
                int r, g, b;
                HsvToRgb(h, s, v, out r, out g, out b);
                return Color.FromArgb(r, g, b);
            }
            public static void HsvToRgb(double h, double S, double V, out int r, out int g, out int b)
            {
                double H = h;
                while (H < 0) { H += 360; };
                while (H >= 360) { H -= 360; };
                double R, G, B;
                if (V <= 0)
                { R = G = B = 0; }
                else if (S <= 0)
                {
                    R = G = B = V;
                }
                else
                {
                    double hf = H / 60.0;
                    int i = (int)Math.Floor(hf);
                    double f = hf - i;
                    double pv = V * (1 - S);
                    double qv = V * (1 - S * f);
                    double tv = V * (1 - S * (1 - f));
                    switch (i)
                    {

                        // Red is the dominant color

                        case 0:
                            R = V;
                            G = tv;
                            B = pv;
                            break;

                        // Green is the dominant color

                        case 1:
                            R = qv;
                            G = V;
                            B = pv;
                            break;
                        case 2:
                            R = pv;
                            G = V;
                            B = tv;
                            break;

                        // Blue is the dominant color

                        case 3:
                            R = pv;
                            G = qv;
                            B = V;
                            break;
                        case 4:
                            R = tv;
                            G = pv;
                            B = V;
                            break;

                        // Red is the dominant color

                        case 5:
                            R = V;
                            G = pv;
                            B = qv;
                            break;

                        // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                        case 6:
                            R = V;
                            G = tv;
                            B = pv;
                            break;
                        case -1:
                            R = V;
                            G = pv;
                            B = qv;
                            break;

                        // The color is not defined, we should throw an error.

                        default:
                            //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                            R = G = B = V; // Just pretend its black/white
                            break;
                    }
                }
                r = Clamp((int)(R * 255.0));
                g = Clamp((int)(G * 255.0));
                b = Clamp((int)(B * 255.0));
            }

            /// <summary>
            /// Clamp a value to 0-255
            /// </summary>
            public static int Clamp(int i)
            {
                if (i < 0) return 0;
                if (i > 255) return 255;
                return i;
            }
        }

        private static class DrawSolutionHelper
        {
            public static bool HasCell(polyomino_solver.Point p, polyomino_solver.Point[] cells)
            {
                foreach (var c in cells)
                    if (c.Equals(p))
                        return true;
                return false;
            }

            public static Color Lighter(Color c)
            {
                return Color.FromArgb(255 - (255 - c.R) / 2,
                    255 - (255 - c.G) / 2,
                    255 - (255 - c.B) / 2);
            }

            public static Color Darker(Color c)
            {
                return Color.FromArgb(c.R / 2, c.G / 2, c.B / 2);
            }
        }

        private void DrawSolution(Graphics g)
        {
            int bevel = 3;
            var originalTransform = g.Transform;
            g.Transform = Transform;
            int colIndex = 0;
            var colors = new Color[8];
            for (int i = 0; i < colors.Length; i++)
                colors[i] = ColorHelper.FromHSV(i * 360.0 / colors.Length, 0.7, 0.8);
            foreach (var piece in Model.Solution)
            {
                var brush = new SolidBrush(colors[colIndex++ % colors.Length]);
                var brushLight = new SolidBrush(DrawSolutionHelper.Lighter(brush.Color));
                var brushDark = new SolidBrush(DrawSolutionHelper.Darker(brush.Color));
                foreach (var p in piece.Cells)
                {
                    g.FillRectangle(brush, new Rectangle(p.X * CellSize, p.Y * CellSize, CellSize, CellSize));

                    Func<int, int, bool> hasCell = (x, y) =>
                    {
                        return DrawSolutionHelper.HasCell(p + new polyomino_solver.Point(x, y), piece.Cells);
                    };
                    Func<int, int, Point> gp = (x, y) => { return new Point(x, y); };
                    Func<int, int, Size> sz = (x, y) => { return new Size(x, y); };

                    var ul = gp(p.X * CellSize, p.Y * CellSize);
                    var ur = gp(ul.X + CellSize, ul.Y);
                    var ll = gp(ul.X, ul.Y + CellSize);
                    var lr = gp(ul.X + CellSize, ul.Y + CellSize);

                    if (!hasCell(-1, 0))
                        g.FillRectangle(brushLight, ul.X, ul.Y, bevel, CellSize);
                    if (!hasCell(0, -1))
                        g.FillRectangle(brushLight, ul.X, ul.Y, CellSize, bevel);
                    if (!hasCell(1, 0))
                        g.FillRectangle(brushDark, ur.X - bevel, ur.Y, bevel, CellSize);
                    if (!hasCell(0, 1))
                        g.FillRectangle(brushDark, ll.X, ll.Y - bevel, CellSize, bevel);

                    if (hasCell(-1, 0) && hasCell(0, -1) && !hasCell(-1, -1))
                        g.FillRectangle(brushLight, ul.X, ul.Y, bevel, bevel);
                    if (hasCell(1, 0) && hasCell(0, 1) && !hasCell(1, 1))
                        g.FillRectangle(brushDark, lr.X - bevel, lr.Y - bevel, bevel, bevel);
                    if (hasCell(-1, 0) && hasCell(0, 1) && !hasCell(-1, 1))
                    {
                        g.FillPolygon(brushLight, new Point[] { ll, ll + sz(bevel, 0), ll + sz(bevel, -bevel) });
                        g.FillPolygon(brushDark, new Point[] { ll, ll + sz(0, -bevel), ll + sz(bevel, -bevel) });
                    }
                    if (hasCell(1, 0) && hasCell(0, -1) && !hasCell(1, -1))
                    {
                        g.FillPolygon(brushLight, new Point[] { ur, ur + sz(-bevel, bevel), ur + sz(0, bevel) });
                        g.FillPolygon(brushDark, new Point[] { ur, ur + sz(-bevel, 0), ur + sz(-bevel, bevel) });
                    }
                }
            }
            g.Transform = originalTransform;
        }
    }
}