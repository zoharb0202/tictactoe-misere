namespace TicTacToeMisere.GameLogic
{
    public class Player
    {
        private readonly string r_Name;
        private readonly eCellSign r_Sign;
        private readonly ePlayerType r_PlayerType;
        private int m_Score;

        public Player(string i_Name, eCellSign i_Sign, ePlayerType i_PlayerType)
        {
            r_Name = i_Name;
            r_Sign = i_Sign;
            r_PlayerType = i_PlayerType;
            m_Score = 0;
        }

        public string Name
        {
            get
            {
                return r_Name;
            }
        }

        public eCellSign Sign
        {
            get
            {
                return r_Sign;
            }
        }

        public ePlayerType PlayerType
        {
            get
            {
                return r_PlayerType;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }
        }

        public bool IsComputer
        {
            get
            {
                return r_PlayerType == ePlayerType.Computer;
            }
        }

        public void AddPoint()
        {
            m_Score++;
        }
    }
}
