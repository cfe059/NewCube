using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/Food")]
#endif

public class FoodObject : ItemObject
{
    public int restoreAmount;
    private void Awake()
    {
        this.ItemType = ItemType.Food;
    }
}
