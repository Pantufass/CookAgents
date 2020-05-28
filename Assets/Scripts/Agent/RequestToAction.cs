using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestToAction
{
    Dictionary<Plate.State, List<string>> conversion = new Dictionary<Plate.State, List<string>>();

    public RequestToAction()
    {
        conversion.Add(Plate.State.onSoup, new List<string>(new string[] { "deliver","getPlate", "replacePanInStove", "deliverSoupInPlate_Onion", "getSoupFromPan_Onion","boilSoup_Onion", "deliverCutedInSoup_Onion", "getCuted_Onion", "cut_Onion","deliverUncuted_Onion", "get_Onion" }));
        conversion.Add(Plate.State.tomato, new List<string>(new string[] { "deliver", "getPlate", "deliverCutedInPlate_Tomato", "getCuted_Tomato", "cut_Tomato", "deliverUncuted_Tomato", "get_Tomato" }));
        conversion.Add(Plate.State.tomSoup, new List<string>(new string[] { "deliver", "getPlate", "replacePanInStove", "deliverSoupInPlateTomato", "getSoupFromPan_Tomato", "boilSoup_Tomato", "deliverCutedInSoup_Tomato", "getCuted_Tomato", "cut_Tomato", "deliverUncuted_Tomato", "get_Tomato" }));
        conversion.Add(Plate.State.salad, new List<string>(new string[] { "deliver", "getPlate", "deliverCutedInPlate_Lettuce", "getCuted_Lettuce", "cut_Lettuce", "deliverUncuted_Lettuce", "get_Lettuce", "deliverCutedInPlate_Tomato", "getCuted_Tomato", "cut_Tomato", "deliverUncuted_Tomato", "get_Tomato" }));
    }

    public List<string> GetConversion(Plate.State state)
    {
        return conversion[state];
    }
}
