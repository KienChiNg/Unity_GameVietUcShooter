using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightAndLabel : MonoBehaviour
{
    [Header("highlight")]
    public GameObject[] highlightObjects;
    List<Material> materialArray;
    [Header("popup label")]
    public GameObject popupLabel;
    public string labelName;
    public Vector3 offset;

    public bool displayLabel = true;
    // Start is called before the first frame update
    private void Start()
    {
        //find phatsang material and add to list
        materialArray = new List<Material>();
        foreach (GameObject o in highlightObjects)
        {
            Material[] matArray = o.GetComponent<Renderer>().materials;
            foreach (Material material in matArray)
            {
                if (material.name == "phatsang (Instance)")
                {
                    materialArray.Add(material);
                }
            }
        }

    }

    private void OnMouseEnter()
    {
        if (!UIEvent.instance.IsPointerOverUIObject() && (transform.GetComponent<CureToolModelController>() == null || transform.GetComponent<CureToolModelController>().inUse == false))
        {
            //change color of phatsang material in materialArray
            foreach (Material material in materialArray)
            {
                material.SetColor("_Color", new Color32(251, 244, 146, 60));
            }

            //display label for 3D object
            if (popupLabel && displayLabel)
            {
                popupLabel.SetActive(true);
                popupLabel.GetComponent<PlayerUI>().setLabel(gameObject, labelName, offset);
            }

        }
    }

    private void OnMouseExit()
    {
        if (!UIEvent.instance.IsPointerOverUIObject())
        {
            //change color of phatsang material in materialArray to 0
            foreach (Material material in materialArray)
            {
                material.SetColor("_Color", new Color32(0, 0, 0, 0));
            }

            //hidden label for 3D object
            if (popupLabel)
            {
                popupLabel.SetActive(false);
            }

        }
    }

    private void OnMouseUp()
    {
        //change color of phatsang material in materialArray to 0
        foreach (Material material in materialArray)
        {
            material.SetColor("_Color", new Color32(0, 0, 0, 0));
        }
        //hidden label for 3D object
        if (popupLabel)
        {
            popupLabel.SetActive(false);
        }
    }
}
