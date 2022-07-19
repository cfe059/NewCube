using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeActive : MonoBehaviour
{
    private rotate_world _rotateWorld;
    // Start is called before the first frame update
    public bool ActivateFace;
    public bool SeenFace;
    public List<GameObject> nodes;
    public List<GameObject> remainingObject;
    public List<GameObject> Floors;
    public GameObject parentFloor;
    public GameObject parentMonster;
    public GameObject parentItems;
    
    public List<GameObject> Monsters;
    void Start()
    {
        _rotateWorld = GameObject.FindWithTag("World").GetComponent<rotate_world>();
    }

    // Update is called once per frame
    void Update()
    {
        List<bool> monstermove = new List<bool>();
        if (ActivateFace)
        {
            Monsters = GetChildren(parentMonster);
           // Debug.Log(Monsters[0].GetComponent<MonsterBase>().isMoving);
           
           if (GManager.Instance._turnBase == GManager.TurnBase.Monster_Turn && !_rotateWorld.rotate_begin)
           {
               if (Monsters.Count == 0)
               {
                   GManager.Instance._turnBase = GManager.TurnBase.Monster_Moving;
                   StartCoroutine(waitTime(0.5f,GManager.TurnBase.Player_Turn));
                   return;
               }
               for (int i = 0; i < Monsters.Count; i++)
               {

                   MonsterMovement mob_base = Monsters[i].GetComponent<MonsterMovement>();
                   MonsterMovement mob_old;
                   if (mob_base.canAttack)
                   {
                       if (!mob_base.justMove)
                       {
                           StartCoroutine(Monster_Attack(mob_base,i));
                       }
                   }
                   else if (!mob_base.justMove)
                   {
                       if (i == 0)
                       {
                           mob_old = Monsters[i].GetComponent<MonsterMovement>();

                       }
                       else
                       {
                           mob_old = Monsters[i - 1].GetComponent<MonsterMovement>();

                       }

                       StartCoroutine(Move_Monster(mob_base, mob_old, i));

                       monstermove.Add(true);
                       //GManager.Instance._turnBase = GManager.TurnBase.Player_Turn;
                   }

               }

               GManager.Instance._turnBase = GManager.TurnBase.Monster_Moving;

           }    
           else if (GManager.Instance._turnBase == GManager.TurnBase.Monster_Moving)
            {
                if (Monsters.Count == 0)
                {
                   // StartCoroutine(waitTime(1,GManager.TurnBase.Player_Turn));
                    return;
                    //GManager.Instance._turnBase = GManager.TurnBase.Monster_Moving;
                }
                int just_move = 0;
                foreach (var mob in Monsters)
                {
                    MonsterMovement mob_base = mob.GetComponent<MonsterMovement>();
                    if (mob_base.justMove)
                    {
                        just_move++;
                    }
                }

                
                if (just_move == Monsters.Count)
                {
                    StartCoroutine(waitTime(1,GManager.TurnBase.Player_Turn));
                    
                    foreach (var mob in Monsters)
                    {
                        MonsterMovement mob_base = mob.GetComponent<MonsterMovement>();
                        mob_base.justMove = false;
                    }

                    GManager.Instance.MonsterMove_Turn = 0;
                    
                }
             
            }
           else
           {
               GManager.Instance.MonsterMove_Turn = 0;
           }
               
        }
        else
        {
            Monsters = null;
        }
    }

    IEnumerator Monster_Attack(MonsterMovement mob,int count)
    {
        yield return new WaitUntil(() => GManager.Instance.MonsterMove == false);
        yield return new WaitUntil(() => GManager.Instance.MonsterMove_Turn == count);

        mob.AttackP();
        yield return new WaitUntil(() => mob.justMove == true);
        
    }
    IEnumerator Move_Monster(MonsterMovement monsterBase,MonsterMovement monsterold,int count) {
        yield return new WaitUntil(() => GManager.Instance.MonsterMove == false);
        yield return new WaitUntil(() => GManager.Instance.MonsterMove_Turn == count);
        GManager.Instance.MonsterMove = true;

       // yield return new WaitForSeconds(1);

        if (count != 0)
        {
            yield return new WaitUntil(() => monsterold.justMove == true);

        }

        
        List<Vector3> canMove = new List<Vector3>();
        List<string> canMove_name = new List<string>();
        monsterBase.get_Moveable(out canMove,out canMove_name);
        if (canMove[FindShortestPlayer(canMove)] == Vector3.zero)
        {
            monsterBase.sendMove("Skip");
        }
        else
        {

            monsterBase.sendMove(canMove_name[FindShortestPlayer(canMove)]);

        }
        // Debug.Log(FindShortest(canMove,monsterBase.gameObject.GetComponent<Pathfinding>()));
        // if (FindShortest(canMove,monsterBase.gameObject.GetComponent<Pathfinding>()) == -1)
        // {
        //     monsterBase.sendMove(canMove_name[FindShortestPlayer(canMove)]);
        // }
        // else
        // {
        //     monsterBase.sendMove(canMove_name[FindShortest(canMove,monsterBase.gameObject.GetComponent<Pathfinding>())]);
        //
        // }
        

        

        
    }

    IEnumerator waitTime(float time,GManager.TurnBase state)
    {
        yield return new WaitForSeconds(time);
        GManager.Instance._turnBase = state;
        
    }
    IEnumerator Check_isMove(MonsterMovement monsterBase) {
        yield return new WaitUntil(() => monsterBase.isMoving == false);
        
        
        
        
    }

    // int FindShortest(List<Vector3> canMove,Pathfinding seeker)
    // {
    //     int _count = 0;
    //     foreach (var move in canMove)
    //     {
    //         if (seeker._path.Count > 0)
    //         {
    //             if (move == seeker._path[0].worldPosition)
    //             {
    //                 return _count;
    //             }
    //         }
    //
    //         _count++;
    //     }
    //
    //     return -1;
    // }
    int FindShortestPlayer(List<Vector3> canMove)
    {
        GameObject player = GameObject.FindWithTag("Player");
        float shortest = 100000;
        int shortest_num = 0;
        int _count = 0;
        foreach (var move in canMove)
        {
            float now = Vector3.Distance(player.transform.position, move);
            if (now  < shortest)
            {
                shortest = now;
                shortest_num = _count;
            }

            _count++;

        }
        
        return shortest_num;
    }
    
    List<GameObject> GetChildren(GameObject obj) {
        Transform children = obj.GetComponentInChildren < Transform > ();
        List<GameObject> objs = new List<GameObject>();
        //子要素がいなければ終了
        if (children.childCount == 0)
        {
            return new List<GameObject>();
        }
        foreach(Transform ob in children) {
            objs.Add(ob.gameObject);
        }
        

        return objs;
    }
    public void resetAllNode()
    {
        foreach (var node in nodes)
        {
            node.transform.GetChild(0).gameObject.GetComponent<nodeBase>().resetNodeStatus();
        }
    }
    public void DestroyAllObject(List<GameObject> gameObjects)
    {
        foreach (var gameObject in gameObjects)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateFace = true;
        }else if (other.transform.name == "Left" || other.transform.name == "Right")
        {
            SeenFace = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateFace = false;
        }else if (other.transform.name == "Left" || other.transform.name == "Right")
        {
            SeenFace = false;
        }
    }
}
