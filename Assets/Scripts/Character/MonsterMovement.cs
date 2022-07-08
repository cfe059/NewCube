using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    private rotate_world _rotateWorld;
    // Start is called before the first frame update
    public Vector3 controller_Up;
    public Vector3 controller_Down;
    public Vector3 controller_Right;
    public Vector3 controller_Left;
    [SerializeField]
    private Vector3 target;
    [SerializeField]
    Quaternion look_target;
    [SerializeField]
    private Vector3 now;
    [SerializeField]
    public bool isMoving;
    
    [SerializeField] private string Player_Near;
    public bool justMove;
    public bool canAttack;
    [SerializeField]
    private Animator _animator;
    private Rigidbody rb;
    public Vector3 a;
    public PlayerController.CharacterState _characterState;
    [SerializeField]
    private Transform body;
    void Start()
    {
        OverLapBox();
        rb = GetComponent<Rigidbody>();
        _rotateWorld = GameObject.FindWithTag("World").GetComponent<rotate_world>();
        _animator = gameObject.GetComponentInChildren<Animator>();
    }
    
    Vector3 loopOverlap(Collider[] hits)
    {
        List<Vector3> _node = new List<Vector3>();
        Vector3 shortDis = new Vector3();
        float Distance = 2000;
        foreach (var hit in hits)
        {
            if (hit.transform.tag == "Node")
            {
                if (hit.GetComponent<nodeBase>()._nodeStatus == nodeBase.nodeStatus.None)
                {
                    canAttack = false;
                    _node.Add(hit.transform.position);
                    break;
                }
                
            }

            
            
        }

        foreach (var node in _node)
        {
            float _d = Vector3.Distance(gameObject.transform.position, node);
            if (_d < Distance)
            {
                Distance = _d;
                shortDis = node;
            }
        }
        return shortDis;

        
    }

    bool FindPlayer(Collider [] hits)
    {
        foreach (var hit in hits)
        {
            if (hit.transform.tag == "Player")
            {
                return true;
            }
        }

        return false;
    }
    void RayPlayer()
    {
        Vector3 scale = new Vector3(0.7f, 0.7f, 0.7f);
        Vector3 up = new Vector3(transform.position.x, transform.position.y, transform.position.z+1f);
        Vector3 down = new Vector3(transform.position.x, transform.position.y, transform.position.z-1f);
        Vector3 right = new Vector3(transform.position.x+1f, transform.position.y, transform.position.z);
        Vector3 left = new Vector3(transform.position.x-1f, transform.position.y, transform.position.z);
        
        Collider[] upHit = Physics.OverlapBox(up,scale/2 , Quaternion.identity);
        Collider[] downHit = Physics.OverlapBox(down,scale/2 , Quaternion.identity);
        Collider[] rightHit = Physics.OverlapBox(right,scale/2 , Quaternion.identity);
        Collider[] leftHit = Physics.OverlapBox(left,scale/2 , Quaternion.identity);
        if (FindPlayer(upHit))
        {
            Player_Near = "Up";
            canAttack = true;
        }else if (FindPlayer(downHit))
        {
            Player_Near = "Down";
            canAttack = true;
        }else if (FindPlayer(leftHit))
        {
            Player_Near = "Left";
            canAttack = true;
        }else if (FindPlayer(rightHit))
        {
            Player_Near = "Right";
            canAttack = true;
        }
        else
        {
            canAttack = false;
            Player_Near = "";
        }
    
        
    }
    
    
    void OverLapBox()
    {
        Vector3 scale = new Vector3(1, 1, 1);
        Vector3 up = new Vector3(transform.position.x, transform.position.y, transform.position.z+1f);
        Vector3 down = new Vector3(transform.position.x, transform.position.y, transform.position.z-1f);
        Vector3 right = new Vector3(transform.position.x+1f, transform.position.y, transform.position.z);
        Vector3 left = new Vector3(transform.position.x-1f, transform.position.y, transform.position.z);
        
        Collider[] upHit = Physics.OverlapBox(up,scale/2 , Quaternion.identity);
        Collider[] downHit = Physics.OverlapBox(down,scale/2 , Quaternion.identity);
        Collider[] rightHit = Physics.OverlapBox(right,scale/2 , Quaternion.identity);
        Collider[] leftHit = Physics.OverlapBox(left,scale/2 , Quaternion.identity);
        controller_Up =  loopOverlap(upHit);
        controller_Down =  loopOverlap(downHit);
        controller_Left =  loopOverlap(leftHit);
        controller_Right =  loopOverlap(rightHit);
        
    }

    public void AttackP()
    {
        StartCoroutine(AttackPlayer());
    }
    
    IEnumerator AttackPlayer()
    {
        GManager.Instance.MonsterMove = true;
        _characterState = PlayerController.CharacterState.Attack;
        _animator.SetTrigger("Attack");
        justMove = true;

        yield return new WaitForSeconds(1);
        _characterState = PlayerController.CharacterState.Idle;
        GManager.Instance.MonsterMove_Turn += 1;
        GManager.Instance.MonsterMove = false;


        
    }
    // Update is called once per frame
    
    void Update()
    {
        now = transform.position;
        OverLapBox();
        RayPlayer();
        //LookAtWhenWalk();
        
       
        if (GManager.Instance._turnBase == GManager.TurnBase.Monster_Turn ||GManager.Instance._turnBase == GManager.TurnBase.Monster_Moving  )
        {
            if (!isMoving && !_rotateWorld.rotate_begin && transform.parent.parent.GetComponent<nodeActive>().ActivateFace )
            {
                LookAtWhenWalk(Player_Near);

            }
            
            if (isMoving)
            {
                //
                // transform.position = Vector3.MoveTowards(transform.position, target, 2 * Time.deltaTime);
                // if (Vector3.Distance(transform.position,target) < 0.01f)
                // {
                //     transform.position = target;
                //     isMoving = false;
                //     OverLapBox();
                //     target = Vector3.zero;
                //     GManager.Instance.MonsterMove = false;
                //     GManager.Instance.MonsterMove_Turn += 1;
                //     justMove = true;
                //     
                // }
            }
            else
            {
                
            }
        
        }
    }

    public void get_Moveable(out List<Vector3> vector3,out List<string> name_v)
    {
        List<Vector3> objs = new List<Vector3>();
        List<string> name = new List<string>();
        if (controller_Up != Vector3.zero)
        {
            objs.Add(controller_Up);
            name.Add("Up");
        }
        if (controller_Down != Vector3.zero)
        {
            objs.Add(controller_Down);
            name.Add("Down");
        }
        if (controller_Left != Vector3.zero)
        {
            objs.Add(controller_Left);
            name.Add("Left");
        }
        if (controller_Right != Vector3.zero)
        {
            objs.Add(controller_Right);
            name.Add("Right");
        }
        vector3 = objs;
        name_v = name;
    }
    void C_Move(Vector3 direction,string s_direction)
    {
        target = direction;
        
        if (target != Vector3.zero)
        {
            _characterState = PlayerController.CharacterState.Walk;
            isMoving = true;
            LookAtWhenWalk(s_direction);
            transform.DOMove(target, 0.5f)
                .SetEase(Ease.OutBounce)
                .OnComplete(() =>
                {
                    transform.position = target;
                    isMoving = false;
                    OverLapBox();
                    target = Vector3.zero;
                    _characterState = PlayerController.CharacterState.Idle;
                    GManager.Instance.MonsterMove = false;
                    GManager.Instance.MonsterMove_Turn += 1;
                    justMove = true;
                })
                .Play();
        }
    }
    public void sendMove(string direction)
    {
        switch (direction)
        {
            case "Up":
                C_Move(controller_Up,"Up");
                break;
            case "Down":
                C_Move(controller_Down,"Down");
                break;
            case "Right":
                C_Move(controller_Right,"Right");
                break;
            case "Left":
                C_Move(controller_Left,"Left");
                break;
            case "Skip":
                target = this.transform.position;
                if (target != Vector3.zero)
                {
                    transform.position = target;
                    isMoving = false;
                    OverLapBox();
                    target = Vector3.zero;
                    GManager.Instance.MonsterMove = false;
                    GManager.Instance.MonsterMove_Turn += 1;
                    justMove = true;
                }

                break;
        }
        GManager.Instance.Logger($"{gameObject.name} : 移動 X:{target.x} Y:{target.y} Z:{target.z}");
        
    }

    void LookAtWhenWalk(string direction)
    {
        //Debug.Log(transform.forward);
        
        switch (direction)
        {
            case "Up":
                body.transform.forward = new Vector3(0,0,1);
                break;
            case "Down":
                body.transform.forward = new Vector3(0,0,-1);
                break;
            case "Right":
                body.transform.forward = new Vector3(1,0,0);
                break;
            case "Left":
                body.transform.forward = new Vector3(-1,0,0);
                break;
        }
    }
    void OnDrawGizmosSelected()
    {
        Vector3 scale = new Vector3(1, 1, 1);

        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Vector3 up = new Vector3(transform.position.x, transform.position.y, transform.position.z+1f);
        Vector3 down = new Vector3(transform.position.x, transform.position.y, transform.position.z-1f);
        Vector3 right = new Vector3(transform.position.x+1f, transform.position.y, transform.position.z);
        Vector3 left = new Vector3(transform.position.x-1f, transform.position.y, transform.position.z);

        Gizmos.DrawCube(up, scale);
        Gizmos.DrawCube(down, scale);
        Gizmos.DrawCube(right, scale);
        Gizmos.DrawCube(left, scale);
    }
}
