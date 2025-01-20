using SQLite4Unity3d;

public class Potion {
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Effect { get; set; }
    public int Rarity { get; set; }
    public float Price { get; set; }

    public override string ToString()
    {
        return $"Potion: {Name}, Effect: {Effect}, Rarity: {Rarity}, Price: {Price}";
    }
}

