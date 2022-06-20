using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public enum Kind { Spade, Diamond, Cloba, Heart};
    public Kind kind;
    public Sprite sprite;
    public int cardNum;

}

[CreateAssetMenu(fileName = "CardDataSO", menuName = "Scriptable Object/CardDataSO")]
public class ItemSO : ScriptableObject
{
    public Item[] items;
}