using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New Armor Object", menuName = "Inventory System/Items/Armor")]
public class ArmorObject : ItemObject
{
    [SerializeField]
    private RandomName_Obj rData;
    public float def;
    private void Awake()
    {
        this.ItemType = ItemType.Armor;
    }
    [MenuItem("Generator/Armor/Generate Items")]
    static public void GenerateAllWeapon()
    {
        NewItem[] items = Resources.LoadAll<NewItem>("Items/Data/");
        foreach (var item in items)
        {

            if (item._ItemType == ItemType.Armor)
            {
                ItemObject _itemObject = new ArmorObject();
                _itemObject.ID = item.ID;
                _itemObject.itemData = Resources.Load<NewItem>($"Items/Data/{item.ID}");
                _itemObject.itemName = item._name;
                //GameObject obj =  Resources.Load<GameObject>($"Items/Display/ItemDisplay");
                //obj.name = item.ID.ToString();
                CreateObj(_itemObject.ID);
                //obj.GetComponent<itemClick>().Itemdata = _itemObject.itemData;
                //_itemObject.itemPrefab = obj;
                _itemObject.itemPrefab = Resources.Load<GameObject>($"Items/Display/{_itemObject.ID}");
                
                AssetDatabase.CreateAsset(_itemObject,$"Assets/ScriptableObjects/Items/Obj/{item.ID}.asset");
                
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
 
    private void OnEnable()
    {
        itemData = Resources.Load<NewItem>($"Items/Data/{ID}");
        itemPrefab = Resources.Load<GameObject>($"Items/Display/{ID}");

    }
}
