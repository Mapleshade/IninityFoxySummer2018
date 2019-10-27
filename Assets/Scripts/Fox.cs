using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.Events;

public class Fox : Player
{
    //дополнительные жизни
    [SerializeField] private int extraLives;

    //месторасположение последнего чекпоинта
    [SerializeField] private Vector3 lastCheckpointPos;

    [SerializeField] private Quaternion lastCheckpointRot;

    //список враов в триггере игрока
    private List<Enemy> enemyes;

    //таймер полученного бонуса
    [SerializeField] private float timerBonus;

    //дефолтная скорость
    [SerializeField] private float defaultSpeed;

    //сколько прыжков может совершить игрок
    [SerializeField] private int countOfJumMax;

    //Сколько прыжков сделал игрок
    [SerializeField] private int countOfJump;

    //событие смерти игрока
    [SerializeField] private UnityEvent _deathEvent;

    //нажата ли клавиша прыжка
    [SerializeField] private bool isPressedJump;

    //нажата ли клавиша первого скилла
    [SerializeField] private bool isPressedFirstDamage;

    //нажата ли клавиша второго скилла
    [SerializeField] private bool isPressedSecondDamage;

    //нажата ли клавиша хилки
    [SerializeField] private bool isPressedHeal;

    //аниматор игрока
    public Animator anim;

    //частички хилки вокруг игрока
    public GameObject partHeal;
    
    

    //событие невозможности использовать скилл
    [SerializeField] private UnityEvent _cantUseSkillEvent;

    private bool inWater;

    //загрузка лисы
    public void StartFox(Vector3 position, Quaternion rot)
    {
        //перемещение на позицию последнего чекпоинта
        lastCheckpointPos = position;
        lastCheckpointRot = rot;
        transform.position = position;
        transform.rotation = rot;
        //здоровье
        maxHealth = 100f;
        health = PlayerPrefs.GetFloat("HealOfPlayer");

        //мана
        mana = PlayerPrefs.GetFloat("ManaofPlayer");
        maxMana = 100f;

        //скорость передвижения
        speed = 4f;
        defaultSpeed = speed;

        //сила прыжка
        forceForJump = 6f;

        //rigidbody игрока
        _rigidbody = GetComponent<Rigidbody>();

        //дополнительные жизни
        extraLives = PlayerPrefs.GetInt("ExtralivesOfPlayer");

        //инициализация списка врагов в триггере игрока
        enemyes = new List<Enemy>();

        //максимальное количество прыжков
        countOfJumMax = 1;
        countOfJump = 0;

        //сброс нажатых клавиш
        isPressedFirstDamage = false;
        isPressedHeal = false;
        isPressedSecondDamage = false;
        isPressedJump = false;

        inWater = false;
        //инициализация магии
        StartSpells();
    }

    //получение магии игроком
    private void StartSpells()
    {
        //забираем доступные спелы
        spells = AllSpells.GetSpells();
Debug.Log(PlayerPrefs.GetInt("IDOfLastSpell"));
        //назначаем хилку
        currentHealSpell = spells[0];

        //находим спелы, которые были активны
        foreach (Spell spell in spells)
        {
            if (spell.NameOfSpell == PlayerPrefs.GetString("NameOfFirstSpell"))
                currentFirstDamageSpell = spell;
            if (spell.NameOfSpell == PlayerPrefs.GetString("NameOfSecondSpell"))
                currentSecondDamageSpell = spell;
        }
    }

    //получить айдишник последнего спела
    public int GetIDOfLastSpell()
    {
        int id = 2;
        foreach (Spell spell in spells)
        {
            if (spell.Id > id)
                id = spell.Id;
        }

        return id;
    }

    private void Update()
    {
        //прыжок
        if (Input.GetKeyDown(KeyCode.Space) && countOfJumMax > countOfJump && !isPressedJump)
        {
            isPressedJump = true;
            Jump();
        }
    }

