using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory System/Items/Weapon")]
public class WeaponObject : ItemObject
{
    public float atk;
    private void Awake()
    {
        this.ItemType = ItemType.Weapon;
    }

    private void OnEnable()
    {
        itemData = Resources.Load<NewItem>($"Items/Data/{ID}");
        itemName = itemData.name;
        itemPrefab = Resources.Load<GameObject>($"Items/Display/{ID}");
    }
}
