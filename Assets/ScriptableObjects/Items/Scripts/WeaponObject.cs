using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory System/Items/Weapon")]
public class WeaponObject : ItemObjet
{
    public float atk;
    private void Awake()
    {
        this.ItemType = ItemType.Weapon;
    }
}
