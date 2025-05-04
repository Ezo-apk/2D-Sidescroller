using UnityEngine;
using TMPro;


public class Tooltips : MonoBehaviour
{
    public TextMeshProUGUI basicControls, doubleJumpControls, wallGrabControls, dashControls, shootingControls;
    private float timeOnScreen;
    
    void Start()
    {
        basicControls.enabled = false;
        doubleJumpControls.enabled = false;
        wallGrabControls.enabled = false;
        dashControls.enabled = false;
        shootingControls.enabled = false;

        EnableStartTip();
    }


    public void EnableDashTip()
    {
        dashControls.enabled = true;
        timeOnScreen = 2f;
    }
    public void EnableDoubleJumpTip()
    {
        doubleJumpControls.enabled = true;
        timeOnScreen = 2f;
    }
    public void EnableWallGrabTip()
    {
        wallGrabControls.enabled = true;
        timeOnScreen = 2f;
    }
    public void EnableStartTip()
    {
        basicControls.enabled = true;
        timeOnScreen = 2f;
    }
    public void EnableSthootingTip()
    {
        shootingControls.enabled = true;
        timeOnScreen = 2f;
    }

    

    //We check every frame if the timer has expired and the text should disappear
    void Update()
    {
        if (timeOnScreen > 0)
        {
            timeOnScreen -= Time.deltaTime;
        } else
        {
            timeOnScreen = 0;
        }

        if(timeOnScreen == 0)
        {
            if(basicControls.enabled == true)
            {
                basicControls.enabled = false;
            }
            
            if(doubleJumpControls.enabled == true)
            {
                doubleJumpControls.enabled = false;
            }
            
            if(dashControls.enabled == true)
            {
                dashControls.enabled = false;
            }
            if(wallGrabControls.enabled == true)
            {
                wallGrabControls.enabled = false;
            }

            if(shootingControls.enabled == true)
            {
                shootingControls.enabled = false;
            }
        }
        
    }
}
