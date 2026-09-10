using System;
using System.Collections.Generic;

namespace TicTacToeMisere.GameLogic
{
    public class ComputerAI
    {
        private readonly Random r_Random;

        public ComputerAI()
        {
            r_Random = new Random();
        }

        public int[] ChooseMove(Board i_Board, eCellSign i_ComputerSign, eCellSign i_OpponentSign)
        {
            List<int[]> emptyCells = i_Board.GetEmptyCells();
            List<int[]> safeMoves = new List<int[]>();
            List<int[]> goodMoves = new List<int[]>();
            List<int[]> losingMoves = new List<int[]>();

            for (int i = 0; i < emptyCells.Count; ++i)
            {
                int[] cell = emptyCells[i];
                int row = cell[0];
                int col = cell[1];

                i_Board.PlaceSignSilently(row, col, i_ComputerSign);
                bool isLosingMove = i_Board.HasWinningSequence(row, col, i_ComputerSign);
                i_Board.ClearCell(row, col);

                if (isLosingMove)
                {
                    losingMoves.Add(cell);
                }
                else if (checkIfOpponentMustLose(i_Board, row, col, i_ComputerSign, i_OpponentSign))
                {
                    goodMoves.Add(cell);
                }
                else
                {
                    safeMoves.Add(cell);
                }
            }

            int[] chosenMove;

            if (goodMoves.Count > 0)
            {
                chosenMove = goodMoves[r_Random.Next(goodMoves.Count)];
            }
            else if (safeMoves.Count > 0)
            {
                chosenMove = chooseMoveWithFewestOpponentSafeMoves(i_Board, safeMoves, i_ComputerSign, i_OpponentSign);
            }
            else
            {
                chosenMove = losingMoves[r_Random.Next(losingMoves.Count)];
            }

            return chosenMove;
        }

        private bool checkIfOpponentMustLose(Board i_Board, int i_Row, int i_Col, eCellSign i_ComputerSign, eCellSign i_OpponentSign)
        {
            bool forcedLoss;

            i_Board.PlaceSignSilently(i_Row, i_Col, i_ComputerSign);

            List<int[]> opponentOptions = i_Board.GetEmptyCells();

            if (opponentOptions.Count == 0)
            {
                forcedLoss = false;
            }
            else
            {
                forcedLoss = true;

                for (int i = 0; i < opponentOptions.Count; ++i)
                {
                    int[] opponentCell = opponentOptions[i];
                    int opponentRow = opponentCell[0];
                    int opponentCol = opponentCell[1];

                    i_Board.PlaceSignSilently(opponentRow, opponentCol, i_OpponentSign);
                    bool opponentLoses = i_Board.HasWinningSequence(opponentRow, opponentCol, i_OpponentSign);
                    i_Board.ClearCell(opponentRow, opponentCol);

                    if (!opponentLoses)
                    {
                        forcedLoss = false;
                        break;
                    }
                }
            }

            i_Board.ClearCell(i_Row, i_Col);

            return forcedLoss;
        }

        private int countOpponentSafeMoves(Board i_Board, int i_Row, int i_Col, eCellSign i_ComputerSign, eCellSign i_OpponentSign)
        {
            int safeMovesCount = 0;

            i_Board.PlaceSignSilently(i_Row, i_Col, i_ComputerSign);

            List<int[]> opponentOptions = i_Board.GetEmptyCells();

            for (int i = 0; i < opponentOptions.Count; ++i)
            {
                int[] opponentCell = opponentOptions[i];
                int opponentRow = opponentCell[0];
                int opponentCol = opponentCell[1];

                i_Board.PlaceSignSilently(opponentRow, opponentCol, i_OpponentSign);

                if (!i_Board.HasWinningSequence(opponentRow, opponentCol, i_OpponentSign))
                {
                    ++safeMovesCount;
                }

                i_Board.ClearCell(opponentRow, opponentCol);
            }

            i_Board.ClearCell(i_Row, i_Col);

            return safeMovesCount;
        }

        private int[] chooseMoveWithFewestOpponentSafeMoves(Board i_Board, List<int[]> i_SafeMoves, eCellSign i_ComputerSign, eCellSign i_OpponentSign)
        {
            List<int[]> bestMoves = new List<int[]>();
            int bestSafeMovesCount = int.MaxValue;

            for (int i = 0; i < i_SafeMoves.Count; ++i)
            {
                int[] move = i_SafeMoves[i];
                int opponentSafeMovesCount = countOpponentSafeMoves(i_Board, move[0], move[1], i_ComputerSign, i_OpponentSign);

                if (opponentSafeMovesCount < bestSafeMovesCount)
                {
                    bestMoves.Clear();
                    bestMoves.Add(move);
                    bestSafeMovesCount = opponentSafeMovesCount;
                }
                else if (opponentSafeMovesCount == bestSafeMovesCount)
                {
                    bestMoves.Add(move);
                }
            }

            return bestMoves[r_Random.Next(bestMoves.Count)];
        }
    }
}
