using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Inventory")]
#endif
public class InventoryObject : ScriptableObject
{
    public string savePath = "";
    public Inventory Container;

  

    public void AddItem(ItemObject item,int amount)
    {
        
        if (item.ItemType == ItemType.Money)
        {
            for (int i = 0; i < Container.Items.Count; i++)
            {
                if (Container.Items[i].item == item)
                {
                    Container.Items[i].AddAmount(amount);
                    return;
                }
            }        
        }
        //SetEmptySlot(item, amount);
        Container.Items.Add(new InventorySlot( item.ID,item, amount));
    }
    // public InventorySlot SetEmptySlot(ItemObject _item, int _amount)
    // {
    //     for (int i = 0; i < Container.Items.Length; i++)
    //     {
    //         if(Container.Items[i].ID <= -1)
    //         {
    //             Container.Items[i].UpdateSlot(_item.ID, _item, _amount);
    //             return Container.Items[i];
    //         }
    //     }
    //     //set up functionality for full inventory
    //     return null;
    // }
    // public void OnAfterDeserialize()
    // {
    //     for (int i = 0; i < Container.Items.Count; i++)
    //         Container.Items[i].item = database.getItem[Container.Items[i].ID];
    // }
    //
    // public void OnBeforeSerialize()
    // {
    // }

    void Save()
    {
        string saveData = JsonUtility.ToJson(this,true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file,saveData);
        file.Close();
    }

    void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite((string) bf.Deserialize(file), this);
            file.Close();
        }
    }
}
[Serializable]
public class Inventory
{
    public List<InventorySlot> Items = new List<InventorySlot>();
   // public InventorySlot [] Items = new InventorySlot[10];
}
[Serializable]
public class InventorySlot 
{
    public int ID ; 
    public ItemObject item;
    public int amount;
    public InventorySlot(int _id,ItemObject item, int amount)
    {
        ID = _id;
        this.item = item;
        this.amount = amount;
    }
    public void AddAmount(int amount)
    {
        this.amount += amount;
    }
    // public void UpdateSlot(int _id, ItemObject _item, int _amount)
    // {
    //     ID = _id;
    //     item = _item;
    //     amount = _amount;
    // }
}