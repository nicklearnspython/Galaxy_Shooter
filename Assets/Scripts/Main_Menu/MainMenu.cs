using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadSinglePlayerGame()
    {
        SceneManager.LoadScene(1); // 1: Game Scene
    }

    public void LoadCoOpGame()
    {
        SceneManager.LoadScene(2); // 2: Co-Op Scene
    }

    public void QuitGame()
    {
        PlayerPrefs.SetInt("Best Score", 0);
        Application.Quit();
    }
}
