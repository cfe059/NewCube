using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject//,ISerializationCallbackReceiver
{
    public string savePath = "";
    private ItemDatabaseObject database;
    public List<InventorySlot> Container = new List<InventorySlot>();

    private void OnEnable()
    {
        database = AssetDatabase.LoadAssetAtPath<ItemDatabaseObject>("Assets/ScriptableObjects/Items/Database.asset");
    }

    public void AddItem(ItemObject item,int amount)
    {
        Container.Add(new InventorySlot( database.getID[item],item, amount));
    }

    // public void OnAfterDeserialize()
    // {
    //     for (int i = 0; i < Container.Count; i++)
    //         Container[i].item = database.getItem[Container[i].ID];
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
}