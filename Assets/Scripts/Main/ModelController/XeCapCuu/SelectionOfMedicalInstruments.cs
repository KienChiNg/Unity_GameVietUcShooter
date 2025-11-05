using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionOfMedicalInstruments : MonoBehaviour
{
    public GameObject UISelect, item, dungtichbong;
    public string NameMI;
    public Sprite UI;
    public Image image;
    public Text nameItemText;
    public HoldingItem holdingItem;
    
    private void Start()
    {
        if(item)
        {
            item.SetActive(false);
        }
        
    }
    private void OnMouseDown()
    {
        if(gameObject.GetComponentInParent<NganKeoController>().isClose == false && !UIEvent.instance.IsPointerOverUIObject())
        {
            //disable dropdown if game object is not E3
            if (gameObject.name.Equals("E3Prepare"))
            {
                dungtichbong.SetActive(true);
            }
            else
            {
                dungtichbong.SetActive(false);
            }
            //open Select item UI
            SelectItem.instance.item = holdingItem;
            holdingItem.item = item;

            UISelect.SetActive(true);
            nameItemText.text = NameMI;
            image.sprite = UI;
            image.preserveAspect = true;
        }
        
    }
}
