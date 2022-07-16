using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
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
    [SerializeField]
    private GameObject objLogger;
    [SerializeField]
    private GameObject objTextLogger;
    private List<GameObject> _listLogger= new List<GameObject>();
    [SerializeField]
    private GameObject _days;

    [SerializeField] private GameObject bgDay;
    [SerializeField] private GameObject bgNight;
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Days_Change(_GameState.days);
    }

    public void Logger(string log)
    {
        Game_Log.Add(log);
        TextLogger(log);
    }

    void Days_Change(int day)
    {
        _days.GetComponent<TextMeshProUGUI>().text = $"{day} 日";
    }
    void TextLogger(string log)
    {
        GameObject obj = Instantiate(objTextLogger, objLogger.transform);
        obj.GetComponent<TextMeshProUGUI>().text = log;
        obj.GetComponent<RectTransform>().localPosition = new Vector3(0, -80f,0);
        if (_listLogger.Count > 0)
        {
            for (int i = 0; i < _listLogger.Count; i++)
            {
                _listLogger[i].GetComponent<RectTransform>().localPosition += new Vector3(0, 30f,0);
            }
        }
        _listLogger.Add(obj);

    }
    public void Change_TurnBase(TurnBase turnBase)
    {
        _turnBase = turnBase; 
        //Logger($"ターン変更　：　{turnBase}");
    }
    
    public void Change_TimeBase()
    {
        if (_GameState.time_days)
        {
            bgNight.GetComponent<SpriteRenderer>().DOFade(0,1f).SetDelay(0.8f)
                .Play();
            bgDay.GetComponent<SpriteRenderer>().DOFade(1, 1f).SetDelay(0.8f)
                .Play();

            _GameState.time_days = false;
            _GameState.days += 1;
            Days_Change(_GameState.days);
            Logger($"日数変更　：　{_GameState.days}");
        }
        else
        {
            _GameState.time_days = true;
            
            bgDay.GetComponent<SpriteRenderer>().DOFade(0,1f).SetDelay(0.8f)
                .Play();
            bgNight.GetComponent<SpriteRenderer>().DOFade(1, 1f).SetDelay(0.8f)
                .Play();
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
