using System;

public class ItemParam
{
    public EItemType type;

    //ItemParam()
    //{
    //    type = EItemType.None;
    //}
    //ItemParam(EItemType type)
    //{
    //    this.type = type;
    //}
}

public class ApplyItemParam : ItemParam
{
    public bool IsHp = false;
    public float Heal = 1.0f;

    public ApplyItemParam(bool isHp, float heal)
    {
        IsHp = isHp;
        this.Heal = heal;
    }
}

public class PotionItemParam : ItemParam
{
    public bool IsHp = false;
    public float Heal = 1.0f;

    public PotionItemParam(bool isHp, float heal)
    {
        IsHp = isHp;
        this.Heal = heal;
    }
}


public enum EOtherItemType
{
    None,
}

public class OtherItem : ItemParam
{
    EOtherItemType itemType;

    public OtherItem(EOtherItemType itemType)
    {
        this.itemType = itemType;
    }
}