using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ui
{
    public partial class MainForm : Form
    {
        private PuzzleModel model = new PuzzleModel();

        public MainForm()
        {
            InitializeComponent();
            this.panel1.HorizontalScroll.Visible = true;
            this.boardViewer1.Model = model;

            this.pieceEditor1.CellsChanged += () => { Solve(); };
        }

        private void UpdatePieces()
        {
            var cells = (bool[,])pieceEditor1.Cells.Clone();
            var editorSize = pieceEditor1.GridSize;
            var pieces = new List<polyomino_solver.Piece>();
            while (true)
            {
                // find any cell in ON state
                var start = new polyomino_solver.Point(-1, -1);
                for (int y = 0; y < editorSize.Height; y++)
                {
                    for (int x = 0; x < editorSize.Width; x++)
                        if (cells[x, y])
                        {
                            start = new polyomino_solver.Point(x, y);
                            break;
                        }
                    if (start.X != -1) break;
                }
                if (start.X == -1) break;

                // keep expanding until all covered
                var foundPoints = new List<polyomino_solver.Point>();
                var q = new Queue<polyomino_solver.Point>();
                q.Enqueue(start);
                cells[start.X, start.Y] = false;
                while (q.Count > 0)
                {
                    var p = q.Dequeue();
                    foundPoints.Add(p - start);
                    var newps = new polyomino_solver.Point[] {
                        new polyomino_solver.Point(p.X - 1, p.Y),
                        new polyomino_solver.Point(p.X + 1, p.Y),
                        new polyomino_solver.Point(p.X, p.Y - 1),
                        new polyomino_solver.Point(p.X, p.Y + 1),
                    };
                    foreach (var newp in newps) {
                        if (newp.X >= 0 && newp.X < editorSize.Width && newp.Y >= 0 && newp.Y < editorSize.Height)
                            if (cells[newp.X, newp.Y])
                            {
                                cells[newp.X, newp.Y] = false;
                                q.Enqueue(newp);
                            }
                    }
                }

                var piece = new polyomino_solver.Piece();
                piece.Points = foundPoints.ToArray();
                pieces.Add(piece);
            }

            model.Pieces = pieces.ToArray();
            Console.WriteLine("# pieces found: " + model.Pieces.Length);
        }

        public void Solve()
        {
            UpdatePieces();
            model.Solve();
            boardViewer1.Invalidate();
        }
    }
}
