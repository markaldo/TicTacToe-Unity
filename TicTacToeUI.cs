using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TicTacToeUI : MonoBehaviour
{
    public TicTacToe TicTacToe;
    public Button[] buttons; // Array of buttons representing the Tic Tac Toe board
    public TextMeshProUGUI Title;
    public Button resetButton; 
    public Button undoButton;

    private void Start()
    {
        resetButton.onClick.AddListener(ResetGame);
        undoButton.onClick.AddListener(UndoMove); // Adding listener for the "undo" button
    }

    public void OnButtonClick(Button button)
    {
        // Check if TicTacToe object is not null before calling its methods
        if (TicTacToe != null)
        {
            // Find the index of the button in the array
            int index = System.Array.IndexOf(buttons, button);

            // Calculating row and column from index (assuming buttons are arranged in a 3x3 grid)
            int row = index / 3;
            int col = index % 3;

            // Place the mark on the Tic Tac Toe board
            if(TicTacToe.PlaceMark(index, row, col))
            {
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = TicTacToe.GetCurrentPlayer().ToString();

                // Set color based on player
                Color purple = new Color(0.5f, 0f, 0.5f);
                buttonText.color = (TicTacToe.GetCurrentPlayer() == 'X')? purple : Color.blue;

                // Check for win or draw
                if (TicTacToe.CheckForWin())
                {
                    // Implementing logic for win
                    if (Title != null)
                    {
                        Title.GetComponent<TextMeshProUGUI>().text = TicTacToe.GetCurrentPlayer().ToString() + " wins!";
                        Title.color = (TicTacToe.GetCurrentPlayer() == 'X') ? purple : Color.blue;
                    }
                    else
                    {
                        // Handle the case where Title object is null
                        Debug.LogError("Title object is null");
                    }
                    
                    if (undoButton != null)
                    {
                        undoButton.interactable = false;
                    }
                    else
                    {
                        Debug.LogError("Undo Button object is null");
                    }
                }
                else if (TicTacToe.CheckForDraw())
                {
                    // Implementing logic for draw
                    if (Title != null)
                    {
                        Title.GetComponent<TextMeshProUGUI>().text = "It's a draw!";
                        Title.color = new Color32(237, 255, 0, 255);
                    }
                    else
                    {
                        // Handling the case where Title object is null
                        Debug.LogError("Title object is null");
                    }
                    
                    // Disabling the "Undo" button since there are no more moves to be made
                    if (undoButton != null)
                    {
                        undoButton.interactable = false;
                    }
                    else
                    {
                        // Handle the case where undoButton object is null
                        Debug.LogError("Undo Button object is null");
                    }
                }

                // Change player
                TicTacToe.ChangePlayer();
                
                //if playing against AI
                if (TicTacToe.IsAI) 
                {
                    TicTacToe.ContinueGame();
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            // Handling the case where TicTacToe object is null
            Debug.LogError("TicTacToe object is null");
        }
    }

    public void UpdateAiMove(int index)
    {
        // Check if the index is within the valid range
        if (index >= 0 && index < buttons.Length)
        {
            TextMeshProUGUI button = buttons[index].GetComponentInChildren<TextMeshProUGUI>();
            button.text = "O";
            button.color = Color.blue;
        }
        else
        {
            Debug.LogError("Invalid index: " + index);
        }
    }

    public void ResetGame()
    {
        // Check if TicTacToe object is not null before calling ResetBoard
        if (TicTacToe != null)
        {
            TicTacToe.ResetBoard();
        }
        else
        {
            // Handle the case where TicTacToe object is null
            Debug.LogError("TicTacToe object is null");
            return; // Exit the method to avoid further execution
        }
        
        if (Title != null)
        {
            Title.GetComponent<TextMeshProUGUI>().text = "Tic Tac Toe";
            Title.color = new Color32(34, 255, 0, 255);
        }
        else
        {
            Debug.LogError("Title object is null");
        }

        // Reset the UI buttons
        foreach (Button button in buttons)
        {
            if (button != null && button.GetComponentInChildren<TextMeshProUGUI>() != null)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
            else
            {
                Debug.LogError("Button or its TextMeshProUGUI component is null");
            }
        }

        if (undoButton != null)
        {
            undoButton.interactable = true;
        }
        else
        {
            Debug.LogError("Undo Button object is null");
        }
    }

    private void UndoMove()
    {
        if(TicTacToe.IsAI && TicTacToe.moveHistory.Count >= 1)
        {
            var array = TicTacToe.moveHistory.ToArray();
            var ai_index = array[0];
            var player_index = array[1];

            var lastAiMove = TicTacToe.moveHistory.Peek();

            buttons[ai_index.index].GetComponentInChildren<TextMeshProUGUI>().text = ""; 
            buttons[player_index.index].GetComponentInChildren<TextMeshProUGUI>().text = "";
            TicTacToe.UndoLastMove();
        }
        else if (!TicTacToe.IsAI && TicTacToe.moveHistory.Count > 0)
        {
            var lastMove = TicTacToe.moveHistory.Peek();
            buttons[lastMove.index].GetComponentInChildren<TextMeshProUGUI>().text = ""; // Clear the text of the corresponding button
            TicTacToe.UndoLastMove();
        }
        else
        {
            Debug.Log("No moves to undo");
        }

    }

    public void AiWon()
    {
        // Checking if Title object is not null before accessing its properties
        if (Title != null)
        {
            Title.GetComponent<TextMeshProUGUI>().text = " AI wins!";
            Title.color = Color.blue;
        }
        else
        {
            // Handle the case where Title object is null
            Debug.LogError("Title object is null");
            return; // Exit the method to avoid further execution
        }
        
        // Disable the Undo Button
        if (undoButton != null)
        {
            undoButton.interactable = false;
        }
        else
        {
            Debug.LogError("Undo Button object is null");
        }
    }


    private void PlayerAgainstAiWon()
    {
        // Checking if Title object is not null before accessing its properties
        if (Title != null)
        {
            // Set the Title text and color
            Color purple = new Color(0.5f, 0f, 0.5f);
            Title.GetComponent<TextMeshProUGUI>().text = " You won!";
            Title.color = purple;
        }
        else
        {
            Debug.LogError("Title object is null");
            return; 
        }
        
        // Disable the Undo Button
        if (undoButton != null)
        {
            undoButton.interactable = false;
        }
        else
        {
            Debug.LogError("Undo Button object is null");
        }
    }

    public void Draw_AI_Player()
    {
        // Check if Title object is not null before accessing its properties
        if (Title != null)
        {
            // Set the Title text and color
            Title.GetComponent<TextMeshProUGUI>().text = " It's a Draw!";
            Title.color = Color.yellow;
        }
        else
        {
            // Handling the case where Title object is null
            Debug.LogError("Title object is null");
            return; // Exit the method to avoid further execution
        }
        
        // Disable the Undo Button
        if (undoButton != null)
        {
            undoButton.interactable = false;
        }
        else
        {
            Debug.LogError("Undo Button object is null");
        }
    }
}
