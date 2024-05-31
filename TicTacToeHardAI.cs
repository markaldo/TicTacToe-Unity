using System;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeHardAI : MonoBehaviour, ITicTacToeAI
{
    private int boardSize;
    private int winCondition;
    private char aiPlayer;
    private char humanPlayer;
    private int maxDepth;
    private Dictionary<string, int> transpositionTable = new Dictionary<string, int>();

    public void InitializeAI(int size, char aiChar, char humanChar, int depth = 4)
    {
        boardSize = size;
        winCondition = (size == 3) ? 3 : (size == 6) ? 4 : 5;
        aiPlayer = aiChar;
        humanPlayer = humanChar;
        maxDepth = depth; // Set the maximum depth for the search
    }

    public (int, int) GetBestMove(char[,] board)
    {
        int bestScore = int.MinValue;
        (int, int) bestMove = (-1, -1);

        List<(int, int)> moves = GetAvailableMoves(board);
        foreach (var move in moves)
        {
            board[move.Item1, move.Item2] = aiPlayer;
            int score = Minimax(board, 0, false, int.MinValue, int.MaxValue);
            board[move.Item1, move.Item2] = ' ';
            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }
        return bestMove;
    }

    private int Minimax(char[,] board, int depth, bool isMaximizing, int alpha, int beta)
    {
        string boardHash = GetBoardHash(board);
        if (transpositionTable.ContainsKey(boardHash))
        {
            return transpositionTable[boardHash];
        }

        if (CheckForWin(board, aiPlayer))
        {
            return 10 - depth;
        }
        if (CheckForWin(board, humanPlayer))
        {
            return depth - 10;
        }
        if (IsBoardFull(board) || depth >= maxDepth) // Check depth limit
        {
            return 0;
        }

        if (isMaximizing)
        {
            int maxEval = int.MinValue;
            List<(int, int)> moves = GetAvailableMoves(board);
            foreach (var move in moves)
            {
                board[move.Item1, move.Item2] = aiPlayer;
                int eval = Minimax(board, depth + 1, false, alpha, beta);
                board[move.Item1, move.Item2] = ' ';
                maxEval = Math.Max(maxEval, eval);
                alpha = Math.Max(alpha, eval);
                if (beta <= alpha)
                {
                    break;
                }
            }
            transpositionTable[boardHash] = maxEval;
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            List<(int, int)> moves = GetAvailableMoves(board);
            foreach (var move in moves)
            {
                board[move.Item1, move.Item2] = humanPlayer;
                int eval = Minimax(board, depth + 1, true, alpha, beta);
                board[move.Item1, move.Item2] = ' ';
                minEval = Math.Min(minEval, eval);
                beta = Math.Min(beta, eval);
                if (beta <= alpha)
                {
                    break;
                }
            }
            transpositionTable[boardHash] = minEval;
            return minEval;
        }
    }

    private List<(int, int)> GetAvailableMoves(char[,] board)
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
        return availableMoves;
    }

    private string GetBoardHash(char[,] board)
    {
        char[] flattenedBoard = new char[boardSize * boardSize];
        int index = 0;
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                flattenedBoard[index++] = board[row, col];
            }
        }
        return new string(flattenedBoard);
    }

    private bool CheckForWin(char[,] board, char player)
    {
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

    private bool IsBoardFull(char[,] board)
    {
        foreach (char cell in board)
        {
            if (cell == ' ')
            {
                return false;
            }
        }
        return true;
    }
}