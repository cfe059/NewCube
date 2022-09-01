using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemNameRandomJson : MonoBehaviour
{
    // Start is called before the first frame update
    public void SaveAllItem()
    {
        ItemRandomList itemDataObjects = new ItemRandomList();
        itemData_Object[] obj_dum = new itemData_Object[]{};

        obj_dum = Resources.LoadAll<itemData_Object>("Items/Data");
        for (int i = 0; i < obj_dum.Length; i++)
        {
            Json_ItemRandom obj_new = new Json_ItemRandom();
            
            
            obj_new._ItemType = obj_dum[i]._ItemType;
            obj_new.ID = obj_dum[i].ID;
            obj_new.R_Data = obj_dum[i].R_Data;
            itemDataObjects.itemDataObjects.Add(obj_new);
        }
        string item_json = JsonUtility.ToJson(itemDataObjects, true);
        string filePath = Application.dataPath + "/Resources/Json/item_random.json";

        StreamWriter streamWriter = new StreamWriter(filePath);
        streamWriter.Write(item_json); 
        streamWriter.Flush ();
        streamWriter.Close ();
    }
    
}

[Serializable]
public class ItemRandomList
{
    public List<Json_ItemRandom> itemDataObjects= new List<Json_ItemRandom>();
}

[Serializable]
public class Json_ItemRandom
{
    public int ID; 
    public RandomName R_Data;
    public ItemType _ItemType;
        
}

