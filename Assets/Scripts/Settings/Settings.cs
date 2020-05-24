using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public int speed = 2;

    void Start()
    {
        Time.fixedDeltaTime = 0.6f/speed;
        Physics2D.gravity = Vector2.zero;
    }

}
