using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeEasyAI : MonoBehaviour, ITicTacToeAI
{
    private int boardSize;
    private System.Random random = new System.Random();

    public void InitializeAI(int size, char aiPlayer, char humanPlayer, int depth)
    {
        this.boardSize = size;
    }

    public (int, int) GetBestMove(char[,] board)
    {
        var availableMoves = new List<(int, int)>();

        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                if (board[row, col] == ' ')
                {
                    availableMoves.Add((row, col));
                }
            }
        }

        if (availableMoves.Count > 0)
        {
            int index = random.Next(availableMoves.Count);
            return availableMoves[index];
        }

        return (-1, -1); // No move found (should not happen in normal gameplay)
    }
}

