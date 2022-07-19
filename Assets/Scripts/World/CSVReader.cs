using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class CSVReader : MonoBehaviour
{
    private TextAsset _csvItem;
    private TextAsset _csvLevel;
    private TextAsset _csvStats;
    
//    public List<string[]> _csvDatas = new List<string[]>();

    public List<Item_System> _itemSystem;
    public List<LevelMaster> _LevelData;
    public List<StatusMaster> _StatusData;
    // Start is called before the first frame update
    //_csv Data :: _csvDatas[x][0] == ID
    //_csv Data :: _csvDatas[x][1] == ItemName
    //_csv Data :: _csvDatas[x][2] == canEquip
    //_csv Data :: _csvDatas[x][3] == Heal
    //_csv Data :: _csvDatas[x][4] == Atk
    //_csv Data :: _csvDatas[x][5] == Hp
    //_csv Data :: _csvDatas[x][6] == Def
    //_csv Data :: _csvDatas[x][7] == evade
    //_csv Data :: _csvDatas[x][8] == crirate
    //_csv Data :: _csvDatas[x][9] == cridmg
    //_csv Data :: start at 1
    
    void Awake()
    {
        LevelDataRead();
        StatusDataRead();
    }

    void LevelDataRead()
    { 
        List<string[]> _csvDatas = new List<string[]>();

        _csvLevel = Resources.Load<TextAsset>("Data/gyakun_level_master");
        StringReader reader = new StringReader(_csvLevel.text);
        
        while (reader.Peek() != -1) 
        {
            string line = reader.ReadLine();
            _csvDatas.Add(line.Split(','));
        }

        for (int i = 1; i < _csvDatas.Count; i++)
        {
            LevelMaster lvl = new LevelMaster();
            lvl.Level = int.Parse(_csvDatas[i][0]);
            lvl.NextExp = int.Parse(_csvDatas[i][1]);
            lvl.TotalExptoNext = int.Parse(_csvDatas[i][2]);
            _LevelData.Add(lvl);
        }
        
    }
    void StatusDataRead()
    { 
        List<string[]> _csvDatas = new List<string[]>();

        _csvStats = Resources.Load<TextAsset>("Data/gyakun_status_master");
        StringReader reader = new StringReader(_csvStats.text);
        
        while (reader.Peek() != -1) 
        {
            string line = reader.ReadLine();
            _csvDatas.Add(line.Split(','));
        }

        for (int i = 1; i < _csvDatas.Count; i++)
        {
            StatusMaster stats = new StatusMaster();
            stats.Level = int.Parse(_csvDatas[i][0]);
            stats.MaxHp = int.Parse(_csvDatas[i][1]);
            stats.Atk = int.Parse(_csvDatas[i][2]);
            stats.Def = int.Parse(_csvDatas[i][3]);
            _StatusData.Add(stats);
        }
        
    }
    void ItemRead()
    {
        List<string[]> _csvDatas = new List<string[]>();

        _csvItem = Resources.Load<TextAsset>("Data/item_data");
        StringReader reader = new StringReader(_csvItem.text);
        
        while (reader.Peek() != -1) 
        {
            string line = reader.ReadLine();
            _csvDatas.Add(line.Split(','));
        }

        for (int i = 1; i < _csvDatas.Count; i++)
        {
            Item_System item = new Item_System();
            item.id = int.Parse(_csvDatas[i][0]);
            item.itemName = _csvDatas[i][1];
            item.isEquipment = bool.Parse(_csvDatas[i][2]);
            if (item.isEquipment)
            {
                item.CanEquipItem.atk = float.Parse(_csvDatas[i][4]);
                item.CanEquipItem.hp = float.Parse(_csvDatas[i][5]);
                item.CanEquipItem.def = float.Parse(_csvDatas[i][6]);
                item.CanEquipItem.evasions = float.Parse(_csvDatas[i][7]);
                item.CanEquipItem.critical_rate = float.Parse(_csvDatas[i][8]);
                item.CanEquipItem.critical_damage = float.Parse(_csvDatas[i][9]);
            }
            else
            {
                item.CanUseItem.hpHeal = float.Parse(_csvDatas[i][3]);
            }
            _itemSystem.Add(item);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
[Serializable]
public class LevelMaster
{
    public int Level;
    public int NextExp;
    public int TotalExptoNext;
}
[Serializable]
public class StatusMaster
{
    public int Level;
    public int MaxHp;
    public int Atk;
    public int Def;
}
