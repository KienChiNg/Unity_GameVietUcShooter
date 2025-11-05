using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnUIEvent : UIBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public UnityEvent onMouseIn;
    public UnityEvent onMouseExit;
    public UnityEvent onMouseClick;

    public void OnPointerExit(PointerEventData eventData)
    {
        onMouseExit.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onMouseIn.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onMouseClick.Invoke();
    }

}
