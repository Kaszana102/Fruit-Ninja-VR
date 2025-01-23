using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GesturePoint : MonoBehaviour
{

    public Vector3 targetPos => transform.position;
    [HideInInspector]
    public Quaternion targetRot;
    [HideInInspector]
    public GesturePoint prevPoint, nextPoint;

    public static readonly float maxDistance = 0.2f;
    public static readonly float maxAngle = 30f;

    [HideInInspector]
    public bool activated = false;

    Transform hand;

    Gesture gesture;

    bool detectable = true;

    public GameObject Arrow;
    private void Start()
    {
        hand = Player.instance.rightHand;
        gesture = GetComponentInParent<Gesture>();
        targetRot = Arrow.transform.rotation;
        gameObject.AddComponent<SphereCollider>().radius = maxDistance;
    }

    private void Update()
    {
        /*if (!activated)
        {
            if (prevPoint == null || prevPoint.activated)
            {
                if(ShouldActivate())
                {
                    OnActivate();
                }
            }
        }*/
    }

    bool ShouldActivate()
    {
        Quaternion handRot = hand.rotation;// * Quaternion.Euler(90, 0, 0);

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
            Debug.Log("Target rot: " + targetRot + ". Got: " + hand.rotation);
        }

        return false;
    }


    void OnActivate()
    {
        activated = true;
        gesture.IncrementActivated();
    }

    // moved from update
    void OnTriggerStay(Collider other)
    {
        Quaternion handRot1 = other.transform.rotation * Quaternion.Euler(90,0, 0) * Quaternion.Euler(0, 90, 0);
        Quaternion handRot2 = other.transform.rotation * Quaternion.Euler(90, 0, 0) * Quaternion.Euler(0, -90, 0);
        if (other.CompareTag("Sword") && !activated)
        {
            if ((prevPoint == null || prevPoint.activated) && (Quaternion.Angle(targetRot, handRot1) < maxAngle || Quaternion.Angle(targetRot, handRot2) < maxAngle))
            //if ((prevPoint == null || prevPoint.activated) && Quaternion.Angle(targetRot, handRot1) < maxAngle)
            {
                OnActivate();
            }
        }
    }

    public void SetDetectable(bool detectable)
    {
        this.detectable = detectable;
    }
}
