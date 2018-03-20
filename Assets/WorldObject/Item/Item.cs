using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class Item : WorldObject
{ 
    //Public variables
    public float capacity;

    //Variables accessible by subclass
    protected float amountLeft;
    protected ItemType itemType;

    /*** Game Engine methods, all can be overridden by subclass ***/

    protected override void Start()
    {
        base.Start();
        amountLeft = capacity;
        itemType = ItemType.Unknown;
    }

    /*** Public methods ***/

    public void Remove(float amount)
    {
        amountLeft -= amount;
        if (amountLeft < 0) amountLeft = 0;
    }

    public bool isEmpty()
    {
        return amountLeft <= 0;
    }

    public ItemType GetItemType()
    {
        return itemType;
    }
}
