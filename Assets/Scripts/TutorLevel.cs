using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorLevel : MonoBehaviour
{
    private static bool _wallCrashed;
    private static bool _bookReaded;
    public GameObject invisibleDoor;
    public AudioClip breakingGlass;
    public GameObject magic;
    private GameObject fox;

    private void Start()
    {
        fox = GameObject.FindGameObjectWithTag("Player");
        if (PlayerPrefs.GetInt("TutorFinished") == 1)
        {
            //CrashWall();
            invisibleDoor.SetActive(false);
            gameObject.SetActive(false);
            _wallCrashed = true;
            _bookReaded = true;
            invisibleDoor.SetActive(false);
        }
        else
        {
            invisibleDoor.SetActive(true);
            _wallCrashed = false;
            _bookReaded = false;
        }
    }

    private IEnumerator BreakWall()
    {
        invisibleDoor.SetActive(false);
        for (int i = 0; i < transform.childCount; i++)
        {
            Rigidbody block = transform.GetChild(i).GetComponent<Rigidbody>();
            block.constraints = 0;
            Vector3 vec = new Vector3(Random.Range(10, 100), Random.Range(10, 100), Random.Range(10, 100));
            block.AddTorque(vec);
            block.AddForce(vec);
        }

        _wallCrashed = true;
        yield break;
    }

    public static bool WallCrashed
    {
        get { return _wallCrashed; }
    }


    public static bool BookReaded
    {
        get { return _bookReaded; }
        set { _bookReaded = value; }
    }

    public void CrashWall()
    {
        GetComponent<AudioSource>().PlayOneShot(breakingGlass);
        StartCoroutine(TakeSpike(fox.transform.position));
        StartCoroutine(BreakWall());
    }
    
    private IEnumerator TakeSpike( Vector3 position)
    {
        Vector3 placeOfSpike = magic.transform.localPosition;
        Quaternion rotation = magic.transform.rotation;


        magic.transform.position = position;
        magic.transform.rotation = Quaternion.LookRotation(placeOfSpike-transform.localPosition,Vector3.up);
        
        magic.SetActive(true);
        
        while ( (placeOfSpike - magic.transform.localPosition).magnitude > 0.1f)
        {
            magic.transform.rotation = Quaternion.LookRotation(placeOfSpike-transform.localPosition,Vector3.up);
            magic.transform.localPosition = Vector3.MoveTowards(magic.transform.localPosition, placeOfSpike, 0.1f);
            yield return new WaitForEndOfFrame();
        }
        
        magic.SetActive(false);
        magic.transform.localPosition = placeOfSpike;
        magic.transform.rotation = rotation;
        
        yield break;
    }
}