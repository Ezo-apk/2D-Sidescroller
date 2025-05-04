using UnityEngine;
using UnityEngine.SceneManagement;


public class WinScreen : MonoBehaviour
{

    public void Again()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
