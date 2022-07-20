using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopup : MonoBehaviour
{
    // Start is called before the first frame update
    public Image itemPopup;
    public itemData_Object itemData;
    public itemClick _ItemDataClick;
    public GameObject FrameitemPopup;
    public TextMeshProUGUI text;
    public GameObject EquipButton;
    public GameObject UnEquipButton;
    public GameObject FoodButton;
    public GameObject HerbButton;
    private void Start()
    {
        EquipButton.SetActive(false);
        UnEquipButton.SetActive(false);
        FoodButton.SetActive(false);
        HerbButton.SetActive(false);
        if ((itemData._ItemType == ItemType.Armor || itemData._ItemType == ItemType.Weapon) && !_ItemDataClick.isEquip)
        {
            EquipButton.SetActive(true);

            GameObject obj = Instantiate(itemPopup,Vector3.zero,Quaternion.identity, FrameitemPopup.transform).gameObject;
            obj.GetComponent<RectTransform>().localPosition = new Vector3();
            text.text = $"{itemData.R_Data.RName}を装備しますか？";
        }else  if ((itemData._ItemType == ItemType.Armor || itemData._ItemType == ItemType.Weapon) && _ItemDataClick.isEquip)
        {
            UnEquipButton.SetActive(true);
            GameObject obj = Instantiate(itemPopup,Vector3.zero,Quaternion.identity, FrameitemPopup.transform).gameObject;
            obj.GetComponent<RectTransform>().localPosition = new Vector3();
            text.text = $"{itemData.R_Data.RName}を外しますか？";
        }else if (itemData._ItemType == ItemType.Herb)
        {
            HerbButton.SetActive(true);
            GameObject obj = Instantiate(itemPopup,Vector3.zero,Quaternion.identity, FrameitemPopup.transform).gameObject;
            obj.GetComponent<RectTransform>().localPosition = new Vector3();
            text.text = $"{itemData.R_Data.RName}を使用しますか？";
        }
        else if (itemData._ItemType == ItemType.Food)
        {
            FoodButton.SetActive(true);
            GameObject obj = Instantiate(itemPopup,Vector3.zero,Quaternion.identity, FrameitemPopup.transform).gameObject;
            obj.GetComponent<RectTransform>().localPosition = new Vector3();
            text.text = $"{itemData.R_Data.RName}を使用しますか？";
        }
    }

    public void cancel()
    {
        Destroy(this.gameObject);
    }
    public void UseHerb()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerBase>().useHerb(Resources.Load<Herb_Obj>($"Items/Data/{itemData.ID}"),_ItemDataClick._index);
        Destroy(_ItemDataClick.GameObject());
        Destroy(this.gameObject);
    }
    public void UseFood()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerBase>().useFood(Resources.Load<Food_Obj>($"Items/Data/{itemData.ID}"));
        Destroy(_ItemDataClick.GameObject());
        Destroy(this.gameObject);
    } 
    public void EquipItem()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerBase>().EquipItem(Resources.Load<Equipment_Obj>($"Items/Data/{itemData.ID}"),_ItemDataClick);
        _ItemDataClick.isEquip = true;
        Destroy(this.gameObject);
    } 
    public void unEquipItem()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerBase>().EquipItem(Resources.Load<Equipment_Obj>($"Items/Data/{itemData.ID}"),_ItemDataClick,true);
        _ItemDataClick.isEquip = false;
        Destroy(this.gameObject);
    }
}
