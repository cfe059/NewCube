using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayInventory : MonoBehaviour
{
    // Start is called before the first frame update
    public InventoryObject InventoryObject;
    public int BAG_SIZE = 10;
    public GameObject BAG_FRAME;
    public int X_START, Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEM;
    public List<GameObject> Frames;
    Dictionary<InventorySlot,GameObject> itemDisplay = new Dictionary<InventorySlot, GameObject>();

    private void Start()
    {
        InventoryObject.Container = new Inventory();
        //createEmpty();
        CreateDisplay();
    }

    void createEmpty()
    {
        for (int i = 0; i < BAG_SIZE; i++)
        {
            var obj = Instantiate(BAG_FRAME, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            Frames.Add(obj);
           // obj.GetComponent<RectTransform>().localScale = new Vector3(150, 150);

        }
    }
    private void Update()
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        for (int i = 0; i < InventoryObject.Container.Items.Count; i++)
        {
            if(itemDisplay.ContainsKey(InventoryObject.Container.Items[i]))
            {
                if ( itemDisplay[InventoryObject.Container.Items[i]].GetComponent<itemClick>().isEquip)
                {
                    itemDisplay[InventoryObject.Container.Items[i]].GetComponentInChildren<TextMeshProUGUI>().text = "E";

                }
                else if (!itemDisplay[InventoryObject.Container.Items[i]].GetComponent<itemClick>().isEquip)
                {
                    itemDisplay[InventoryObject.Container.Items[i]].GetComponentInChildren<TextMeshProUGUI>().text = "";

                }
                
                //itemDisplay[InventoryObject.Container.Items[i]].GetComponentInChildren<TextMeshProUGUI>().text = InventoryObject.Container.Items[i].amount.ToString("n0");
            }
            else
            {
                this.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                var obj = Instantiate(InventoryObject.Container.Items[i].item.itemPrefab,Vector3.zero, Quaternion.identity,transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponent<itemClick>()._index = i;
//                obj.GetComponentInChildren<TextMeshProUGUI>().text = InventoryObject.Container.Items[i].amount.ToString("n0");
                itemDisplay.Add(InventoryObject.Container.Items[i], obj);
                this.GetComponent<RectTransform>().pivot = new Vector2(1f, 0.5f);

            }
        }

        
    }
    
    void CreateDisplay()
    {
        int count = 0;
        foreach (var item in InventoryObject.Container.Items)
        {
            var obj = Instantiate(item.item.itemPrefab,Vector3.zero, Quaternion.identity,Frames[count].transform);
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

