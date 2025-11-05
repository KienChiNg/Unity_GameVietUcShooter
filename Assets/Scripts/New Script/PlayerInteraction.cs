using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerInteraction : xrInput
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        targetDeviceLeft.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue_L);
        targetDeviceLeft.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue_L);
        targetDeviceRight.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue_R);
        targetDeviceRight.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue_R);

        if(triggerValue_L > 0.1f && primaryButtonValue_L)
        {

        }
    }

}
