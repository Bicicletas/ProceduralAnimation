using UnityEngine;

public class CrystalItem : Item
{
    public override string GiveName()
    {
        return "Crystal";
    }
    public override int MaxStacks()
    {
        return 5;
    }
    public override int GiveScale()
    {
        return 1;
    }
    public override Sprite GiveItemImage()
    {
        return Resources.Load<Sprite>("UI/Item Images/Crystal");
    }
    public override Mesh GiveItemMesh()
    {
        return Resources.Load<Mesh>("Pickup Items/Crystal/CrystalMesh");
    }
    public override Material GiveItemMat()
    {
        return Resources.Load<Material>("Pickup Items/Crystal/CrystalMat");
    }
}
