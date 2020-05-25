using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject ground;

    private List<Vector3> positions = new List<Vector3>();

    private Item hold;
    private Vector3 front;
    private BoxCollider2D col;

    private bool pan = false;
    private bool holding = false;
    private bool triggered = false;

    public Counter c = null;

    private FoodRequests fr;


    private void Start()
    {
        this.front = new Vector3(0,1,0);

        this.ground = GameObject.FindGameObjectWithTag("Ground");

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
    }


    public void Act(List<bool> commands)
    {
        int xValue = 0, yValue = 0, up = 0;



        //move left
        if (commands[0])
        {
            Debug.Log("Move Left");
            xValue -= 1;
        }
        //move right
        else if (commands[1])
        {
            Debug.Log("Move Right");
            xValue += 1;
        }
        //move up
        else if (commands[2])
        {
            Debug.Log("Move UP");
            yValue += 1;
        }

        //move down
        else if (commands[3])
        {
            Debug.Log("Move Down");
            yValue -= 1;
        }

        //rotate left
        if (commands[4]) up += 1;
        //rotate right
        if (commands[5]) up -= 1;

        if (c != null && triggered)
        {
            //pick up
            if (commands[6])
            {
                c.use(gameObject);
            }
            //use
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
                    if (c.pickUp(gameObject)) holding = true;
                }
            }
        }
        Vector3 a = transform.position + new Vector3(xValue, yValue);
        if (positions.Contains(a))
        {
            front += new Vector3(xValue, yValue);
            transform.position = a;
        }
        transform.Rotate(up * new Vector3(0, 0, 1), 90f);
        front = Quaternion.AngleAxis(90, up * new Vector3(0, 0, 1)) * front;
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

    public Plate.State LastRequest()
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

}
