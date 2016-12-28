using System.Windows.Forms;
using System.Drawing;

namespace ui
{
    class BoardViewer : UserControl
    {
        public BoardViewer()
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            var font = new Font("Consolas", 8.0f);

            if (this.DesignMode)
            {
                g.DrawString("BoardViewer", font, Brushes.White, new PointF(10, 10));
            }

            var app = ((MainForm)this.ParentForm).App;
            var boardSize = app.puzzle.BoardSize;
            g.DrawString(string.Format("({0}, {1})", boardSize.X, boardSize.Y), font, Brushes.White, new PointF(10, 30));
        }
    }

    class PieceBuilder : UserControl
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.DesignMode)
            {
                var g = e.Graphics;
                g.DrawString("PieceBuilder",
                    new Font("Consolas", 8.0f),
                    Brushes.White,
                    new PointF(10, 10));
            }
        }
    }

    class PiecesViewer : UserControl
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.DesignMode)
            {
                var g = e.Graphics;
                g.DrawString("PiecesViewer",
                    new Font("Consolas", 8.0f),
                    Brushes.White,
                    new PointF(10, 10));
            }
        }
    }
}