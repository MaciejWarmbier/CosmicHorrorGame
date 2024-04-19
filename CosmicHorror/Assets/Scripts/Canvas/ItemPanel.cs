using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanel : MonoBehaviour
{
    public static ItemPanel ItemPanelInstance = null;

    [SerializeField] RectTransform itemContainer;
    [SerializeField] GameObject itemPrefab;

    private List<GameObject> spawnedItemsQueue = new();

    void Start()
    {
        ItemPanelInstance = this;
    }

    public void AddItem(Sprite sprite)
    {
        var item = Instantiate(itemPrefab, itemContainer);
        spawnedItemsQueue.Add(item);

        var image = item.GetComponent<Image>();
        image.sprite = sprite;
    }
}
