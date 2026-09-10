using System;
using System.Collections.Generic;

namespace TicTacToeMisere.GameLogic
{
    public delegate void CellChangedEventHandler(int i_Row, int i_Col, eCellSign i_NewSign);

    public class Board
    {
        private readonly int r_Size;
        private readonly eCellSign[,] r_Cells;
        private int m_EmptyCellsCount;

        public event CellChangedEventHandler CellChanged;

        public Board(int i_Size)
        {
            r_Size = i_Size;
            r_Cells = new eCellSign[i_Size, i_Size];
            m_EmptyCellsCount = i_Size * i_Size;
        }

        public int Size
        {
            get
            {
                return r_Size;
            }
        }

        public bool IsFull
        {
            get
            {
                return m_EmptyCellsCount == 0;
            }
        }

        public eCellSign GetCellSign(int i_Row, int i_Col)
        {
            return r_Cells[i_Row, i_Col];
        }

        public bool IsCellEmpty(int i_Row, int i_Col)
        {
            return r_Cells[i_Row, i_Col] == eCellSign.Empty;
        }

        public bool IsInBounds(int i_Row, int i_Col)
        {
            return i_Row >= 0 && i_Row < r_Size && i_Col >= 0 && i_Col < r_Size;
        }

        public void PlaceSign(int i_Row, int i_Col, eCellSign i_Sign)
        {
            if (IsCellEmpty(i_Row, i_Col) && i_Sign != eCellSign.Empty)
            {
                r_Cells[i_Row, i_Col] = i_Sign;
                m_EmptyCellsCount--;
                OnCellChanged(i_Row, i_Col, i_Sign);
            }
        }

        public void PlaceSignSilently(int i_Row, int i_Col, eCellSign i_Sign)
        {
            if (IsCellEmpty(i_Row, i_Col) && i_Sign != eCellSign.Empty)
            {
                r_Cells[i_Row, i_Col] = i_Sign;
                m_EmptyCellsCount--;
            }
        }

        public void ClearCell(int i_Row, int i_Col)
        {
            if (!IsCellEmpty(i_Row, i_Col))
            {
                r_Cells[i_Row, i_Col] = eCellSign.Empty;
                m_EmptyCellsCount++;
            }
        }

        public List<int[]> GetEmptyCells()
        {
            List<int[]> emptyCells = new List<int[]>();

            for (int row = 0; row < r_Size; row++)
            {
                for (int col = 0; col < r_Size; col++)
                {
                    if (IsCellEmpty(row, col))
                    {
                        emptyCells.Add(new int[] { row, col });
                    }
                }
            }

            return emptyCells;
        }

        public void Reset()
        {
            for (int row = 0; row < r_Size; row++)
            {
                for (int col = 0; col < r_Size; col++)
                {
                    if (!IsCellEmpty(row, col))
                    {
                        r_Cells[row, col] = eCellSign.Empty;
                        OnCellChanged(row, col, eCellSign.Empty);
                    }
                }
            }

            m_EmptyCellsCount = r_Size * r_Size;
        }

        public bool HasWinningSequence(int i_LastRow, int i_LastCol, eCellSign i_Sign)
        {
            bool hasWinningSequence = false;

            if (i_Sign != eCellSign.Empty)
            {
                hasWinningSequence = checkLineForSign(i_LastRow, 0, 0, 1, i_Sign);

                if (!hasWinningSequence)
                {
                    hasWinningSequence = checkLineForSign(0, i_LastCol, 1, 0, i_Sign);
                }

                if (!hasWinningSequence && i_LastRow == i_LastCol)
                {
                    hasWinningSequence = checkLineForSign(0, 0, 1, 1, i_Sign);
                }

                if (!hasWinningSequence && (i_LastRow + i_LastCol) == r_Size - 1)
                {
                    hasWinningSequence = checkLineForSign(0, r_Size - 1, 1, -1, i_Sign);
                }
            }

            return hasWinningSequence;
        }

        protected virtual void OnCellChanged(int i_Row, int i_Col, eCellSign i_NewSign)
        {
            if (CellChanged != null)
            {
                CellChanged.Invoke(i_Row, i_Col, i_NewSign);
            }
        }

        private bool checkLineForSign(int i_StartRow, int i_StartCol, int i_DeltaRow, int i_DeltaCol, eCellSign i_Sign)
        {
            bool allCellsHaveSameSign = true;
            int row = i_StartRow;
            int col = i_StartCol;

            for (int step = 0; step < r_Size; step++)
            {
                if (r_Cells[row, col] != i_Sign)
                {
                    allCellsHaveSameSign = false;
                    break;
                }

                row += i_DeltaRow;
                col += i_DeltaCol;
            }

            return allCellsHaveSameSign;
        }
    }
}
