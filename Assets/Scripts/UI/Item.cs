using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public abstract string GiveName();
    public virtual int MaxStacks()
    {
        return 30;
    }
    public virtual float GiveScale()
    {
        return 50;
    }
    public virtual Sprite GiveItemImage()
    {
        return Resources.Load<Sprite>("UI/Item Images/No Item Image Icon");
    }
    public virtual GameObject DropObject()
    {
        return Resources.Load<GameObject>("Pickup Items/Default Item");
    }
    public virtual Mesh GiveItemMesh()
    {
        return Resources.Load<Mesh>("Pickup Items/Rock/RockMesh");
    }
    public virtual Material GiveItemMat()
    {
        return Resources.Load<Material>("Pickup Items/Rock/RockMat");
    }
}
