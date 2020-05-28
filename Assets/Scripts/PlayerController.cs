﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject ground;

    private Agent agent;

    private List<Vector3> positions;

    private Item hold;
    public Vector3 front;
    private BoxCollider2D col;

    private bool pan = false;
    private bool holding = false;
    private bool triggered = false;

    public Counter c = null;

    private FoodRequests fr;

    private PlayerController[] players;

    //able/disable agent control
    private bool agentControl = true;

    int xValue = 0;
    int yValue = 0;
    int up = 0;
    public PlayerController(Agent agent)
    {
        this.agent = agent;
    }

    public void addAgent(Agent agent)
    {
        this.agent = agent;
    }

    private void Start()
    {
        front = new Vector3(0,1,0);

        positions = new List<Vector3>();

        ground = GameObject.FindGameObjectWithTag("Ground");

        foreach(Transform t in ground.transform)
        {
            positions.Add(new Vector3(t.position.x, t.position.y, -1));
        }
        col = gameObject.AddComponent<BoxCollider2D>();
        col.offset = front;
        //col.isTrigger = true;
        col.enabled = true;
        col.size = new Vector2(0.8f, 0.8f);

        fr = FindObjectOfType<FoodRequests>();

        players = FindObjectsOfType<PlayerController>();
    }

    void LateUpdate()
    {
        List<bool> commands = getInput();
        if(!agentControl) cycle(commands);
        
    }

    public void cycle(List<bool> commands)
    {
        xValue = 0;
        yValue = 0;
        up = 0;


        //move left
        if (commands[0]) xValue -= 1;
        //move right
        if (commands[1]) xValue += 1;
        //move up
        if (commands[2]) yValue += 1;
        //move down
        if (commands[3]) yValue -= 1;

        //rotate left
        if (commands[4]) up += 1;
        //rotate right
        if (commands[5]) up -= 1;

        if (c != null && triggered)
        {
            //use
            if (commands[6])
            {
                c.use(gameObject);
            }
            //pick up
            if (commands[7])
            {
                if (pan)
                {
                    if (c.addPan(hold as Pan))
                    {
                        holding = false;
                        pan = false;
                    }
                }
                else if (holding)
                {
                    if (c.addOnTop(hold)) holding = false;
                }
                else
                {
                    if (c.pickUp(gameObject))
                    {
                        holding = true;
                        GameEvents.current.PickUp(agent.id);
                    }
                }
            }
        }
        if (canMove())
        {
            front += new Vector3(xValue, yValue);
            transform.position = transform.position + new Vector3(xValue, yValue);
        }
        transform.Rotate(up * new Vector3(0, 0, 1), 90f);
        front = Quaternion.AngleAxis(90, up * new Vector3(0, 0, 1)) * front;
    }

    bool isTherePLayer(Vector3 pos)
    {
        foreach (PlayerController p in players)
        {
            if (p.transform.position == pos)
            {
                return true;
            }
        }
        return false;
    }

    bool canMove()
    {
        Vector3 a = transform.position + new Vector3(xValue, yValue);
        Vector3 pos = transform.position;
            if (positions.Contains(a))
                if (!isTherePLayer(a)) return true;
        
        a += new Vector3(-xValue, 0);
        int aux = xValue;
        xValue = 0;
        if (positions.Contains(a) && a != transform.position)
            if (!isTherePLayer(a)) return true;
        xValue = aux;
        a += new Vector3(xValue, -yValue);
        yValue = 0;
        if (positions.Contains(a) && a != transform.position)
            if (!isTherePLayer(a)) return true;
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        triggered = true;
        c = collision.gameObject.GetComponent<Counter>();
        Debug.Log(c);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        triggered = false;
        c = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        triggered = true;
        c = collision.gameObject.GetComponent<Counter>();
    }

    private Plate.State lastRequest()
    {
        return fr.lastRequest();
    }

    public bool holdItem(Item i)
    {
        if (!holding)
        {
            Pan p = i as Pan;
            pan = p != null;

            hold = i;
            holding = true;
            return true;
        }
        return false;
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
