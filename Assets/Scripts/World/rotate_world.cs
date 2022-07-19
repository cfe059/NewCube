using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate_world : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotate_speed = 50f;
    public bool rotate_begin;
    public Transform _target;
    public Vector3 Rotate_Left,Rotate_Right,Rotate_Up,Rotate_Down;
    public GameObject Player;
    private WorldGenerate _worldGenerate;
    public enum Rotate_Direction
    {
        Right , Left , Up , Down,None
    }

    public Rotate_Direction _rotateDirection = Rotate_Direction.None;
    void Start()
    {
        _worldGenerate = GetComponent<WorldGenerate>();
        Rotate_Right = new Vector3(90, 0, 0);
        Rotate_Left = new Vector3(0, 0, -90);
        Rotate_Down = new Vector3(0, 0, 90);
        Rotate_Up = new Vector3(-90, 0, 0);
    }
    
    // Update is called once per frame
    void Update()
    {
        float step = rotate_speed * Time.deltaTime;
        
        if (_rotateDirection == Rotate_Direction.Right)
        {
            if (!rotate_begin)
            {
                rotate_begin = true;
                _target.Rotate(Rotate_Right,Space.World);
            }
           
            
            
        }else if (_rotateDirection == Rotate_Direction.Left)
        {
            if (!rotate_begin)
            {
                rotate_begin = true;
                _target.Rotate(Rotate_Left,Space.World);
            }
        }else if (_rotateDirection == Rotate_Direction.Up)
        {
            if (!rotate_begin)
            {
                rotate_begin = true;
                _target.Rotate(Rotate_Up,Space.World);
            }
        }else if (_rotateDirection == Rotate_Direction.Down)
        {
            if (!rotate_begin)
            {
                rotate_begin = true;
                _target.Rotate(Rotate_Down,Space.World);
            }
        }
        
        if (_rotateDirection != Rotate_Direction.None)
        {
           // Player.GetComponent<Rigidbody>().useGravity = false;
            if (_target.rotation == transform.rotation)
            {
                // if (_rotateDirection == Rotate_Direction.Down)
                // {
                //     Rotate_Up = new Vector3(Rotate_Up.x, Rotate_Up.z, Rotate_Up.y);
                //     Rotate_Right = new Vector3(Rotate_Right.x, Rotate_Right.z, Rotate_Right.y);
                // }
                // else if (_rotateDirection == Rotate_Direction.Up)
                // {
                //     //Vector3 old = Rotate_Right;
                //     
                //         
                //     Rotate_Down = new Vector3(Rotate_Down.y, Rotate_Down.x, Rotate_Down.z);
                //     Rotate_Left = new Vector3(Rotate_Left.y, Rotate_Left.x, Rotate_Left.z);
                //     
                //     //Rotate_Down = old;
                // }
                //
                //Player.GetComponent<Rigidbody>().useGravity = true;
                _rotateDirection = Rotate_Direction.None;
                StartCoroutine(RandomWorld());


            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _target.rotation, step);
                
            }
        }

        IEnumerator RandomWorld()
        {
            yield return new WaitForSeconds(1f);
            rotate_begin = false;
            _worldGenerate.RandomBackWorld();

        }
        
    }

  
}
