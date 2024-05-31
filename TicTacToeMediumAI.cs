using System;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeMediumAI : MonoBehaviour, ITicTacToeAI
{
    private int boardSize;
    private int winCondition;

    public void InitializeAI(int size, char aiPlayer, char humanPlayer, int depth)
    {
        this.boardSize = size;
        winCondition = (size == 3) ? 3 : (size == 6) ? 4 : 5;
    }

    public (int, int) GetBestMove(char[,] board)
    {
        char currentPlayer = 'O';

        // Check for winning move
        var winningMove = FindWinningMove(board, currentPlayer);
        if (winningMove != (-1, -1))
        {
            return winningMove;
        }

        // Check for blocking move
        char opponent = (currentPlayer == 'X') ? 'O' : 'X';
        var blockingMove = FindWinningMove(board, opponent);
        if (blockingMove != (-1, -1))
        {
            return blockingMove;
        }

        // If no winning or blocking move, pick a random move
        return GetRandomMove(board);
    }

    private (int, int) FindWinningMove(char[,] board, char player)
    {
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                if (board[row, col] == ' ')
                {
                    board[row, col] = player;
                    if (IsWinningBoard(board, player))
                    {
                        board[row, col] = ' ';
                        return (row, col);
                    }
                    board[row, col] = ' ';
                }
            }
        }
        return (-1, -1);
    }

    private (int, int) GetRandomMove(char[,] board)
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
            int index = new System.Random().Next(availableMoves.Count);
            return availableMoves[index];
        }
        return (-1, -1); // No move found (should not happen in normal gameplay)
    }

    private bool IsWinningBoard(char[,] board, char player)
    {
        // Check rows, columns, and diagonals for a win
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col <= boardSize - winCondition; col++)
            {
                if (IsWinningLine(board, player, row, col, 0, 1))
                {
                    return true;
                }
            }
        }
        for (int col = 0; col < boardSize; col++)
        {
            for (int row = 0; row <= boardSize - winCondition; row++)
            {
                if (IsWinningLine(board, player, row, col, 1, 0))
                {
                    return true;
                }
            }
        }
        for (int row = 0; row <= boardSize - winCondition; row++)
        {
            for (int col = 0; col <= boardSize - winCondition; col++)
            {
                if (IsWinningLine(board, player, row, col, 1, 1) || IsWinningLine(board, player, row, col + winCondition - 1, 1, -1))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsWinningLine(char[,] board, char player, int startRow, int startCol, int stepRow, int stepCol)
    {
        for (int i = 0; i < winCondition; i++)
        {
            if (board[startRow + i * stepRow, startCol + i * stepCol] != player)
            {
                return false;
            }
        }
        return true;
    }
}
