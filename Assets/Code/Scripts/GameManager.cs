using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	[SerializeField] List<FruitCannon> cannons= new List<FruitCannon>();
	[SerializeField] List<GameObject> fruitPrefabs= new List<GameObject>();
	

	public int remainingFruits;


	[SerializeField] TextMeshProUGUI pointsText;

	[SerializeField] TextMeshProUGUI fruitsText;
	[SerializeField] TextMeshProUGUI abilityCooldownText;
    Player player;


    [SerializeField]
    GameObject FinalScoreMenu;

    public enum GameState
    {
        MainMenu,
        FruitCut,
        FruitResult,
        Paused
    }

    public GameState state = GameState.MainMenu;

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
        player = GameObject.FindObjectOfType<Player>();
		LoadAndDistributeFruits();
		UpdateUI();
        
        timeToShot = Time.time + 2;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GameState.FruitCut:
                abilityCooldownText.text = player.getRemainingCooldown().ToString("0.00");
                if (remainingFruits ==0 && Fruit.fruitCount == 0)
                {
					UpdateUI();
					Debug.Log("GAME FINISHED");
                    state = GameState.FruitResult;
                    FinalScoreMenu.SetActive(true);
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
			chosenCannon = cannons[UnityEngine.Random.Range(0, cannons.Count)];

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


    public void PauseGame()
    {
        PauseMenu.Instance.ShowMenu();
        Time.timeScale = 0;
        foreach(Fruit fruit in Fruit.thrownFruits)
        {
            fruit.SetDetectable(false);
        }

        state = GameState.Paused;
    }

    public void UnpauseGame()
    {
        if (PauseMenu.Instance != null)
        {
            PauseMenu.Instance.HideMenu();
            Time.timeScale = 1;
            foreach (Fruit fruit in Fruit.thrownFruits)
            {
                fruit.SetDetectable(true);
            }
            state = GameState.FruitCut;
        }
    }

    void OnLevelFinished()
    {
        Debug.Log("LEVEL FINISHED");
    }


    public void LoadDojo()
    {
        state = GameState.FruitCut;
        timeToShot = 1f;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Dojo");        
    }

    public void LoadMainMenu()
    {
        state = GameState.MainMenu;
        SceneManager.LoadScene("MainMenu");
    }

    private void LoadAndDistributeFruits()
    {
        Dictionary<string, int> fruitsDictionary = new Dictionary<string, int>();
		
        foreach(GameObject fruitprefab in fruitPrefabs)
        {
            string name  = fruitprefab.name;

            if(PlayerPrefs.HasKey(name))
            {
                int amount = PlayerPrefs.GetInt(name);
                fruitsDictionary.Add(name, amount);
            }
        }

        // generate list using keys and values
		List<string> results = fruitsDictionary.SelectMany(kv => Enumerable.Repeat("Prefabs/Fruits/"+kv.Key, kv.Value)).ToList();

		System.Random random = new System.Random();

		foreach (FruitCannon cannon in cannons)
        {
            cannon.GetComponent<FruitCannon>().fruitPrefabs.Clear();
        }
        Debug.Log("clearing all fruits from cannons");

        while (results.Count > 0)
        {
            int cannonIndex = random.Next(cannons.Count);
            int fruitIndex = random.Next(results.Count);

            cannons[cannonIndex].GetComponent<FruitCannon>().fruitPrefabs.Add(results[fruitIndex]);
            results.RemoveAt(fruitIndex);
        }

	}
}
