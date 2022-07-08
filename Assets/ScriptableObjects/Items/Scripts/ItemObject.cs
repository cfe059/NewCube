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
public class ItemObjet : ScriptableObject
{
    public GameObject itemPrefab;
    public ItemType ItemType;
    [TextArea(15, 20)]
    public string description;
}
