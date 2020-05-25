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
    public delegate void callRequest(Requirement r, int layer);

    public delegate void pickUp(int id);


    public event callRecipe OnRecipeEnter;

    public event callRequest OnRequestEnter;

    public event pickUp OnPickUp;

    public void RecipeEnter(Plate.State r)
    {
        if(OnRecipeEnter != null)
        {
            OnRecipeEnter(r);
        }
    }


    public void RequestEnter(Requirement r, int l)
    {
        if(OnRequestEnter != null)
        {
            OnRequestEnter(r,l);
        }
    }

    public void PickUp(int id)
    {
        if(OnPickUp != null)
        {
            OnPickUp(id);
        }
    }
}
