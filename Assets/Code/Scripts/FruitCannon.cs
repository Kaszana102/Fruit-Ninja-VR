using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FruitCannon : MonoBehaviour
{
    float timeToShot = 0;

    public float shotForce = 0.75f;

    private GameObject barrel;
	private GameObject firePoint;

    [SerializeField] SoundRandomizer sound;


    /// <summary>
    /// Needs to be set by GameManager in the menu.
    /// </summary>
    public List<String> fruitPrefabs = new List<string> 
        {
        "Prefabs/Fruits/Watermelon",
        "Prefabs/Fruits/Watermelon",
        "Prefabs/Fruits/Watermelon",
        "Prefabs/Fruits/Watermelon"};    

    public void ShuffleFruits()
    {
        fruitPrefabs = fruitPrefabs.OrderBy(i => Guid.NewGuid()).ToList();        
    }


    private void Start()
    {
		barrel = transform.GetChild(0).gameObject;
		firePoint = barrel.transform.GetChild(0).gameObject;

		timeToShot = Time.time +  3 + UnityEngine.Random.Range(0, 4);
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Time.time > timeToShot)
        {
            timeToShot = Time.time + 3 + UnityEngine.Random.Range(0, 4);
            if (fruitPrefabs.Count > 0)
            {
                ShootFruit();
            }
        }*/
    }


    public void ShootFruit()
    {
        String path = fruitPrefabs.First();
        fruitPrefabs.RemoveAt(0);       

        GameObject fruitPrefab = Resources.Load(path) as GameObject;
        GameObject fruitObject = Instantiate(fruitPrefab);

        fruitObject.transform.position = firePoint.transform.position;

        fruitObject.GetComponent<Fruit>().Throw(barrel.transform.forward*shotForce);

        sound.Play();

    }
}
