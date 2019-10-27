using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    //уровень, на котором находится канва
    [SerializeField] private Level level;

    private void Start()
    {
        //находим уровень при старте
        level = GameObject.FindGameObjectWithTag("Level").GetComponent<Level>();
    }

    //рестарт уровня
    public void RestartLevel()
    {
        //рестарт
        level.RestartLevel();
        //выставляем таймскейл в нормальное сосотояние
        Time.timeScale = 1;
        //выключаем канву
        gameObject.SetActive(false);
    }

    //открыть меню при гибели персонажа
    public void OpenThisMenu()
    {
        //останавливаем игру
        Time.timeScale = 0;
        //включаем канву
        gameObject.SetActive(true);
    }
}