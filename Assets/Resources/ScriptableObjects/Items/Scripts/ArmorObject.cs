using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CreateAssetMenu(fileName = "New Armor Object", menuName = "Inventory System/Items/Armor")]
#endif
public class ArmorObject : ItemObject
{

    [SerializeField]
    private RandomName_Obj rData;
    public float def;
    private void Awake()
    {
        this.ItemType = ItemType.Armor;
    }
#if UNITY_EDITOR
    
    [MenuItem("Generator/Armor/Generate Items")]

    static public void GenerateAllWeapon()
    {
        Equipment_Obj[] items = Resources.LoadAll<Equipment_Obj>("Items/Data/");
        foreach (var item in items)
        {

            if (item._ItemType == ItemType.Armor)
            {
                ArmorObject _itemObject = new ArmorObject();
                _itemObject.ID = item.ID;
                _itemObject.itemData_E = Resources.Load<Equipment_Obj>($"Items/Data/{item.ID}");
                _itemObject.itemName = item._name;
                //GameObject obj =  Resources.Load<GameObject>($"Items/Display/ItemDisplay");
                //obj.name = item.ID.ToString();
                CreateObj(_itemObject.ID);
                //obj.GetComponent<itemClick>().Itemdata = _itemObject.itemData;
                //_itemObject.itemPrefab = obj;
                _itemObject.itemPrefab = Resources.Load<GameObject>($"Items/Display/{_itemObject.ID}");
               // CreateinGamePrefab(_itemObject.ID);
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
        ItemObject itemObject = Resources.Load<ArmorObject>($"ScriptableObjects/Items/Obj/{ID}");
        GameObject objSource = (GameObject)PrefabUtility.InstantiatePrefab(source);
        GameObject obj = PrefabUtility.SaveAsPrefabAsset(objSource, $"Assets/Resources/Items/Prefabs/{ID}.prefab");
        obj.GetComponent<ItemEquipment>().item = itemObject;

        DestroyImmediate(objSource);
    }
#endif

    private void OnEnable()
    {
        itemData_E = Resources.Load<Equipment_Obj>($"Items/Data/{ID}");
        //itemPrefab = Resources.Load<GameObject>($"Items/Display/{ID}");

    }
}
