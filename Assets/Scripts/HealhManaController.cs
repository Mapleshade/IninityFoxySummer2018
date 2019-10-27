using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealhManaController : MonoBehaviour
{
    //тулбар здоровья
    public HealtAndManaBar health;

    //тулбар маны
    public HealtAndManaBar mana;

    //текстовое поле для вывода количества дополнительных жизней
    public Text countExtraLives;

    //количество дополнительных жизней у игрока
    private int extraLives;

    //игрок
    private Fox fox;

    void Start()
    {
        //находим игрока
        fox = GameObject.FindGameObjectWithTag("Player").GetComponent<Fox>();
    }

    private void FixedUpdate()
    {
        //если значение здоровья игрока не совпадает со значением в тулбаре, то уравниваем
        if (fox.Health != health.current)
            health.current = fox.Health;

        //если значение маны игрока не совпадает со значением в тулбаре, то уравниваем
        if (fox.Mana != mana.current)
            mana.current = fox.Mana;

        //если количество запомненных жизней не совпадает с текущим количеством доп жизней игрока, то уравниваем
        if (extraLives != fox.ExtraLives)
        {
            countExtraLives.text = "x " + fox.ExtraLives;
            extraLives = fox.ExtraLives;
        }
    }
}