using UnityEngine;

[CreateAssetMenu(fileName = "PlantDatabase", menuName = "ScriptableObjects/PlantDatabase", order = 1)]
public class PlantDatabase : ScriptableObject {
    [System.Serializable]
    public class Item {
        public string englishName; 
        public string witchName;
        public string latinName;
        public string description;
        public Sprite icon;     
    }

    public Item[] items; // Tárgyak listája
}