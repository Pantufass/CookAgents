using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{

    private PlayerController controller;

    private PlayerMap map;
    // Start is called before the first frame update
    void Start()
    {
        controller = new PlayerController(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        perceive();
        decide();
        act();
    }

    private void perceive()
    {


    }

    private void decide()
    {

    }

    private void act()
    {

    }
}
