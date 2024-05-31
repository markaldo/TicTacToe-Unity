using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManagerScript : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadScene("Welcome-Scene");
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Credits");
    }
}
