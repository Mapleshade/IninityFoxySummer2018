using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //событие сохранения игры
    [SerializeField] private UnityEvent _SaveGameEvent;

    //выйти в главное меню
    public void ToMainMenu()
    {
        Level.GameDidntSavedYet();

        //выставляем нормальное течение времени
        Time.timeScale = 1;

        //вызываем у уровня метод сохранения уровня
        _SaveGameEvent.Invoke();

        //ждем, пока игра сохранится, и выходим в главное меню
        StartCoroutine(WaitSaves());
    }


    //корутина ожидания сохранения и по его завершению выход в главное меню
    private IEnumerator WaitSaves()
    {
        yield return new WaitUntil(() => Level.GameSaved);
        SceneManager.LoadScene("MainMenu");
    }
}