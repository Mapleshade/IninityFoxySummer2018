using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Vector3 direction;
    float maxDistance;

    private void Start()
    {
        direction = Camera.main.transform.position - transform.position;
        maxDistance = direction.magnitude;
    }


    void FixedUpdate()
    {
        RaycastHit hit;
        //Debug.DrawRay(transform.position, Camera.main.transform.position - transform.position, Color.yellow);
       if (Physics.Raycast(transform.position, Camera.main.transform.position - transform.position, out hit))
        {
           
            Debug.Log(hit.collider.gameObject.name);
            if (hit.distance < maxDistance && !hit.collider.isTrigger && !hit.collider.gameObject.CompareTag("Brick") && !hit.collider.gameObject.CompareTag("Floor"))
                Camera.main.transform.position = hit.point;
        }

        
//        if (Physics.SphereCast(transform.position, maxDistance, Camera.main.transform.position - transform.position,
//            out hit))
//        {
//            Debug.Log(hit.collider.gameObject.name);
//            if (hit.distance < maxDistance && !hit.collider.isTrigger && !hit.collider.gameObject.CompareTag("Ground"))
//                Camera.main.transform.position = hit.point;
//        }
    }
}