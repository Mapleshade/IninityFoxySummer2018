using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    //меню паузы
    public GameObject pauseMenu;

    //игровое меню
    public GameObject gameMenu;

    //меню смерти
    public GameObject deathMenu;

    //меню завершения уровня
    public GameObject endOfGameMenu;

    //какой таймскейл был до открытия меню паузы
    private float currentTimeScale;

    void FixedUpdate()
    {
        //если нажата клавишка эскейп и не открыты меню завершения игры и смерти игрока
        if (Input.GetKeyDown(KeyCode.Escape) && (!deathMenu.activeInHierarchy || !endOfGameMenu.activeInHierarchy))
        {
            //если меню паузы ещё не открыто
            if (!pauseMenu.activeInHierarchy)
            {
                //запоминаем таймскейл в игре
                currentTimeScale = Time.timeScale;
                //сбарсываем таймскейл
                Time.timeScale = 0;
                //включаем меню паузы
                pauseMenu.SetActive(true);
                //выключаем игровое меню
                gameMenu.SetActive(false);
            }
            //если меню паузы уже включено
            else
            {
                //возвращаем к игре
                ToGame();
            }
        }
    }

    //вернуться к игре
    public void ToGame()
    {
        //восстанавливаем таймскейл
        Time.timeScale = currentTimeScale;
        //выключаем меню паузы
        pauseMenu.SetActive(false);
        //включаем игровое меню
        gameMenu.SetActive(true);
    }
}