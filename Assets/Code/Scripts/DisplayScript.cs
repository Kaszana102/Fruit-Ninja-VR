using UnityEngine;
using System.Collections;
using System.Linq;

public class DisplayScript : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON.
        // Check if additional displays are available and activate each.
        Display.displays[0].Activate();
        if(Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
        }
        
    }
}