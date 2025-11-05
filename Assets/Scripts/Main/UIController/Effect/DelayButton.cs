using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DelayButton : MonoBehaviour, IPointerUpHandler
{
    public float delaytime;
    float pressTime;
    // Start is called before the first frame update
    void Start()
    {
        pressTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //disable button when clicked
        //and enable it after DELAY TIME
        if (pressTime + delaytime > Time.unscaledTime)
        {
            GetComponent<Button>().interactable = false;
        } else
        {
            GetComponent<Button>().interactable = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (pressTime + delaytime > Time.unscaledTime)
            return;
        //set press time
        pressTime = Time.unscaledTime;
    }
}
