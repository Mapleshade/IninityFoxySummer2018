using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replica
{
    private int ID;
    
    private string whoSaid;

    private string whatSay;

    public string WhoSaid
    {
        get { return whoSaid; }
    }

    public string WhatSay
    {
        get { return whatSay; }
    }

    public int Id
    {
        get { return ID; }
    }

    public Replica(int ID, string whoSaid, string whatSay)
    {
        this.ID = ID;
        this.whoSaid = whoSaid;
        this.whatSay = whatSay;
    }
}