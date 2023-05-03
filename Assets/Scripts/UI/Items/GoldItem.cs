using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldItem : Item
{
    public override string GiveName()
    {
        return "Gold";
    }
    public override int MaxStacks()
    {
        return 2;
    }
    public override int GiveScale()
    {
        return 1;
    }
    public override Sprite GiveItemImage()
    {
        return Resources.Load<Sprite>("UI/Item Images/Gold");
    }
    public override Mesh GiveItemMesh()
    {
        return Resources.Load<Mesh>("Pickup Items/Gold/GoldMesh");
    }
    public override Material GiveItemMat()
    {
        return Resources.Load<Material>("Pickup Items/Gold/GoldMat");
    }
}
