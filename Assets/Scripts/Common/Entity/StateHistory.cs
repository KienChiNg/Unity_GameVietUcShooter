using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StateHistory 
{
    public DateTime time;
    public State state;

    public StateHistory(DateTime time, State state)
    {
        this.time = time;
        this.state = state;
    }

}
