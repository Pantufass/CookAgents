using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodRequests : MonoBehaviour
{
    public List<Plate.State> requests;
    void Start()
    {
        requests = new List<Plate.State>();
        addForNow();
    }


    private void addForNow()
    {
        requests.Add(Plate.State.onSoup);
        requests.Add(Plate.State.tomSoup);
        requests.Add(Plate.State.salad);
        requests.Add(Plate.State.salad);
        requests.Add(Plate.State.onSoup);
        requests.Add(Plate.State.salad);
        requests.Add(Plate.State.tomSoup);
        requests.Add(Plate.State.onSoup);
        requests.Add(Plate.State.tomSoup);
        requests.Add(Plate.State.tomSoup);
    }

    public bool clearRequest(Plate.State plate)
    {
        foreach(Plate.State s in requests)
        {
            if(plate == s)
            {
                requests.Remove(s);
                return true;
            }
        }
        return false;
    }
}
