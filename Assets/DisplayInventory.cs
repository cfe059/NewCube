using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayInventory : MonoBehaviour
{
    // Start is called before the first frame update
    public InventoryObject InventoryObject;
    public int X_START, Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEM;
    IDictionary<InventorySlot,GameObject> itemDisplay = new Dictionary<InventorySlot, GameObject>();

    private void Start()
    {
        CreateDisplay();
    }

    private void Update()
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        for (int i = 0; i < InventoryObject.Container.Count; i++)
        {
            if(itemDisplay.ContainsKey(InventoryObject.Container[i]))
            {
                itemDisplay[InventoryObject.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = InventoryObject.Container[i].amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(InventoryObject.Container[i].item.itemPrefab,Vector3.zero, Quaternion.identity,transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = InventoryObject.Container[i].amount.ToString("n0");
                itemDisplay.Add(InventoryObject.Container[i], obj);
            }
        } 
    }
    
    void CreateDisplay()
    {
        int count = 0;
        foreach (var item in InventoryObject.Container)
        {
            var obj = Instantiate(item.item.itemPrefab,Vector3.zero, Quaternion.identity,transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(count);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = item.amount.ToString("n0");
            itemDisplay.Add(item,obj);
            count++;
            
        }
    }
    Vector3 GetPosition(int i)
    {
        return new Vector3(X_START +  (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)),Y_START+   (-Y_SPACE_BETWEEN_ITEM * (i / NUMBER_OF_COLUMN)), 0);
    }
    
}

