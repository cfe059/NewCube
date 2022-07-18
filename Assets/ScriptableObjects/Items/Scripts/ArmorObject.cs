using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Object", menuName = "Inventory System/Items/Armor")]
public class ArmorObject : ItemObject
{
    public float def;
    private void Awake()
    {
        this.ItemType = ItemType.Armor;
    }

    private void OnEnable()
    {
        itemData = Resources.Load<NewItem>($"Items/Data/{ID}");
        itemName = itemData.name;
        itemPrefab = Resources.Load<GameObject>($"Items/Display/{ID}");
    }
}
