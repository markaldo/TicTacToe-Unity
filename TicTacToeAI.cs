using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeAI
{
    public Move GetBestMove(char[,] board)
    {
        int bestScore = int.MinValue;
        Move bestMove = null;

        // Iterate through all empty cells on the board
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == '-')
                {
                    // Make a hypothetical move for the AI player
                    board[i, j] = 'O';

                    // Calculate the score for this move using minimax
                    int score = Minimax(board, false);

                    // Undo the hypothetical move
                    board[i, j] = '-';

                    // Update the best move if this move has a higher score
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = new Move(i, j);
                    }
                }
            }
        }

        return bestMove;
    }

    private int Minimax(char[,] board, bool isMaximizingPlayer)
    {
        // Base cases: check for terminal states (win, lose, draw)
        if (CheckWin(board, 'O')) // AI wins
            return 1;
        else if (CheckWin(board, 'X')) // Player wins
            return -1;
        else if (IsBoardFull(board)) // Draw
            return 0;

        // Recursive case: search through possible future game states
        if (isMaximizingPlayer) // AI's turn
        {
            int bestScore = int.MinValue;
            // Iterate through all empty cells on the board
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == '-')
                    {
                        // Make a hypothetical move for the AI player
                        board[i, j] = 'O';
                        // Calculate the score for this move
                        int score = Minimax(board, false);
                        // Undo the hypothetical move
                        board[i, j] = '-';
                        // Update the best score
                        bestScore = Mathf.Max(bestScore, score);
                    }
                }
            }
            return bestScore;
        }
        else // Player's turn
        {
            int bestScore = int.MaxValue;
            // Iterate through all empty cells on the board
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == '-')
                    {
                        // Make a hypothetical move for the player
                        board[i, j] = 'X';
                        // Calculate the score for this move
                        int score = Minimax(board, true);
                        // Undo the hypothetical move
                        board[i, j] = '-';
                        // Update the best score
                        bestScore = Mathf.Min(bestScore, score);
                    }
                }
            }
            return bestScore;
        }
    }

    private bool CheckWin(char[,] board, char player)
    {
        // Check rows, columns, and diagonals for a win
        for (int i = 0; i < 3; i++)
        {
            if ((board[i, 0] == player && board[i, 1] == player && board[i, 2] == player) ||
                (board[0, i] == player && board[1, i] == player && board[2, i] == player))
            {
                return true;
            }
        }
        if ((board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) ||
            (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player))
        {
            return true;
        }
        return false;
    }

    private bool IsBoardFull(char[,] board)
    {
        // Check if the board is full
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == '-')
                {
                    return false;
                }
            }
        }
        return true;
    }
}



