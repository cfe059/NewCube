using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    [SerializeField]
    public Monster_Data monster;
    public nodeBase node;

    [SerializeField] private Collider _collider;
    public void get_Damage(GameObject other,float atk)
    {
        float dmg = atk;
        monster.hp -= dmg;
        DamagePopup(dmg);
        GManager.Instance.Logger($"{other.name} が攻撃した。{dmg}のダメージを与えた！");
        //GManager.Instance.Logger($"{this.name} に{dmg}のダメージを与えた！");

        if (Death())
        {
            other.GetComponent<PlayerBase>().getExp(10);
            Destroy(this.gameObject);
        }
    }
    private void FixedUpdate()
    {
        
       
    }

    bool Death()
    {
        if (monster.hp <= 0)
        {
            transform.parent.parent.GetComponent<nodeActive>().Monsters.Remove(this.gameObject);
            this.tag = "DeadMonster";
            _collider.enabled = false;
            // GetComponentInChildren<Collider>().enabled = false;
            node.resetNodeStatus();
            return true;
            Destroy(gameObject);
            
        }

        return false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Node"))
        {
            node = other.gameObject.GetComponent<nodeBase>();
        }    
    }


    void DamagePopup(float dmg)
    {
        GameObject d = Resources.Load<GameObject>("Damage");
        d.GetComponent<DamagePopup>().damage = (int) dmg;
        GameObject i = Instantiate(d);
        i.transform.position = this.transform.position;
        GetComponent<MonsterMovement>()._characterState = PlayerController.CharacterState.Idle;

    }
    private void OnCollisionExit(Collision other)
    {
        if (GetComponent<MonsterMovement>()._characterState != PlayerController.CharacterState.Attack)
        {
            return;
        }
        if (other.transform.CompareTag("Player") )
        {
            other.gameObject.GetComponent<PlayerBase>().get_Damage(this.gameObject,monster.atk);
            // GameObject d = Resources.Load<GameObject>("Damage");
            // d.GetComponent<DamagePopup>().damage = (int) monster.atk;
            // GameObject i = Instantiate(d);
            // i.transform.position = other.transform.position;
            // GetComponent<MonsterMovement>()._characterState = PlayerController.CharacterState.Idle;
        
        }
    }
}
[Serializable]
public class Monster_Data
{
    public int id;
    public float hp;
    public float atk;
    public float def;
}