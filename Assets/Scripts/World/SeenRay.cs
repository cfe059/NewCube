using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeenRay : MonoBehaviour
{
    // Start is called before the first frame update
    private float range = 5f;
    public nodeActive seenFace;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.forward;
        Ray theRay = new Ray(transform.position, transform.TransformDirection(direction * range));
        Debug.DrawRay(transform.position, transform.TransformDirection(direction * range));
        if (Physics.Raycast(theRay,out RaycastHit hit,range))
        {
            if (seenFace != null)
            {
                seenFace.SeenFace = false;
                seenFace = null;
            }
            if (hit.collider.CompareTag("Surface"))
            {
                seenFace = hit.collider.GetComponent<nodeActive>();
                seenFace.SeenFace = true;
            }

            
        }
        
    }
}
