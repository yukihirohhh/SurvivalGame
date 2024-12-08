using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public RecipieTemplate recipieTemplate;

    public CraftingRecipieSO[] recipies;
    public Transform contentHolder;

    private void Start()
    {
        GenerateRecipies();
    }

    public void GenerateRecipies()
    {
        for(int i = 0; i < recipies.Length; i++)
        {
            RecipieTemplate recipie = Instantiate(recipieTemplate.gameObject, contentHolder).GetComponent<RecipieTemplate>();

            recipie.icon.sprite = recipies[i].icon;
            recipie.nameText.text = recipies[i].recipieName;

            for(int b = 0; b < recipies[i].requirements.Length; b++)
            {
                if (b == 0)
                    recipie.requirementText.text = $"{recipies[i].requirements[b].data.itemName} {recipies[i].requirements[b].amountNeeded}";
                else
                    recipie.requirementText.text = $"{recipie.requirementText.text}, {recipies[i].requirements[b].data.itemName} {recipies[i].requirements[b].amountNeeded}";
            }
        }
    }
}
