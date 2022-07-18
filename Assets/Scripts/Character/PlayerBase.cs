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
    [SerializeField] private GameObject dialogPickup;
    [SerializeField] private GameObject dialogMove;
    [SerializeField] private GameObject gold;
    [SerializeField] private Slider hpObj;
    [SerializeField] private Slider expObj;
    [SerializeField] private GameObject bag;
    private void Start()
    {
        hpObj.maxValue = _player.stats.Maxhp;
        expObj.maxValue = 10;
        hpObj.value = _player.stats.hp;
        expObj.value = _player.exp;
//        expObj.maxValue = GManager.Instance._levelMaster[_player.Level-1].NextExp;
        GManager.Instance.ChangeLevel(_player.Level);
    }

    
    
    private void Update()
    {
        if (GManager.Instance._turnBase != GManager.TurnBase.Player_Turn)
        {
            dialogMove.SetActive(false);
            dialogPickup.SetActive(false);
            return;
        }
        dialogMove.SetActive(true);
        dialogPickup.SetActive(true);

        if (Input.GetKey(KeyCode.E))
        {
            if (stand_item != null &&  (_inventory.Container.Items.Count < 9))
            {

                _inventory.AddItem(stand_item.item, 1);
                GManager.Instance.Logger($"{stand_item.GetComponent<Item>().item.itemName}を発見！鞄にしまった。");
                ForceTurnChange();
                moveToBag(stand_item.gameObject);
                //Destroy(stand_item.gameObject);
            }
            else if( _inventory.Container.Items.Count >= 9)
            {
                ForceTurnChange();
                GManager.Instance.Logger($"足元を調べた。{stand_item.GetComponent<Item>().item.itemName}を発見！しかし鞄がいっぱいだった…。");
 
            }
            else
            {
                ForceTurnChange();
                GManager.Instance.Logger($"足元を調べた。しかし何も見つからなかった。");

            }

        }


    }

    public void getExp(int exp)
    {
        _player.exp += exp;

        expObj
            .DOValue(_player.exp,0.5f)
            .OnComplete(() =>
            {
                if (LevelUp())
                {
                    expObj
                        .DOValue(_player.exp,0.5f)
                        .Play();
                    GManager.Instance.ChangeLevel(_player.Level);
                    expObj.maxValue = GManager.Instance._levelMaster[_player.Level - 1].NextExp;
                    //effect
                }
            })
            .Play();
        
        
    }

    bool LevelUp()
    {
        LevelMaster _level = GManager.Instance._levelMaster[_player.Level - 1];

        if (_player.exp >= _level.NextExp)
        {
            _player.Level += 1;
            _player.exp -= _level.NextExp;
            
            return true;
        }

        return false;
    }

    public void EquipItem(NewItem obj,itemClick obj_e)
    {
        if (obj._ItemType == ItemType.Weapon)
        {
            _player.Equipment.weapon = obj;
            if (_player.Equipment.weaponE != null)
            {
                _player.Equipment.weaponE.isEquip = false;
            }

            _player.Equipment.weaponE = obj_e;
        }else if (obj._ItemType == ItemType.Armor)
        {
            _player.Equipment.Armor = obj;
            if (_player.Equipment.ArmorE != null)
            {
                _player.Equipment.ArmorE.isEquip = false;
            }

            _player.Equipment.ArmorE = obj_e;
        }
    }
    void moveToBag(GameObject obj)
    {
        obj.transform.DOMove(bag.transform.position, 1f)
            .OnComplete(() =>
            {
                Destroy(obj);
            })
            .Play();
    }
    void ForceTurnChange()
    {
        GManager.Instance._turnBase = GManager.TurnBase.Monster_Turn;
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
        Debug.Log(other.name);
        float dmg = atk;
        _player.stats.hp -= dmg;
        DamagePopup(dmg);
        GManager.Instance.Logger($"{other.name} が攻撃してきた。{dmg}のダメージを受けた！");
        //GManager.Instance.Logger($"{other.name} から{dmg}のダメージを受けた！");
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

    void DamagePopup(float damage)
    {
        GameObject d = Resources.Load<GameObject>("Damage");
        d.GetComponent<DamagePopup>().damage = (int)damage;
        GameObject i = Instantiate(d);
        i.transform.position = this.transform.position;
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
            float total_atk = 0;
            if (_player.Equipment.weapon != null)
            {
                total_atk += _player.Equipment.weapon.atk;
            }

            if (_player.Equipment.Armor != null)
            {
                total_atk += _player.Equipment.Armor.atk;

            }

            total_atk += _player.stats.atk;
            other.gameObject.GetComponent<MonsterBase>().get_Damage(this.gameObject,total_atk);
        
        }
    }
}

[Serializable]
public class Player
{
    public int Level;
    public int exp;
    public Character_Stats stats;
    public Equipment Equipment;
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
    public NewItem weapon;
    public itemClick weaponE;
    public NewItem Armor;
    public itemClick ArmorE;

    
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