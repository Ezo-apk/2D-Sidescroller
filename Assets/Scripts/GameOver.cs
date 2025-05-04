using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{

    public void Restart()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
