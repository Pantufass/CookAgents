using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : Counter
{
    
    public void Cook(Pan p)
    {
        if(p.soup.canBoil())
        {
            p.soup.Boiled();
        }
    }

}