    private void FixedUpdate()
    {
        //если мана не полная, то восстанавливаем её
        if (mana < maxMana)
            mana += Time.deltaTime * 2;

        //если мана вышла за пределы максимального значения, то приравниваем к максимальному
        if (mana > maxMana)
            mana = maxMana;

        //если бонус ещё активен, то уменьшаем время его действия
        if (timerBonus > 0)
            timerBonus -= Time.deltaTime;

        //если время действия бонуса вышло, то прекращаем его действие
        if (timerBonus <= 0)
        {
            speed = defaultSpeed;
            countOfJumMax = 1;
        }

        //перезаряка способности лечения
        if (currentCoolDownHeal > 0)
        {
            currentCoolDownHeal -= Time.deltaTime;
        }

        //перезарядка первой атакующей способности
        if (currentCoolDownFirstDamage > 0)
        {
            currentCoolDownFirstDamage -= Time.deltaTime;
        }

        //перезарядка первой атакующей способности
        if (currentCoolDownSecondDamage > 0)
        {
            currentCoolDownSecondDamage -= Time.deltaTime;
        }

        //движение вперед
        if (Input.GetKey(KeyCode.W))
        {
            if (!inWater)
            {
                transform.position += transform.forward * speed * Time.deltaTime;
                anim.SetFloat("speed", 1f);
            }
            else
            {
                transform.position += transform.forward * speed/2 * Time.deltaTime;
                anim.SetFloat("speed", 2f);
            }
 
            
        }

        //стоим на месте
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) &&
            !Input.GetKey(KeyCode.D))
        {
            anim.SetFloat("speed", 0f);
        }

        //движение назад (скорость снижена вдвое)
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * speed / 4 * Time.deltaTime;
            anim.SetFloat("speed", -1f);
        }


        //движение влево
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
        {
            transform.Rotate(Vector3.down);
        }


        //движение вправо
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
        {
            transform.Rotate(-Vector3.down);
        }

        //eсли поворот на месте
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) &&
            (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)))
        {
            anim.SetFloat("speed", -0.3f);
        }

        //использование скилла лечения
        if (Input.GetKeyDown(KeyCode.Q) && !isPressedHeal)
        {
            isPressedHeal = true;
            UseSpell(currentHealSpell, 0);
        }

        //использования скила первой атаки
        if (Input.GetKeyDown(KeyCode.E) && !isPressedFirstDamage)
        {
            isPressedFirstDamage = true;
            UseSpell(currentFirstDamageSpell, 1);
        }

        //использование скила второй атаки
        if (Input.GetKeyDown(KeyCode.F) && !isPressedSecondDamage)
        {
            isPressedSecondDamage = true;
            UseSpell(currentSecondDamageSpell, 2);
        }

