using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Money Object", menuName = "Inventory System/Items/Money")]
public class MoneyObject : ItemObject
{
    private void Awake()
    {
        this.ItemType = ItemType.Money;
    }
}
