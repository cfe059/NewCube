using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    None,
    Weapon,
    Armor,
    Potion,
    Scroll,
    Key,
    Food,
    Money,
}
public class ItemObject : ScriptableObject
{
    public int ID;
    public NewItem itemData;
    public GameObject itemPrefab;
    public string itemName;
    public ItemType ItemType;
    [TextArea(15, 20)]
    public string description;
}
