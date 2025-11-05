using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Parameter
{
    public int typeId;
    public string name;
    public float value;
    public float value_max;
    public string value_str;
    public string unit;

    public Parameter(int typeId, string name, string unit)
    {
        this.typeId = typeId;
        this.name = name;
        this.unit = unit;
    }
}
