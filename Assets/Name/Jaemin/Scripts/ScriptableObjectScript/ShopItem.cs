using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Shop/Item")]

public class ShopItem : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
    public int price;
}
