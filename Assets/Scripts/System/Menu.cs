using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Cena 1");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void ResumeGame()
    {
        SceneManager.LoadScene("Menu");
    }
}
