﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject ground; 
    private List<Vector3> positions = new List<Vector3>();
    private GameObject hold;
    private Vector3 front;
    private BoxCollider2D col;

    private bool pickup = false;
    private bool use = false;
    private void Start()
    {
        front = this.gameObject.transform.position;

        ground = GameObject.Find("Ground");

        foreach(Transform t in ground.transform)
        {
            positions.Add(new Vector3(t.position.x, t.position.y, -1));
        }
        col = gameObject.AddComponent<BoxCollider2D>();
        col.offset = front;
        col.isTrigger = true;
        col.enabled = false;
    }

    void FixedUpdate()
    {
        List<bool> commands = getInput();
        int xValue = 0, yValue = 0;
        int up = 0;

        //move left
        if (commands[0]) xValue -= 1;
        //move right
        if (commands[1]) xValue += 1;
        //move up
        if (commands[2]) yValue += 1;
        //move down
        if (commands[3]) yValue -= 1;

        //rotate left
        if (commands[4]) up -= 1;
        //rotate right
        if (commands[5]) up += 1;

        //pick up
        if (commands[6]) pickUp();
        //use
        if (commands[7]) useAction();

        Vector3 a = transform.position + new Vector3(xValue,yValue);
        if (positions.Contains(a))
        {
            front += new Vector3(xValue, yValue);
            transform.position = a;
        }
        transform.Rotate(up*new Vector3(0,0,1),90f);
        front = Quaternion.AngleAxis(90, up * new Vector3(0, 0, 1)) * front;
    }

    private void pickUp()
    {
        col.enabled = true;
        pickup = true;
    }

    private void useAction()
    {
        col.enabled = true;
        use = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Counter c = collision.gameObject.GetComponent<Counter>();
        if (c != null)
        {
            if (use)
            {

            }
            else if (pickup)
            {

            }
        }
    }

    private List<bool> getInput()
    {
        List<bool> com = new List<bool>();
        com.Add(Input.GetKey(KeyCode.A));
        com.Add(Input.GetKey(KeyCode.D));
        com.Add(Input.GetKey(KeyCode.W));
        com.Add(Input.GetKey(KeyCode.S));
        com.Add(Input.GetKey(KeyCode.Q));
        com.Add(Input.GetKey(KeyCode.E));
        com.Add(Input.GetKey(KeyCode.X));
        com.Add(Input.GetKey(KeyCode.Z));
        return com;
    }
  
}
