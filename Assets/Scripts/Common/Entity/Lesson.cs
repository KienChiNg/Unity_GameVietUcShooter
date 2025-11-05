using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson
{
    public string name;
    public int lessonid;
    public string usercreated;
    public string datecreated;
    public string events;
    public string currentstate;
}
public class LESSONCODE
{
    public string lessoncode;
    public string currentstate;
    public string events;
    public int lessonid;
}

[Serializable]
public class LESSON_DETAILS
{
    public int lesson_step;
    public string current_state;
    public string next_state;
    public List<string> required_events;
}