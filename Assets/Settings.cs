using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public int speed = 5;

    void Start()
    {
        Application.targetFrameRate = speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
