using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopup : MonoBehaviour
{
    // Start is called before the first frame update
    public Image itemPopup;
    public NewItem itemData;
    public itemClick _ItemDataClick;
    public GameObject FrameitemPopup;
    public TextMeshProUGUI text;
    private void Start()
    {
        if ((itemData._ItemType == ItemType.Armor || itemData._ItemType == ItemType.Weapon))
        {
            GameObject obj = Instantiate(itemPopup,Vector3.zero,Quaternion.identity, FrameitemPopup.transform).gameObject;
            obj.GetComponent<RectTransform>().localPosition = new Vector3();
            text.text = $"{itemData.R_Data.RName}を装備しますか？";
        }
    }

    public void cancel()
    {
        Destroy(this.gameObject);
    }

    public void EquipItem()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerBase>().EquipItem(itemData,_ItemDataClick);
        _ItemDataClick.isEquip = true;
        Destroy(this.gameObject);
    }
}
