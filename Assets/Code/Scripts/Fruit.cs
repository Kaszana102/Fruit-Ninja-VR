using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tree structure
/// 
/// Fruit (empty)
/// | model
/// | explosion particle system
/// | gesture
/// </summary>
public class Fruit : MonoBehaviour
{
    public static int fruitCount = 0;

    [SerializeField]
    int points;

    [SerializeField]
    Gesture gesture;    

    [SerializeField]
    ParticleSystem explosion;

    [SerializeField]
    GameObject model;

    public static List<Fruit> thrownFruits = new List<Fruit>();

    Vector3 speed;
    const float gravity = .1f;

    private bool fell = false;

    private void Awake()
    {
        fruitCount++;
    }

    private void Start()
    {        
        gesture.SetGestureOnFinishedCallback(OnGestureCaptured);
    }

    // Update is called once per frame
    void Update()
    {        
        speed -= Vector3.up * gravity * Time.deltaTime;
        transform.position += speed * Time.deltaTime;

        if (transform.position.y < 0 && !fell)
        {
            ShotMissed();
			
		}
    }

    public void Throw(Vector3 startVelocity)
    {
        speed = startVelocity;
        thrownFruits.Add(this);
    }

    void OnGestureCaptured()
    {
        model.SetActive(false);
        gesture.gameObject.SetActive(false);
        explosion.Play();
        GameManager.Instance.playerPoints += points;
        Debug.Log("added points: "+points+"!");
		GameManager.Instance.UpdateUI();

		StartCoroutine(DeleteFruit());
    }

    IEnumerator DeleteFruit()
    {
        thrownFruits.Remove(this);
        yield return new WaitForSeconds(2);        
        Destroy(gameObject);        
    }

    /// <summary>
    ///  when the fruit has fallen below the ground
    /// </summary>
    void ShotMissed()
    {
        fell = true;
        GameManager.Instance.playerPoints -= points/2;
        Debug.Log("subtracted points: " + points/2 + "!");
		GameManager.Instance.UpdateUI();
        explosion.gameObject.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
		explosion.Play();
		thrownFruits.Remove(this);
		StartCoroutine(DeleteFruit());
		//Destroy(gameObject);
	}

    public void SetDetectable(bool detectable)
    {
        gesture.SetDetectable(detectable);
    }

    private void OnDestroy()
    {
        fruitCount--;
    }
}
