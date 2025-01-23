using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    

    private void Awake()
    {
        // If there is an instance, and it's not me, delete other.

        if (Instance != null && Instance != this)
        {
            Destroy(Instance);            
        }
        Instance = this;

        gameObject.SetActive(false);
    }

    public void UnpauseGame()
    {
        GameManager.Instance.UnpauseGame();
    }

    public void ShowMenu()
    {
        gameObject.SetActive(true);
    }

    public void HideMenu()
    {
        gameObject.SetActive(false);
    }

    public void LoadMainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }
}
