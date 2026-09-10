using TicTacToeMisere.GameLogic;
using Xunit;

namespace TicTacToeMisere.Tests
{
    public class BoardTests
    {
        [Fact]
        public void FullRow_IsDetected()
        {
            Board board = new Board(4);

            for (int col = 0; col < 4; col++)
            {
                board.PlaceSign(1, col, eCellSign.X);
            }

            Assert.True(board.HasWinningSequence(1, 3, eCellSign.X));
        }

        [Fact]
        public void FullAntiDiagonal_IsDetected()
        {
            Board board = new Board(5);

            for (int i = 0; i < 5; i++)
            {
                board.PlaceSign(i, 4 - i, eCellSign.O);
            }

            Assert.True(board.HasWinningSequence(2, 2, eCellSign.O));
        }

        [Fact]
        public void MixedRow_IsNotASequence()
        {
            Board board = new Board(4);

            board.PlaceSign(0, 0, eCellSign.X);
            board.PlaceSign(0, 1, eCellSign.X);
            board.PlaceSign(0, 2, eCellSign.O);
            board.PlaceSign(0, 3, eCellSign.X);

            Assert.False(board.HasWinningSequence(0, 3, eCellSign.X));
        }

        [Fact]
        public void PlacingOnOccupiedCell_IsIgnored()
        {
            Board board = new Board(4);

            board.PlaceSign(0, 0, eCellSign.X);
            board.PlaceSign(0, 0, eCellSign.O);

            Assert.Equal(eCellSign.X, board.GetCellSign(0, 0));
            Assert.Equal(15, board.GetEmptyCells().Count);
        }

        [Fact]
        public void Reset_ClearsBoardAndRaisesEvents()
        {
            Board board = new Board(4);
            int clearedCells = 0;

            board.PlaceSign(0, 0, eCellSign.X);
            board.PlaceSign(3, 3, eCellSign.O);
            board.CellChanged += (row, col, sign) =>
            {
                if (sign == eCellSign.Empty)
                {
                    clearedCells++;
                }
            };
            board.Reset();

            Assert.Equal(2, clearedCells);
            Assert.Equal(16, board.GetEmptyCells().Count);
        }
    }
}
