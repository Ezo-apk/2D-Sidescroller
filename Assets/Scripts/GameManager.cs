using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; private set; }
    private GameObject player;

    private void Awake()
    {

        if(gameManager != null && gameManager != this)
        {
            Destroy(this);
        } else {
            gameManager = this;
        }
        
        player = GameObject.FindGameObjectWithTag("Player");

        if (player.GetComponent<PlayerStats>().currentHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


}
