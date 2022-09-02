using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Player _player;
    public InventoryObject _inventory;
    private ItemEquipment _standItemEquipment;
    private GameObject _standNode;
    [SerializeField] private GameObject dialogPickup;
    [SerializeField] private GameObject dialogMove;
    [SerializeField] private GameObject gold;
    [SerializeField] private Slider hpObj;
    [SerializeField] private Slider hungryObj;
    [SerializeField] private Slider expObj;
    [SerializeField] private GameObject bag;

    public Player _PLayerData
    {
        get
        {
            return _player;
        }
    }

    private void Start()
    {
        InitialPlayer();
        _player.stats.hp = _player.stats.Maxhp;
        hpObj.maxValue = _player.stats.Maxhp;
        hungryObj.maxValue = _player.Maxhungry;
        hungryObj.value = _player.hungry;
        expObj.maxValue =  GManager.Instance.GetComponent<CSVReader>()._LevelData[_player.Level-1].NextExp;;
        hpObj.value = _player.stats.hp;
        expObj.value = _player.exp;
//        expObj.maxValue = GManager.Instance._levelMaster[_player.Level-1].NextExp;
        GManager.Instance.ChangeLevel(_player.Level);
    }

    void SavePlayer()
    {
        //
        // string json_player = JsonUtility.ToJson(_player, true);
        // string json_inventory = JsonUtility.ToJson(_inventory);
        // string filePath = Application.dataPath + "/Resources/Json/player.json";
        //
        // StreamWriter streamWriter = new StreamWriter(filePath);
        // streamWriter.Write(json_player); 
        // streamWriter.Flush ();
        // streamWriter.Close ();
    }

    void PlayerOverrite(Player data)
    {
        _player.buffs = data.buffs;
        _player.exp = data.exp;
        _player.hungry = data.hungry;
        _player.Level = data.Level;
        _player.stats = data.stats;
        _player.Level = data.Level;
        _player.golds = data.golds;
        _player.Maxhungry = data.Maxhungry;
        
        
    }

    public void LoadInventory(Inventory data)
    {
      //  _inventory.Container = new Inventory();
        _inventory.Container = data;
        var display = GameObject.Find("InventorySystem").GetComponent<DisplayInventory>();
        
    

        foreach (var obj in display._display)
        {
            Destroy(obj);
        }

        display._display = new List<GameObject>();
        display.UpdateDisplay();
    }
    
    public void LoadData(Player _data)
    {

      //  _player = _data;
        gameObject.transform.position =
            new Vector3(_data._PlayerVector3.x, _data._PlayerVector3.y, _data._PlayerVector3.z);
        gameObject.transform.eulerAngles = new Vector3(_data._PlayerRotate.x, _data._PlayerRotate.y, _data._PlayerRotate.z);
        
        //Equipment
        _player.Equipment = new Equipment();
        var display = GameObject.Find("InventorySystem").GetComponent<DisplayInventory>();
        Debug.Log(display._display.Count);
        if (_data.Equipment.weaponEarray != -1)
        {
            
            EquipItem(Resources.Load<Equipment_Obj>($"Items/Data/{_data.Equipment.weaponID}"),display._display[_data.Equipment.weaponEarray].GetComponent<itemClick>());
        }

        if (_data.Equipment.armorEarray != -1)
        {
            var obj_e = display._display[_data.Equipment.armorEarray].GetComponent<itemClick>();
            EquipItem(Resources.Load<Equipment_Obj>($"Items/Data/{_data.Equipment.armorID}"),obj_e);
            

        }
        PlayerOverrite(_data);
        //_player = _data;
    }
    void InitialPlayer()
    {
        List<StatusMaster> stats =  GManager.Instance.GetComponent<CSVReader>()._StatusData;
        _player.stats.Maxhp = stats[_player.Level - 1].MaxHp;
        _player.stats.atk = stats[_player.Level - 1].Atk;
        _player.stats.def = stats[_player.Level - 1].Def;
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
            if (_standItemEquipment != null &&  (_inventory.Container.Items.Count < 9))
            {
                
                _inventory.AddItem(_standItemEquipment.item, 1);
                GManager.Instance.Logger($"{_standItemEquipment.GetComponent<ItemEquipment>().item.itemName}を発見！鞄にしまった。");
                P_TurnChange();
                moveToBag(_standItemEquipment.gameObject);
                GManager.Instance.WorldData.WorldGens[Convert.ToInt32(_standNode.transform.parent.name)]
                    .Items[Convert.ToInt32(_standNode.name)] = "";
                //Destroy(stand_item.gameObject);
            }
            else if( _inventory.Container.Items.Count >= 9)
            {
                P_TurnChange();
                GManager.Instance.Logger($"足元を調べた。{_standItemEquipment.GetComponent<ItemEquipment>().item.itemName}を発見！しかし鞄がいっぱいだった…。");
 
            }
            else
            {
                P_TurnChange();
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
                    InitialPlayer();
                    hpObj.maxValue = _player.stats.Maxhp;

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

    
    public void EquipItem(Equipment_Obj obj,itemClick obj_e,bool unequip = false)
    {
     
        if (obj._ItemType == ItemType.Weapon)
        {
            
            if (unequip)
            {
                _player.Equipment.weapon = null;
                if (_player.Equipment.weaponE != null)
                {
                    _player.Equipment.weaponE.isEquip = false;
                    _player.Equipment.weaponE = null;
                    _player.Equipment.weaponEarray = -1;
                    _player.Equipment.weaponID = -1;
                }

                return;
            }
            _player.Equipment.weaponID = obj.ID;
        
            _player.Equipment.weapon = obj;
            if (_player.Equipment.weaponE != null)
            {
                _player.Equipment.weaponE.isEquip = false;
            }
            _player.Equipment.weaponEarray = obj_e._index;
            _player.Equipment.weaponE = obj_e;
            obj_e.isEquip = true;
            obj_e.edit_text("E");
        }else if (obj._ItemType == ItemType.Armor)
        {
            if (unequip)
            {
                _player.Equipment.Armor = null;
                if (_player.Equipment.ArmorE != null)
                {
                    _player.Equipment.ArmorE.isEquip = false;
                    _player.Equipment.ArmorE = null;
                    _player.Equipment.armorEarray = -1;
                    _player.Equipment.armorID = -1;
                }

                return;
            }
            
            _player.Equipment.armorID = obj.ID;
            _player.Equipment.Armor = obj;
            
            if (_player.Equipment.ArmorE != null)
            {
                _player.Equipment.ArmorE.isEquip = false;
            }
            _player.Equipment.armorEarray = obj_e._index;
            obj_e.isEquip = true;
            obj_e.edit_text("E");
            _player.Equipment.ArmorE = obj_e;
        }
        
    }
    public void useFood(Food_Obj obj)
    {

        if (obj._ItemType == ItemType.Food)
        {
            
            float totalhungry = 0;
            if (_player.Maxhungry  < obj.hungry + _player.hungry)
            {
                totalhungry =  (_player.Maxhungry + obj.hungry )-(_player.hungry + obj.hungry);
                _player.hungry += totalhungry;

            }
            else
            {
                totalhungry = obj.hungry;
                _player.hungry += totalhungry;
            }

            if (totalhungry > 0)
            {
                setPopup(totalhungry,Color.yellow);
            }
            GManager.Instance.Logger($"{obj.ID}を使用します");

            _player.Maxhungry += obj.Maxhungry;
            
            hpObj.maxValue = _player.stats.Maxhp;
            hpObj.value = _player.stats.hp;
            hungryObj.maxValue = _player.Maxhungry;
            hungryObj.value = _player.hungry;
        }   
        
    }

    public void useHungry(float num)
    {
        _player.hungry -= num;
        hungryObj.value = _player.hungry;
    }
    public void useHerb(Herb_Obj obj,int _index)
    {

        if (obj._ItemType == ItemType.Herb)
        {
            float totalheal = 0;
            float totalhungry = 0;
            if (_player.Maxhungry < (obj.hungry + _player.hungry))
            {
                
                totalhungry =  (_player.Maxhungry + obj.hungry )-(_player.hungry + obj.hungry);

                _player.hungry += totalhungry;

            }
            else
            {
                totalhungry = obj.hungry;
                _player.hungry += totalhungry;

            }
            _player.Maxhungry += obj.Maxhungry;
            if (_player.stats.Maxhp  < obj.hp + _player.stats.hp)
            {
                totalheal =  ( _player.stats.Maxhp + obj.hp)-(_player.stats.hp + obj.hp) ;
                _player.stats.hp += totalheal;
                
            }
            else
            {
                totalheal = obj.hp;
                _player.stats.hp += totalheal;
            }

            if (obj.effect.turn != 0)
            {
                PlayerBuff buff = new PlayerBuff();
                buff.buff = obj.effect;
                buff.turn = obj.effect.turn;
                _player.buffs.Add(buff);
            }
            if (totalheal > 0)
            {
                setPopup(totalheal,Color.green);   
            }

            if (totalhungry > 0)
            {
                setPopup(totalhungry,Color.yellow);
            }
            GManager.Instance.Logger($"{obj.ID}を使用します");
            _inventory.RemoveItem(_index);
            hpObj.maxValue = _player.stats.Maxhp;
            hpObj.value = _player.stats.hp;
            hungryObj.maxValue = _player.Maxhungry;
            hungryObj.value = _player.hungry;
            
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
    public void P_TurnChange()
    {
       ItemExpire();
       buffExpire();
       _player._PlayerVector3 = new newVector3(gameObject.transform.position.x,gameObject.transform.position.y,gameObject.transform.position.z);
       _player._PlayerRotate = new newVector3(gameObject.transform.eulerAngles.x,gameObject.transform.eulerAngles.y,gameObject.transform.eulerAngles.z);
       SavePlayer();
       
       GManager.Instance._turnBase = GManager.TurnBase.Monster_Turn;
    }

    void buffExpire()
    {
        for (int i = 0; i < _player.buffs.Count; i++)
        {
            _player.buffs[i].turn -= 1;
            if (_player.buffs[i].turn <= 0)
            {
                _player.buffs.RemoveAt(i);
            }
        }
    }
    void ItemExpire()
    {
        for (int i = 0; i < _inventory.Container.Items.Count; i++)
        {
            if (_inventory.Container.Items[i].item.ItemType == ItemType.Food && _inventory.Container.Items[i].expire_turn != 0)
            {
                _inventory.Container.Items[i].expire_turn -= 1;
            }

            if (_inventory.Container.Items[i].item.ItemType == ItemType.Food &&_inventory.Container.Items[i].expire_turn == 0 && _inventory.Container.Items[i].item.itemData_F.newID != 0)
            {
                _inventory.AddItem(Resources.Load<FoodObject>($"ScriptableObjects/Items/Obj/{_inventory.Container.Items[i].item.itemData_F.newID}"),1);
                _inventory.Container.Items.RemoveAt(i);
            }
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
            var item = other.GetComponent<ItemEquipment>();
            if (item)
            {
                _standItemEquipment = item;
                //  _inventory.AddItem(item.item,1);
                //  Destroy(other.gameObject);
            }
        }

        if (other.gameObject.tag == "Node")
        {
            _standNode = other.gameObject.transform.parent.gameObject;
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
    void setPopup(float number,Color color)
    {
        GameObject d = Resources.Load<GameObject>("Damage");
        d.GetComponent<DamagePopup>().damage = (int)number;
        d.GetComponentInChildren<TextMeshProUGUI>().color = color;
        GameObject i = Instantiate(d);
        i.transform.position = this.transform.position;

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Item") &&
            GetComponent<PlayerController>()._playerState != PlayerController.CharacterState.Attack)
        {
            _standItemEquipment = null;
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

    void EndTurn()
    {
        
    }
}

[Serializable]
public class Player
{
    public int Level;
    public int exp;
    public Character_Stats stats;
    public Equipment Equipment;
    public List<PlayerBuff> buffs;
    public float hungry;
    public float Maxhungry;
    public float golds;
    public newVector3 _PlayerVector3;
    public newVector3 _PlayerRotate;
    
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
}[Serializable]
public class PlayerBuff
{
    public Buff_Obj buff;
    public int turn;
}
[Serializable]
public class Equipment
{
    public int weaponID = -1;
    public Equipment_Obj weapon;
    public int weaponEarray = -1;
    public itemClick weaponE;
    public int armorID = -1;
    public Equipment_Obj Armor;
    public int armorEarray =-1;

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
public class newVector3
{
    public float x, y, z;
    public newVector3(float _x,float _y, float _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }
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