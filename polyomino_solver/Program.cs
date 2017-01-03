using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polyomino_solver
{
    public struct Point
    {
        public int X, Y;

        public Point(int x, int y)
        {
            X = x; Y = y;
        }

        public override int GetHashCode()
        {
            return (17 * 31 + X.GetHashCode()) * 31 + Y.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var rhs = obj as Point?;
            if (rhs == null || !rhs.HasValue) return false;
            return this.X == rhs.Value.X && this.Y == rhs.Value.Y;
        }

        public static Point operator+(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }
        public static Point operator*(Point p, int s)
        {
            return new Point(p.X * s, p.Y * s);
        }
    }

    public class Piece
    {
        public Point[] Points = new Point[0];
    }

    public class Puzzle
    {
        public Point BoardSize = new Point(8, 6);
        public Piece[] Pieces = new Piece[0];
        public class SolutionPiece
        {
            public Piece Piece;
            public Point Origin;
            public int Turns; // clockwise
            public Point[] Cells;
            /*IEnumerable<Point> Cells
            {
                get
                {
                    Point xaxis, yaxis;
                    switch (turns)
                    {
                        case 1: xaxis = new Point(0, 1); yaxis = new Point(-1, 0); break;
                        case 2: xaxis = new Point(-1, 0); yaxis = new Point(0, -1); break;
                        case 3: xaxis = new Point(0, -1); yaxis = new Point(1, 0); break;
                        case 0:
                        default:
                            xaxis = new Point(1, 0); yaxis = new Point(0, 1); break;
                    }

                    foreach (var p in piece.Points)
                        yield return xaxis * p.X + yaxis * p.Y + origin;
                }
            }*/
        }

        public void InitPieces()
        {
            CreatePieces(@"RDL,RDD,LRUDRL,RRD,DDRU,LRDURL,RU,RDD,RRR,DDD,DDR,RRU");
        }

        public void CreatePieces(string text)
        {
            var pieces = new List<Piece>();
            foreach (var term in text.Split(',').Select(x => x.Trim()))
            {
                if (term == "") continue;
                var piece = new Piece();
                var cursor = new Point(0, 0);
                var cells = new HashSet<Point>();
                cells.Add(cursor);
                foreach (var c in term)
                {
                    var dir = new Point(0, 0);
                    switch (c)
                    {
                        case 'U': dir = new Point(0, -1); break;
                        case 'D': dir = new Point(0, 1); break;
                        case 'L': dir = new Point(-1, 0); break;
                        case 'R': dir = new Point(1, 0); break;
                    }
                    cursor += dir;
                    cells.Add(cursor);
                }
                piece.Points = cells.ToArray();
                pieces.Add(piece);
            }
            this.Pieces = pieces.ToArray();
        }

        public bool CheckPieces
        {
            get
            {
                int count = 0;
                foreach (var piece in Pieces)
                    count += piece.Points.Length;
                //Console.WriteLine("CheckPieces = " + count);
                return count <= BoardSize.X * BoardSize.Y;
            }
        }

        public bool[][] BuildMatrix()
        {
            var result = new List<bool[]>();
            int numCols = Pieces.Length + (BoardSize.X * BoardSize.Y);
            int offset = Pieces.Length;

            for (int pieceIndex = 0; pieceIndex < Pieces.Length; pieceIndex++)
                for (int x = 0; x < BoardSize.X; x++)
                    for (int y = 0; y < BoardSize.Y; y++)
                        for (int r = 0; r < 4; r++)
                        {
                            var piece = Pieces[pieceIndex];
                            var row = new bool[numCols];
                            for (int i = 0; i < row.Length; i++) row[i] = false;
                            row[pieceIndex] = true;

                            // axes
                            Point xaxis, yaxis;
                            switch (r)
                            {
                                case 1: xaxis = new Point(0, 1); yaxis = new Point(-1, 0); break;
                                case 2: xaxis = new Point(-1, 0); yaxis = new Point(0, -1); break;
                                case 3: xaxis = new Point(0, -1); yaxis = new Point(1, 0); break;
                                case 0:
                                default:
                                    xaxis = new Point(1, 0); yaxis = new Point(0, 1); break;
                            }

                            Point origin = new Point(x, y);

                            bool outside = false;
                            foreach (var p in piece.Points)
                            {
                                var cellPos = origin + xaxis * p.X + yaxis * p.Y;
                                if (cellPos.X < 0 || cellPos.X >= BoardSize.X || cellPos.Y < 0 || cellPos.Y >= BoardSize.Y)
                                {
                                    outside = true;
                                    break;
                                }
                                row[offset + cellPos.Y * BoardSize.X + cellPos.X] = true;
                            }
                            if (outside) continue;

                            result.Add(row);
                        }

            return result.ToArray();
        }

        public SolutionPiece[] Solve(bool[][] problem)
        {
            if (!CheckPieces) {
               // Console.WriteLine("CheckPieces failed!");
                return null; }
            var dlx = new DLX();
            dlx.NumPieces = Pieces.Length;
            bool[][] soln = dlx.Solve(problem);
            if (soln == null) return null;

            // convert matrix back to "pieces" form
            var result = new List<SolutionPiece>();
            foreach (var row in soln)
            {
                var spiece = new SolutionPiece();
                Piece piece = null;
                for (int i = 0; i < Pieces.Length; i++)
                    if (row[i])
                    {
                        piece = Pieces[i];
                        break;
                    }

                var cells = new List<Point>();
                int index = Pieces.Length;
                for (int y = 0; y < BoardSize.Y; y++)
                    for (int x = 0; x < BoardSize.X; x++)
                        if (row[index++])
                            cells.Add(new Point(x, y));
                spiece.Piece = piece;
                spiece.Cells = cells.ToArray();
                result.Add(spiece);
            }
            return result.ToArray();
        }

        public void PrintSolution(bool[][] soln)
        {
            foreach (var row in soln)
            {
                Console.WriteLine("-12345678");
                int index = Pieces.Length;
                for (int y = 0; y < BoardSize.Y; y++)
                {
                    Console.Write(y+1);
                    for (int x = 0; x < BoardSize.X; x++)
                        Console.Write(row[index++] ? "+" : " ");
                    Console.WriteLine();
                }
            }

        }
    }

    class DLX
    {
        private class Node
        {
            public Node L = null, R = null, U = null, D = null;
            public ColumnNode Column = null;
        }

        private class ColumnNode : Node
        {
            public string Name = "";
            public int Size = 0;
            public int Index = 0;
        }

        private bool[][] problem;
        public bool[][] Solve(bool[][] problem)
        {
            this.problem = problem;
            if (problem.Length == 0) return new bool[0][];
            this.h = CreateNodes();
            int numCols = problem[0].Length;
            this.solution = new Node[numCols];
            if (Recurse(0))
            {
                //Console.WriteLine("Eureka!");
                //PrintSolution();
                var result = new List<bool[]>();
                for (int i = 0; i < solutionSize; i++)
                {
                    var O = solution[i];
                    var row = new bool[numCols];
                    for (var r = O; true; r = r.R)
                    {
                        row[r.Column.Index] = true;
                        if (r.R == O) break;
                    }
                    result.Add(row);
                }
                return result.ToArray();
            }
            else
            {
                //Console.WriteLine("Fail");
                return null;
            }
        }

        public int NumPieces { get; set; }
        private ColumnNode h;
        private Node[] solution;
        private int solutionSize = 0;
        private bool Recurse(int k)
        {
            if (h.R == h) { solutionSize = k; return true; }

            // check rows
            bool hasRows = false;
            for (var col = h.R; col != h; col = col.R)
            {
                if (col.D != col)
                {
                    hasRows = true;
                    break;
                }
            }
            //if (!hasRows) { solutionSize = k; return true; }

            // all pieces used?
            if (!hasRows && h.R.Column.Index >= NumPieces)
            {
                solutionSize = k; return true;
            }


            // choose a column with minumum 1s
            ColumnNode chosenColumn = (ColumnNode)h.R;
            int minSize = chosenColumn.Size;
            for (var col = (ColumnNode)h.R; col != h; col = (ColumnNode)col.R)
                if (minSize == 0 || (col.Size < minSize && col.Size != 0))
                {
                    chosenColumn = col;
                    minSize = chosenColumn.Size;
                }

            CoverColumn(chosenColumn);
            /*int colcount = 0;
            for (var col = h.R; col != h; col = col.R)
            {
                colcount++;
            }*/
            //Console.WriteLine("k: " + k + ", Columns: " + colcount);
            for (var r = chosenColumn.D; r != chosenColumn; r = r.D)
            {
                solution[k] = r;
                for (var j = r.R; j != r; j = j.R)
                    CoverColumn(j);
                if (Recurse(k + 1)) return true;
                for (var j = r.L; j != r; j = j.L)
                    UncoverColumn(j);
            }
            UncoverColumn(chosenColumn);

            return false;
        }

        private void CoverColumn(Node node)
        {
            ColumnNode c = node.Column;
            c.R.L = c.L; c.L.R = c.R;
            for (var i = c.D; i != c; i = i.D)
            {
                for (var j = i.R; j != i; j = j.R)
                {
                    j.D.U = j.U; j.U.D = j.D;
                    j.Column.Size--;
                }
            }
        }
        private void UncoverColumn(Node node)
        {
            ColumnNode c = node.Column;
            for (var i = c.U; i != c; i = i.U)
            {
                for (var j = i.L; j != i; j = j.L)
                {
                    j.Column.Size++;
                    j.D.U = j; j.U.D = j;
                }
            }
            c.R.L = c; c.L.R = c;
        }

        private ColumnNode CreateNodes()
        {
            var h = new ColumnNode();
            h.R = h;
            h.L = h;
            if (problem.Length == 0) return h;

            int numCols = problem[0].Length;

            // create column headers
            var prev = h;
            for (int i = 0; i < numCols; i++)
            {
                var col = new ColumnNode();
                col.Index = i;
                prev.R = col;
                col.L = prev;
                col.Column = col;
                prev = col;
            }
            prev.R = h;
            h.L = prev;

            // create matrix nodes
            var prevNodes = new Node[numCols];
            int pi = 0;
            for (var col = h.R; col != h; col = col.R)
                prevNodes[pi++] = col;
            foreach (var row in problem)
            {
                pi = 0;
                Node prevCell = null;
                Node startCell = null;
                foreach (var cell in row)
                {
                    if (cell)
                    {
                        var node = new Node();
                        node.U = prevNodes[pi];
                        prevNodes[pi].D = node;
                        if (prevCell != null)
                        {
                            prevCell.R = node;
                            node.L = prevCell;
                        }
                        else
                        {
                            prevCell = node;
                            startCell = node;
                        }
                        node.Column = prevNodes[pi].Column;
                        node.Column.Size++;
                        prevNodes[pi] = node;
                        prevCell = node;
                    }
                    pi++;
                }
                prevCell.R = startCell;
                startCell.L = prevCell;
            }
            for (int i = 0; i < numCols; i++)
            {
                var col = prevNodes[i].Column;
                prevNodes[i].D = col;
                col.U = prevNodes[i];
            }

            return h;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var puzzle = new Puzzle();
            //puzzle.InitPieces();
            puzzle.CreatePieces("LRUDRLDU");
            Console.WriteLine(puzzle.CheckPieces);
            var m = puzzle.BuildMatrix();
            /*using (var fout = new StreamWriter("output.txt"))
            {
                foreach (var row in m)
                {
                    foreach (var cell in row)
                    {
                        fout.Write(cell ? '1' : '0');
                    }
                    fout.WriteLine();
                }
            }*/
            var soln = puzzle.Solve(m);
            Console.ReadKey();
        }
    }
}
