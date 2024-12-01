using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GesturePoint : MonoBehaviour
{

    public Vector3 targetPos => transform.position;
    [HideInInspector]
    public Quaternion targetRot;
    [HideInInspector]
    public GesturePoint prevPoint,nextPoint;

    public static readonly float maxDistance = 0.2f;
    public static readonly float maxAngle = 20f;

    [HideInInspector]
    public bool activated = false;

    Transform hand;

    Gesture gesture;

    public GameObject Arrow;
    private void Start()
    {
        hand = Player.instance.rightHand;
        gesture = GetComponentInParent<Gesture>();
        targetRot = Arrow.transform.rotation;
    }

    private void Update()
    {
        if (!activated)
        {
            if (prevPoint == null || prevPoint.activated)
            {
                if(ShouldActivate())
                {
                    OnActivate();

                }
            }
        }
    }

    bool ShouldActivate()
    {
        Quaternion handRot = hand.rotation * Quaternion.Euler(90, 0, 0);

        if ((Vector3.Distance(targetPos, hand.position) < maxDistance)
            &&
            (Quaternion.Angle(targetRot, handRot) < maxAngle)
                    )
        {
            Debug.Log("ACTIVATED");
            return true;
        }
        if ((Vector3.Distance(targetPos, hand.position) < maxDistance)
            &&
            !(Quaternion.Angle(targetRot, handRot) < maxAngle)
                    )
        {
            Debug.Log("Target rot: " + targetRot + ". Got: "+ hand.rotation);            
        }

        return false;
    }


    void OnActivate()
    {
        activated = true;
        gesture.IncrementActivated();
    }
}
