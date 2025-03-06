using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/ItemDatabase", order = 1)]
public class ItemDatabase : ScriptableObject {
    [System.Serializable]
    public class Item {
        public string englishName; // Tárgy neve
        public string witchName;
        public string latinName;
        public string description;
        public Sprite icon;     // Tárgy ikonja (opcionális)
    }

    public Item[] items; // Tárgyak listája
}