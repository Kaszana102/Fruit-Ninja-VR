using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Gesture))]
public class GestureEditor : Editor
{
    public void OnSceneGUI()
    {
        // Get the chosen GameObject
        Gesture t = target as Gesture;
        DrawGesture(t);
    }

    public static void DrawGesture(Gesture t) {
        if (t == null || t.gameObject == null)
        {
            return;
        }

        if (t.pointsContainer == null) return;
        if (t.rotationSource == null) return;


        // get points from hierarchy
        List<GesturePoint> points = new List<GesturePoint>();
        points =
            t.pointsContainer.Cast<Transform>()
            .OrderBy(t => t.GetSiblingIndex())
            .Select(t => {
                GesturePoint gest = t.GetComponent<GesturePoint>();
                if (gest != null) return gest;
                return t.gameObject.AddComponent<GesturePoint>(); 
            })
            .ToList();

        LineRenderer lineRenderer = t.GetComponent<LineRenderer>();
        lineRenderer.positionCount = points.Count;

        // draw editor lines
        for (int i = 0; i < points.Count; i++)
        {
            Handles.color = Color.Lerp(Color.green, Color.red, 1.0f / points.Count * i);
            // draw line from prev            
            if (i - 1 >= 0)
            {                
                Handles.DrawLine(
                    points[i - 1].transform.position,
                    points[i].transform.position,
                    5f
                );                
            }

            Handles.DrawWireDisc(
                points[i].transform.position,
                Camera.current.transform.position - points[i].transform.position,
                    GesturePoint.maxDistance
            );

            // Set arrows
            if (points[i].Arrow == null)
            {
                GameObject arrowObject = Resources.Load("Prefabs/Gestures/Arrow") as GameObject;
                points[i].Arrow = PrefabUtility.InstantiatePrefab(arrowObject) as GameObject;
                points[i].Arrow.transform.SetParent(points[i].transform,true);
            }
            Vector3 arrowDirection;            
                    
            Handles.color = Color.blue;
            if (t.parallelRotations)
            {
                arrowDirection = t.rotationSource.transform.forward;                
            }
            else
            {
                arrowDirection = (points[i].transform.position - t.rotationSource.transform.position).normalized;                
            }

            Vector3 arrowStart = points[i].transform.position - arrowDirection;
            points[i].Arrow.transform.position = points[i].transform.position - arrowDirection/3;
            points[i].Arrow.transform.LookAt(arrowStart + arrowDirection);
            points[i].Arrow.transform.localRotation *= Quaternion.Euler(90, 0, 0);
            // add rotation depending on the verticality
            AddArrowRotationDependingOnVeticality(points, i);            
            points[i].Arrow.transform.localScale = Vector3.one * t.arrowSize;
        }

        // set lineRenderer points
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i].transform.position);
        }
        
    }

    static void AddArrowRotationDependingOnVeticality(List<GesturePoint> points, int i)
    {
        // check if there are 2 points, otherwise unknown behaviour
        if (points.Count != 1) {
            float xdif,ydif;
            // if first
            if (i == 0)
            {
                xdif = points[1].transform.position.z - points[0].transform.position.z;
                ydif = points[1].transform.position.y - points[0].transform.position.y;
            }
            // if last
            else if (i == points.Count - 1)
            {
               xdif = points[points.Count - 1].transform.position.z - points[points.Count - 2].transform.position.z;
               ydif = points[points.Count - 1].transform.position.y - points[points.Count - 2].transform.position.y;
            }
            else
            {
                xdif = points[i-1].transform.position.z - points[i+1].transform.position.z;
                ydif = points[i-1].transform.position.y - points[i+1].transform.position.y;
            }

            float additionalAngle = (float)Math.Atan2(ydif, xdif) * Mathf.Rad2Deg;
            points[i].Arrow.transform.localRotation *= Quaternion.Euler(0, additionalAngle, 0);
        }
    }
}



[CustomEditor(typeof(Transform))]
[CanEditMultipleObjects]
public class GestureOnTransformEditor : Editor
{
    //Unity's built-in editor
    Editor defaultEditor;
    Transform transform;
    void OnEnable()
    {
        //When this inspector is created, also create the built-in inspector
        defaultEditor = Editor.CreateEditor(targets, Type.GetType("UnityEditor.TransformInspector, UnityEditor"));
        transform = target as Transform;
    }

    void OnDisable()
    {
        //When OnDisable is called, the default editor we created should be destroyed to avoid memory leakage.
        //Also, make sure to call any required methods like OnDisable
        MethodInfo disableMethod = defaultEditor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (disableMethod != null)
            disableMethod.Invoke(defaultEditor, null);
        DestroyImmediate(defaultEditor);
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Local Space", EditorStyles.boldLabel);
        defaultEditor.OnInspectorGUI();        
          
        /*//Show World Space Transform
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("World Space", EditorStyles.boldLabel);

        GUI.enabled = false;
        Vector3 localPosition = transform.localPosition;
        transform.localPosition = transform.position;

        Quaternion localRotation = transform.localRotation;
        transform.localRotation = transform.rotation;

        Vector3 localScale = transform.localScale;
        transform.localScale = transform.lossyScale;

        defaultEditor.OnInspectorGUI();
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
        transform.localScale = localScale;
        GUI.enabled = true;*/
        
    }
    public void OnSceneGUI()
    {
        // Get the chosen GameObject
        Transform t = target as Transform;
        Gesture gesture = t.GetComponentInParent<Gesture>();

        if(gesture != null)
        {
            GestureEditor.DrawGesture(gesture);
        }
        
    }
}

    



