using System;
using System.Drawing;
using System.Windows.Forms;
using TicTacToeMisere.GameLogic;

namespace TicTacToeMisere.WinFormsUI
{
    public class GameSettingsForm : Form
    {
        private const string k_ComputerName = "[Computer]";

        private readonly Label r_LabelPlayers;
        private readonly Label r_LabelPlayer1;
        private readonly Label r_LabelBoardSize;
        private readonly Label r_LabelRows;
        private readonly Label r_LabelCols;
        private readonly TextBox r_TextBoxPlayer1;
        private readonly TextBox r_TextBoxPlayer2;
        private readonly CheckBox r_CheckBoxPlayer2;
        private readonly NumericUpDown r_NumericUpDownRows;
        private readonly NumericUpDown r_NumericUpDownCols;
        private readonly Button r_ButtonStart;

        public GameSettingsForm()
        {
            r_LabelPlayers = new Label();
            r_LabelPlayer1 = new Label();
            r_LabelBoardSize = new Label();
            r_LabelRows = new Label();
            r_LabelCols = new Label();
            r_TextBoxPlayer1 = new TextBox();
            r_TextBoxPlayer2 = new TextBox();
            r_CheckBoxPlayer2 = new CheckBox();
            r_NumericUpDownRows = new NumericUpDown();
            r_NumericUpDownCols = new NumericUpDown();
            r_ButtonStart = new Button();

            initializeComponents();
        }

        public string Player1Name
        {
            get
            {
                return r_TextBoxPlayer1.Text.Trim();
            }
        }

        public string Player2Name
        {
            get
            {
                return r_TextBoxPlayer2.Text.Trim();
            }
        }

        public bool IsPlayer2Computer
        {
            get
            {
                return !r_CheckBoxPlayer2.Checked;
            }
        }

        public int BoardSize
        {
            get
            {
                return (int)r_NumericUpDownRows.Value;
            }
        }

        private void initializeComponents()
        {
            this.Text = "Game Settings";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(280, 220);

            initializePlayersSection();
            initializeBoardSizeSection();
            initializeStartButton();
        }

        private void initializePlayersSection()
        {
            r_LabelPlayers.Text = "Players:";
            r_LabelPlayers.Location = new Point(15, 15);
            r_LabelPlayers.AutoSize = true;

            r_LabelPlayer1.Text = "Player 1:";
            r_LabelPlayer1.Location = new Point(30, 45);
            r_LabelPlayer1.AutoSize = true;

            r_TextBoxPlayer1.Location = new Point(120, 42);
            r_TextBoxPlayer1.Width = 130;

            r_CheckBoxPlayer2.Text = "Player 2:";
            r_CheckBoxPlayer2.Location = new Point(30, 73);
            r_CheckBoxPlayer2.AutoSize = true;
            r_CheckBoxPlayer2.CheckedChanged += checkBoxPlayer2_CheckedChanged;

            r_TextBoxPlayer2.Location = new Point(120, 70);
            r_TextBoxPlayer2.Width = 130;
            r_TextBoxPlayer2.Text = k_ComputerName;
            r_TextBoxPlayer2.Enabled = false;

            this.Controls.Add(r_LabelPlayers);
            this.Controls.Add(r_LabelPlayer1);
            this.Controls.Add(r_TextBoxPlayer1);
            this.Controls.Add(r_CheckBoxPlayer2);
            this.Controls.Add(r_TextBoxPlayer2);
        }

        private void initializeBoardSizeSection()
        {
            r_LabelBoardSize.Text = "Board Size:";
            r_LabelBoardSize.Location = new Point(15, 110);
            r_LabelBoardSize.AutoSize = true;

            r_LabelRows.Text = "Rows:";
            r_LabelRows.Location = new Point(30, 140);
            r_LabelRows.AutoSize = true;

            r_NumericUpDownRows.Location = new Point(75, 137);
            r_NumericUpDownRows.Width = 45;
            r_NumericUpDownRows.Minimum = GameManager.MinBoardSize;
            r_NumericUpDownRows.Maximum = GameManager.MaxBoardSize;
            r_NumericUpDownRows.ValueChanged += numericUpDownRows_ValueChanged;

            r_LabelCols.Text = "Cols:";
            r_LabelCols.Location = new Point(145, 140);
            r_LabelCols.AutoSize = true;

            r_NumericUpDownCols.Location = new Point(185, 137);
            r_NumericUpDownCols.Width = 45;
            r_NumericUpDownCols.Minimum = GameManager.MinBoardSize;
            r_NumericUpDownCols.Maximum = GameManager.MaxBoardSize;
            r_NumericUpDownCols.ValueChanged += numericUpDownCols_ValueChanged;

            this.Controls.Add(r_LabelBoardSize);
            this.Controls.Add(r_LabelRows);
            this.Controls.Add(r_NumericUpDownRows);
            this.Controls.Add(r_LabelCols);
            this.Controls.Add(r_NumericUpDownCols);
        }

        private void initializeStartButton()
        {
            r_ButtonStart.Text = "Start!";
            r_ButtonStart.Location = new Point(75, 180);
            r_ButtonStart.Width = 130;
            r_ButtonStart.Click += buttonStart_Click;

            this.Controls.Add(r_ButtonStart);
        }

        private void checkBoxPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            r_TextBoxPlayer2.Enabled = r_CheckBoxPlayer2.Checked;

            if (r_CheckBoxPlayer2.Checked)
            {
                r_TextBoxPlayer2.Text = string.Empty;
            }
            else
            {
                r_TextBoxPlayer2.Text = k_ComputerName;
            }
        }

        private void numericUpDownRows_ValueChanged(object sender, EventArgs e)
        {
            r_NumericUpDownCols.Value = r_NumericUpDownRows.Value;
        }

        private void numericUpDownCols_ValueChanged(object sender, EventArgs e)
        {
            r_NumericUpDownRows.Value = r_NumericUpDownCols.Value;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (validateSettings())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool validateSettings()
        {
            bool settingsAreValid = true;

            if (Player1Name.Length == 0)
            {
                MessageBox.Show("Please enter a name for Player 1.", "Missing Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                settingsAreValid = false;
            }
            else if (r_CheckBoxPlayer2.Checked && Player2Name.Length == 0)
            {
                MessageBox.Show("Please enter a name for Player 2.", "Missing Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                settingsAreValid = false;
            }

            return settingsAreValid;
        }
    }
}
