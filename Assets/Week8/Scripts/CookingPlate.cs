using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CookingPlate : MonoBehaviour
{
    [Header("Recipes")]
    public List<RecipeSO> recipes;

    [Header("Spawn Point")]
    public Transform resultSpawnPoint;

    private List<IngredientType> currentIngredients = new();

    private void OnTriggerEnter(Collider other)
    {
        Ingredient ingredient = other.GetComponent<Ingredient>();
        if (ingredient == null) return;

        XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>();
        if (grab != null && grab.isSelected) return;

        AddIngredient(ingredient);
    }

    private void AddIngredient(Ingredient ingredient)
    {
        currentIngredients.Add(ingredient.type);

        Destroy(ingredient.gameObject);

        TryCook();
    }

    private void TryCook()
    {
        foreach (var recipe in recipes)
        {
            if (IsMatch(recipe))
            {
                Cook(recipe);
                return;
            }
        }
    }

    private bool IsMatch(RecipeSO recipe)
    {
        if (recipe.requiredIngredients.Count != currentIngredients.Count)
            return false;

        List<IngredientType> temp = new(currentIngredients);

        foreach (var req in recipe.requiredIngredients)
        {
            if (!temp.Remove(req))
                return false;
        }

        return true;
    }

    private void Cook(RecipeSO recipe)
    {
        Instantiate(recipe.resultPrefab, resultSpawnPoint.position, Quaternion.identity);
        currentIngredients.Clear();
    }
}
