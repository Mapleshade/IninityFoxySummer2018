using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class EndOfLevelCanvas : MonoBehaviour
{
    //текстовое поле со статистикой
    public GameObject statistic;

    //открыть канву оончания уровня
    public void OpenThisMenu()
    {
        Time.timeScale = 0;
        
        //вывод статистики уровня
        statistic.GetComponentInChildren<Text>().text = "Врагов убито: " + Level.CountOfEnemies + "\nБонусов собрано: " + Level.CountOfBonuses +
                         "\nСмертей игрока: " + Level.CountOfDeaths;
        //включить эту канву
        gameObject.SetActive(true);
    }

    public void CloseInfo()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}