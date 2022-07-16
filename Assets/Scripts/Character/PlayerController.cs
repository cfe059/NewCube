using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
    public bool isMoving;
    //private List<GameObject> _listOfEnemies;
    [SerializeField]
    public IDictionary<string,GameObject> _listOfEnemies;
    private Animator _animator;
    private Rigidbody rb;
    [SerializeField]
    private GameObject _player_Object;
    public CharacterState _playerState = CharacterState.Idle;
    public enum CharacterState
    {
        Idle,
        Walk,
        Run,
        Attack,
        Death
    }
    
    void Start()
    {
        _animator = this.gameObject.GetComponentInChildren<Animator>();
        OverLapBox();
        rb = GetComponent<Rigidbody>();
        _rotateWorld = GameObject.FindWithTag("World").GetComponent<rotate_world>();
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
                if(hit.GetComponent<nodeBase>()._nodeStatus == nodeBase.nodeStatus.None || hit.GetComponent<nodeBase>()._nodeStatus == nodeBase.nodeStatus.Item)
                    _node.Add(hit.transform.position);
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
    void LookAtWhenWalk(string direction)
    {
        
        switch (direction)
        {
            case "Up":
                _player_Object.transform.forward = new Vector3(0,0,-1);
                //transform.forward 
                break;
            case "Down":
                _player_Object.transform.forward = new Vector3(0,0,1);
                //transform.forward = new Vector3(0,0,1);
                break;
            case "Right":
                _player_Object.transform.forward = new Vector3(-1,0,0);
                //transform.forward = new Vector3(-1,0,0);
                break;
            case "Left":
                _player_Object.transform.forward = new Vector3(1,0,0);
                //transform.forward = new Vector3(1,0,0);
                break;
        }
    }
    void C_Move(Vector3 direction)
    {
        target = direction;
        if (target != Vector3.zero)
        {
            _playerState = CharacterState.Walk;
            isMoving = true;
            GManager.Instance.Change_TurnBase(GManager.TurnBase.Player_Moving);
            transform.DOMove(target, 0.5f)
                .SetEase(Ease.OutBounce)
                .OnComplete(()=>
                {
                    transform.position = target;
                    isMoving = false;
                    OverLapBox();
                    target = Vector3.zero;
                    _playerState = CharacterState.Idle;
                    StartCoroutine(ChangeState());
                }).Play();
        }
    }
    void OverLapBox()
    {
        Vector3 scale = new Vector3(1, 2, 1);
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

    // Update is called once per frame
    IEnumerator ChangeState()
    {
        yield return new WaitForSeconds(1);

        yield return new WaitUntil(() => _rotateWorld.rotate_begin == false);

        GManager.Instance.Change_TurnBase(GManager.TurnBase.Monster_Turn);
    }

    IEnumerator Attack(string s_direction)
    {
        //yield return new WaitUntil(() => GManager.Instance._turnBase == GManager.TurnBase.Player_Moving);
        Debug.Log(s_direction);
        LookAtWhenWalk(s_direction);
        _playerState = CharacterState.Attack;
        _animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1);
        _playerState = CharacterState.Idle;
        StartCoroutine(ChangeState());
        yield return null;
    
    }
    // IEnumerator Attack()
    // {
    //     yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("idle"));
    //     GManager.Instance.Change_TurnBase(GManager.TurnBase.Monster_Turn);
    // }
    // void Send_Attack()
    // {
    //     _animator.SetTrigger("Attack");
    //     StartCoroutine(Attack());
    // }
    void Update()
    {
        
        OverLapBox();
        if (GManager.Instance._turnBase == GManager.TurnBase.Player_Turn || GManager.Instance._turnBase == GManager.TurnBase.Player_Moving)
        {
            RayMonster();
            
            if (isMoving &&  GManager.Instance._turnBase == GManager.TurnBase.Player_Moving)
            {
                //transform.position = Vector3.MoveTowards(transform.position, target, 5 * Time.deltaTime);
                
                // if (Vector3.Distance(transform.position,target) < Vector3.kEpsilon)
                // {
                //     transform.position = target;
                //     isMoving = false;
                //     OverLapBox();
                //     target = Vector3.zero;
                //     StartCoroutine(ChangeState());
                // }
            }
            else
            {
                if (!_rotateWorld.rotate_begin && GManager.Instance._turnBase == GManager.TurnBase.Player_Turn && !isMoving)
                {


                    if (Input.GetAxisRaw("Vertical") == 1)
                    {
                        C_Move(controller_Up);
                        LookAtWhenWalk("Up");
                    }
                    else if (Input.GetAxisRaw("Vertical") == -1)
                    {
                        C_Move(controller_Down);
                        LookAtWhenWalk("Down");
                    }
                    else if (Input.GetAxisRaw("Horizontal") == 1)
                    {
                        C_Move(controller_Right);
                        LookAtWhenWalk("Right");
                    }
                    else if (Input.GetAxisRaw("Horizontal") == -1)
                    {
                        C_Move(controller_Left);
                        LookAtWhenWalk("Left");
                    
                    }else if (Input.GetAxisRaw("Jump") == 1 && GManager.Instance._turnBase == GManager.TurnBase.Player_Turn && _listOfEnemies != null)
                    {
                        //Send_Attack();
                        if (_listOfEnemies != null)
                        {
                            foreach (var enemy in _listOfEnemies)
                            {
                                StartCoroutine(Attack(enemy.Key));
                                break;
                            }
                        }
                        
                        // StartCoroutine(Attack(_listOfEnemies[][1]));
                        GManager.Instance.Change_TurnBase(GManager.TurnBase.Player_Moving);
                    }

                    if (isMoving)
                    {
                        //GManager.Instance.Logger($"Player : 移動　X:{(int)transform.position.x} Y:{(int)transform.position.y} Z:{(int)transform.position.z}");
                    }

                }
            }
        }
    }
    bool FindMonsters(Collider [] hits , out GameObject monster)
    {
        monster = null;
        foreach (var hit in hits)
        {
            if (hit.transform.tag == "Monster")
            {
                monster = hit.gameObject;
                return true;
            }
        }

        return false;
    }
    void RayMonster()
    {
        //List<Collider[]> hitList = new List<Collider[]>();
        IDictionary<Collider[],string> hitList = new Dictionary<Collider[],string>();
        IDictionary<string,GameObject> monsterList = new Dictionary<string,GameObject>();
        Vector3 scale = new Vector3(0.5f, 0.3f, 0.5f);
        Vector3 up = new Vector3(transform.position.x, transform.position.y, transform.position.z+1f);
        Vector3 down = new Vector3(transform.position.x, transform.position.y, transform.position.z-1f);
        Vector3 right = new Vector3(transform.position.x+1f, transform.position.y, transform.position.z);
        Vector3 left = new Vector3(transform.position.x-1f, transform.position.y, transform.position.z);
        
        Collider[] upHit = Physics.OverlapBox(up,scale/2 , Quaternion.identity);
        Collider[] downHit = Physics.OverlapBox(down,scale/2 , Quaternion.identity);
        Collider[] rightHit = Physics.OverlapBox(right,scale/2 , Quaternion.identity);
        Collider[] leftHit = Physics.OverlapBox(left,scale/2 , Quaternion.identity);
        hitList.Add(upHit,"Up");
        hitList.Add(downHit,"Down");
        hitList.Add(rightHit,"Right");
        hitList.Add(leftHit,"Left");
        foreach (var hit in hitList)
        {
            GameObject monster = null;
            if (FindMonsters(hit.Key,out monster))
            {
                    monsterList.Add(hit.Value,monster);
                    
            }
        }

        _listOfEnemies = monsterList;
        

    }
    void OnDrawGizmosSelected()
    {
        Vector3 scale = new Vector3(1, 2, 1);

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
