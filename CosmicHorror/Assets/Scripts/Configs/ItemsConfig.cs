using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemsConfig", menuName = "ScriptableObjects/ItemsConfig", order = 1)]
public class ItemsConfig : ScriptableObject
{
    public List<ItemData> itemData;

    public GameObject GetItem(ItemsEnum item)
    {
        return itemData.FirstOrDefault(x => x.itemEnum == item).gameObject;
    }

    [Serializable]
    public class ItemData
    {
        public ItemsEnum itemEnum;
        public GameObject gameObject;
    }

    [Serializable]
    public class RandomItemData
    {
        public ItemsEnum itemEnum;
        public int chanceMin;
        public int chanceMax;
    }

    public enum ItemsEnum
    {
        None =1,
        Pills =2,
        Bandages =3,
        Bullet = 4,
        Apple =5,
        Adrenaline = 6,
        Morphine =7
    }
}
