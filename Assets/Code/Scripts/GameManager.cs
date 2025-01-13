using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public int playerPoints = 0;

    public float timeToShot = 1.0f;

    public bool readyToShoot = true;

    public List<FruitCannon> cannons= new List<FruitCannon>();

    public int remainingFruits;


	[SerializeField] TextMeshProUGUI pointsText;

	[SerializeField] TextMeshProUGUI fruitsText;


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
		UpdateUI();
	}

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GameState.FruitCut:
                if(remainingFruits ==0 && Fruit.fruitCount == 0)
                {
					UpdateUI();
					Debug.Log("GAME FINISHED");
                    state = GameState.FruitResult;
                }

                if(remainingFruits > 0 )
                {
                    if (Time.time > timeToShot)
                    {
                        timeToShot = Time.time + 3 + UnityEngine.Random.Range(0, 4);

                        ChooseCannon().ShootFruit();
                        UpdateUI();

					}
				}

                break;
            case GameState.MainMenu:
                break;
            case GameState.Paused:
                break;

        }

    }

	public void CountFruits()
    {
        remainingFruits = 0;

		foreach (FruitCannon cannon in cannons)
        {
            remainingFruits += cannon.fruitPrefabs.Count;
        }
    }


	public FruitCannon ChooseCannon()
    {
        FruitCannon chosenCannon;

		while (true)
        {
			chosenCannon = cannons[Random.Range(0, cannons.Count)];

            if (chosenCannon.fruitPrefabs.Count>0)
            {
                return chosenCannon;
            }
		}
    }

	public void UpdateUI()
	{
        CountFruits();

		pointsText.text = playerPoints.ToString();
        fruitsText.text = remainingFruits.ToString(); 
	}



	void OnLevelFinished()
    {
        Debug.Log("LEVEL FINISHED");
    }


    public void LoadDojo()
    {
        SceneManager.LoadScene("Dojo");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
