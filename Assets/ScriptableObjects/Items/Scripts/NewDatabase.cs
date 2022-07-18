using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class NewDatabase : ScriptableObject
{
    // Start is called before the first frame update


    static private TextAsset itemCSVPath;

    [MenuItem("Generator/Generate Items")]
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
            NewItem item = ScriptableObject.CreateInstance<NewItem>();
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
    
}


