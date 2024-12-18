using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Item/ItemSO")]
public class ItemSO : ScriptableObject
{
    public ItemName itemName;
    public ItemType itemType;
    public float durationTime; // 아이템 효과 지속시간
    public float rotationNum;
    public float speed;
}
