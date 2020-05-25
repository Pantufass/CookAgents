using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    private void Awake()
    {
        current = this;
    }

    public delegate void callRecipe(Plate.State s);
    public delegate void callRequest(Requirement r);

    public event callRecipe OnRecipeEnter;

    public event callRequest OnRequestEnter;

    public void RecipeEnter(Plate.State r)
    {
        if(OnRecipeEnter != null)
        {
            OnRecipeEnter(r);
        }
    }


    public void RequestEnter(Requirement r)
    {
        if(OnRequestEnter != null)
        {
            OnRequestEnter(r);
        }
    }
}
