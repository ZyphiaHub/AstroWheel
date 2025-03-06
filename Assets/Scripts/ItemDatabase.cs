using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/ItemDatabase", order = 1)]
public class ItemDatabase : ScriptableObject {
    [System.Serializable]
    public class Item {
        public string englishName; // T�rgy neve
        public string witchName;
        public string latinName;
        public string description;
        public Sprite icon;     // T�rgy ikonja (opcion�lis)
    }

    public Item[] items; // T�rgyak list�ja
}