//Following is C#, I can covert it to JS if you wish

using System;
using UnityEngine;
using System.Collections;

public class GravityCtr : MonoBehaviour
{
    private void Start()
    {
        centreOfGravity = GameObject.FindWithTag("Stage").transform;
    }

    public Transform centreOfGravity; //The object to gravitate towards, in this case the cube
    public float gravity = 9f; //The force of the gravity to be applied

    public float
        maxDistance; //The maximum distance from the centreOfGravity this object can be whilst still being affected by gravity

    private Rigidbody rb;

    // Use FixedUpdate for physics calculations
    void FixedUpdate()
    {
        if (!GetComponent<Rigidbody>())
            return; //If no rigidbody attached, return
        // if (Vector3.Distance(transform.position, centreOfGravity.position) > maxDistance)
        //     return; //If we are further than the max distance, return

        if (!rb)
            rb = GetComponent<Rigidbody>(); //If the rb variable is null, get the rigidbody attached to this object
        if (rb.useGravity)
            rb.useGravity = false; //Make sure the rigidbody isn't using the global gravity variable

        Vector3 direction = (centreOfGravity.position - transform.position).normalized;
        RaycastHit hit; 
        //Debug.DrawRay(transform.position, direction);
        if (GameObject.FindWithTag("World").GetComponent<rotate_world>().rotate_begin)
            return;
        if (centreOfGravity.GetComponent<Collider>().Raycast(new Ray(transform.position, direction), out hit, maxDistance))
            //transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        {
            //Debug.Log(hit.collider.name);
           //Debug.Log("Hit"+hit.normal);
           // Debug.Log("VectorUp"+Vector3.up);
            Quaternion up = Quaternion.FromToRotation(Vector3.up, hit.normal);
            rb.AddForce(-hit.normal * gravity * rb.mass); //Apply the force to the rigidbody towards the relative surface of the cube. *note* that this will multiply the force by the rigidbody's mass so that all objects will fall at the same rate. If you don't want this, remove that section
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            //transform.rotation = Quaternion.Lerp(transform.localRotation , up,10 * Time.deltaTime);
        }
}

}