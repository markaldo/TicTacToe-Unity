using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManagerII : MonoBehaviour
{
    public TicTacToeII ticTacToe; // Reference to TicTacToeII script
    public TicTacToeUIII ticTacToeUI; // Reference to TicTacToeUIII script
    public GameObject gameUI; // Assign the panel GameObject in the Inspector
    public GameObject menuPanel; // Assign the panel GameObject in the Inspector
    public TMP_Dropdown boardSizeDropdown;
    public TMP_Dropdown aiDifficultyDropdown;
    public Toggle aiToggle;
    private int boardSize;
    private bool isAI;
    private string aiDifficulty;

    void Start()
    {
        if (gameUI == null || menuPanel == null || boardSizeDropdown == null || aiDifficultyDropdown == null || aiToggle == null)
        {
            Debug.LogError("One or more UI components are not assigned in the Inspector.");
            return;
        }

        gameUI.SetActive(false);
        menuPanel.SetActive(true); // Activates the panel
    }

    public void StartGame()
    {
        boardSize = GetBoardSizeFromDropdown();
        isAI = aiToggle.isOn;
        aiDifficulty = GetAIDifficultyFromDropdown();
    
        // Find the GameObject with the TicTacToeII script attached
        GameObject ticTacToeObject = GameObject.Find("GameControl");
        if (ticTacToeObject == null)
        {
            Debug.LogError("GameControl GameObject not found!");
            return;
        }

        // Get the TicTacToeII component from the GameObject
        ticTacToe = ticTacToeObject.GetComponent<TicTacToeII>();
        if (ticTacToe == null)
        {
            Debug.LogError("TicTacToeII component not found!");
            return;
        }

        // Initialize the game with stored preferences
        ticTacToe.InitializeGame(boardSize, isAI, aiDifficulty);
        ticTacToeUI.InitializeGrid(boardSize);

        gameUI.SetActive(true);
        menuPanel.SetActive(false);
    }

    private int GetBoardSizeFromDropdown()
    {
        switch (boardSizeDropdown.value)
        {
            case 0: return 3;
            case 1: return 6;
            case 2: return 9;
            default: return 3;
        }
    }

    private string GetAIDifficultyFromDropdown()
    {
        switch (aiDifficultyDropdown.value)
        {
            case 0: return "Easy";
            case 1: return "Medium";
            case 2: return "Hard";
            default: return "Easy";
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Welcome-scene");
    }

    public void ReloadScene()
    {
        // Reload the current active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadCreditScreen()
    {
        SceneManager.LoadScene("Credits");
    }
}
