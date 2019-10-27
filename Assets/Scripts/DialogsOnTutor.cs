using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public static class DialogsOnTutor
{
    private static Queue<Replica> _queueDialogs;

    public static void GenerateDialogs()
    {
        _queueDialogs = new Queue<Replica>();
        _queueDialogs.Enqueue(new Replica(0, "CAT", "ZZzzZz..."));
        _queueDialogs.Enqueue(new Replica(1, "CAT", "...?!"));
        _queueDialogs.Enqueue(new Replica(2, "CAT", "Я надеюсь, это разбилась не Хрустальная Косточка Могучих Псов?"));
        _queueDialogs.Enqueue(new Replica(3, "FOX",
            "Это такая прозрачная фигня, похожая на кость? Да не, не, ни в коем случае..."));
        _queueDialogs.Enqueue(new Replica(4, "CAT",
            "Что же ты наделал, юноша! Теперь духи, выпущенные тобой, угрожают всему живому!"));
        _queueDialogs.Enqueue(new Replica(5, "FOX",
            "Господин Архивариус, я не хотел! Могу я как-то исправить свою ошибку?"));
        _queueDialogs.Enqueue(new Replica(6, "CAT",
            "Я уже слишком стар и не могу покинуть свою картину, чтобы тебе помочь, поэтому ты должен сделать всё сам. Беги по коридору дальше, там будет артефакт, способный повернуть время вспять..."));
        _queueDialogs.Enqueue(new Replica(7, "CAT", ""));
        _queueDialogs.Enqueue(new Replica(8, "FOX",
            "Заклинания на кошачьем? Я учил кошачий целых два месяца... Так что тут... О! Вот тут написано \"Частички\"! Оно, должно быть собирает в единое целое то, что разбито!"));
        _queueDialogs.Enqueue(new Replica(9, "FOX", "Мяу... Мурмяу... МУРМУР... Миу... МЯВК!"));
        _queueDialogs.Enqueue(new Replica(10, "CAT", "СТООООООООООООООООЙ!!"));
        _queueDialogs.Enqueue(new Replica(11, "CAT",
            "О, нет! Зачем ты прочитал Заклинание из Книги Разрушений! Я говорил о Лапке-Перчатке за стеклянной стеной! Ты рассыпал в пыль половину населения Вселенной!"));
        _queueDialogs.Enqueue(new Replica(12, "CAT",
            "Так, спокойно... Есть ещё шанс всё исправить. Отправляйся к стеклянной стене, собери все Коготки Бесконечности и отправляйся в прошлое, чтобы спасти Вселенную. "));
        _queueDialogs.Enqueue(new Replica(13, "CAT", "Беги, не накосячь только ещё больше :D"));
        _queueDialogs.Enqueue(new Replica(14, "CAT", ""));
        _queueDialogs.Enqueue(new Replica(15, "CAT",
            "Это Лапка-Перчатка Бесконечности, она может повернуть время вспять. Но Коготки может получить только достойный. " +
            "Тебе нужно пройти в порталы за Коготками, и если справишься с испытаниями, которые они устроят, то сможешь собрать Лапку-Перчатку."));        
        _queueDialogs.Enqueue(new Replica(16, "CAT", ""));
    }

    public static Replica GetNextReplica()
    {
        return _queueDialogs.Dequeue();
    }
}