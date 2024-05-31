using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeAIII : MonoBehaviour
{
    private int boardSize;

    public void InitializeAI(int size)
    {
        boardSize = size;
    }

    public (int, int) GetBestMove(char[,] board)
    {
        List<(int, int)> availableMoves = new List<(int, int)>();

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

        if (availableMoves.Count == 0)
        {
            return (-1, -1); // No valid moves
        }

        int randomIndex = UnityEngine.Random.Range(0, availableMoves.Count);
        return availableMoves[randomIndex];
    }
}



