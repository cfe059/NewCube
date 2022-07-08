using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCheck : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3[] nextPos;
    public Vector3 pos;
    public Vector3 pos2;
    public bool up, down, left, right;
    public bool canMoveUp, canMoveDown, canMoveLeft, canMoveRight;
    void Start()
    {
        
    }

   
    void Update()
    {
       up =  valid(transform.forward,"OuterBorder"); // Up
       right = valid(transform.forward * -1,"OuterBorder"); // Right
       down = valid(transform.right,"OuterBorder"); // Down
       left = valid(transform.right * -1,"OuterBorder")
           
           ;  
       canMoveUp =  valid(transform.forward,"Monster"); // Up
       canMoveRight = valid(transform.forward * -1,"Monster"); // Right
       canMoveDown = valid(transform.right,"Monster"); // Down
       canMoveLeft = valid(transform.right * -1,"Monster"); // Left
    }
    bool valid(Vector3 posi,string tag) // Forward is Up 
    {
        
        float rayLenght = 10f;
        Ray myRay = new Ray(transform.position + new Vector3(0, 0f, 0), 
            posi); 
        RaycastHit hit;
        Debug.DrawRay(myRay.origin,myRay.direction,Color.red);
        if (Physics.Raycast(myRay,out hit,1f))
        {
            if (hit.collider.tag == tag)
            {
                return true;
            }
            
        }

        return false;
    }
}
