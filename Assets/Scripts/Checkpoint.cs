using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    //идентификатор чекпоинта
    [SerializeField] private int ID;

    //если сталкивается с каким-то триггером, то проверяет игрок ли это и какой чекпоинт был до этого, если всё подходит, то сохраняет чекпоинт
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && (ID > Level.GetIndexOfLastCheckPoint() || ID == 0) && !other.isTrigger)
        {
            Level.ChangeLastCheckPoint(ID);
            other.gameObject.GetComponent<Fox>().SaveLastCheckPoint(transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }

    //геттер айдишника
    public int Id
    {
        get { return ID; }
    }
}