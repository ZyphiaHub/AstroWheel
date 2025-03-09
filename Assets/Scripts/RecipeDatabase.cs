using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeBook", menuName = "ScriptableObjects/RecipeBook", order = 2)]
public class RecipeBook : ScriptableObject {
    
   
    public List<Recipe> recipes; // Az összes recept
}