using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform leftHand, rightHand;
    public static Player instance { get; private set; } // singleton

    bool waitingForDevices = true;
    bool gotDevices = false;
    UnityEngine.XR.InputDevice rightController;


    bool slowMoActive = false;
    float slowMoStartTime = float.MinValue;
    const float slownoDuration = 4f;
    float slowmoCooldown = 14f;
    const float slomoScale = 0.5f;

    public void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(instance); // what if scene change?
        }
        Player.instance = this;
    }

    IEnumerator setupVR()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        while (!gotDevices)
        {

            UnityEngine.XR.InputDevices.GetDevices(inputDevices);
            if (inputDevices.Count < 2)
            {
                yield return null;
            }
            else
            {
                gotDevices = true;
            }
        }

        //if got them all
        foreach (var device in inputDevices)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
#pragma warning restore CS0618 // Type or member is obsolete
        }


        var rightHandedControllers = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, rightHandedControllers);
        rightController = rightHandedControllers[0];
        
        waitingForDevices = false;
    }

    void checkIfGotDevices()
    {
        if (!rightController.isValid)
        {
            if (!waitingForDevices)
            {
                StartCoroutine(setupVR());
                waitingForDevices = true;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(setupVR());
    }

    // Update is called once per frame
    void Update()
    {
        checkIfGotDevices();

        if (gotDevices)
        {
            CheckMenu();
            CheckSlomo();
          
        }

        if(slowMoActive && GameManager.Instance.state == GameManager.GameState.FruitCut)
        {
            Time.timeScale = slomoScale;
        }

        if(slowMoActive && Time.time > slowMoStartTime + slownoDuration)
        {
            slowMoActive = false;
            Time.timeScale = 1f;
        }
    }

    bool gripButtonWasPressed = false;
    void CheckSlomo()
    {
        bool gripPressed;
        if (rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out gripPressed))
        {
            if (gripPressed)
            {
                if (gripButtonWasPressed)
                {
                    // pass
                }
                else
                {
                    gripButtonWasPressed = true;
                    Debug.Log("Grip button is pressed.");

                    if (GameManager.Instance.state != GameManager.GameState.Paused)
                    {
                        if (!slowMoActive && Time.time > slowMoStartTime + slowmoCooldown)
                        {
                            slowMoActive = true;
                            slowMoStartTime = Time.time;
                            Time.timeScale = slomoScale;
                        }
                    }                                    
                }
            }
            else
            {
                gripButtonWasPressed = false;
            }
            
        }
    }

    bool menuButtonWasPressed = false;
    void CheckMenu()
    {
        bool menuButtonPressed;
        if (rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.menuButton, out menuButtonPressed))
        {
            if (menuButtonPressed)
            {
                if (menuButtonWasPressed)
                {
                    // pass
                }
                else
                {
                    menuButtonWasPressed = true;
                    Debug.Log("Menu button is pressed.");
                    if (GameManager.Instance.state == GameManager.GameState.FruitCut)
                    {                        
                        GameManager.Instance.PauseGame();
                    }
                    else
                    {
                        GameManager.Instance.UnpauseGame();
                    }
                }
            }
            else
            {
                menuButtonWasPressed = false;
            }
        }
    }

    public float getRemainingCooldown()
    {

        return (slowMoStartTime + slowmoCooldown) - Time.time > 0 ?
            (slowMoStartTime + slowmoCooldown) - Time.time : 0;
    }


}
