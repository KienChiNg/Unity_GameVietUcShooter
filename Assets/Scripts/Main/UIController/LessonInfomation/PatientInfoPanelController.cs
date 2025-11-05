using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatientInfoPanelController : MonoBehaviour
{
    public static PatientInfoPanelController instance;
    [Header("Parameters")]
    public GameObject heartRate;
    public GameObject awRR;
    public GameObject glucozo;
    public GameObject Tperi;
    public GameObject nbp;
    public GameObject SPO2;
    public GameObject refill;
    public GameObject apvu;
    public GameObject ecg;
    public GameObject tiengkeu;
    public GameObject tiengphoiphai;
    public GameObject tiengphoitrai;
    [Header("Status bar")]
    public GameObject statusBtn;
    public GameObject statusBar;
    public GameObject scrollBar;

    [Header("Effect")]
    public UIEffectComponent openEffect;
    public UIEffectComponent closeEffect;
    
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void updateStatusBar(SortedList sortedList)
    {
        //destroy old prefabs
        for(int j = 0; j < statusBar.transform.childCount; j ++)
        {
           Destroy(statusBar.transform.GetChild(j).gameObject);
        }
        
        //init new prefabs
        for(int i = 0; i < sortedList.Count; i ++)
        {
            //get state history
            StateHistory stateHistory = (StateHistory)sortedList.GetByIndex(i);
            //set state history to prefab
            TimeSpan timeSpan = stateHistory.time.TimeOfDay;
            GameObject sttBtn = Instantiate(statusBtn, statusBar.transform);
            sttBtn.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Trạng thái " + (i + 1) + "\n" + "(" + timeSpan.ToString("hh\\:mm\\:ss") + ")";
            //sttBtn.transform.GetChild(1).gameObject.GetComponent<Text>().text = timeSpan.ToString("hh\\:mm\\:ss");
            sttBtn.name = i.ToString();

            sttBtn.GetComponent<Button>().onClick.AddListener(() => { OnChooseState(sttBtn.name); });

            if(i == sortedList.Count - 1)
            {
                sttBtn.GetComponent<Image>().color = new Color32(116, 208, 209, 255);
                selectState(sortedList.Count - 1);
                scrollBar.GetComponent<Scrollbar>().value = 1f;
            }
        }

    }

    //onclick state button
    public void OnChooseState(string indexStr)
    {
        //change color of clicked button
        int index = int.Parse(indexStr);
        for(int i = 0; i < statusBar.transform.childCount; i ++)
        {
            if(i == index)
            {
                statusBar.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color32(116, 208, 209, 255);
                
            } else
            {
                statusBar.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color32(34, 196, 197, 255);
            }
        }
        //display state parameter corresponding
        selectState(index);
    }

    public void selectState(int index)
    {
        //get state history
        StateHistory stateHistory = (StateHistory)StateHistorySynchronize.instance.listState.GetByIndex(index);
        
        //update state parameters
        if(index == StateHistorySynchronize.instance.listState.Count - 1 && !PhotonNetwork.CurrentRoom.CustomProperties["lessonCode"].Equals("MANUAL"))
        {
            setCurrentStateDetailParameter();
        } else
        {
            setStateDetailParameter(stateHistory.state);
        }
    }

    //
    public void setCurrentStateDetailParameter()
    {
        heartRate.GetComponent<Text>().text = "--";
        SPO2.GetComponent<Text>().text = "--";
        nbp.GetComponent<Text>().text = "--";
        glucozo.GetComponent<Text>().text = "--";
        Tperi.GetComponent<Text>().text = "--";
        awRR.GetComponent<Text>().text = "--";
        refill.GetComponent<Text>().text = "--";
        apvu.GetComponent<Text>().text = "--";
        ecg.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/ECGSprite/EMPTY");
    }



    //update UI follow State parameters
    public void setStateDetailParameter(State state)
    {
        if (state.state.ContainsKey("HR"))
        {
            SetParameter(heartRate.GetComponent<Text>(), state.state["HR"].getValue());
        }

        if (state.state.ContainsKey("SPO2"))
        {
            SetParameter(SPO2.GetComponent<Text>(), state.state["SPO2"].getValue());
        }

        if (state.state.ContainsKey("SNBP") && state.state.ContainsKey("DNBP"))
        {
            if(state.state["SNBP"].getValue().Equals("0") || state.state["DNBP"].getValue().Equals("0"))
            {
                nbp.GetComponent<Text>().text = "--";
            } else
            {
                nbp.GetComponent<Text>().text = getPressure(state.state["SNBP"].getValue(), state.state["DNBP"].getValue());
            }
        }

        if (state.state.ContainsKey("DUONGHUYET"))
        {
            SetParameter(glucozo.GetComponent<Text>(), state.state["DUONGHUYET"].getValue());
        }

        if (state.state.ContainsKey("TPERI"))
        {
            SetParameter(Tperi.GetComponent<Text>(), state.state["TPERI"].getValue());
        }

        if (state.state.ContainsKey("AWRR"))
        {
            SetParameter(awRR.GetComponent<Text>(), state.state["AWRR"].getValue());
        }

        if (state.state.ContainsKey("REFILL"))
        {
            SetParameter(refill.GetComponent<Text>(), state.state["REFILL"].getValue());
        }

        if (state.state.ContainsKey("APVU"))
        {
            apvu.GetComponent<Text>().text = state.state["APVU"].getValue();
        }
        
        if(state.state.ContainsKey("ECG"))
        {
            ecg.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/ECGSprite/" + state.state["ECG"].getValue());
        }

        if(state.state.ContainsKey("TIENGKEU"))
        {
            tiengkeu.GetComponent<Text>().text = getMouthSound(state.state["TIENGKEU"].getValue());
        }

        if (state.state.ContainsKey("RIGHT_LUNG_SOUND"))
        {
            tiengphoiphai.GetComponent<Text>().text = getLungSound(state.state["RIGHT_LUNG_SOUND"].getValue());
        }

        if (state.state.ContainsKey("LEFT_LUNG_SOUND"))
        {
            tiengphoitrai.GetComponent<Text>().text = getLungSound(state.state["LEFT_LUNG_SOUND"].getValue());
        }
    }

    public string getMouthSound(string sound)
    {
        switch (sound.ToUpper())
        {
            case "IM_LANG":
                return "Im lặng";
            case "LA_HET":
                return "La hét";
            case "HO":
                return "Ho";
            case "NON":
                return "Nôn";
            case "REN_RI":
                return "Rên rỉ";
            default:
                return "Error";
        }
    }

    public string getLungSound(string sound)
    {
        switch (sound.ToUpper())
        {
            case "IM_LANG":
                return "Im lặng";
            case "NORMAL":
                return "Bình thường";
            case "STRIDOR":
                return "Thở rít";
            case "WHEEZES":
                return "Khò khè";
            case "COARSE CRACKLES":
                return "Ran ẩm";
            default:
                return "Error";
        }
    }

    public string getPressure(string systolic, string diastolic)
    {
        //calculate avarage pressure
        int average = int.Parse(diastolic) + (int.Parse(systolic) - int.Parse(diastolic)) / 3;
        return systolic + "/" + diastolic + "\n(" + average + ")";
    }

    public void SetParameter(Text text, string value)
    {
        if(value.Equals("0"))
        {
            text.text = "--";
        } else
        {
            text.text = value;
        }
    }

    public void OpenPopup()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        openEffect.DisplayEffect(gameObject, true);
    }

    public void ClosePopup()
    {
        closeEffect.DisplayEffect(gameObject, false);
    }
}
