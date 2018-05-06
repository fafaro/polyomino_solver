using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ui
{
    class PuzzleModel
    {
        private polyomino_solver.Puzzle puzzle = new polyomino_solver.Puzzle();

        public polyomino_solver.Point BoardSize
        {
            get { return puzzle.BoardSize; }
            set { puzzle.BoardSize = value; }
        }

        public polyomino_solver.Piece[] Pieces
        {
            get { return puzzle.Pieces; }
            set { puzzle.Pieces = value; }
        }

        public void Solve()
        {
            solution = puzzle.Solve(puzzle.BuildMatrix());
        }

        private polyomino_solver.Puzzle.SolutionPiece[] solution = null;
        public polyomino_solver.Puzzle.SolutionPiece[] Solution
        {
            get
            {
                return solution;
            }
        }
    }
}
