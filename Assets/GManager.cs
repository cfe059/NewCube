using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


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
    private List<string[]> _csvSpawn = new List<string[]>();
    
    [SerializeField] private GameObject bgDay;
    [SerializeField] private GameObject bgNight;
    [SerializeField] public List<LevelMaster> _levelMaster;
    [SerializeField] private TextMeshProUGUI _levelText;
    
    [SerializeField] public List<canSpawn> _canSpawns ;
    [SerializeField] public List<RandomName> _RandomNameWeapon ;
    [SerializeField] public List<RandomName> _RandomNameArmor ;
    [SerializeField] public List<RandomName> _RandomNameFoods ;
    [SerializeField] public List<RandomName> _RandomNameHerbs ;
    
    void Awake()
    {
        Instance = this;
        canSpawnItem();
        CSV_RName();
        RandomData();
    }
    
    private void Start()
    {
        Days_Change(_GameState.days);
        _levelMaster = GetComponent<CSVReader>()._LevelData;
        
    }

    void RandomData()
    {
        RandomDataFoods();
        RandomDataHerbs();
        RandomDataEquipMent();
    }

    void RandomDataEquipMent()
    {
        Equipment_Obj[] items = Resources.LoadAll<Equipment_Obj>("Items/Data/");
        foreach (var item in items)
        {
            if (item._ItemType == ItemType.Weapon)
            {
                int r = Random.Range(0, _RandomNameWeapon.Count);
                item.R_Data = _RandomNameWeapon[r];
                _RandomNameWeapon.RemoveAt(r);
                
            }else if (item._ItemType == ItemType.Armor)
            {
                int r = Random.Range(0, _RandomNameArmor.Count);
                item.R_Data = _RandomNameArmor[r];
                _RandomNameArmor.RemoveAt(r);
            }
        }
    }
    void RandomDataFoods()
    {
        Food_Obj[] items = Resources.LoadAll<Food_Obj>("Items/Data/");
        foreach (var item in items)
        {
            if (item._ItemType == ItemType.Food)
            {
                int r = Random.Range(0, _RandomNameFoods.Count);
                item.R_Data = _RandomNameFoods[r];
                _RandomNameFoods.RemoveAt(r);

            }
        }
    }
    void RandomDataHerbs()
    {
        Herb_Obj[] items = Resources.LoadAll<Herb_Obj>("Items/Data/");
        foreach (var item in items)
        {
            if (item._ItemType == ItemType.Herb)
            {
                int r = Random.Range(0, _RandomNameHerbs.Count);
                item.R_Data = _RandomNameHerbs[r];
                _RandomNameHerbs.RemoveAt(r);

            }
        }
    }
    void CSV_RName()
    {
        List<string[]> _csvData = new List<string[]>();
        TextAsset _csvItem = Resources.Load<TextAsset>("Data/randomname_master");
        StringReader reader = new StringReader(_csvItem.text);
        
        while (reader.Peek() != -1) 
        {
            string line = reader.ReadLine();
            _csvData.Add(line.Split(','));
        }
    
        
        List<RandomName> _tmpNameW = new List<RandomName>();
        List<RandomName> _tmpNameA = new List<RandomName>();
        List<RandomName> _tmpNameF = new List<RandomName>();
        List<RandomName> _tmpNameH = new List<RandomName>();

        for (int i = 1; i < _csvData.Count; i++)
        {
            RandomName _tmp = new RandomName();
            _tmp.ID = int.Parse(_csvData[i][0]);
            _tmp.RName = _csvData[i][1];
            _tmp.Rimg = _csvData[i][2];
            //Debug.Log(_tmp.ID);
            if (_csvData[i][0].Substring(0,1) == "1")
            {
                _tmpNameW.Add(_tmp);

            }
            else if (_csvData[i][0].Substring(0,1) == "2")
            {
                _tmpNameA.Add(_tmp);

            }else if (_csvData[i][0].Substring(0,1) == "4")
            {
                _tmpNameH.Add(_tmp);

            }else if (_csvData[i][0].Substring(0,1) == "5")
            {
                _tmpNameF.Add(_tmp);
            }
        }
        _RandomNameWeapon = _tmpNameW;
        _RandomNameArmor = _tmpNameA;
        _RandomNameHerbs = _tmpNameH;
        _RandomNameFoods = _tmpNameF;
    }
    public void ChangeLevel(int level)
    {
        _levelText.text = $"Lvl.{level}";
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
            canSpawnItem();
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
    
    void canSpawnItem()
    {
        if (_csvSpawn.Count==  0)
        {
            TextAsset _csvItem = Resources.Load<TextAsset>("Data/drop_master");
            StringReader reader = new StringReader(_csvItem.text);
        
            while (reader.Peek() != -1) 
            {
                string line = reader.ReadLine();
                _csvSpawn.Add(line.Split(','));
            }
        }
        
        List<canSpawn> _tmpSpawns = new List<canSpawn>();

        for (int i = 1; i < _csvSpawn.Count; i++)
        {
            canSpawn _tmpSpawn = new canSpawn();
//            Debug.Log(_csvSpawn[i][1]);
            if (_GameState.days >= int.Parse(_csvSpawn[i][1]) && _GameState.days <= int.Parse(_csvSpawn[i][2]))
            {
                _tmpSpawn.ID = int.Parse(_csvSpawn[i][0]);
                _tmpSpawns.Add(_tmpSpawn);
            }
            
        }

        _canSpawns = _tmpSpawns;
    }
}

[Serializable]
public class canSpawn
{
    public int ID;

}
[Serializable]
public class RandomName
{
    public int ID;
    public string RName;
    public string Rimg;

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
