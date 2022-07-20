using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/Food")]
#endif

public class HerbObject : ItemObject
{

#if UNITY_EDITOR
    
    [MenuItem("Generator/Herbs/Generate Items")]
    static public void GenerateAllHerb()
    {
        Herb_Obj[] items = Resources.LoadAll<Herb_Obj>("Items/Data/");
        foreach (var item in items)
        {
            if (item._ItemType == ItemType.Herb)
            {
                ItemObject _itemObject = new HerbObject();
                _itemObject.ID = item.ID;
                _itemObject.itemData_H = Resources.Load<Herb_Obj>($"Items/Data/{item.ID}");
                _itemObject.itemName = item._name;
                //GameObject obj =  Resources.Load<GameObject>($"Items/Display/ItemDisplay");
                //obj.name = item.ID.ToString();
                CreateObj(_itemObject.ID);
                //obj.GetComponent<itemClick>().Itemdata = _itemObject.itemData;
                //_itemObject.itemPrefab = obj;
                _itemObject.itemPrefab = Resources.Load<GameObject>($"Items/Display/{_itemObject.ID}");
                //CreateinGamePrefab(_itemObject.ID);
                AssetDatabase.CreateAsset(_itemObject,$"Assets/Resources/ScriptableObjects/Items/Obj/{item.ID}.asset");
                
            }
        }
    }

    static void CreateObj(int ID)
    {
        GameObject source =  Resources.Load<GameObject>($"Items/Display/ItemDisplay"); 
        GameObject objSource = (GameObject)PrefabUtility.InstantiatePrefab(source);
        GameObject obj = PrefabUtility.SaveAsPrefabAsset(objSource, $"Assets/Resources/Items/Display/{ID}.prefab");
        DestroyImmediate(objSource);
    }
    static void CreateinGamePrefab(int ID)
    {
        GameObject source =  Resources.Load<GameObject>($"Items/Prefabs/ItemBase"); 
        //ItemObject itemObject =
            
        GameObject objSource = (GameObject)PrefabUtility.InstantiatePrefab(source);
        GameObject obj = PrefabUtility.SaveAsPrefabAsset(objSource, $"Assets/Resources/Items/Prefabs/{ID}.prefab");
        //obj.GetComponent<ItemEquipment>().item = itemObject;
       
        DestroyImmediate(objSource);
    }
#endif

    public RandomName_Obj rData;
    public int restoreAmount;
    private void Awake()
    {
        this.ItemType = ItemType.Herb;
    }
}
