using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGift", menuName = "GiftData/Gift")]

public class OrderGiftScriptable : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
}
