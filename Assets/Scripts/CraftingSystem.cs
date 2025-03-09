using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour {
    public Inventory inventory; // A j�t�kos inventoryja
    public List<Recipe> recipes; // Az �sszes recept

    // Egy recept elk�sz�t�se
    public void Craft(Recipe recipe)
    {
        // Ellen�rizz�k, hogy a j�t�kos rendelkezik-e a sz�ks�ges �sszetev�kkel
        foreach (var ingredient in recipe.requiredIngredients)
        {
            if (ingredient.plantItem != null)
            {
                // N�v�ny ellen�rz�se
                if (!inventory.items.ContainsKey(ingredient.plantItem) || inventory.items[ingredient.plantItem] < ingredient.quantity)
                {
                    Debug.LogWarning($"Nincs el�g {ingredient.plantItem.englishName} a recept elk�sz�t�s�hez!");
                    return;
                }
            } else if (ingredient.craftedItem != null)
            {
                // Craftolt t�rgy ellen�rz�se
                if (!inventory.craftedItems.ContainsKey(ingredient.craftedItem) || inventory.craftedItems[ingredient.craftedItem] < ingredient.quantity)
                {
                    Debug.LogWarning($"Nincs el�g {ingredient.craftedItem.craftedItemName} a recept elk�sz�t�s�hez!");
                    return;
                }
            }
        }

        // Elt�vol�tjuk a sz�ks�ges �sszetev�ket az inventoryb�l
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

        // Hozz�adjuk az �j t�rgyat az inventoryhoz
        inventory.AddCraftedItem(recipe.resultItem, recipe.resultQuantity);

        Debug.Log($"Sikeresen elk�sz�tetted a(z) {recipe.recipeName} receptet!");
    }

    // Egy recept keres�se a recept neve alapj�n
    public void CraftRecipeByName(string recipeName)
    {
        Recipe recipe = recipes.Find(r => r.recipeName == recipeName);
        if (recipe != null)
        {
            Craft(recipe);
        } else
        {
            Debug.LogWarning($"A(z) {recipeName} recept nem tal�lhat�!");
        }
    }
}