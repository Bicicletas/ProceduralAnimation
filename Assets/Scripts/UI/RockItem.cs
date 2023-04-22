using UnityEngine;

public class RockItem : Item
{
    public override string GiveName()
    {
        return "Rock";
    }
    public override int MaxStacks()
    {
        return 64;
    }
    public override Sprite GiveItemImage()
    {
        return Resources.Load<Sprite>("UI/Item Images/Rock");
    }
}
