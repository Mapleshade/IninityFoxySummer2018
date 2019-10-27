using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject MainBut;
    public GameObject AutorsBut;
    
    //старт новой игры
    public void StartNewGame()
    {
        //сохраняем дефолтные значения
        GameInformation.SaveGame(0, 0, 0, 100f, 100f, 2, "RainOfFire", "FireArrow", 0, 0, 0, 4, 0, 0);

        //должен быть ещё включение комикса

        //загружаем уровень обучения
        SceneManager.LoadScene("Tutorial");
    }

    //загрузка сохраненного уровня
    public void Continue()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("PlayerOnTutorLvl") == 0
            ? "Tutorial"
            : GameInformation.GetNameOfLevel());
    }

    //выход из игры
    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowAutors()
    {
        MainBut.SetActive(false);
        AutorsBut.SetActive(true);
    }

    public void CloseAutors()
    {
        MainBut.SetActive(true);
        AutorsBut.SetActive(false);
    }
}