using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell
{
    //идентификатор 
    private int ID;

    //тип
    private string type;

    //время перезарядки
    private float cooldown;

    //значение
    private float value;

    //сколько требуется маны
    private float manaValue;

    //название магии
    private string nameOfSpell;

    //маленькая иконка
    private Sprite smallSprite;

    //большая иконка
    private Sprite largeSprite;

    //название, выводимое в игре
    private string nameForGame;

    //конструктор
    public Spell(int id, string type, float cooldown, float value, float manaValue, string nameOfSpell,
        string nameForGame)
    {
        this.ID = id;
        this.type = type;
        this.cooldown = cooldown;
        this.value = value;
        this.manaValue = manaValue;
        this.nameOfSpell = nameOfSpell;
        this.nameForGame = nameForGame;
    }


    public float ManaValue
    {
        get { return manaValue; }
    }

    public string Type
    {
        get { return type; }
    }

    public float Cooldown
    {
        get { return cooldown; }
    }

    public float Value
    {
        get { return value; }
    }

    public string NameOfSpell
    {
        get { return nameOfSpell; }
    }

    public int Id
    {
        get { return ID; }
    }

    public Sprite SmallSprite
    {
        get { return smallSprite; }
        set { smallSprite = value; }
    }

    public Sprite LargeSprite
    {
        get { return largeSprite; }
        set { largeSprite = value; }
    }

    public string NameForGame
    {
        get { return nameForGame; }
    }
}