using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Object", menuName = "Inventory System/Items/Armor")]
public class ArmorObject : ItemObjet
{
    public float def;
    private void Awake()
    {
        this.ItemType = ItemType.Armor;
    }
}
