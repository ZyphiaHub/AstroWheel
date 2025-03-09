using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour {
    public Inventory inventory; // A játékos inventoryja
    public List<Recipe> recipes; // Az összes recept

    // Egy recept elkészítése
    public void Craft(Recipe recipe)
    {
        // Ellenõrizzük, hogy a játékos rendelkezik-e a szükséges összetevõkkel
        foreach (var ingredient in recipe.requiredIngredients)
        {
            if (ingredient.plantItem != null)
            {
                // Növény ellenõrzése
                if (!inventory.items.ContainsKey(ingredient.plantItem) || inventory.items[ingredient.plantItem] < ingredient.quantity)
                {
                    Debug.LogWarning($"Nincs elég {ingredient.plantItem.englishName} a recept elkészítéséhez!");
                    return;
                }
            } else if (ingredient.craftedItem != null)
            {
                // Craftolt tárgy ellenõrzése
                if (!inventory.craftedItems.ContainsKey(ingredient.craftedItem) || inventory.craftedItems[ingredient.craftedItem] < ingredient.quantity)
                {
                    Debug.LogWarning($"Nincs elég {ingredient.craftedItem.craftedItemName} a recept elkészítéséhez!");
                    return;
                }
            }
        }

        // Eltávolítjuk a szükséges összetevõket az inventoryból
        foreach (var ingredient in recipe.requiredIngredients)
        {
            if (ingredient.plantItem != null)
            {
                inventory.RemoveItem(ingredient.plantItem, ingredient.quantity);
            } else if (ingredient.craftedItem != null)
            {
                inventory.RemoveCraftedItem(ingredient.craftedItem, ingredient.quantity);
            }
        }

        // Hozzáadjuk az új tárgyat az inventoryhoz
        inventory.AddCraftedItem(recipe.resultItem, recipe.resultQuantity);

        Debug.Log($"Sikeresen elkészítetted a(z) {recipe.recipeName} receptet!");
    }

    // Egy recept keresése a recept neve alapján
    public void CraftRecipeByName(string recipeName)
    {
        Recipe recipe = recipes.Find(r => r.recipeName == recipeName);
        if (recipe != null)
        {
            Craft(recipe);
        } else
        {
            Debug.LogWarning($"A(z) {recipeName} recept nem található!");
        }
    }
}