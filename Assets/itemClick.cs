using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class itemClick : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler
{
    [SerializeField] private GameObject canvasPopup;
    public int _index;
    public bool isEquip;
    // Start is called before the first frame update
    [SerializeField]
     private itemData_Object Itemdata;
     

    
    private void Start()
    {
        Regex regexObj = new Regex(@"[^\d]");
        string iD = regexObj.Replace(name, "");
        Itemdata = Resources.Load<itemData_Object>($"Items/Data/{iD}");
        GetComponent<Image>().sprite = Resources.Load<Sprite>($"Items/item_icon_kari/{Itemdata.R_Data.Rimg}");

    }

    public void edit_text(string st)
    {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = st;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Itemdata._ItemType == ItemType.Weapon || Itemdata._ItemType == ItemType.Armor ||Itemdata._ItemType == ItemType.Herb || Itemdata._ItemType == ItemType.Food  )
        {
            
            Debug.Log($"want to use {Itemdata.R_Data.RName}");
            ItemPopup popup = canvasPopup.GetComponent<ItemPopup>();
            popup._ItemDataClick = GetComponent<itemClick>();
            popup.itemData = Itemdata;
            popup.itemPopup = GetComponent<Image>();
            Instantiate(popup);

        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(Itemdata.R_Data.RName);
        
       // Debug.Log($"want to use {Itemdata.name}");
    }
    
}
