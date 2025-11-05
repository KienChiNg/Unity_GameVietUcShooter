using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingItem : MonoBehaviour
{
    [Range(0,10)] public float height = 3f;
    public bool start = false;// move item befor click UI select item 
    public Collider offCollider;
    Vector3 oldPos, oldLocal;
    public GameObject item;
    Vector3 target;
    float percentLerp = 0;
    public float timeLerp = 2;

    public Vector3 GetCenterOfItem(GameObject item)
    {
        if(item.GetComponent<Renderer>())
        {
            return item.GetComponent<Renderer>().bounds.center;
        } else
        {
            return item.transform.position;
        }
    }

    private void Update()
    {
        if (start)
        {
            if(percentLerp <= 1)
            {
                percentLerp += Time.deltaTime / timeLerp;
                transform.position = Vector3.Lerp(oldPos, target, percentLerp);

                transform.Translate(new Vector3(0, height * Mathf.Sin(Mathf.PI * percentLerp), 0));

            } else
            {
                start = false;
                percentLerp = 0;
                //transform.localPosition = oldLocal;
                gameObject.SetActive(false);
                item.SetActive(true);
            }
          
        }
        
    }

    public void SetUpPosAndStart()
    {
        start = true;
        offCollider.enabled = false;
        //
        oldLocal = transform.localPosition;
        oldPos = transform.position;
        target = GetCenterOfItem(item);
        //calculate target coord
        Vector3 dif = transform.position - GetCenterOfItem(gameObject);
        target += dif;
    }
}
