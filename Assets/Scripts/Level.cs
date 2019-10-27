using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class Level : MonoBehaviour
{
    [SerializeField] private int IdOfLevel;
    
    //Количество убитых врагов на уровне
    private static int countOfEnemies;

    //количество смертей игрока
    private static int countOfDeaths;

    //количество собранных бонусов на уровне
    private static int countOfBonuses;

    //массив чекпоинтов на уровне
    private Checkpoint[] checkpoints;

    //массив врагов на уровне
    private GameObject[] enemyes;

    //массив бонусов
    private GameObject[] bonuses;

    //какой чекпоинт был посещен последним
    private static int indexOfLastCheckPoint;

    //атлас иконок спеллов
    public SpriteAtlas atlas;

    //ссылка на игрока
    private Fox fox;

    private static bool gameSaved = false;

    public static bool GameSaved
    {
        get { return gameSaved; }
    }

    public static void GameDidntSavedYet()
    {
        gameSaved = false;
    }


    //подготовка данных при старте уровня
    private void Start()
    {
        //выравниваем время
        Time.timeScale = 1;

        //передаем атлас картинок спеллов спелам
        AllSpells.GenerateSpells(atlas);


        //сбрасываем параметры уровня к сохраненным
        GetIndexOfLastCheckPointAndCountsFromSaves();

        //находим все чекпоинты на уровне
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Checkpoint");

        //объявляем массив чекпоинтов размером равным количеству найденных чекпоинтов
        checkpoints = new Checkpoint[objects.Length];

        //записываем все скрипты чекпоинтов в массив на места равные их ID
        for (var index = 0; index < objects.Length; index++)
        {
            Checkpoint checkpoint = objects[index].GetComponent<Checkpoint>();
            checkpoints[checkpoint.Id] = checkpoint;
        }

        //если игра продолжается с сохранения, то выключаем все чекпоинты до этого
        if (indexOfLastCheckPoint > 0)
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.Id <= indexOfLastCheckPoint)
                    checkpoint.gameObject.SetActive(false);
            }

        //находим игрока на уровне
        fox = GameObject.FindGameObjectWithTag("Player").GetComponent<Fox>();
        
        //выгружаем его данные из сохранения и переносим на место последнего чекпоинта
        fox.StartFox(checkpoints[indexOfLastCheckPoint].transform.position, checkpoints[indexOfLastCheckPoint].transform.rotation);
        
        //находим всех врагов на уровне
        enemyes = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemye in enemyes)
        {
            enemye.GetComponent<Enemy>().StartEnemy();
        }

        //находим все бонусы на уровне
        bonuses = GameObject.FindGameObjectsWithTag("Bonus");
    }

    //сохранение уровня
    public void SaveGame()
    {
        
        GameInformation.SaveGame(IdOfLevel, 1, indexOfLastCheckPoint, fox.Health, fox.Mana,
            fox.ExtraLives, fox.CurrentFirstDamageSpell.NameOfSpell, fox.CurrentSecondDamageSpell.NameOfSpell,
            countOfEnemies, countOfDeaths, countOfBonuses, fox.GetIDOfLastSpell(), 1, PlayerPrefs.GetInt("GameFinished"));
        gameSaved = true;
    }

    //запись ID посещенного игроком чекпоинта
    public static void ChangeLastCheckPoint(int index)
    {
        indexOfLastCheckPoint = index;
    }

    //получить ID последднего посещенного игроком чекпоинта
    public static int GetIndexOfLastCheckPoint()
    {
        return indexOfLastCheckPoint;
    }

    //добавить убитого врага в счетчик
    public static void CountNewEnemy()
    {
        countOfEnemies++;
    }

    //добавить найденный предмет в счетчик
    public static void CountNewBonus()
    {
        countOfBonuses++;
    }

    //добавить мерть игрока в счетчик
    public static void CountNewDeath()
    {
        countOfDeaths++;
    }


    //начать уровнеь заново
    public void RestartLevel()
    {
        //Сбрасываем параметры лисы
        fox.RestartFox(checkpoints[0].transform.position, checkpoints[0].transform.rotation);

        //включаем всех врагов на уровне, сбрасываем их параметры к дефолтным
        foreach (GameObject o in enemyes)
        {
            o.SetActive(true);
            o.GetComponent<Enemy>().RestartEnemy();
        }

        //включаем все бонусы на уровне
        foreach (GameObject o in bonuses)
        {
            o.SetActive(true);
        }

        foreach (Checkpoint checkpoint in checkpoints)
        {
            checkpoint.gameObject.SetActive(true);
        }

        //сбрасываем посещенные чекпоинты
        ResetIndexOfLastCheckPointAndCounts();

        //выставляем обычное время
        Time.timeScale = 1;
        
        //сохраняем данные
        SaveGame();
    }

    //выгрузить данные сохранения
    void GetIndexOfLastCheckPointAndCountsFromSaves()
    {
        indexOfLastCheckPoint = PlayerPrefs.GetInt("IdCheckPoint");
        countOfEnemies = PlayerPrefs.GetInt("countOfEnemies");
        countOfDeaths = PlayerPrefs.GetInt("countOfDeaths");
        countOfBonuses = PlayerPrefs.GetInt("countOfBonuses");
    }

    //сброс посещенных чекпоинтов
    void ResetIndexOfLastCheckPointAndCounts()
    {
        indexOfLastCheckPoint = 0;
        countOfEnemies = 0;
        countOfDeaths = 0;
        countOfBonuses = 0;
    }

    public static int CountOfEnemies
    {
        get { return countOfEnemies; }
    }

    public static int CountOfDeaths
    {
        get { return countOfDeaths; }
    }

    public static int CountOfBonuses
    {
        get { return countOfBonuses; }
    }
}