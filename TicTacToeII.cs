using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToeII : MonoBehaviour
{
    private char[,] board;
    private char currentPlayer;
    public Stack<MoveII> moveHistory;
    public TicTacToeEasyAI easyAi;
    public TicTacToeMediumAI mediumAi;
    public TicTacToeHardAI hardAi;
    private ITicTacToeAI aiPlayer; // Reference to the current AI script (easy, medium, or hard)
    private bool isAI; // Indicates if playing against AI
    public TicTacToeUIII ticTacToeui; // instance
    private int boardSize;
    private int winCondition;
    private string aiDifficulty;

    public void InitializeGame(int size, bool aiMode, string aiDifficulty)
    {
        InitializeBoard(size);
        this.isAI = aiMode;
        this.aiDifficulty = aiDifficulty;
        SetGameMode(aiMode);
        if (aiMode)
        {
            SetAIDifficulty(aiDifficulty);
        }
    }

    public void InitializeBoard(int size)
    {
        boardSize = size;
        board = new char[size, size];
        currentPlayer = 'X'; // Assuming X always starts
        moveHistory = new Stack<MoveII>();

        // Set win condition based on board size
        winCondition = (size == 3) ? 3 : (size == 6) ? 4 : 5;

        // Initialize the board to empty
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                board[row, col] = ' ';
            }
        }

        // Additional initialization if needed
    }

    public void SetGameMode(bool aiMode)
    {
        isAI = aiMode;
    }

    public void SetAIDifficulty(string aiDifficulty)
    {
        switch (aiDifficulty)
        {
            case "Easy":
                aiPlayer = easyAi;
                break;
            case "Medium":
                aiPlayer = mediumAi;
                break;
            case "Hard":
                aiPlayer = hardAi;
                break;
            default:
                Debug.LogError("Unknown AI difficulty level!");
                break;
        }

        if (aiPlayer != null)
        {
            aiPlayer.InitializeAI(boardSize, 'O', 'X', winCondition); // Assuming AI plays as 'O' and human plays as 'X'
        }
    }

    public bool IsAI
    {
        get { return isAI; }
    }

    public bool PlaceMark(int index, int row, int col)
    {
        if (row >= 0 && row < boardSize && col >= 0 && col < boardSize)
        {
            if (board[row, col] == ' ')
            {
                board[row, col] = currentPlayer;
                moveHistory.Push(new MoveII(index, row, col));
                return true;
            }
            else
            {
                Debug.LogError($"Attempted to place mark on occupied cell at ({row}, {col}).");
            }
        }
        else
        {
            Debug.LogError($"Invalid move at row {row}, col {col}. Board size is {boardSize}.");
        }
        return false;
    }

    public bool CheckForWin()
    {
        return CheckRowsForWin() || CheckColumnsForWin() || CheckDiagonalsForWin();
    }

    private bool CheckRowsForWin()
    {
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col <= boardSize - winCondition; col++)
            {
                if (IsWinningLine(row, col, 0, 1))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckColumnsForWin()
    {
        for (int col = 0; col < boardSize; col++)
        {
            for (int row = 0; row <= boardSize - winCondition; row++)
            {
                if (IsWinningLine(row, col, 1, 0))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckDiagonalsForWin()
    {
        for (int row = 0; row <= boardSize - winCondition; row++)
        {
            for (int col = 0; col <= boardSize - winCondition; col++)
            {
                if (IsWinningLine(row, col, 1, 1) || IsWinningLine(row, col + winCondition - 1, 1, -1))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsWinningLine(int startRow, int startCol, int stepRow, int stepCol)
    {
        char first = board[startRow, startCol];
        if (first == ' ')
        {
            return false;
        }

        for (int i = 1; i < winCondition; i++)
        {
            if (board[startRow + i * stepRow, startCol + i * stepCol] != first)
            {
                return false;
            }
        }
        return true;
    }

    public bool CheckForDraw()
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

    public void ChangePlayer()
    {
        currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
        ticTacToeui.UpdateStatusText();
    }

    public char GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public void ResetBoard(int size)
    {
        InitializeBoard(size);
        currentPlayer = 'X'; // Reset the current player to 'X'
        moveHistory.Clear();
    }

    public void UndoLastMove()
    {
        if (moveHistory.Count > 0)
        {
            // Undo the player's move
            MoveII lastMove = moveHistory.Pop();
            board[lastMove.Row, lastMove.Col] = ' ';
            ticTacToeui.UpdateCellUI(lastMove.Index, ' ');

            // If playing against AI, undo the AI's move as well
            if (isAI && moveHistory.Count > 0)
            {
                MoveII aiMove = moveHistory.Pop();
                var (index, row, col) = aiMove;
                board[row, col] = ' ';
                ticTacToeui.UpdateCellUI(aiMove.Index, ' ');
            }

            // Change the current player back
            ChangePlayer();
        }
    }

    public char GetMark(int row, int col)
    {
        return board[row, col];
    }

    public void ContinueGame()
    {
        if (isAI && currentPlayer == 'O')
        {
            if (boardSize == 9 && aiDifficulty == "Hard")
            {
                MakeAIMoveInstant();
            }
            else
            {
                StartCoroutine(MakeAIMove());
            }
        }
    }

    private IEnumerator MakeAIMove()
    {
        yield return new WaitForSeconds(1f); // Simulate thinking time

        (int row, int col) = aiPlayer.GetBestMove(board);
        
        if (row >= 0 && row < boardSize && col >= 0 && col < boardSize)
        {
            int index = row * boardSize + col;
            if (PlaceMark(index, row, col))
            {
                ticTacToeui.UpdateBoardUI();
                if (CheckForWin())
                {
                    ticTacToeui.Status("Player AI wins!");
                    ticTacToeui.DisableBoardInteraction();
                }
                else if (CheckForDraw())
                {
                    ticTacToeui.Status("It's a draw!");
                    ticTacToeui.DisableBoardInteraction();
                }
                else
                {
                    ChangePlayer();
                }
            }
            else
            {
                Debug.LogError($"AI attempted to place a mark on an occupied cell at ({row}, {col}).");
            }
        }
        else
        {
            Debug.LogError("AI generated an invalid move.");
        }
    }

    private void MakeAIMoveInstant()
    {
        (int row, int col) = aiPlayer.GetBestMove(board);

        if (row >= 0 && row < boardSize && col >= 0 && col < boardSize)
        {
            int index = row * boardSize + col;
            if (PlaceMark(index, row, col))
            {
                ticTacToeui.UpdateBoardUI();
                if (CheckForWin())
                {
                    ticTacToeui.Status("Player AI wins!");
                    ticTacToeui.DisableBoardInteraction();
                }
                else if (CheckForDraw())
                {
                    ticTacToeui.Status("It's a draw!");
                    ticTacToeui.DisableBoardInteraction();
                }
                else
                {
                    ChangePlayer();
                }
            }
            else
            {
                Debug.LogError($"AI attempted to place a mark on an occupied cell at ({row}, {col}).");
            }
        }
        else
        {
            Debug.LogError("AI generated an invalid move.");
        }
    }

    // Public getter for boardSize
    public int BoardSize
    {
        get { return boardSize; }
    }
}
