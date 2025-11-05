using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highlight : MonoBehaviour
{
    Material[] mats;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        mats = GetComponent<Renderer>().materials;
    }

    // Update is called once per frame
    void Update()
    {
        float alphaCh = Mathf.Sin(Mathf.Deg2Rad * i);
        i += 3;
        mats[0].SetColor("_Color", new Color(mats[0].GetColor("_Color").r, mats[0].GetColor("_Color").g, mats[0].GetColor("_Color").b, Mathf.Abs(alphaCh) * 0.3f));
        
    }

    private void OnDisable()
    {
        mats[0].SetColor("_Color", new Color(mats[0].GetColor("_Color").r, mats[0].GetColor("_Color").g, mats[0].GetColor("_Color").b, 0));
    }

}
