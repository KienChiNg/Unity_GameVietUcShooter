using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnMouseEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isEnter = false;

    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isEnter = true;
    }

    
}
