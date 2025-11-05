using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectComponent : MonoBehaviour
{
    [HideInInspector]
    public UIEffect effect = new UIEffect();

    // Start is called before the first frame update
    void Start()
    {

    }

    public bool DisplayEffect(GameObject gObject, bool finalState)
    {
        effect.gameObject = gObject;
        effect.finalState = finalState;

        while(effect.gameObject != gObject)
        {
            effect.gameObject = gObject;
        }

        return UIEffectController.instance.Run(effect);  
    }

}
