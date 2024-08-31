using System;

public class ItemParam { }

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