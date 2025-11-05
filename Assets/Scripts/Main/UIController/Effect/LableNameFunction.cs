using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//this script display label for UI components attached
public class LableNameFunction : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [Header("popup label")]
    public GameObject popupLabel;
    public string labelName;
    // Start is called before the first frame update
    void Start()
    {
        popupLabel.SetActive(false);
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        //display label when mouse over
        popupLabel.SetActive(true);
        popupLabel.transform.position = gameObject.GetComponent<RectTransform>().position + new Vector3(0, - GetComponent<RectTransform>().sizeDelta.y - 20, 0);
        popupLabel.transform.GetChild(0).gameObject.GetComponent<Text>().text = labelName;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        popupLabel.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        popupLabel.SetActive(false);
    }
}
