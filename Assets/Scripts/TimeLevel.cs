using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeLevel : MonoBehaviour
{
    //завершен ли уровень
    private bool levelFinished;

    //замедлить или ускорить 
    private bool slow;

    //значение рандома
    private float randomValue;

    //событие ускоренного времени
    [SerializeField] private UnityEvent _TimeFastEvent;
    
    //событие замедленного времени
    [SerializeField] private UnityEvent _TimeSlowEvent;
    
    //событие нормального времени
    [SerializeField] private UnityEvent _TimeNormalEvent;
    
    //завершение уровня
    public void LevelEnd()
    {
        levelFinished = true;
    }

    private void Start()
    {
        levelFinished = false;
        StartCoroutine(GameOfTimes());
    }

    private IEnumerator GameOfTimes()
    {
        while (!levelFinished)
        {
            yield return new WaitForSeconds(10f * Time.timeScale);
            if (Time.timeScale.Equals(1f))
            {
                randomValue = Random.Range(0f, 1f);
                //изменить ли время в этот раз
                if (randomValue > 0.5)
                {
                    if (randomValue < 0.7)
                    {
                        Time.timeScale = Random.Range(0.3f, 0.5f);
                        _TimeSlowEvent.Invoke();
                    }                                            
                    else
                    {
                        Time.timeScale = Random.Range(1.5f, 3f);
                        _TimeFastEvent.Invoke();
                    }
                }
            }
            else
            {
                Time.timeScale = 1f;
                _TimeNormalEvent.Invoke();
            }
        }

        Time.timeScale = 1f;
    }
}