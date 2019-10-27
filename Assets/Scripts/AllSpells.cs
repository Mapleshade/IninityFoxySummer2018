using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

public static class AllSpells
{
    //список всех доступных спеллов в игре
    private static List<Spell> spells;

    //заполнение списка спеллов и нахождение их картинок
    public static void GenerateSpells(SpriteAtlas atlas)
    {
        if (spells == null)
        {
            spells = new List<Spell>();
            spells.Add(new Spell(0, "HEAL", 1f, 20f, 10f, "heal", "Лечение"));
            spells.Add(new Spell(1, "ALLDAMAGE", 4f, 20f, 20f, "RainOfFire", "Огненный дождь"));
            spells.Add(new Spell(2, "SINGLEDAMAGE", 3f, 15f, 15f, "FireArrow", "Огненная стрела"));
            spells.Add(new Spell(3, "SINGLEDAMAGE", 3f, 20f, 18f, "IceSpike", "Ледяной шип"));
            spells.Add(new Spell(4, "ALLDAMAGE", 3f, 25f, 20f, "Lightning", "Молнии"));

            GetImages(atlas);
        }
    }

    //начальные навыки игрока
    public static List<Spell> StarterSpellsForFox()
    {
        List<Spell> sp = new List<Spell>();
        sp.Add(spells[0]);
        sp.Add(spells[1]);
        sp.Add(spells[2]);
        return sp;
    }

    //нахождение картинок спеллов
    private static void GetImages(SpriteAtlas atlas)
    {
        foreach (Spell spell in spells)
        {
            spell.LargeSprite = atlas.GetSprite(spell.NameOfSpell);
            spell.SmallSprite = atlas.GetSprite(spell.NameOfSpell + "_small");
        }
    }

    //получить спелл по айдишнику
    public static Spell GetSpellById(int id)
    {
        return spells[id];
    }

    //выдать все доступные игроку спеллы 
    public static List<Spell> GetSpells()
    {
        //находим айдишник последнего доступного скилла для игрока
        int idOfLastSpell = PlayerPrefs.GetInt("IDOfLastSpell");
        
        //создаем список способностей
        List<Spell> sp = new List<Spell>();

        //ищем все сохраненные  спеллы
        foreach (Spell spell in spells)
        {
            if (spell.Id <= idOfLastSpell)
                sp.Add(spell);
        }

        return sp;
    }

    public static Spell GetRandomSpellForEnemy()
    {
        int id = Random.Range(1, spells.Count);
        return spells[id];
    }
}