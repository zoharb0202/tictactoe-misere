using System;
using System.Windows.Forms;
using TicTacToeMisere.GameLogic;

namespace TicTacToeMisere.WinFormsUI
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();

            GameSettingsForm settingsForm = new GameSettingsForm();

            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                GameManager gameManager = new GameManager(
                    settingsForm.BoardSize,
                    settingsForm.Player1Name,
                    settingsForm.IsPlayer2Computer ? "Computer" : settingsForm.Player2Name,
                    settingsForm.IsPlayer2Computer);

                GameForm gameForm = new GameForm(gameManager);

                Application.Run(gameForm);
            }
        }
    }
}
