using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineOfflineMode : MonoBehaviour
{
    public Toggle onoffToggle;
    public Text toggleLabel;
    public InputField ipInput;

    // Start is called before the first frame update
    void Start()
    {
        //add event to onofftoggle
        onoffToggle.onValueChanged.AddListener(delegate {
            if(onoffToggle.isOn)
            {
                toggleLabel.text = "ONLINE";
                toggleLabel.color = Color.green;
                Common.instance.isOnline = true;
                //disable input ip
                ipInput.gameObject.SetActive(false);
                
            } else
            {
                toggleLabel.text = "OFFLINE";
                toggleLabel.color = Color.red;
                Common.instance.isOnline = false;
                //enable input ip
                ipInput.gameObject.SetActive(true);
               
            }
        });

        //add event to ipInput
        ipInput.onValueChanged.AddListener(delegate
        {
            Common.instance.ipAddress = ipInput.text;
        });

        Common.instance.ipAddress = ipInput.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
