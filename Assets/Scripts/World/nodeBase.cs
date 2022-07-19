using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeBase : MonoBehaviour
{
    // Start is called before the first frame update
    public enum nodeStatus
    {
        None,Enemy,Player,Item,cantWalkable,Debuff
    }

    public nodeStatus _nodeStatus;
    
    

    public void resetNodeStatus()
    {
        _nodeStatus = nodeStatus.None;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (_nodeStatus == nodeStatus.cantWalkable || _nodeStatus == nodeStatus.Enemy)
        {
            int unwalkable = LayerMask.NameToLayer("Unwalkable");
            gameObject.layer = unwalkable;
        }else if (_nodeStatus != nodeStatus.cantWalkable || _nodeStatus == nodeStatus.Debuff || _nodeStatus == nodeStatus.Item )
        {
            int walkable = LayerMask.NameToLayer("Walkable");

            _nodeStatus = nodeStatus.None;
            gameObject.layer = walkable;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        int unwalkable = LayerMask.NameToLayer("Unwalkable");

        switch (other.tag)
        {
            case "Player":
                _nodeStatus = nodeStatus.Player;
                //_nodeStatus = nodeStatus.None;
                break;
            case "Monster":
                _nodeStatus = nodeStatus.Enemy;
                //gameObject.layer = unwalkable;
                break;
            case "Item":
                _nodeStatus = nodeStatus.Item;
                break;
            case "DeadMonster":
                _nodeStatus = nodeStatus.None;
                break;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        int walkable = LayerMask.NameToLayer("Walkable");
        _nodeStatus = nodeStatus.None;
        //this.gameObject.layer = walkable;
    }


    private void OnDrawGizmos()
    {
        Vector3 scale = new Vector3(0.5f, 0.5f, 0.5f);
        switch (_nodeStatus)
        {
            case nodeStatus.None :
                Gizmos.color = new Color(0, 1f, 0, 0.5f);
                break;
            case nodeStatus.cantWalkable :
                Gizmos.color = new Color(1f, 0, 0, 0.5f);
                break;
            
            default:
                Gizmos.color = new Color(1f, 1f ,1f, 0.5f);
                break;
            
        }
        Gizmos.DrawCube(transform.position, scale);
    }
}

