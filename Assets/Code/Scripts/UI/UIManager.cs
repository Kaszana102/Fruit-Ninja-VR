using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    private GameObject deathUI;
    private GameObject winningUI;
    private Text respawnText;
    private Text summaryText;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {

        deathUI = gameObject.GetNamedChild("DeathUI");
        winningUI = gameObject.GetNamedChild("WinningUI");

        respawnText = deathUI.GetNamedChild("RespawnText").GetComponent<Text>();
        summaryText = winningUI.GetNamedChild("SummaryText").GetComponent<Text>();

        deathUI.SetActive(false);
        winningUI.SetActive(false);
    }

    /// <summary>
    /// shows death UI text
    /// </summary>    
    public void ShowDeathUI()
    {
        deathUI.SetActive(true);
    }

    /// <summary>
    /// hides death UI text
    /// </summary>    
    public void HideDeathUI()
    {
        deathUI.SetActive(false);
    }


    /// <summary>
    /// shows winning UI text
    /// </summary>    
    public void ShowWinningUI()
    {
        winningUI.SetActive(true);
    }

    /// <summary>
    /// hides winning UI text
    /// </summary>    
    public void HideWinningUI()
    {
        winningUI.SetActive(false);
    }

    /// <summary>
    /// updates text afetr player dies
    /// </summary>    
    public void UpdateRespawnText(float seconds)
    {
        respawnText.text = "Respawning in: " + seconds + " seconds";
    }

    public void SetSummaryText(float elapsedSeconds, int deaths)
    {
        float minutes = Mathf.FloorToInt(elapsedSeconds / 60);
        float seconds = Mathf.FloorToInt(elapsedSeconds % 60);


        string timeToDisplay = string.Format("{0:00}:{1:00}", minutes,seconds);

        summaryText.text = "You completed the maze in "+ timeToDisplay + " and "+ deaths+" deaths.";
    }
}
