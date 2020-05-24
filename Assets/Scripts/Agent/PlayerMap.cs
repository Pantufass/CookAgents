using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMap
{

    private List<GameObject> otherPlayers = new List<GameObject>();

    private List<Vector3> positions;


    public List<List<Vector3>> FindPossiblePaths(Vector3 start, Vector3 goal)
    {
        List<Vector3> walkablePositions = this.FindClosestWalkablePosition(goal);

        List<List<Vector3>> allPossiblePaths = new List<List<Vector3>>();

        foreach(Vector3 w in walkablePositions)
        {
            List<List<Vector3>> lp = this.FindPaths(start, w, new List<Vector3>());

            foreach(List<Vector3> p in lp)
            {
                allPossiblePaths.Add(p);
            }
        }
        return allPossiblePaths;
        
    }



    private List<List<Vector3>> FindPaths(Vector3 start, Vector3 goal, List<Vector3> path)
    {

        if (start.Equals(goal))
        {
            path.Add(start);
            List<List<Vector3>> paths = new List<List<Vector3>>();
            paths.Add(path);
            return paths;
        }
        List<List<Vector3>> possiblePaths = new List<List<Vector3>>();

        foreach(Vector3 v in positions)
        {
            if(AdjacentPositions(start, v)) //Adjacent square he can walk
            {
                if(path.Contains(v)){ // path where it is coming from

                    return new List<List<Vector3>>();  //returns empty because its not valid to turn back 

                }
                else        //new path
                {
                    List<Vector3> pathCopy = new List<Vector3>(path);
                    pathCopy.Add(start);
                    List<List<Vector3>> pathsReturned = FindPaths(v, goal, pathCopy);
                    foreach(List<Vector3> p in pathsReturned)
                    {
                        possiblePaths.Add(p);
                    }
                }
            }
        }
        return possiblePaths;

    }


    private List<Vector3> FindClosestWalkablePosition(Vector3 goal)
    {
        List<Vector3> walkablePositions = new List<Vector3>();

        if (positions.Contains(goal))
        {
            walkablePositions.Add(goal);
            return walkablePositions;
        }
        else
        {
            foreach(Vector3 p in positions)
            {
                float xDiference = Mathf.Abs(p.x - goal.x);
                float yDiference = Mathf.Abs(p.y - goal.y);

                if((xDiference + yDiference) <= 1)
                {
                    walkablePositions.Add(p);
                }
            }
            return walkablePositions;
        }
    }

    private bool AdjacentPositions(Vector3 pos1, Vector3 pos2)
    {
        float xDiference = Mathf.Abs(pos2.x - pos1.x);
        float yDiference = Mathf.Abs(pos2.y - pos1.y);

        return !((xDiference + yDiference) > 1);
    }


    public Vector3 GetPositionOf(GameObject desiredObject)   //FIXME Gameobject ??
    {
        //TODO
        return new Vector3();

    }

    public void AddOtherPlayer(GameObject player)
    {
        otherPlayers.Add(player);
    }
}   
