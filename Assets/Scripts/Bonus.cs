using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bonus : MonoBehaviour
{
    //тип бонуса
    [SerializeField] private String type;

    //значение бонуса
    [SerializeField] private float value;

    //время действия эффекта
    [SerializeField] private float timer;

    //событие подъема бонуса
    [SerializeField] private UnityEvent _bonusEvent;

    //событие завершения уровня
    [SerializeField] private UnityEvent _endOfLevel;

    //событие подъема бонуса жизни
    [SerializeField] private UnityEvent _bonusLiveEvent;

    //событие подъема бонуса скорости
    [SerializeField] private UnityEvent _bonusSpeedEvent;

    //событие подъема бонуса прыжка
    [SerializeField] private UnityEvent _bonusJumpEvent;

    //использование бонуса
    private void UseBonus(Fox fox)
    {
        //проверка по типу, что это за бонус
        switch (type)
        {
            //дополнительная жизнь
            case "EXTRALIVE":
                fox.AddExtraLive();
                _bonusLiveEvent.Invoke();
                break;
            //бустер скорости
            case "SPEED":
                fox.AddBonusSpeed(value, timer);
                _bonusSpeedEvent.Invoke();
                break;
            //дополнительные прыжки
            case "JUMP":
                fox.AddBonusJump(value, timer);
                _bonusJumpEvent.Invoke();
                break;
            //новая способность
            case "SPELL":
                //пока что нет подъема других спеллов
                break;
            //коготь
            case "CLAW":
                PlayerPrefs.SetInt("GameFinished", 1);
                _endOfLevel.Invoke();
                break;
        }

        //если это был не коготь, завершающий уровень, то подсчитываем бонус в статистику
        if (type != "CLAW")
            _bonusEvent.Invoke();
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.down);
    }

    //если в триггер бонуса попал игрок, то используем бонус
    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger && other.gameObject.tag == "Player")
        {
            UseBonus(other.gameObject.GetComponent<Fox>());
            Level.CountNewBonus();
            gameObject.SetActive(false);
        }
    }
}