using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class GameCanvas : MonoBehaviour
{
    //окно со способностями игрока
    public GameObject spellsGameObject;

    //текстовое поле кнопки добавления способности
    public Text addSpell1;

    //текстовое поле кнопки добавления способности
    public Text addSpell2;

    //картинка хилки
    public Image HealImage;

    //объект с кнопками добавления способностей
    public GameObject addSpells;

    //ссылка на игрока
    private Fox fox;

    //какой навык выбран
    private int whichSpellChoosen;

    //массив кнопок спелов
    [SerializeField] private GameObject[] buttons;

    //массив картинок спелов
    [SerializeField] private Sprite[] images;

    //картинка первой способности
    public Image firstSkill;

    //картинка второй способности
    public Image secondSkill;

    //Картинка бонуса
    public Image bonusImage;

    //таймер бонуса (изначальное значение)
    [SerializeField] private float timerBonus;

    //текстовое поле, в котором будут отображаться пояснения
    public Text history;

    //кнопка обучения
    public GameObject tutorialButton;

    private void Start()
    {
        //включить кнопку обчения
        tutorialButton.SetActive(true);
        
        //находим игрока на сцене
        fox = GameObject.FindWithTag("Player").GetComponent<Fox>();

        //устанавливаем подходящие навыки спеллов игрока
        firstSkill.sprite = fox.CurrentFirstDamageSpell.SmallSprite;
        secondSkill.sprite = fox.CurrentSecondDamageSpell.SmallSprite;
    }

    //закрыть кнопку обучения
    public void CloseTutorialButton()
    {
        tutorialButton.SetActive(false);
    }

    //открыть меню смены активных спеллов
    public void ChangeChoosenSpell(int value, string nameOfSpell)
    {
        //запоминаем, на какой хотим сменить
        whichSpellChoosen = value;
        
        //выводим на кнопках какую способность на какую позицию назначить
        addSpell1.text = "Установить на первую способность: " + nameOfSpell;
        addSpell2.text = "Установить на вторую способность: " + nameOfSpell;
        
        //включаем меню смены спеллов
        addSpells.SetActive(true);
    }

    private void FixedUpdate()
    {
        //если нажали колесико мыши, то открываем или закрываем меню способностей
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            MenuSpells();
        }

        //отображение кулдауна хилки
        if (fox.CurrentCoolDownHeal > 0)
            HealImage.fillAmount =
                (fox.CurrentHealSpell.Cooldown - fox.CurrentCoolDownHeal) / fox.CurrentHealSpell.Cooldown;

        //отображение кулдауна первого скила
        if (fox.CurrentCoolDownFirstDamage > 0)
            firstSkill.fillAmount =
                (fox.CurrentFirstDamageSpell.Cooldown - fox.CurrentCoolDownFirstDamage) /
                fox.CurrentFirstDamageSpell.Cooldown;

        //отображение кулдауна второго скила
        if (fox.CurrentCoolDownSecondDamage > 0)
            secondSkill.fillAmount =
                (fox.CurrentSecondDamageSpell.Cooldown - fox.CurrentCoolDownSecondDamage) /
                fox.CurrentSecondDamageSpell.Cooldown;
        
        //отображение кулдауна бонуса
        if (fox.TimerBonus > 0)
        {
            if (!bonusImage.gameObject.activeInHierarchy)
            {
                GetComponent<AudioSource>().Play();
                bonusImage.gameObject.SetActive(true);
            }

            bonusImage.fillAmount = fox.TimerBonus / timerBonus;
        }
        else if (fox.TimerBonus <= 0 && bonusImage.gameObject.activeInHierarchy)
        {
            EndBuster();
            bonusImage.gameObject.SetActive(false);
        }
    }

    //открыть/закрыть окно способностей
    private void MenuSpells()
    {
        //открываем или закрываем меню способностей
        spellsGameObject.SetActive(!spellsGameObject.activeInHierarchy);
        
        //если меню способностей открыто
        if (spellsGameObject.activeInHierarchy)
        {
            //узнаем у игрока, какие у него есть способности
            List<Spell> spells = fox.Spells;
            
            //создаем индекс для кнопок
            int index = 0;
            
            //проходим по всем кнопкам
            foreach (Spell spell in spells)
            {
                //если способность наносит урон
                if (spell.Type != "HEAL")
                {
                    //выводим название спелла
                    buttons[index].GetComponentInChildren<Text>().text = spell.NameForGame;
                    
                    //добавляем листенер на клик
                    buttons[index].GetComponent<Button>().onClick
                        .AddListener(() => ChangeChoosenSpell(spell.Id, spell.NameForGame));

                    //выводим картинку
                    buttons[index].GetComponent<Image>().sprite = spell.LargeSprite;

                    //включаем кнопку
                    buttons[index].SetActive(true);
                    
                    //увеличиваем счетчик кнопки
                    index++;
                }
            }
        }
        else
        {
            //выключаем меню добавления спеллов
            addSpells.SetActive(false);
        }
    }

    //сохраням новый спелл
    public void SaveNewSpell(int value)
    {
        //value - это первая или вторая атакующая способность, whichSpellChoosen передается фоксу и заменяет активный на новый 
        fox.SetDamageSpell(value, whichSpellChoosen);
        ChangeSpriteOnToolBars(value, AllSpells.GetSpellById(whichSpellChoosen).SmallSprite);
        
        //выключаем менюшки спеллов
        addSpells.SetActive(false);
        spellsGameObject.SetActive(false);
    }

    //изменить спрайт в тулбаре, если изменили способность
    private void ChangeSpriteOnToolBars(int number, Sprite sprite)
    {
        if (number == 1)
        {
            firstSkill.sprite = sprite;
        }
        else
        {
            secondSkill.sprite = sprite;
        }
    }

    //получить таймер бонуса у игрока
    public void GetTimerBonus()
    {
        timerBonus = fox.TimerBonus;
    }

    //вывести информацию о бонусе
    public void TakeBonusExtraLive()
    {
        if (history.gameObject.activeInHierarchy)
            history.text += "\nПодобрана дополнительная жизнь!";
        else
            history.text = "Подобрана дополнительная жизнь!";
        StartCoroutine(ShowInfo());
    }
    
    //вывести информацию о бонусе
    public void TakeBonusSpeed()
    {
        if (history.gameObject.activeInHierarchy)
            history.text += "\nПолучено ускорение!";
        else
            history.text = "Получено ускорение!";
        StartCoroutine(ShowInfo());
    }

    //вывести информацию о бонусе
    public void TakeBonusJump()
    {
        if (history.gameObject.activeInHierarchy)
            history.text += "\nПолучены дополнительные прыжки!";
        else
            history.text = "Получены дополнительные прыжки!";
        StartCoroutine(ShowInfo());
    }

    //вывести информацию о событии времени
    public void TimeSlowEvent()
    {
        if (history.gameObject.activeInHierarchy)
            history.text += "\nВремя замедленно!";
        else
            history.text = "Время замедленно!";
        StartCoroutine(ShowInfo());
    }

    //вывести информацию о событии времени
    public void TimeFastEvent()
    {
        if (history.gameObject.activeInHierarchy)
            history.text += "\nВремя ускоренно!";
        else
            history.text = "Время ускоренно!";
        StartCoroutine(ShowInfo());
    }

    //вывести информацию о событии времени
    public void TimeNormalEvent()
    {
        if (history.gameObject.activeInHierarchy)
            history.text += "\nВремя вернулось в обычное русло!";
        else
            history.text = "Время вернулось в обычное русло!";
        StartCoroutine(ShowInfo());
    }
    
    //вывести информацию о невозможности использовать способность
    public void CantUseSpell()
    {
        if (history.gameObject.activeInHierarchy)
            history.text += "\nНевозможно использовать способность прямо сейчас!";
        else
            history.text = "Невозможно использовать способность прямо сейчас!";
        StartCoroutine(ShowInfo());
    }

    //завершение действия бустера
    private void EndBuster()
    {
        GetComponent<AudioSource>().Stop();
        if (history.gameObject.activeInHierarchy)
            history.text += "\nДействие бустера закончилось!";
        else
            history.text = "Действие бустера закончилось!";
        StartCoroutine(ShowInfo());
    }

    //корутина отображения уведомления
    private IEnumerator ShowInfo()
    {
        history.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f * Time.timeScale);
        history.gameObject.SetActive(false);
    }
}