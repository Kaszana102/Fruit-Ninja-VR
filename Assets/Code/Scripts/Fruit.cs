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


    Vector3 speed;
    const float gravity = 0.1f;

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

        if (transform.position.y < -3)
        {
            ShotMissed();
        }
    }



    public void Throw(Vector3 startVelocity)
    {
        speed = startVelocity;                
    }

    void OnGestureCaptured()
    {
        model.SetActive(false);
        gesture.gameObject.SetActive(false);
        explosion.Play();
        GameManager.Instance.playerPoints += points;
        Debug.Log("added points: "+points+"!");

        StartCoroutine(DeleteFruit());
    }

    IEnumerator DeleteFruit()
    {
        yield return new WaitForSeconds(2);
        fruitCount--;
        Destroy(gameObject);        
    }

    /// <summary>
    ///  when the fruit has fallen below the ground
    /// </summary>
    void ShotMissed()
    {
        // TODO decrease score in game manager
        GameManager.Instance.playerPoints -= points/2;
        Debug.Log("subtracted points: " + points/2 + "!");
        fruitCount--;
        Destroy(gameObject);
    }
}
