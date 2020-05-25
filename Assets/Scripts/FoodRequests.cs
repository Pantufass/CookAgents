using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodRequests : MonoBehaviour
{
    public List<Plate.State> requests;
    private System.Random r;


    public Sprite tomato;
    public Sprite letuce;
    public Sprite Tsoup;
    public Sprite Osoup;
    public Sprite fullSalad;

    private ActiveRequests ar;

    void Start()
    {
        requests = new List<Plate.State>();
        //addForNow();
        r = new System.Random();
        ar = FindObjectOfType<ActiveRequests>();
    }


    private void addForNow()
    {
        addRecipe(Plate.State.onSoup);
        addRecipe(Plate.State.salad);
        addRecipe(Plate.State.tomSoup);
        addRecipe(Plate.State.lettuce);
        addRecipe(Plate.State.tomSoup);
    }

    private void LateUpdate()
    {
        if(requests.Count < 5)
        {
            createRequest();
        }
    }

    private void createRequest()
    {
        System.Array values = System.Enum.GetValues(typeof(Plate.State));
        Plate.State random = (Plate.State)values.GetValue(r.Next(values.Length));
        if (random != Plate.State.empty)
        {
            addRecipe(random);
        }
        else
        {
            createRequest();
        }
    }

    private void addRecipe(Plate.State r)
    {
        requests.Add(r);
        ar.changeThem(); 
        //call event, new recipe
        GameEvents.current.RecipeEnter(r);
    }

    public Sprite getRecipe(int i)
    {
        if(i < requests.Count)
        switch (requests[i])
        {
            case Plate.State.lettuce:
                return letuce;
            case Plate.State.tomato:
                return tomato;
            case Plate.State.tomSoup:
                return Tsoup;
            case Plate.State.onSoup:
                return Osoup;
            case Plate.State.salad:
                return fullSalad;
        }
        return null;
    }

    public bool clearRequest(Plate.State plate)
    {
        foreach(Plate.State s in requests)
        {
            if(plate == s)
            {
                requests.Remove(s);
                ar.changeThem();
                return true;
            }
        }
        return false;
    }

    public Plate.State lastRequest()
    {
        return requests[0];
    }

}
