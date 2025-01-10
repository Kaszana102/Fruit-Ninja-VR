using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public int playerPoints = 0;


    enum GameState
    {
        MainMenu,
        FruitCut,
        FruitResult,
        Paused
    }

    GameState state = GameState.FruitCut;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GameState.FruitCut:
                if(FruitCannon.fruitPrefabs.Count ==0 && Fruit.fruitCount == 0)
                {
                    Debug.Log("GAME FINISHED");
                    state = GameState.FruitResult;
                }
                break;
            case GameState.MainMenu:
                break;
            case GameState.Paused:
                break;

        }
    }

    void OnLevelFinished()
    {
        Debug.Log("LEVEL FINISHED");
    }
}
