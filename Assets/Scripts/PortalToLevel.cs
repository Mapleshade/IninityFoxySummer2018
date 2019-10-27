using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PortalToLevel : MonoBehaviour
{
    
    [SerializeField] private int idLevel;
    [SerializeField] private UnityEvent _toTutorialLvl;
    private void OnTriggerEnter(Collider other)
    {
        //если игрок попадает в триггер
        if (other.gameObject.CompareTag("Player") && !other.isTrigger)
        {
            
            //если айди уровня, куда ведет портал, больше на 1, чем айди уровня,
            //на котором игрок уже был, то загружаем сцену с этим уровнем
            if (idLevel == PlayerPrefs.GetInt("IDLevel") + 1)
                SceneManager.LoadScene(GameInformation.GetNameOfLevelByID(idLevel));
            if(idLevel == 0)
                ToTutorial();
        }
    }

    public void openPortal()
    {
        gameObject.SetActive(true);
    }
    
    //выйти на уровень тутора
    private void ToTutorial()
    {
        Level.GameDidntSavedYet();
        //выставляем нормальное течение времени
        Time.timeScale = 1;
        
        //вызываем у уровня метод сохранения уровня
        _toTutorialLvl.Invoke();
        
        //ждем, пока игра сохранится, и выходим в главное меню
        StartCoroutine(WaitSaves());
    }


    //корутина ожидания сохранения и по его завершению выход в главное меню
    private IEnumerator WaitSaves()
    {
        yield return new WaitUntil(() => Level.GameSaved);
        SceneManager.LoadScene("Tutorial");
    }
}