using System.Collections.Generic;
using TicTacToeMisere.GameLogic;
using Xunit;

namespace TicTacToeMisere.Tests
{
    public class ComputerAITests
    {
        [Fact]
        public void NeverCompletesItsOwnLine_WhenASafeMoveExists()
        {
            // Row 0 has three O's: playing (0,3) would lose for O
            Board board = new Board(4);
            board.PlaceSign(0, 0, eCellSign.O);
            board.PlaceSign(0, 1, eCellSign.O);
            board.PlaceSign(0, 2, eCellSign.O);
            ComputerAI ai = new ComputerAI();

            for (int attempt = 0; attempt < 50; attempt++)
            {
                int[] move = ai.ChooseMove(board, eCellSign.O, eCellSign.X);

                Assert.False(move[0] == 0 && move[1] == 3);
            }
        }

        [Fact]
        public void LeavesTheBoardUnchanged()
        {
            Board board = new Board(4);
            board.PlaceSign(1, 1, eCellSign.X);
            board.PlaceSign(2, 2, eCellSign.O);
            ComputerAI ai = new ComputerAI();

            ai.ChooseMove(board, eCellSign.O, eCellSign.X);

            Assert.Equal(14, board.GetEmptyCells().Count);
            Assert.Equal(eCellSign.X, board.GetCellSign(1, 1));
            Assert.Equal(eCellSign.O, board.GetCellSign(2, 2));
        }

        [Fact]
        public void ReturnsAnEmptyCell()
        {
            Board board = new Board(5);
            board.PlaceSign(0, 0, eCellSign.X);
            ComputerAI ai = new ComputerAI();

            int[] move = ai.ChooseMove(board, eCellSign.O, eCellSign.X);

            Assert.True(board.IsCellEmpty(move[0], move[1]));
        }
    }
}
