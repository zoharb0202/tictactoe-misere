namespace TicTacToeMisere.GameLogic
{
    public class GameManager
    {
        private const int k_MinBoardSize = 4;
        private const int k_MaxBoardSize = 10;

        private readonly Board r_Board;
        private readonly Player r_Player1;
        private readonly Player r_Player2;
        private readonly ComputerAI r_ComputerAI;
        private Player m_CurrentPlayer;
        private Player m_Winner;
        private eGameStatus m_Status;

        public GameManager(int i_BoardSize, string i_Player1Name, string i_Player2Name, bool i_IsPlayer2Computer)
        {
            ePlayerType player2Type = i_IsPlayer2Computer ? ePlayerType.Computer : ePlayerType.Human;

            r_Board = new Board(i_BoardSize);
            r_Player1 = new Player(i_Player1Name, eCellSign.X, ePlayerType.Human);
            r_Player2 = new Player(i_Player2Name, eCellSign.O, player2Type);
            r_ComputerAI = new ComputerAI();
            m_CurrentPlayer = r_Player1;
            m_Winner = null;
            m_Status = eGameStatus.InProgress;
        }

        public static int MinBoardSize
        {
            get
            {
                return k_MinBoardSize;
            }
        }

        public static int MaxBoardSize
        {
            get
            {
                return k_MaxBoardSize;
            }
        }

        public Board Board
        {
            get
            {
                return r_Board;
            }
        }

        public Player Player1
        {
            get
            {
                return r_Player1;
            }
        }

        public Player Player2
        {
            get
            {
                return r_Player2;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return m_CurrentPlayer;
            }
        }

        public Player Winner
        {
            get
            {
                return m_Winner;
            }
        }

        public eGameStatus Status
        {
            get
            {
                return m_Status;
            }
        }

        public bool IsCurrentPlayerComputer
        {
            get
            {
                return m_CurrentPlayer.IsComputer;
            }
        }

        public bool TryMakeMove(int i_Row, int i_Col, out string o_ErrorMessage)
        {
            bool moveSucceeded = false;

            o_ErrorMessage = string.Empty;

            if (m_Status != eGameStatus.InProgress)
            {
                o_ErrorMessage = "Game is not in progress";
            }
            else if (!r_Board.IsInBounds(i_Row, i_Col))
            {
                o_ErrorMessage = "Cell is out of board bounds";
            }
            else if (!r_Board.IsCellEmpty(i_Row, i_Col))
            {
                o_ErrorMessage = "Cell is already occupied";
            }
            else
            {
                r_Board.PlaceSign(i_Row, i_Col, m_CurrentPlayer.Sign);
                updateGameStatusAfterMove(i_Row, i_Col);
                moveSucceeded = true;
            }

            return moveSucceeded;
        }

        public int[] GetComputerMove()
        {
            return r_ComputerAI.ChooseMove(r_Board, m_CurrentPlayer.Sign, getOpponentPlayer().Sign);
        }

        public void StartNewRound()
        {
            r_Board.Reset();
            m_CurrentPlayer = r_Player1;
            m_Winner = null;
            m_Status = eGameStatus.InProgress;
        }

        public static bool IsBoardSizeValid(int i_Size)
        {
            return i_Size >= k_MinBoardSize && i_Size <= k_MaxBoardSize;
        }

        private void updateGameStatusAfterMove(int i_Row, int i_Col)
        {
            bool createdWinningSequence = r_Board.HasWinningSequence(i_Row, i_Col, m_CurrentPlayer.Sign);

            if (createdWinningSequence)
            {
                m_Status = eGameStatus.Win;
                m_Winner = getOpponentPlayer();
                m_Winner.AddPoint();
            }
            else if (r_Board.IsFull)
            {
                m_Status = eGameStatus.Tie;
            }
            else
            {
                switchCurrentPlayer();
            }
        }

        private void switchCurrentPlayer()
        {
            m_CurrentPlayer = (m_CurrentPlayer == r_Player1) ? r_Player2 : r_Player1;
        }

        private Player getOpponentPlayer()
        {
            return (m_CurrentPlayer == r_Player1) ? r_Player2 : r_Player1;
        }
    }
}
