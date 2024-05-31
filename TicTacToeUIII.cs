using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TicTacToeUIII : MonoBehaviour
{
    public TicTacToeII game;
    public Transform gridPanel;
    public Button cellPrefab;
    public TextMeshProUGUI statusText;
    private List<Button> cellButtons;
    private GridLayoutGroup gridLayoutGroup;
    public Button resetButton; 
    public Button undoButton;
    public GameObject ai_image_panel; // Reference to the panel or image you want to disable

    private void Awake()
    {
        cellButtons = new List<Button>();
        gridLayoutGroup = gridPanel.GetComponent<GridLayoutGroup>();

        if (gridLayoutGroup == null)
        {
            Debug.LogError("GridLayoutGroup component not found on gridPanel!");
            return;
        }
    }

    public void InitializeGrid(int size)
    {
        if (cellButtons == null)
        {
            Debug.LogError("cellButtons list is not initialized!");
            return;
        }

        // Clear existing buttons
        foreach (Button button in cellButtons)
        {
            Destroy(button.gameObject);
        }
        cellButtons.Clear();

        // Set GridLayoutGroup constraints
        gridLayoutGroup.constraintCount = size;

        // Adjust cell size based on grid size
        float panelWidth = gridLayoutGroup.GetComponent<RectTransform>().rect.width;
        float panelHeight = gridLayoutGroup.GetComponent<RectTransform>().rect.height;
        float cellWidth = panelWidth / size - gridLayoutGroup.spacing.x;
        float cellHeight = panelHeight / size - gridLayoutGroup.spacing.y;
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);

        // Calculate font size based on cell size
        float fontSize = CalculateFontSize(cellWidth, cellHeight);

        // Create buttons for each cell in the grid
        for (int i = 0; i < size * size; i++)
        {
            Button cellButton = Instantiate(cellPrefab, gridPanel);
            int index = i;
            int row = index / size;
            int col = index % size;
            cellButton.onClick.AddListener(() => OnCellClicked(index, row, col));
            cellButtons.Add(cellButton);

            // Adjust font size based on grid size
            TextMeshProUGUI buttonText = cellButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.fontSize = fontSize;
        }

        // Initialize the game logic with the new grid size
        DisablePanel();
    }

    public void StartGame(int size, bool aiMode, string aiDifficulty)
    {
        InitializeGrid(size);
        game.InitializeGame(size, aiMode, aiDifficulty);
    }

    private float CalculateFontSize(float cellWidth, float cellHeight)
    {
        // Set the font size to be a fraction of the smaller dimension of the cell
        float cellSize = Mathf.Min(cellWidth, cellHeight);
        return cellSize * 1f; // Adjust the multiplier as needed for better fit
    }

    private void OnCellClicked(int index, int row, int col)
    {
        if (game.PlaceMark(index, row, col))
        {
            UpdateBoardUI();
            if (game.CheckForWin())
            {
                statusText.text = $"Player {game.GetCurrentPlayer()} wins!";
                DisableBoardInteraction();
                if (undoButton != null)
                {
                    undoButton.interactable = false;
                }
            }
            else if (game.CheckForDraw())
            {
                statusText.text = "It's a draw!";
                DisableBoardInteraction();
                if (undoButton != null)
                {
                    undoButton.interactable = false;
                }
            }
            else
            {
                game.ChangePlayer();
                if (game.IsAI && game.GetCurrentPlayer() == 'O')
                {
                    game.ContinueGame();
                }
            }
        }
    }

    public void UpdateBoardUI()
    {
        for (int row = 0; row < game.BoardSize; row++)
        {
            for (int col = 0; col < game.BoardSize; col++)
            {
                int index = row * game.BoardSize + col;
                char mark = game.GetMark(row, col);
                TextMeshProUGUI buttonText = cellButtons[index].GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = mark.ToString();
                buttonText.color = (mark == 'X') ? Color.yellow : Color.blue;
            }
        }
    }

    public void UpdateStatusText()
    {
        statusText.text = "Player " + game.GetCurrentPlayer() + "'s turn";
        statusText.color = (game.GetCurrentPlayer() == 'X') ? Color.yellow : Color.blue;
    }
    
    public void Status(string status) {
        statusText.text = status;
    }

    public void EnableBoardInteraction()
    {
        foreach (Button button in cellButtons)
        {
            button.interactable = true;
        }
    }
    
    public void DisableBoardInteraction()
    {
        foreach (Button button in cellButtons)
        {
            button.interactable = false;
        }
    }

    public void UndoMove()
    {
        game.UndoLastMove();
        UpdateBoardUI();
    }

    public void UpdateCellUI(int index, char mark)
    {
        if (index >= 0 && index < cellButtons.Count)
        {
            TextMeshProUGUI buttonText = cellButtons[index].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = mark.ToString();
        }
    }

    public void ResetGame()
    {
        int size = game.BoardSize;
        InitializeGrid(size);
        UpdateStatusText();
        game.InitializeBoard(size);
        
    }

    private int CalculateFontSize(int gridSize)
    {
        int baseFontSize = 60; // Base font size for 3x3 grid
        int minFontSize = 20;  // Minimum font size to ensure readability
        return Mathf.Max(baseFontSize / gridSize, minFontSize);
    }

    private void DisablePanel()
    {
        if (ai_image_panel != null && game.IsAI)
        {
            ai_image_panel.SetActive(true);
        }
    }
}
