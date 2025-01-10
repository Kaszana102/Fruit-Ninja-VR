using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FruitCannon : MonoBehaviour
{
    float timeToShot = 0;


    /// <summary>
    /// Needs to be set by GameManager in the menu.
    /// </summary>
    public static List<String> fruitPrefabs = new List<string> 
        { 
        "Prefabs/Fruits/Fruit Example",
        "Prefabs/Fruits/Fruit Example",
        "Prefabs/Fruits/Fruit Example",
        "Prefabs/Fruits/Fruit Example"};    

    static public void ShuffleFruits()
    {
        fruitPrefabs = fruitPrefabs.OrderBy(i => Guid.NewGuid()).ToList();        
    }


    private void Start()
    {
        timeToShot = Time.time +  5 + UnityEngine.Random.Range(0, 4);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > timeToShot)
        {
            timeToShot = Time.time + 3 + UnityEngine.Random.Range(0, 4);
            if (fruitPrefabs.Count > 0)
            {
                ShootFruit();
            }
        }
    }


    void ShootFruit()
    {
        String path = fruitPrefabs.First();
        fruitPrefabs.RemoveAt(0);       

        GameObject fruitPrefab = Resources.Load(path) as GameObject;
        GameObject fruitObject = GameObject.Instantiate(fruitPrefab);

        fruitObject.transform.position = this.transform.position;
        fruitObject.GetComponent<Fruit>().Throw(transform.up*3/4);
    }
}
