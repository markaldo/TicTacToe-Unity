using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TicTacToe ticTacToe;
    public TicTacToeUI ticTacToeUI;
    public TicTacToeAI aiPlayer;

    void Awake()
    {
        // Find the GameObject with the TicTacToe script attached
        GameObject ticTacToeObject = GameObject.Find("GameControl");
        if (ticTacToeObject != null)
        {
            // Get the TicTacToe component from the GameObject
            ticTacToe = ticTacToeObject.GetComponent<TicTacToe>();
            if (ticTacToe != null)
            {
                // Set the references
                ticTacToe.ticTacToeui = ticTacToeUI;
                ticTacToe.aiPlayer = aiPlayer;
                Debug.Log("TicTacToe component found!");
                // Set references...
            }
            else
            {
                Debug.LogError("TicTacToe component not found!");
            }
        }
        else
        {
            Debug.LogError("TicTacToe GameObject not found!");
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Welcome-scene");
    }

    public void LoadCreditScreen()
    {
        SceneManager.LoadScene("Credits");
    }
}

