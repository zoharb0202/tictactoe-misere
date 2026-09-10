using TicTacToeMisere.GameLogic;
using Xunit;

namespace TicTacToeMisere.Tests
{
    public class GameManagerTests
    {
        [Fact]
        public void CompletingALine_LosesTheRound()
        {
            GameManager game = new GameManager(4, "Alice", "Bob", false);
            string error;

            // Alice (X) fills row 0, Bob (O) plays on row 1
            game.TryMakeMove(0, 0, out error);
            game.TryMakeMove(1, 0, out error);
            game.TryMakeMove(0, 1, out error);
            game.TryMakeMove(1, 1, out error);
            game.TryMakeMove(0, 2, out error);
            game.TryMakeMove(1, 3, out error);
            game.TryMakeMove(0, 3, out error);

            Assert.Equal(eGameStatus.Win, game.Status);
            Assert.Equal("Bob", game.Winner.Name);
            Assert.Equal(1, game.Player2.Score);
            Assert.Equal(0, game.Player1.Score);
        }

        [Fact]
        public void MoveOnOccupiedCell_IsRejected()
        {
            GameManager game = new GameManager(4, "Alice", "Bob", false);
            string error;

            game.TryMakeMove(2, 2, out error);
            bool succeeded = game.TryMakeMove(2, 2, out error);

            Assert.False(succeeded);
            Assert.Equal("Cell is already occupied", error);
            Assert.Equal("Bob", game.CurrentPlayer.Name);
        }

        [Theory]
        [InlineData(3, false)]
        [InlineData(4, true)]
        [InlineData(10, true)]
        [InlineData(11, false)]
        public void BoardSizeValidation(int size, bool expected)
        {
            Assert.Equal(expected, GameManager.IsBoardSizeValid(size));
        }
    }
}
