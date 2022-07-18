using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip_World : MonoBehaviour
{
    private rotate_world _rotateWorld;
    [SerializeField]
    private GameObject world;

    private PlayerController _controller;
    //private NewController _controller;

    public string _name;

    //public Transform _target;
    // Start is called before the first frame update
    void Start()
    {
        _rotateWorld = world.GetComponent<rotate_world>();
        _controller = GetComponent<PlayerController>();
        //_controller = GetComponent<NewController>();
    }

    // Update is called once per frame
    
    void Update()
    {
        if (!_controller.isMoving)
        {
            switch (_name)
            {
                case "Up":
                    _rotateWorld._rotateDirection = rotate_world.Rotate_Direction.Up;
                    GManager.Instance.Change_TimeBase();
                    _name = null;
                    break;
                case "Down":
                    _rotateWorld._rotateDirection = rotate_world.Rotate_Direction.Down;
                    GManager.Instance.Change_TimeBase();
                    _name = null;
                    break;
                case "Left":
                    _rotateWorld._rotateDirection = rotate_world.Rotate_Direction.Left; 
                    GManager.Instance.Change_TimeBase();
                    _name = null;
                    break;
                case "Right":
                    _rotateWorld._rotateDirection = rotate_world.Rotate_Direction.Right;
                    GManager.Instance.Change_TimeBase();
                    _name = null;
                    break;
            }

            // if (_target.rotation != transform.rotation)
            // {
            //     transform.rotation = Quaternion.RotateTowards(transform.rotation,  _target.rotation, 
            //         _rotateWorld.rotate_speed * Time.deltaTime);
            // }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "Up" || other.gameObject.name == "Down" || 
            other.gameObject.name == "Left" || other.gameObject.name == "Right" )
        {
            _name = other.gameObject.name;
        }
    }
}
