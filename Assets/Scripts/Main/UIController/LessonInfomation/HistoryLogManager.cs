using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class HistoryLogManager : MonoBehaviour
{
    public static HistoryLogManager instance;
    public GameObject layout;
    public GameObject statePrefabs;
    public GameObject historyPrefabs;

    public Scrollbar scrollbar;
    int length;

    public UIEffectComponent openEffect;
    public UIEffectComponent closeEffect;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {     
        length = 0;
        gameObject.SetActive(false);
        clearPreviousItem();
    }

    //reset history prefabs
    public void ResetHistoryLog()
    {
        SortedList listLog = HistoryLogSynchronize.instance.getListLog();

        if (length != listLog.Count)
        {
            length = listLog.Count;

            clearPreviousItem();

            for (int i = 0; i < length; i++)
            {
                HistoryLog log = (HistoryLog)listLog.GetByIndex(i);
                AddHistoryLog(log, i);
            }
        }
    }

    //add history prefab
    public void AddHistoryLog(HistoryLog log, int index)
    {
        for(int i = 0; i < layout.transform.childCount; i ++)
        {
            if(layout.transform.GetChild(i).name == log.statecode)
            {
                if(addExistedHistory(layout.transform.GetChild(i), log) == true)
                {
                    return;
                }
                //init prefab
                GameObject historyChild1 = Instantiate(historyPrefabs, Vector3.zero, Quaternion.identity, layout.transform.GetChild(i));
                historyChild1.GetComponent<HistoryLogChild>().setHistoryLog(log);
                historyChild1.GetComponent<HistoryLogChild>().UpdateTextValue();

                //change color of prefab follow index
                if (layout.transform.childCount % 2 == 1)
                {
                    historyChild1.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                }

                //scroll to 0
                scrollbar.value = 0;
                return;
            }
        }

        //init new parent
        GameObject historyParent = Instantiate(statePrefabs, Vector3.zero, Quaternion.identity, layout.transform);
        historyParent.name = log.statecode;
        historyParent.transform.GetChild(0).GetComponent<Text>().text = "Trạng thái " + (layout.transform.childCount);
        //init prefab
        GameObject historyChild = Instantiate(historyPrefabs, Vector3.zero, Quaternion.identity, historyParent.transform);
        historyChild.GetComponent<HistoryLogChild>().setHistoryLog(log);
        historyChild.GetComponent<HistoryLogChild>().UpdateTextValue();
        
        //scroll to 0
        scrollbar.value = 0;

    }

    public bool addExistedHistory(Transform list, HistoryLog log)
    {
        for(int i = 1; i < list.childCount; i ++)
        {
            HistoryLogChild hlc = list.GetChild(i).GetComponent<HistoryLogChild>();
            if (hlc.log.userName == log.userName && log.e.eventcode == hlc.log.e.eventcode)
            {
                if(log.e.eventcode == "E5" || log.e.eventcode == "E3")
                {
                    hlc.log.time = log.time;
                    hlc.log.count++;
                    hlc.UpdateTextValue();
                    return true;
                }
               
            }
            
        }

        return false;
    }

    //delete previous prefabs
    public void clearPreviousItem()
    {
        for(int i = 0; i < layout.transform.childCount; i++)
        {
            GameObject.Destroy(layout.transform.GetChild(i).gameObject);
        }
    }

    public void OpenPopup()
    {
        if(!UIEffectController.instance.isGameObjectExist(gameObject))
        {
            gameObject.SetActive(true);
            openEffect.DisplayEffect(gameObject, true);
        }
        
    }

    public void ClosePopup()
    {
        if (!UIEffectController.instance.isGameObjectExist(gameObject))
        {
            closeEffect.DisplayEffect(gameObject, false);
        }
        
    }
}
