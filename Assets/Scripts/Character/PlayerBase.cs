using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Player _player;
    public InventoryObject _inventory;
    private Item stand_item;
    [SerializeField] private GameObject dialog;
    [SerializeField] private GameObject gold;
    [SerializeField] private Slider hpObj;
    
    private void Start()
    {
        hpObj.maxValue = _player.stats.Maxhp;
        hpObj.value = _player.stats.hp;
    }

    
    
    private void Update()
    {
        if (GManager.Instance._turnBase != GManager.TurnBase.Player_Turn)
            return;
        
        if (stand_item != null)
        {
            if (Input.GetKey(KeyCode.E))
            {
                _inventory.AddItem(stand_item.item,1);
                Destroy(stand_item.gameObject);
            }
            dialog.SetActive(true);
        }
        else
        {
            dialog.SetActive(false);
        }
        
    }

    private void FixedUpdate()
    {
        _player.golds = MoneyUpdate();
        gold.GetComponent<TextMeshProUGUI>().text = $"Golds : {_player.golds}";
    }

    int MoneyUpdate()
    {
        int money = 0;
        foreach (var item in _inventory.Container.Items)
        {
            if (item.item.ItemType == ItemType.Money)
            {
                money = item.amount;
            }
        }

        return money;
    }
    public void get_Damage(GameObject other,float atk)
    {
        float dmg = atk;
        _player.stats.hp -= dmg;
        DamagePopup(other,dmg);
        hpObj.value = _player.stats.hp;

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Item") && GetComponent<PlayerController>()._playerState != PlayerController.CharacterState.Attack)
        {
            var item = other.GetComponent<Item>();
            if (item)
            {
                stand_item = item;
                //  _inventory.AddItem(item.item,1);
                //  Destroy(other.gameObject);
            }
        }
    }

    void DamagePopup(GameObject other,float damage)
    {
        GameObject d = Resources.Load<GameObject>("Damage");
        d.GetComponent<DamagePopup>().damage = (int)damage;
        GameObject i = Instantiate(d);
        i.transform.position = other.transform.position;
        GetComponent<PlayerController>()._playerState = PlayerController.CharacterState.Idle;

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Item") &&
            GetComponent<PlayerController>()._playerState != PlayerController.CharacterState.Attack)
        {
            stand_item = null;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (GetComponent<PlayerController>()._playerState != PlayerController.CharacterState.Attack)
        {
            return;
        }
        //
        if (other.transform.CompareTag("Monster") )
        {
            other.gameObject.GetComponent<MonsterBase>().get_Damage(this.gameObject,_player.stats.atk);
        
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
 
}
[Serializable]
public class Character_Stats
{
    public float Maxhp;
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