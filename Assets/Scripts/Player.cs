using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    //идентификатор
    [SerializeField] protected int ID;

    //максимальное здоровье
    [SerializeField] protected float maxHealth;

    //текущее здоровье
    [SerializeField] protected float health;

    //максимальная мана
    [SerializeField] protected float maxMana;

    //текущая мана
    [SerializeField] protected float mana;

    //скорость передвижения
    [SerializeField] protected float speed;

    //сила прыжка
    [SerializeField] protected float forceForJump;

    //кулдаун хилящей способности
    [SerializeField] protected float currentCoolDownHeal;

    //кулдаун первой атакующей способности
    [SerializeField] protected float currentCoolDownFirstDamage;

    //кулдаун второй атакующей способности
    [SerializeField] protected float currentCoolDownSecondDamage;

    //рига персонажа
    protected Rigidbody _rigidbody;

    //список доступных заклинаний
    protected List<Spell> spells;

    //текущая хилящая способность
    protected Spell currentHealSpell;

    //первая атакующая способность
    protected Spell currentFirstDamageSpell;

    //вторая атакующя способность
    protected Spell currentSecondDamageSpell;
    
    public GameObject PartFireBall;
    public GameObject PartLightning;
    public GameObject PartArrow;
    public GameObject PartSpike;

    //использование спелла
    protected abstract void UseSpell(Spell spell, int numberOfAttackSpell);

    //прыжок
    protected abstract void Jump();

    //смерть
    protected abstract void Death();

    public void PlaySpell(Spell spell, Vector3 position)
    {
        if(gameObject.activeInHierarchy)
        switch (spell.NameOfSpell)
        {
                case "RainOfFire" :
                    StartCoroutine(TakeFireBall());
                    break;
                case "Lightning" :
                    StartCoroutine(TakeLightning());
                    break;
                case "FireArrow" :
                    StartCoroutine(TakeArrow(position));
                    break;
                case "IceSpike" :
                    StartCoroutine(TakeSpike(position));
                    break;
        }
    }
    
    private IEnumerator TakeFireBall()
    {
        Vector3 placeOfBall = PartFireBall.transform.localPosition;
        PartFireBall.SetActive(true);
        GameObject destination = transform.GetComponentInChildren<Animator>().gameObject;
        while ( (destination.transform.localPosition -PartFireBall.transform.localPosition).magnitude > 0.1f)
        {
            PartFireBall.transform.localPosition = Vector3.MoveTowards(PartFireBall.transform.localPosition, destination.transform.localPosition, 0.3f);
            yield return new WaitForEndOfFrame();
        }
        
        PartFireBall.SetActive(false);
        PartFireBall.transform.localPosition = placeOfBall;
        
        yield break;
    }
    
    private IEnumerator TakeLightning()
    {
        PartLightning.gameObject.SetActive(true);
        yield return new WaitForSeconds(10f);
        PartLightning.gameObject.SetActive(false);
    }
    
    private IEnumerator TakeSpike( Vector3 position)
    {
        Vector3 placeOfSpike = PartSpike.transform.localPosition;
        Quaternion rotation = PartSpike.transform.rotation;


        PartSpike.transform.position = position;
        PartSpike.transform.rotation = Quaternion.LookRotation(placeOfSpike-transform.localPosition,Vector3.up);
        
        PartSpike.SetActive(true);
        
        while ( (placeOfSpike - PartSpike.transform.localPosition).magnitude > 0.1f)
        {
            PartSpike.transform.rotation = Quaternion.LookRotation(placeOfSpike-transform.localPosition,Vector3.up);
            PartSpike.transform.localPosition = Vector3.MoveTowards(PartSpike.transform.localPosition, placeOfSpike, 0.1f);
            yield return new WaitForEndOfFrame();
        }
        
        PartSpike.SetActive(false);
        PartSpike.transform.localPosition = placeOfSpike;
        PartArrow.transform.rotation = rotation;
        
        yield break;
    }
    
    private IEnumerator TakeArrow(Vector3 position)
    {
        Vector3 placeOfArrow = PartArrow.transform.localPosition;
        Quaternion rotation = PartArrow.transform.rotation;

        PartArrow.transform.position = position;
        
        PartArrow.transform.rotation = Quaternion.LookRotation(placeOfArrow-transform.localPosition,Vector3.up);
        
        PartArrow.SetActive(true);
        
        while ( (placeOfArrow - PartArrow.transform.localPosition).magnitude > 0.1f)
        {
            PartArrow.transform.rotation = Quaternion.LookRotation(placeOfArrow-transform.localPosition,Vector3.up);
            PartArrow.transform.localPosition = Vector3.MoveTowards(PartArrow.transform.localPosition, placeOfArrow, 0.1f);
            yield return new WaitForEndOfFrame();
        }
        
        PartArrow.SetActive(false);
        PartArrow.transform.localPosition = placeOfArrow;
        PartArrow.transform.rotation = rotation;
        
        yield break;
    }
    
}