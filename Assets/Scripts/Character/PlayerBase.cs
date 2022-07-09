using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Player _player;
    public InventoryObject _inventory;

    private void Awake()
    {
        
    }


    public void get_Damage(float atk)
    {
        _player.stats.hp -= atk;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Item") && GetComponent<PlayerController>()._playerState != PlayerController.CharacterState.Attack)
        {
            var item = other.GetComponent<Item>();
            if (item)
            {
                
                _inventory.AddItem(item.item,1);
                Destroy(other.gameObject);
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (GetComponent<PlayerController>()._playerState != PlayerController.CharacterState.Attack)
        {
            return;
        }

        if (other.transform.CompareTag("Monster") )
        {
            other.gameObject.GetComponent<MonsterBase>().get_Damage(_player.stats.atk);
            GameObject d = Resources.Load<GameObject>("Damage");
            d.GetComponent<DamagePopup>().damage = (int) _player.stats.atk;
            GameObject i = Instantiate(d);
            i.transform.position = other.transform.position;
            GetComponent<PlayerController>()._playerState = PlayerController.CharacterState.Idle;

        }
    }
}

[Serializable]
public class Player
{
    public int Level;
    public float exp;
    public Character_Stats stats;
    public float hungry;
    public float golds;
    public Item_Bag _itemBag ;
    

    public void AddItem(ItemObjet item,int amount)
    {
        bool itemExist = false;
        // foreach (InventorySlot itemObjet in Container)
        // {
        //     if (itemObjet.item == item)
        //     {
        //         itemObjet.amount += amount;
        //         itemExist = true;
        //     }
        // }
        if (!itemExist)
        {
            _itemBag.ItemList.Add(new InventorySlot(item, amount));
        }
    }
}
[Serializable]
public class Character_Stats
{
    public float hp;
    public float atk;
    public float def;
    public float critical_rate;
    public float critical_damage;
    public float evasions;
    
    
    
}
[Serializable]
public class Item_Bag
{
    public List<InventorySlot> ItemList;
}
[Serializable]
public class Equipment
{
    
}
[Serializable]
public struct canEquipItem
{
    public float atk;
    public float hp;
    public float def;
    public float evasions;
    public float critical_rate;
    public float critical_damage;
}
[Serializable]
public struct canUseItem
{
    [SerializeField] public float hpHeal;
}

[Serializable]
public class Item_System
{
    public int id;
    public string itemName;
    public bool isEquipment;
    
    //canUseType
    public canUseItem CanUseItem;

    //canEquip
    public canEquipItem CanEquipItem;

    
}


[Serializable]
public class Stats
{
    private float baseValue;
    private List<float> modifiers = new List<float>();

    public float get_Value()
    {
        return baseValue;
    }

    public void AddModifier(float value)
    {
        if (value != 0f)
        {
            modifiers.Add(value);
        }
    }

    public void RemoveModifier(float value)
    {
        if (value != 0f)
        {
            modifiers.Remove(value);
        }
    }
}