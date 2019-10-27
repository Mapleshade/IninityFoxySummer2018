using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Player
{
    //зона, где враг может ходить
    [SerializeField] private GameObject enemyZone;

    //жив ли враг
    [SerializeField] private bool isAlive;

    //случайное значение для определения направления движения призрака
    [SerializeField] private int randomValue;

    //находится ли враг в своей зоне
    [SerializeField] private bool inEnemyZone;

    //находится ли игрок в зоне врага
    [SerializeField] private bool playerInEnemyZone;

    [SerializeField] private bool playerOnDistanceForAttack;

    //игрок, которого надо преследовать и атаковать
    [SerializeField] private GameObject playerTarget;

    public AudioClip GrowlClip;
    private bool growling;
    

    public void StartEnemy()
    {
        playerTarget = GameObject.FindWithTag("Player");
        playerInEnemyZone = false;
        playerOnDistanceForAttack = false;
        isAlive = true;
        //здоровье
        maxHealth = 100f;
        health = 100f;
        //мана
        mana = 100f;
        maxMana = 100f;
        //скорость передвижения
        speed = 1f;
        //сила прыжка
        forceForJump = 6f;
        //rigidbody игрока
        _rigidbody = GetComponent<Rigidbody>();

        currentFirstDamageSpell = AllSpells.GetRandomSpellForEnemy();
        StartCoroutine(Move());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyZone"))
        {
            if (enemyZone == null)
                enemyZone = other.gameObject;
            inEnemyZone = true;
        }

        if (other.gameObject.CompareTag("Player") && !other.isTrigger)
        {
            playerOnDistanceForAttack = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyZone"))
        {
            //возвращаемся к родному триггеру
            inEnemyZone = false;
            randomValue = 4;
        }

        if (other.gameObject.CompareTag("Player") && !other.isTrigger)
        {
            playerOnDistanceForAttack = false;
        }
    }

    private void FixedUpdate()
    {
        if (mana < maxMana)
            mana += Time.deltaTime;
        if (mana > maxMana)
            mana = maxMana;

        if (currentCoolDownFirstDamage > 0)
            currentCoolDownFirstDamage -= Time.deltaTime;

        if (!playerInEnemyZone && !playerOnDistanceForAttack)
        {
            if (inEnemyZone && randomValue < 4)
            {
                if (randomValue == 0)
                {
                    transform.position += transform.forward * speed * Time.deltaTime;
                }
                else if (randomValue == 1)
                {
                    transform.Rotate(Vector3.down);
                    transform.position += transform.forward * speed * Time.deltaTime;
                }
                else if (randomValue == 2)
                {
                    transform.Rotate(-Vector3.down);
                    transform.position += transform.forward * speed * Time.deltaTime;
                }
            }
            else
            {
                transform.rotation =
                    Quaternion.LookRotation(enemyZone.transform.position - transform.position, Vector3.up);

                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, enemyZone.transform.position, step);
                if ((transform.position - enemyZone.transform.position).magnitude < 0.6f)
                {
                    randomValue = 3;
                }
            }
        }
        else
        {
            //поворачиваем к игроку
            transform.rotation =
                Quaternion.LookRotation(playerTarget.transform.position - transform.position, Vector3.up);
            //если игрок в зоне врага и не в радиусе атаки, то преследуем
            if (playerInEnemyZone && !playerOnDistanceForAttack)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, step);
            }

            //если игрок в радиусе атаки
            if (playerOnDistanceForAttack && currentCoolDownFirstDamage <= 0 && mana >= currentFirstDamageSpell.ManaValue)
            {
                UseSpell(currentFirstDamageSpell, 1);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Death();
        }
    }

    protected override void UseSpell(Spell spell, int numberOfAttackSpell)
    {
        mana -= spell.ManaValue;
        currentCoolDownFirstDamage = spell.Cooldown;
        playerTarget.GetComponent<Fox>().TakeDamage(spell.Value);
        //Debug.Log("Attack");
        playerTarget.GetComponent<Fox>().PlaySpell(spell, transform.position);
    }

    private IEnumerator Move()
    {
        //пока враг жив
        while (isAlive)
        {
            //ждем 10 секунд
            yield return new WaitForSeconds(5f * Time.timeScale);

            //рандомим направление
            randomValue = Random.Range(0, 4);

            if (growling)
            {
                GetComponent<AudioSource>().PlayOneShot(GrowlClip);
            }

            growling = !growling;
        }
    }


    protected override void Jump()
    {
        throw new System.NotImplementedException();
    }

    protected override void Death()
    {
        isAlive = false;
        gameObject.SetActive(false);
        Level.CountNewEnemy();
    }

    public void RestartEnemy()
    {
        health = maxHealth;
        mana = maxMana;
        isAlive = true;
        currentCoolDownFirstDamage = 0;
    }

    public void FollowPlayer()
    {
        playerInEnemyZone = !playerInEnemyZone;
    }

    public float Health
    {
        get { return health; }
    }
}