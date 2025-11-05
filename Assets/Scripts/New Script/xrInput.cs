using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class xrInput : MonoBehaviour
{
    protected InputDevice targetDeviceRight;
    protected InputDevice targetDeviceLeft;

    protected bool triggerPress_L;
    protected bool gripPress_L;
    protected bool menubutton;

    // Start is called before the first frame update
    public virtual void Start()
    {
        List<InputDevice> devicesRight = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devicesRight);

        if (devicesRight.Count > 0)
        {
            targetDeviceRight = devicesRight[0];
        }

        List<InputDevice> devicesLeft = new List<InputDevice>();
        InputDeviceCharacteristics leftControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devicesLeft);

        if (devicesLeft.Count > 0)
        {
            targetDeviceLeft = devicesLeft[0];
        }

        

    }

    // Update is called once per frame
    public virtual void Update()
    {


        //RIGHT
        targetDeviceRight.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue_R);
        if (primaryButtonValue_R)
        {
            //Debug.Log("Right Primary A Button pressed!");
        }

        targetDeviceRight.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue_R);
        if (secondaryButtonValue_R)
        {
            //Debug.Log("Right Secondary B Button pressed!");
        }

        targetDeviceRight.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue_R);
        if (triggerValue_R > 0.1f)
        {
            //Debug.Log("Right Trigger pressed " + triggerValue_R);
        }

        targetDeviceRight.TryGetFeatureValue(CommonUsages.grip, out float gripValue_R);
        if (gripValue_R > 0.1f)
        {
            //Debug.Log("Right Grip pressed " + gripValue_R);
        }

        //LEFT
        targetDeviceLeft.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue_L);
        if (primaryButtonValue_L)
        {
            //Debug.Log("Left Primary A Button pressed!");
        }

        targetDeviceLeft.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue_L);
        if (secondaryButtonValue_L)
        {
            //Debug.Log("Left Secondary B Button pressed!");
        }

        targetDeviceLeft.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue_L);
        if (triggerValue_L > 0.1f)
        {
            //Debug.Log("Left Trigger pressed " + triggerValue_L);
        }

        targetDeviceLeft.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPress_L);


        targetDeviceLeft.TryGetFeatureValue(CommonUsages.gripButton, out gripPress_L);

        
        targetDeviceLeft.TryGetFeatureValue(CommonUsages.grip, out float gripValue_L);
        if (gripValue_L > 0.1f)
        {
            //Debug.Log("Left Grip pressed " + gripValue_L);
        }
       
    }
}
