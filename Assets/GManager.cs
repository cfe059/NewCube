using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager Instance;
    // Start is called before the first frame update
    public enum TurnBase
    {
        Player_Turn,Player_Moving,Monster_Turn,Monster_Moving
    }
    public GameState _GameState;
    public TurnBase _turnBase;
    public bool MonsterMove;
    public int MonsterMove_Turn=0;
    public List<string> Game_Log;
    
    void Awake()
    {
        Instance = this;
    }
    public void Logger(string log)
    {
        Game_Log.Add(log);
    }
    public void Change_TurnBase(TurnBase turnBase)
    {
        _turnBase = turnBase;
        Logger($"ターン変更　：　{turnBase}");
    }
    
    public void Change_TimeBase()
    {
        if (_GameState.time_days)
        {
            _GameState.time_days = false;
            _GameState.days += 1;
            Logger($"日数変更　：　{_GameState.days}");
        }
        else
        {
            _GameState.time_days = true;
            

        }
    }
}
[System.Serializable]
public class GameState
{
    public int days;
    public bool time_days;
}
class Equip
{
    // weapon , armour, accessary
}

class BagItem
{
    
}
