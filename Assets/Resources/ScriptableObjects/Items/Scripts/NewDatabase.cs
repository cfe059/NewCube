using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
#if UNITY_EDITOR

using UnityEditor;
#endif
using UnityEngine;

public class NewDatabase : ScriptableObject
{
    // Start is called before the first frame update


    static private TextAsset itemCSVPath;
#if UNITY_EDITOR
    
    [MenuItem("Generator/Generate Equipment")]

    static public void GenerateItems()
    {
        List<string[]> _csvDatas = new List<string[]>();

        itemCSVPath = Resources.Load<TextAsset>("Data/Equipment_master");
        StringReader reader = new StringReader(itemCSVPath.text);
        
        while (reader.Peek() != -1) 
        {
            string line = reader.ReadLine();
            _csvDatas.Add(line.Split(','));
        }
        for (int i = 1; i < _csvDatas.Count; i++)
        {
            Equipment_Obj item = ScriptableObject.CreateInstance<Equipment_Obj>();
            item.ID = int.Parse(_csvDatas[i][0]);
            if (item.ID.ToString().Substring(0,1) == "1")
            {
                item._ItemType = ItemType.Weapon;
            }else if (item.ID.ToString().Substring(0, 1) == "2")
            {
                item._ItemType = ItemType.Armor;
            }
            item._name = _csvDatas[i][1].ToString();
            item.atk = int.Parse(_csvDatas[i][2]);
            item.def = int.Parse(_csvDatas[i][3]);
            
            AssetDatabase.CreateAsset(item,$"Assets/Resources/Items/Data/{item.ID}.asset");
        }
        AssetDatabase.SaveAssets();
    }
    [MenuItem("Generator/Generate Foods")]
    static public void GenerateFoods()
    {
        List<string[]> _csvDatas = new List<string[]>();

        itemCSVPath = Resources.Load<TextAsset>("Data/cake_master");
        StringReader reader = new StringReader(itemCSVPath.text);
        
        while (reader.Peek() != -1) 
        {
            string line = reader.ReadLine();
            _csvDatas.Add(line.Split(','));
        }
        for (int i = 1; i < _csvDatas.Count; i++)
        {
            Food_Obj item = ScriptableObject.CreateInstance<Food_Obj>();
            item.ID = int.Parse(_csvDatas[i][0]);
            if (item.ID.ToString().Substring(0,1) == "5")
            {
                item._ItemType = ItemType.Food;
            }
            Debug.Log(item._ItemType);
            item._name = _csvDatas[i][1].ToString();
            item.hungry = int.Parse(_csvDatas[i][2]);
            item.Maxhungry = int.Parse(_csvDatas[i][3]);
            item.changeID_turn = int.Parse(_csvDatas[i][4]);
            item.newID = int.Parse(_csvDatas[i][5]);
            item.description = _csvDatas[i][6];
            AssetDatabase.CreateAsset(item,$"Assets/Resources/Items/Data/{item.ID}.asset");
        }
        AssetDatabase.SaveAssets();
    }  
    [MenuItem("Generator/Generate Herbs")]
    static public void GenerateHerbs()
    {
        List<string[]> _csvDatas = new List<string[]>();

        itemCSVPath = Resources.Load<TextAsset>("Data/item_master");
        StringReader reader = new StringReader(itemCSVPath.text);
        
        while (reader.Peek() != -1) 
        {
            string line = reader.ReadLine();
            _csvDatas.Add(line.Split(','));
        }
        for (int i = 1; i < _csvDatas.Count; i++)
        {
            Herb_Obj item = ScriptableObject.CreateInstance<Herb_Obj>();
            item.ID = int.Parse(_csvDatas[i][0]);
            if (item.ID.ToString().Substring(0,1) == "4")
            {
                item._ItemType = ItemType.Herb;
            }
            item._name = _csvDatas[i][1].ToString();
            item.hungry = int.Parse(_csvDatas[i][2]);
            item.Maxhungry = int.Parse(_csvDatas[i][3]);
            item.hp = int.Parse(_csvDatas[i][4]);
            Buff_Obj effect = new Buff_Obj();
            effect.atk = int.Parse(_csvDatas[i][5]);
            effect.def = int.Parse(_csvDatas[i][6]);
            effect.turn = int.Parse(_csvDatas[i][7]);
            item.addEffect(effect);
            item.description = _csvDatas[i][8];
            AssetDatabase.CreateAsset(item,$"Assets/Resources/Items/Data/{item.ID}.asset");
        }
        AssetDatabase.SaveAssets();
    }

#endif

}


