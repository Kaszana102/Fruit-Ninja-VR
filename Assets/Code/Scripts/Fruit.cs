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
    [SerializeField]
    int points;

    [SerializeField]
    Gesture gesture;

    Rigidbody rb;

    [SerializeField]
    ParticleSystem explosion;

    [SerializeField]
    GameObject model;

    private void Awake()
    {
        rb= GetComponent<Rigidbody>();
        gesture.SetGestureOnFinishedCallback(OnGestureCaptured);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -3)
        {
            ShotMissed();
        }
    }



    public void Throw(Vector3 startVelocity)
    {
        rb.velocity = startVelocity;
    }

    void OnGestureCaptured()
    {
        model.SetActive(false);
        explosion.Play();
        // TODO add points to game manager
        Debug.Log("add points!");

        StartCoroutine(DeleteFruit());
    }

    IEnumerator DeleteFruit()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    /// <summary>
    ///  when the fruit has fallen below the ground
    /// </summary>
    void ShotMissed()
    {
        // TODO decrease score in game manager
        Debug.Log("subtract points!");
        Destroy(gameObject);
    }
}
