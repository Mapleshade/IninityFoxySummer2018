using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseInTutor : MonoBehaviour {

	//меню паузы
	public GameObject pauseMenu;

	//игровое меню
	public GameObject gameMenu;

	void FixedUpdate()
	{
		//если нажата клавишка эскейп 
		if (Input.GetKeyDown(KeyCode.Escape) )
		{
			//если меню паузы ещё не открыто
			if (!pauseMenu.activeInHierarchy)
			{
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
		Time.timeScale = 1;
		//выключаем меню паузы
		pauseMenu.SetActive(false);
		//включаем игровое меню
		gameMenu.SetActive(true);
	}

	public void ToMainMenu()
	{
		//выставляем нормальное течение времени
		Time.timeScale = 1;
		GameInformation.SaveGameFromTutorialLevel();
		SceneManager.LoadScene("MainMenu");
	}
}
