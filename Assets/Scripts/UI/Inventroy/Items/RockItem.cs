using UnityEngine;

public class RockItem : Item
{
    public override string GiveName()
    {
        return "Rock";
    }
    public override int MaxStacks()
    {
        return 5;
    }
    public override float GiveScale()
    {
        return .5f;
    }
    public override Sprite GiveItemImage()
    {
        return Resources.Load<Sprite>("UI/Item Images/Rock");
    }
    public override Mesh GiveItemMesh()
    {
        return Resources.Load<Mesh>("Pickup Items/Rock/RockMesh");
    }
    public override Material GiveItemMat()
    {
        return Resources.Load<Material>("Pickup Items/Rock/RockMat");
    }
}
