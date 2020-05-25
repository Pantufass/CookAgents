using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public int speed = 1;

    void Start()
    {
        Application.targetFrameRate = speed;
        Physics2D.gravity = Vector2.zero;
    }

}
