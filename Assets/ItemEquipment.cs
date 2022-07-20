using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class ItemEquipment : MonoBehaviour
{
    // Start is called before the first frame update
    public string RName;
    public ItemObject item;
    public Sprite itemImg;
    private void Start()
    {
        transform.DOLocalMoveY(0.1f, 0.5f,false).SetLink(this.gameObject)
            .SetLoops(-1, LoopType.Yoyo).Play();
        if (item.ItemType == ItemType.Armor || item.ItemType == ItemType.Weapon)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Items/item_icon_kari/{item.itemData_E.R_Data.Rimg}");

        }else if (item.ItemType == ItemType.Herb)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Items/item_icon_kari/{item.itemData_H.R_Data.Rimg}");

        }else if (item.ItemType == ItemType.Food)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Items/item_icon_kari/{item.itemData_F.R_Data.Rimg}");

        }
    }

 
}