//        //прыжок
//        if (Input.GetKeyDown(KeyCode.Space) && countOfJumMax > countOfJump && !isPressedJump)
//        {
//            isPressedJump = true;
//            Jump();
//        }

        //если кнопка не нажата, сбрасываем булин
        if (!Input.GetKey(KeyCode.Space))
            isPressedJump = false;

        //если кнопка не нажата, сбрасываем булин
        if (!Input.GetKey(KeyCode.Q))
            isPressedHeal = false;

        //если кнопка не нажата, сбрасываем булин
        if (!Input.GetKey(KeyCode.E))
            isPressedFirstDamage = false;

        //если кнопка не нажата, сбрасываем булин
        if (!Input.GetKey(KeyCode.F))
            isPressedSecondDamage = false;
    }

    //ожидание завершения анимации
    private IEnumerator WaitHealAnimation()
    {
        yield return new WaitUntil(() => !partHeal.GetComponent<Animation>().isPlaying);
        partHeal.SetActive(false);
    }

    private IEnumerator TakeFireBall()
    {
        Vector3 placeOfBall = PartFireBall.transform.localPosition;
        PartFireBall.SetActive(true);
        GameObject destination = transform.GetComponentInChildren<Animator>().gameObject;
        while ( (destination.transform.localPosition -PartFireBall.transform.localPosition).magnitude > 0.1f)
        {
            PartFireBall.transform.localPosition = Vector3.MoveTowards(PartFireBall.transform.localPosition, destination.transform.localPosition, Time.deltaTime*Time.timeScale);
        }
        
        PartFireBall.SetActive(false);
        PartFireBall.transform.localPosition = placeOfBall;
        
        yield break;
    }

    private void OnCollisionEnter(Collision other)
    {
        //если игрок коснулся земли или воды, то сбрасываем прыжки 
        if (other.collider.gameObject.CompareTag("Ground") || other.collider.gameObject.CompareTag("Water"))
        {
            anim.SetBool("isJump", false);
            countOfJump = 0;
        }

        //если игрок упал в лаву, то он погибает
        if (other.gameObject.CompareTag("Lava"))
            TakeDamage(health);

        if (other.collider.gameObject.CompareTag("Water"))
            inWater = true;
    }

    private void OnCollisionExit(Collision other)
    {
        //если игрок вышел из воды, то возвращаем бег 
        if (other.collider.gameObject.CompareTag("Water"))
        {
            inWater = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !other.isTrigger)
        {
            //добавление врага в список врагов в триггере игрока, если он зашел туда
            enemyes.Add(other.gameObject.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //удаление врага из списка врагов в триггере игрока, если он вышел оттуда
        if (other.gameObject.CompareTag("Enemy") && !other.isTrigger)
        {
            enemyes.Remove(other.gameObject.GetComponent<Enemy>());
        }
    }

    //использование скилла
    protected override void UseSpell(Spell spell, int numberOfAttackSpell)
    {
        //проверяем тип способности
        switch (spell.Type)
        {
            //если это лечение, то применяем на игрока
            case "HEAL":
                //если способность перезаряжена, маны достаточно и здоровье не полное, то применяем
                if (currentCoolDownHeal <= 0 && mana >= spell.ManaValue && health < maxHealth)
                {
                    partHeal.SetActive(true);
                    StartCoroutine(WaitHealAnimation());
                    mana -= spell.ManaValue;
                    currentCoolDownHeal = spell.Cooldown;
                    health += spell.Value;
                    if (health > maxHealth)
                        health = maxHealth;
                }
                else
                {
                    _cantUseSkillEvent.Invoke();
                }

                break;

            //Если тип способности "атакующая по всем врагам"
            case "ALLDAMAGE":
                //если она сохранена на первый спелл игрока или на второй спелл  и достаточно маны, в радиусе поражения есть враги
                if ((numberOfAttackSpell == 1 && currentCoolDownFirstDamage <= 0 ||
                     numberOfAttackSpell == 2 && currentCoolDownSecondDamage <= 0) && mana >= spell.ManaValue &&
                    enemyes.Count > 0)
                {
                    //тратим ману
                    mana -= spell.ManaValue;

                    //список погибших врагов
                    List<Enemy> diedEnemy = new List<Enemy>();

                    //для всех врагов в триггере игрока
                    foreach (Enemy enemy in enemyes)
                    {
                        //наносим урон
                        enemy.TakeDamage(spell.Value);
                        
                        enemy.PlaySpell(spell, transform.position);
                        //если враг погиб, то запоминаем его, чтобы потом удалить из списка
                        if (enemy.Health <= 0)
                            diedEnemy.Add(enemy);
                    }

                    //если список погибших врагов не пуст, то удаляем из списка врагов в триггере игрока всех погибших
                    if (diedEnemy.Count != 0)
                        foreach (Enemy enemy in diedEnemy)
                        {
                            enemyes.Remove(enemy);
                        }


                    //отправляем на перезарядку использованный скилл
                    if (numberOfAttackSpell == 1)
                    {
                        currentCoolDownFirstDamage = spell.Cooldown;
                    }
                    else
                    {
                        currentCoolDownSecondDamage = spell.Cooldown;
                    }
                }
                else
                {
                    _cantUseSkillEvent.Invoke();
                }

                break;

            //Если тип способности "атакующая по одному врагу"  
            case "SINGLEDAMAGE":
                //если она сохранена на первый спелл игрока или на второй спелл  и достаточно маны, в радиусе поражения есть враги
                if ((numberOfAttackSpell == 1 && currentCoolDownFirstDamage <= 0 ||
                     numberOfAttackSpell == 2 && currentCoolDownSecondDamage <= 0) && mana >= spell.ManaValue &&
                    enemyes.Count > 0)
                {
                    //тратим ману
                    mana -= spell.ManaValue;

                    //наносим урон врагу
                    if (enemyes.Count == 1)
                    {
                        enemyes[0].TakeDamage(spell.Value);
                        
                        enemyes[0].PlaySpell(spell, transform.position);
                        if (enemyes[0].Health <= 0)
                            enemyes.Remove(enemyes[0]);
                    }
                    else
                    {
                        Enemy nearEnemy = FindNearlierEnemy();
                        if (nearEnemy != null)
                        {
                            nearEnemy.TakeDamage(spell.Value);
                            
                            nearEnemy.PlaySpell(spell, transform.position);
                            if (nearEnemy.Health <= 0)
                                enemyes.Remove(nearEnemy);
                        }
                    }

                    //отправляем на перезарядку использованный скилл
                    if (numberOfAttackSpell == 1)
                    {
                        currentCoolDownFirstDamage = spell.Cooldown;
                    }
                    else
                    {
                        currentCoolDownSecondDamage = spell.Cooldown;
                    }
                }
                else
                {
                    _cantUseSkillEvent.Invoke();
                }

                break;
        }
    }

    //найти ближайшего врага в триггере
    private Enemy FindNearlierEnemy()
    {
        float distance;
        Enemy nearlierEnemy = null;

        if (enemyes.Count != 0)
        {
            distance = Vector3.Distance(transform.position, enemyes[0].gameObject.transform.position);
            nearlierEnemy = enemyes[0];

            foreach (Enemy enemy in enemyes)
            {
                if (distance > Vector3.Distance(transform.position, enemy.gameObject.transform.position))
                {
                    distance = Vector3.Distance(transform.position, enemy.gameObject.transform.position);
                    nearlierEnemy = enemy;
                }
            }
        }

        return nearlierEnemy;
    }

    //прыжок
    protected override void Jump()
    {
        anim.SetBool("isJump", true);
        countOfJump++;
        _rigidbody.AddForce(transform.up * forceForJump, ForceMode.Impulse);
    }

    //смерть персонажа
    protected override void Death()
    {
        //обавляем в счетчик смерть
        Level.CountNewDeath();

        //если игрок погиб и имеет дополнительные жизни, то начинает с чекпоинта; 
        if (extraLives > 0)
        {
            extraLives--;
            transform.position = lastCheckpointPos;
            transform.rotation = lastCheckpointRot;
            health = maxHealth;
        }
        else
        {
            _deathEvent.Invoke();
        }
    }

    //сбросить параметры лисы к начальным
    public void RestartFox(Vector3 statrLevel, Quaternion startRot)
    {
        health = maxHealth;
        mana = maxMana;
        extraLives = 2;
        currentCoolDownHeal = 0;
        currentCoolDownFirstDamage = 0;
        currentCoolDownSecondDamage = 0;
        transform.position = statrLevel;
        transform.rotation = startRot;
        timerBonus = 0;
    }

    public float Health
    {
        get { return health; }
    }

    public float Mana
    {
        get { return mana; }
    }

    public int ExtraLives
    {
        get { return extraLives; }
    }

    //сохранить месторасположение последнего чекпоинта
    public void SaveLastCheckPoint(Vector3 position, Quaternion rot)
    {
        lastCheckpointPos = position;
        lastCheckpointRot = rot;
    }

    //добавить дополнительную жизнь
    public void AddExtraLive()
    {
        extraLives++;
    }

    //добавляем персонажу скорости, убираем двойной прыжок
    public void AddBonusSpeed(float value, float timer)
    {
        speed += value;
        if (timerBonus > 0)
        {
            countOfJumMax = 1;
        }

        timerBonus = timer;
    }

    //добавляем возможность двойного прыжка, убираем доп скорость
    public void AddBonusJump(float value, float timer)
    {
        countOfJumMax = (int) value;
        if (timerBonus > 0)
        {
            speed = defaultSpeed;
        }

        timerBonus = timer;
    }

    //получение урона игроком
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Death();
        }
    }

    public List<Spell> Spells
    {
        get { return spells; }
    }

    //установка способности на кнопку
    public void SetDamageSpell(int number, int id)
    {
        foreach (Spell spell in spells)
        {
            if (spell.Id == id)
            {
                if (number == 1)
                {
                    currentFirstDamageSpell = spell;
                }
                else
                {
                    currentSecondDamageSpell = spell;
                }

                break;
            }
        }
    }

    public float CurrentCoolDownHeal
    {
        get { return currentCoolDownHeal; }
    }

    public float CurrentCoolDownFirstDamage
    {
        get { return currentCoolDownFirstDamage; }
    }

    public float CurrentCoolDownSecondDamage
    {
        get { return currentCoolDownSecondDamage; }
    }

    public Spell CurrentHealSpell
    {
        get { return currentHealSpell; }
    }

    public Spell CurrentFirstDamageSpell
    {
        get { return currentFirstDamageSpell; }
    }

    public Spell CurrentSecondDamageSpell
    {
        get { return currentSecondDamageSpell; }
    }

    public float TimerBonus
    {
        get { return timerBonus; }
    }
}