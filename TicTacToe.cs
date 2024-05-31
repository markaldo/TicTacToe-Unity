using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TicTacToe : MonoBehaviour
{
    private char[,] board;
    private char currentPlayer;
    public Stack<(int index, int row, int col)> moveHistory;
    public TicTacToeAI aiPlayer; // Instance of TicTacToeAI
    private bool isAI; // Indicates if playing against AI
    public TicTacToeUI ticTacToeui; // instance
    public GameObject ai_image_panel; // Reference to the panel or image you want to disable

    private void Start()
    {
        isAI = PlayerPrefs.GetInt("IsAI", 0) == 1; // Default to false if "IsAI" key is not found
        board = new char[3, 3];
        currentPlayer = 'X';
        moveHistory = new Stack<(int, int, int)>();
        InitializeBoard();
        DisablePanel();
    }

    private void InitializeBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                board[i, j] = '-';
            }
        }
    }

    // Getter for isAI property
    public bool IsAI
    {
        get { return isAI; }
    }

    // Setter for isAI property
    public void SetIsAI(bool value)
    {
        isAI = value;
    }

    public bool PlaceMark(int index, int row, int col)
    {
        // Checking if the selected cell is already occupied and valid
        if ((row >= 0 && row < 3) && (col >= 0 && col < 3) && (board[row, col] == '-'))
        {
            board[row, col] = currentPlayer;
            moveHistory.Push((index, row, col)); // Push the move onto the history stack
            return true;
        }
        else
        {
            //Debug.LogWarning("Selected cell is already occupied.");
            return false; //Invalid row and column values (invalid button)
        }
    }

    public bool CheckForWin()
    {
        return (CheckRowsForWin() || CheckColumnsForWin() || CheckDiagonalsForWin());
    }

    private bool CheckRowsForWin()
    {
        for (int i = 0; i < 3; i++)
        {
            if (CheckRowCol(board[i, 0], board[i, 1], board[i, 2]))
            {
                return true;
            }
        }
        return false;
    }
    

    public bool CheckForDraw()
    {
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


    private bool CheckColumnsForWin()
    {
        for (int i = 0; i < 3; i++)
        {
            if (CheckRowCol(board[0, i], board[1, i], board[2, i]))
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckDiagonalsForWin()
    {
        return (CheckRowCol(board[0, 0], board[1, 1], board[2, 2]) ||
                CheckRowCol(board[0, 2], board[1, 1], board[2, 0]));
    }

    private bool CheckRowCol(char c1, char c2, char c3)
    {
        return (c1 != '-' && c1 == c2 && c2 == c3);
    }

    public void ChangePlayer()
    {
        currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
    }

    public char GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public void ResetBoard()
    {
        InitializeBoard(); // Reset the board to its initial state
        currentPlayer = 'X'; // Reset the current player to 'X'
        moveHistory.Clear();
    }

    public void UndoLastMove()
    {
        CommitUndoMove();

            if(isAI)
            {
                CommitUndoMove();
                currentPlayer = 'X';
            }
            else
            {
                currentPlayer = (currentPlayer == 'X') ? 'O' : 'X'; // Switch back to the previous player
            }
    }

    private void CommitUndoMove()
    {
        var lastMove = moveHistory.Pop();
        int last_row = lastMove.row;
        int last_col = lastMove.col;
        board[last_row, last_col] = '-'; // Clear the cell
    }

    public char GetMark(int row, int col)
    {
        if (row >= 0 && row < 3 && col >= 0 && col < 3)
        {
            return board[row, col];
        }
        else
        {
            //Debug.LogError("Invalid row or column index.");
            return '-';
        }
    }

    //on making the AI know that the player has made a move.
    public void ContinueGame()
    {
        // If playing against AI and it's AI's turn, trigger AI move
        if (currentPlayer == 'O')
        {
            if (!CheckForDraw())
            {
                // Trigger AI move
                StartCoroutine(MakeAIMove());
            }
            else
            {
                //Debug.Log("Board is full !");
                return;
            }
        }
        else
        {
            // Switch player
            ChangePlayer();
        }
    }

    private IEnumerator MakeAIMove()
    {
        
        Move aiMove = aiPlayer.GetBestMove(board);
        yield return new WaitForSeconds(1);
        int btn_index = aiMove.Row * 3 + aiMove.Col;

        PlaceMark(btn_index, aiMove.Row, aiMove.Col);
        ticTacToeui.UpdateAiMove(btn_index);

        if (CheckForWin())
        {
            // Handle win
            ticTacToeui.AiWon();
        }
        else if (CheckForDraw())
        {
            // Handle draw
            ticTacToeui.Draw_AI_Player();
        }
        else
        {
            ChangePlayer();
        }
    }

    private void DisablePanel()
    {
        if (ai_image_panel != null && IsAI)
        {
            ai_image_panel.SetActive(true);
        }
    }
}
