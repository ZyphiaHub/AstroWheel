using UnityEditor;
using UnityEngine;

public class ItemInitializer : MonoBehaviour {
    public ItemDatabase itemDatabase;

    void Start()
    {
        //adatok hozz�ad�sa
        itemDatabase.items = new ItemDatabase.Item[]
        {
            new ItemDatabase.Item
            {
                englishName = "Jade Orchid",
                witchName = "Squirrel's ear",
                latinName = "Goodyera repens",
                description = "Material for  Crystals.",
                icon = LoadSprite("Assets/Art/Matik/Avarvir�g.png")
            },
            new ItemDatabase.Item
            {
                englishName = "Velvet Bean",
                witchName = "Donkey's eye",
                latinName = "Mucuna pruriens",
                description = "Herbal drug used for the management of male infertility, nervous disorders, and also as an aphrodisiac.",
                icon = LoadSprite("Assets/Art/Matik/Beng�li b�rsonybab.png")
            },
            
            
            new ItemDatabase.Item
            {
                englishName = "Horseweed",
                witchName = "Colt's tail",
                latinName = "Erigeron canadensis",
                description = "Treat for sore throat and dysentery.",
                icon = LoadSprite("Assets/Art/Matik/Bety�rk�r�.png")
            },
            new ItemDatabase.Item
            {
                englishName = "Starflower Pincushions",
                witchName = "Cat's eye",
                latinName = "Scabiosa stellata",
                description = "It is known in ancient times to treat scurvy.",
                icon = LoadSprite("Assets/Art/Matik/Csillag�rd�gszem.png")
            },
            new ItemDatabase.Item
            {
                englishName = "Spotted Cranesbill",
                witchName = "Crowfoot",
                latinName = "Geranium maculatum",
                description = "Egy er�teljes krist�ly, amely var�zslatokban haszn�lhat�.",
                icon = LoadSprite("Assets/Art/Matik/Foltos g�lyaorr.png")
            },
            
            new ItemDatabase.Item
            {
                englishName = "Bulbous Buttercup",
                witchName = "Frog leg",
                latinName = "Ranunculus bulbosus",
                description = "Used for skin diseases, arthritis, nerve pain, flu. Can cause irritation.",
                icon = LoadSprite("Assets/Art/Matik/Hagym�s bogl�rka.png")
            },
            new ItemDatabase.Item
            {
                englishName = "Canadian Snakeroot",
                witchName = "Cat's paw",
                latinName = "Asarum canadense",
                description = "Used for bronchitis, bronchial spasms, and bronchial asthma.",
                icon = LoadSprite("Assets/Art/Matik/Kanadai kapotnyak.png")
            },
            
            new ItemDatabase.Item
            {
                englishName = "White Turtlehead",
                witchName = "Snake's head",
                latinName = "Chelone glabra",
                description = "It has been used as a method of birth control.",
                icon = LoadSprite("Assets/Art/Matik/Kopasz gerlefej.png")
            },
            new ItemDatabase.Item
            {
                englishName = "Crystal",
                witchName = "Var�zskrist�ly",
                latinName = "Crystallum Magica",
                description = "Egy er�teljes krist�ly, amely var�zslatokban haszn�lhat�.",
                icon = LoadSprite("Assets/Art/Matik/crystal_icon.png")
            },
            new ItemDatabase.Item
            {
                englishName = "Crystal",
                witchName = "Var�zskrist�ly",
                latinName = "Crystallum Magica",
                description = "Egy er�teljes krist�ly, amely var�zslatokban haszn�lhat�.",
                icon = LoadSprite("Assets/Art/Matik/crystal_icon.png")
            },
            new ItemDatabase.Item
            {
                englishName = "Narrowleaf Plantain",
                witchName = "Lamb's tongue",
                latinName = "Plantago lanceolata",
                description = "Effective treatment for bleeding, it quickly staunches blood flow and encourages the repair of damaged tissue.",
                icon = LoadSprite("Assets/Art/Matik/L�ndzs�s utif�.png")
            },
            new ItemDatabase.Item
            {
                englishName = "Crystal",
                witchName = "Var�zskrist�ly",
                latinName = "Crystallum Magica",
                description = "Egy er�teljes krist�ly, amely var�zslatokban haszn�lhat�.",
                icon = LoadSprite("Assets/Art/Matik/crystal_icon.png")
            },
            new ItemDatabase.Item
            {
                englishName = "Crystal",
                witchName = "Var�zskrist�ly",
                latinName = "Crystallum Magica",
                description = "Egy er�teljes krist�ly, amely var�zslatokban haszn�lhat�.",
                icon = LoadSprite("Assets/Art/Matik/crystal_icon.png")
            },
            new ItemDatabase.Item
            {
                englishName = "Crystal",
                witchName = "Var�zskrist�ly",
                latinName = "Crystallum Magica",
                description = "Egy er�teljes krist�ly, amely var�zslatokban haszn�lhat�.",
                icon = LoadSprite("Assets/Art/Matik/crystal_icon.png")
            },
            new ItemDatabase.Item
            {
                englishName = "Crystal",
                witchName = "Var�zskrist�ly",
                latinName = "Crystallum Magica",
                description = "Egy er�teljes krist�ly, amely var�zslatokban haszn�lhat�.",
                icon = LoadSprite("Assets/Art/Matik/crystal_icon.png")
            },
            new ItemDatabase.Item
            {
                englishName = "Crystal",
                witchName = "Var�zskrist�ly",
                latinName = "Crystallum Magica",
                description = "Egy er�teljes krist�ly, amely var�zslatokban haszn�lhat�.",
                icon = LoadSprite("Assets/Art/Matik/crystal_icon.png")
            }

            
        };

        // Opcion�lisan mentheted az adatb�zist
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(itemDatabase);
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }
    private Sprite LoadSprite(string path)
    {
#if UNITY_EDITOR
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
#else
        return null;
#endif
    }
}