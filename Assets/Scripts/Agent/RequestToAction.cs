using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestToAction
{
    Dictionary<Plate.State, List<string>> conversion = new Dictionary<Plate.State, List<string>>();

    public RequestToAction()
    {
        conversion.Add(Plate.State.onSoup, new List<string>(new string[] { "deliverOnionSoup", "getOnionSoup", "cutOnion", "getOnion" })); 
    }

    public List<string> GetConversion(Plate.State state)
    {
        return conversion[state];
    }
}
