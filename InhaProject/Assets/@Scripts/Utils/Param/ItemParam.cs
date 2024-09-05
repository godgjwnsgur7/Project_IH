using System;

public class ItemParam 
{
    public EItemType type;

    //ItemParam(EItemType type)
    //{
    //    this.type = type;
    //}
}

public class ConsumeItem : ItemParam { }

public class PotionItem : ConsumeItem { }


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