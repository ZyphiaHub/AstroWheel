using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftedItemDatabase", menuName = "ScriptableObjects/CraftedItemDatabase", order = 3)]
public class CraftedItemDatabase : ScriptableObject {
    public List<CraftedItem> craftedItems; // Az összes craftolt tárgy
}