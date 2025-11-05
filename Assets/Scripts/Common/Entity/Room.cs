using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public static Room instance = new Room();
    public string roomId;
    public string simID = "";
    public LESSONCODE currentLessonCode;
}
