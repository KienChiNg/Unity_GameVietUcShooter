using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class State
{
    public long id;
    public string name;
    public string code;
    public Dictionary<string,State_details> state;

    public State()
    {
        state = new Dictionary<string, State_details>();
        code = "";
    }

    public void addStateDetails(JSONObject details)
    {
        State_details sd = new State_details();
        sd.id = details.GetField("id").i;
        sd.name = details.GetField("name").str;
        sd.valueType = (int)details.GetField("valuetype").i;
        sd.spcode = details.GetField("spcode").str;
        sd.bodypart = (int)details.GetField("bodypart").i;
        sd.onmanequi = (int)details.GetField("onmanequi").i;

        switch (sd.valueType)
        {
            case TypeParameterCode.FLOAT_DON:
                
                sd.value = details.GetField("value").n.ToString();
                break;

            case TypeParameterCode.STRING:
                sd.value = details.GetField("value").str;
                break;
        }

        if(!state.ContainsKey(sd.spcode.ToUpper()))
        {
            state.Add(sd.spcode.ToUpper(), sd);
        }
       
    }

    public void addStateDetails(State_details sd)
    {
        if (!state.ContainsKey(sd.spcode.ToUpper()))
        {
            state.Add(sd.spcode.ToUpper(), sd);
        }
    }

    
}

[System.Serializable]
public class State_details
{
    public long id;
    public string name;
    public int bodypart;
    public int onmanequi;
    public string spcode;
    public int valueType;
    public string value;
    public string value_max;
    
    public string getValue()
    {
        switch(valueType)
        {
            case TypeParameterCode.FLOAT_DAI:
                int rand = Random.Range(int.Parse(value), int.Parse(value_max));
                return rand.ToString();
     
        }

        return value;
    }
}
