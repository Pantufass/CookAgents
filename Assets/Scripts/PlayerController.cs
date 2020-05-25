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

    public string facing = "up";

    int up = 0;

    public Counter c = null;

    private FoodRequests fr;

    private Quaternion Left = new Quaternion(); 

    private Quaternion Right = new Quaternion(); 

    private Quaternion Up = new Quaternion(); 

    private Quaternion Down = new Quaternion(); 


    private void Start()
    {
        Up.Set(0, 0, 0, 1);
        Down.Set(0.0f, 0.0f, 1, 0);
        Right.Set(0.0f, 0.0f, -0.7f, 0.7f);
        Left.Set(0.0f, 0.0f, 0.7f, 0.7f);



        this.front = new Vector3(0,1,0);

        this.ground = GameObject.FindGameObjectWithTag("Ground");

        foreach(Transform t in ground.transform)
        {
            positions.Add(new Vector3(t.position.x, t.position.y, -1));
        }
        col = gameObject.AddComponent<BoxCollider2D>();
        col.offset = front;
        col.isTrigger = true;
        col.enabled = true;
        col.size = new Vector2(0.8f, 0.8f);

        fr = FindObjectOfType<FoodRequests>();
    }


    public void Act(List<bool> commands)
    {
        int xValue = 0, yValue = 0;


        //move left
        if (commands[0])
        {
            Debug.Log("Agent " + this.GetComponent<Agent>().GetId() + "  MOVING LEFT");

            RotateLeft();
            xValue -= 1;
        }
        //move right
        else if (commands[1])
        {
            Debug.Log("Agent " + this.GetComponent<Agent>().GetId() + "  MOVING RIGHT");
            RotateRight();
            xValue += 1;
        }
        //move up
        else if (commands[2])
        {
            Debug.Log("Agent " + this.GetComponent<Agent>().GetId() + "  MOVING UP");
            RotateUp();
            yValue += 1;
        }

        //move down
        else if (commands[3])
        {
            Debug.Log("Agent " + this.GetComponent<Agent>().GetId() + "  MOVING DOWN");
            RotateDown();
            yValue -= 1;
        }

        //rotate left
        if (commands[4]) this.up += 1;
        //rotate right
        if (commands[5]) this.up -= 1;

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
                    Debug.Log("Yes its a fcking pan");
                    if (c.addPan(hold as Pan))
                    {   
                        Debug.Log("meti?");
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
        transform.Rotate(this.up * new Vector3(0, 0, 1), 90f);
        Debug.Log(this.gameObject.transform.rotation);
        front = Quaternion.AngleAxis(90, this.up * new Vector3(0, 0, 1)) * front;
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

    public void RotateTowards(Vector3 target)
    {
        Vector3 difference = target - this.gameObject.transform.position;
        if (difference.y < 0)
        {
            RotateDown();
        }
        else if(difference.y > 0)
        {
            RotateUp();
        }
        else if(difference.x < 0)
        {
            RotateLeft();
        }
        else if(difference.x > 0)
        {
           RotateRight();
        }
        transform.Rotate(this.up * new Vector3(0, 0, 1), 90f);
        Debug.Log(this.gameObject.transform.rotation);
        front = Quaternion.AngleAxis(90, this.up * new Vector3(0, 0, 1)) * front;
    }

    private void RotateUp()
    {
        Debug.Log("Agent " + this.GetComponent<Agent>().GetId() + "  Rotating UP");
        Debug.Log(this.gameObject.transform.rotation);
        if (facing.Equals("down"))
        {
            this.up += 2;
        }
        else if (facing.Equals("left"))
        {
            this.up -= 1;
        }
        else if (facing.Equals("right"))
        {
            this.up += 1;
        }
        facing = "up";

    }

    private void RotateDown()
    {
        Debug.Log("Agent " + this.GetComponent<Agent>().GetId() + "  Rotating DOWN");
        Debug.Log(this.gameObject.transform.rotation);
        if (facing.Equals("up"))
        {
            this.up -= 2;
        }
        else if (facing.Equals("left"))
        {
            this.up += 1;
        }
        else if (facing.Equals("right"))
        {
            this.up -= 1;
        }
        facing = "down";
    }

    private void RotateLeft()
    {
        Debug.Log("Agent " + this.GetComponent<Agent>().GetId() + "  Rotating LEFT");
        Debug.Log(this.gameObject.transform.rotation);
        if (facing.Equals("up"))
        {
            this.up += 1;
        }
        else if (facing.Equals("right"))
        {
            this.up += 2;
        }
        else if (facing.Equals("down"))
        {
            this.up -= 1;
        }
        facing = "left";
    }

    private void RotateRight()
    {
        Debug.Log("Agent " + this.GetComponent<Agent>().GetId() + "  Rotating RIGHT");
        Debug.Log(this.gameObject.transform.rotation);
        if (facing.Equals("up"))
        {
            this.up -= 1;
        }
        else if (facing.Equals("left"))
        {
            this.up += 2;
        }
        else if (facing.Equals("down"))
        {
            this.up += 1;
        }
        facing = "right";
    }
}
