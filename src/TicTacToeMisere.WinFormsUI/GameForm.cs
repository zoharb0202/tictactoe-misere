using System;
using System.Drawing;
using System.Windows.Forms;
using TicTacToeMisere.GameLogic;

namespace TicTacToeMisere.WinFormsUI
{
    public class GameForm : Form
    {
        private const int k_ButtonSize = 45;
        private const int k_ButtonMargin = 5;
        private const int k_BoardPadding = 10;
        private const int k_ScoreAreaHeight = 35;

        private readonly GameManager r_GameManager;
        private readonly Button[,] r_BoardButtons;
        private readonly Label r_LabelPlayer1Score;
        private readonly Label r_LabelPlayer2Score;

        public GameForm(GameManager i_GameManager)
        {
            r_GameManager = i_GameManager;
            r_BoardButtons = new Button[i_GameManager.Board.Size, i_GameManager.Board.Size];
            r_LabelPlayer1Score = new Label();
            r_LabelPlayer2Score = new Label();

            initializeComponents();
            r_GameManager.Board.CellChanged += board_CellChanged;
        }

        private void initializeComponents()
        {
            this.Text = "TicTacToeMisere";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            initializeBoardButtons();
            initializeScoreLabels();
            setFormSizeByBoard();
            updateScoreLabels();
        }

        private void initializeBoardButtons()
        {
            int boardSize = r_GameManager.Board.Size;

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    Button cellButton = new Button();

                    cellButton.Size = new Size(k_ButtonSize, k_ButtonSize);
                    cellButton.Location = new Point(k_BoardPadding + (col * (k_ButtonSize + k_ButtonMargin)), k_BoardPadding + (row * (k_ButtonSize + k_ButtonMargin)));
                    cellButton.Tag = new int[] { row, col };
                    cellButton.Click += boardButton_Click;

                    r_BoardButtons[row, col] = cellButton;
                    this.Controls.Add(cellButton);
                }
            }
        }

        private void initializeScoreLabels()
        {
            int boardSize = r_GameManager.Board.Size;
            int boardPixelHeight = k_BoardPadding + (boardSize * (k_ButtonSize + k_ButtonMargin));

            r_LabelPlayer1Score.AutoSize = true;
            r_LabelPlayer1Score.Font = new Font(this.Font, FontStyle.Bold);
            r_LabelPlayer1Score.Location = new Point(k_BoardPadding + 20, boardPixelHeight + 8);

            r_LabelPlayer2Score.AutoSize = true;
            r_LabelPlayer2Score.Location = new Point(k_BoardPadding + 130, boardPixelHeight + 8);

            this.Controls.Add(r_LabelPlayer1Score);
            this.Controls.Add(r_LabelPlayer2Score);
        }

        private void setFormSizeByBoard()
        {
            int boardSize = r_GameManager.Board.Size;
            int boardPixelSize = (2 * k_BoardPadding) + (boardSize * (k_ButtonSize + k_ButtonMargin)) - k_ButtonMargin;

            this.ClientSize = new Size(boardPixelSize, boardPixelSize + k_ScoreAreaHeight);
        }

        private void board_CellChanged(int i_Row, int i_Col, eCellSign i_NewSign)
        {
            Button cellButton = r_BoardButtons[i_Row, i_Col];

            cellButton.Text = signToText(i_NewSign);
            cellButton.Enabled = i_NewSign == eCellSign.Empty;
        }

        private string signToText(eCellSign i_Sign)
        {
            string signText = string.Empty;

            if (i_Sign == eCellSign.X)
            {
                signText = "X";
            }
            else if (i_Sign == eCellSign.O)
            {
                signText = "O";
            }

            return signText;
        }

        private void boardButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton != null && r_GameManager.Status == eGameStatus.InProgress)
            {
                int[] cellPosition = (int[])clickedButton.Tag;
                string errorMessage;
                bool moveSucceeded = r_GameManager.TryMakeMove(cellPosition[0], cellPosition[1], out errorMessage);

                if (moveSucceeded)
                {
                    continueGameFlowAfterMove();
                }
            }
        }

        private void continueGameFlowAfterMove()
        {
            if (r_GameManager.Status == eGameStatus.InProgress && r_GameManager.IsCurrentPlayerComputer)
            {
                playComputerTurn();
            }

            if (r_GameManager.Status != eGameStatus.InProgress)
            {
                announceRoundResult();
            }
        }

        private void playComputerTurn()
        {
            int[] computerMove = r_GameManager.GetComputerMove();
            string errorMessage;

            r_GameManager.TryMakeMove(computerMove[0], computerMove[1], out errorMessage);
        }

        private void announceRoundResult()
        {
            updateScoreLabels();

            string resultMessage = buildResultMessage();
            string messageTitle = r_GameManager.Status == eGameStatus.Win ? "A Win!" : "A Tie!";
            DialogResult userAnswer = MessageBox.Show(resultMessage, messageTitle, MessageBoxButtons.YesNo);

            if (userAnswer == DialogResult.Yes)
            {
                startNewRound();
            }
            else
            {
                this.Close();
            }
        }

        private string buildResultMessage()
        {
            string resultMessage;

            if (r_GameManager.Status == eGameStatus.Win)
            {
                resultMessage = string.Format(
                    "The winner is {0}!{1}Would you like to play another round?", r_GameManager.Winner.Name, Environment.NewLine);
            }
            else
            {
                resultMessage = string.Format("Tie!{0}Would you like to play another round?", Environment.NewLine);
            }

            return resultMessage;
        }

        private void startNewRound()
        {
            r_GameManager.StartNewRound();
            updateScoreLabels();
        }

        private void updateScoreLabels()
        {
            r_LabelPlayer1Score.Text = string.Format("{0}: {1}", r_GameManager.Player1.Name, r_GameManager.Player1.Score);
            r_LabelPlayer2Score.Text = string.Format("{0}: {1}", r_GameManager.Player2.Name, r_GameManager.Player2.Score);
        }
    }
}
