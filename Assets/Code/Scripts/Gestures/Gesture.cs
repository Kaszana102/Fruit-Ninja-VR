using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Gesture : MonoBehaviour
{
    public Transform pointsContainer;
    public Transform rotationSource;

    public bool parallelRotations = true;
    List<GesturePoint> points;
    bool finished = false;

    LineRenderer lr;
    public float arrowSize = 0.05f;

    float lastActivatedGesturePoint = 0;
    float maxDelay = 5f; // should be much lower
    int activationCounter = 0;
    Material fillMaterial;

    Action callback;

    private void OnEnable()
    {
        callback = () => Debug.LogError("Gesture callback for object: " + gameObject.name + " not set");
    }

    void Start()
    {
        // get points from hierarchy
        points =
            pointsContainer.Cast<Transform>()
            .OrderBy(t => t.GetSiblingIndex())
            .Select(t => t.GetComponent<GesturePoint>())
            .ToList();

        for(int i=0; i < points.Count; i++)
        {
            // update prev
            if (i-1>=0)
            {
                points[i-1].nextPoint = points[i];
                points[i].prevPoint = points[i-1];
            }

            // update next
            if (i+1 <= points.Count-1)
            {
                points[i].nextPoint = points[i + 1];
                points[i + 1].prevPoint = points[i];
            }
        }

        lr = GetComponent<LineRenderer>();
        lr.positionCount = points.Count;

        fillMaterial = lr.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (!finished)
        {            
            if (points.Last().activated)
            {
                finished = true;
                OnGestureFinished();
            }

            if (ShouldResetGesture())
            {
                ResetGesture();
            }
        }
        
    }


    bool ShouldResetGesture()
    {
        if (activationCounter > 0)
        {
            return Time.time > lastActivatedGesturePoint + maxDelay;
        }
        return false;
    }

    void ResetGesture()
    {
        foreach(GesturePoint point in points)
        {
            point.activated = false;
        }
        fillMaterial.SetFloat("_fill", 0);
        activationCounter = 0;
    }

    public void IncrementActivated()
    {
        lastActivatedGesturePoint = Time.time;
        activationCounter++;
        float percentage = ((float)activationCounter) / points.Count;
        fillMaterial.SetFloat("_fill", percentage);
    }

    void OnGestureFinished()
    {
        Debug.Log("GESTURE CAPTURED");
        callback();
    }

    public void SetGestureOnFinishedCallback(Action callback)
    {
        this.callback = callback;
    }
}
