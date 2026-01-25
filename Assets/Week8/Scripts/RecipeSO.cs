using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cooking/Recipe")]
public class RecipeSO : ScriptableObject
{
    public string recipeName;

    [Header("Required Ingredients")]
    public List<IngredientType> requiredIngredients;

    [Header("Result")]
    public GameObject resultPrefab;
}
