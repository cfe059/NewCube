using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

[CreateAssetMenu(fileName = "New Money Object", menuName = "Inventory System/Items/Money")]
#endif

public class MoneyObject : ItemObject
{
    private void Awake()
    {
        this.ItemType = ItemType.Money;
    }
}
