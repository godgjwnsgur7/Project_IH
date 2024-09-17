using System.Collections.Generic;
using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class JItemSlotData
    {
        public string ItemType;
        public string Name;
        public string Script;
    }

    public class ItemSlotDataLoader : ILoader<string, JItemSlotData>
    {
        public List<JItemSlotData> items = new List<JItemSlotData>();

        public Dictionary<string, JItemSlotData> MakeDict()
        {
            Dictionary<string, JItemSlotData> itemSlots = new Dictionary<string, JItemSlotData>();
            foreach (JItemSlotData item in items)
            {
                itemSlots.Add(item.ItemType, item);
            }
            return itemSlots;
        }
    }
}
