using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class MenuScript : MonoBehaviour
{
    public GameObject Guide;
    public Button startGame;
    public Button exitGame;
    public Button openGuide;
    public Button closeGuide; 

    private void Start()
    {   
        //  Adding listeners to respective buttons
        startGame.onClick.AddListener(LoadGame);
        exitGame.onClick.AddListener(ExitGame); 
        openGuide.onClick.AddListener(OpenGuide);
        closeGuide.onClick.AddListener(CloseGuide); 
    }

    public void OpenGuide() 
    {
        Guide.SetActive(true);
    }

    public void CloseGuide()
    {
        Guide.SetActive(false);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("TicTacToeBoardI");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
