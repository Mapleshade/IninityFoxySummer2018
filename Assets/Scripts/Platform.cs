using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private float speed;
    [SerializeField] private float timer;
    [SerializeField] private bool moveUp;
    [SerializeField] private bool startCorutine;



    private void FixedUpdate()
    {
        float step = speed * Time.deltaTime;

        if (moveUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, step);
            if ((transform.position - endPos).magnitude < 0.5f && !startCorutine)
                StartCoroutine(WaitSomeTime());
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, step);
            if ((transform.position - startPos).magnitude < 0.5f && !startCorutine)
                StartCoroutine(WaitSomeTime());
        }
    }

    private IEnumerator WaitSomeTime()
    {
        startCorutine = true;
       
            yield return new WaitForSeconds(timer * Time.timeScale);
            moveUp = !moveUp;
        

        startCorutine = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        other.transform.parent = transform;
    }

    private void OnCollisionExit(Collision other)
    {
        other.transform.parent = null;
    }
}