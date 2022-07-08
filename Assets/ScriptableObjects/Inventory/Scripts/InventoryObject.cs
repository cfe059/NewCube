using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();
    public void AddItem(ItemObjet item,int amount)
    {
       bool itemExist = false;
        // foreach (InventorySlot itemObjet in Container)
        // {
        //     if (itemObjet.item == item)
        //     {
        //         itemObjet.amount += amount;
        //         itemExist = true;
        //     }
        // }
        if (!itemExist)
        {
            Container.Add(new InventorySlot(item, amount));
        }
    }
}

[Serializable]
public class InventorySlot
{
    public ItemObjet item;
    public int amount;
    public InventorySlot(ItemObjet item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
    public void AddAmount(int amount)
    {
        this.amount += amount;
    }
}