using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonEventCodes
{
    public const byte HISTORY_LOG = 1;
    public const byte EVENT_DATA = 2;
    public const byte REQUEST_LESSION_ALL_EVENT_DATA = 3;
    public const byte SEND_LESSION_ALL_EVENT_DATA = 4;
    public const byte REQUEST_All_HISTORY_LOG = 5;
    public const byte SEND_All_HISTORY_LOG = 6;
    public const byte REQUEST_LESSION_CURRENT_STATE = 7;
    public const byte SEND_LESSION_CURRENT_STATE = 8;
    public const byte UPDATE_STATE_HISTORY = 9;
    public const byte REQUEST_CURRENT_STATE_HISTORY = 10;
    public const byte SEND_CURRENT_STATE_HISTORY = 11;
    public const byte REQUEST_BANG_VIET_CONTENT = 12;
    public const byte SEND_BANG_VIET_CONTENT = 13;
    public const byte SEND_EVENT_COMMAND = 14;
    public const byte SEND_CURRENT_LIST_USER = 15;
    public const byte REQUEST_CURRENT_LIST_USER = 16;
    public const byte START_XET_NGHIEM_MAU = 17;
    public const byte START_TIEM_DIEN = 18;
    public const byte STOP_TIEM_DIEN = 19;
    public const byte REQUEST_ALL_USING_TIEM_DIEN = 20;
    public const byte SEND_ALL_USING_TIEM_DIEN = 21;
    public const byte REQUEST_LESSON_STEP_DETAILS = 22;
    public const byte SEND_ALL_LESSON_STEP_DETAILS = 23;
    public const byte EVENT_RESPONSE_MESSAGE = 24;
    public const byte START_CHUP_X_QUANG = 25;
    public const byte ELECTRIC_SHOCK_CHARGE = 26;



    public const byte START_GAME = 27;
    public const byte PAUSE_GAME = 28;
    public const byte END_GAME = 29;


    public const byte CHEST_COMPRESSION = 30;
    public const byte AMBU_MASK = 31;

    public const byte WIN_PLAYER = 32;
    public const byte WIN_PLAYER_TIME = 33;
    public const byte WIN_PLAYER_INDEX = 34;
    public const byte REMATCH = 35;
    public const byte NAME_PLAYER = 36;
    public const byte PING_PLAYER = 37;
}
