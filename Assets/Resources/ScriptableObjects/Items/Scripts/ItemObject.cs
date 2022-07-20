using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    None,
    Weapon,
    Armor,
    Herb,
    Potion,
    Scroll,
    Key,
    Food,
    Money,
}
public class ItemObject : ScriptableObject
{
    public int ID;
    public Equipment_Obj itemData_E;
    public Food_Obj itemData_F;
    public Herb_Obj itemData_H;
    public GameObject itemPrefab;
    public string itemName;
    public ItemType ItemType;
    [TextArea(15, 20)]
    public string description;
}
