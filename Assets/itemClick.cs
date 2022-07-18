using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class itemClick : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler
{
    [SerializeField] private GameObject canvasPopup;

    public bool isEquip;
    // Start is called before the first frame update
    private NewItem Itemdata;
    private void Start()
    {
        Regex regexObj = new Regex(@"[^\d]");
        string iD = regexObj.Replace(name, "");
        Itemdata = Resources.Load<NewItem>($"Items/Data/{iD}");
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"want to use {Itemdata.name}");
        ItemPopup popup = canvasPopup.GetComponent<ItemPopup>();
        popup._ItemDataClick = GetComponent<itemClick>();
        popup.itemData = Itemdata;
        popup.itemPopup = GetComponent<Image>();
        Instantiate(popup);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(Itemdata.name);
        
       // Debug.Log($"want to use {Itemdata.name}");
    }
    
}
