using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmAction : MonoBehaviour
{
    public static ConfirmAction instance;
    public NhiVR.Event action;
    public GameObject label;
    public GameObject button;
    public GameObject parameters;
    public GameObject inputPrefabs;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initConfirmDrug(NhiVR.Event action)
    {
        button.GetComponent<Button>().onClick.RemoveAllListeners();
        label.GetComponent<Text>().text = action.eventname;
        this.action = action;
        for (int i = 0; i < parameters.transform.childCount; i++)
        {
            Destroy(parameters.transform.GetChild(i).gameObject);
        }

        if (action.parameters.typeId == TypeParameterCode.FLOAT_DON || action.parameters.typeId == TypeParameterCode.FLOAT_DAI)
        {
            GameObject input = Instantiate(inputPrefabs, parameters.transform);
            input.transform.GetChild(0).gameObject.GetComponent<Text>().text = action.parameters.name;
            input.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = action.parameters.unit;
        }
        else
        {

        }
        
        button.GetComponent<Button>().onClick.AddListener(() => { OnClickStart(); });

    }

    public void OnClickStart()
    {
        
        for (int i = 0; i < parameters.transform.childCount; i++)
        {
            InputField value = parameters.transform.GetChild(i).GetChild(1).GetChild(1).gameObject.GetComponent<InputField>();
            switch(action.parameters.typeId)
            {
                case TypeParameterCode.FLOAT_DON:
                    float result;
                    if (float.TryParse(value.text, out result))
                    {
                        action.parameters.value = result;
                    }
                    break;
            }
            
        }
        action.parameters.value_str = "TRUE";
        action.Execute();

    }
}
