using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{

    private Action action;

    private List<List<Vector3>> possiblePaths;

    private List<Vector3> chosenPath;
    

    public Task(Action action, List<List<Vector3>> possiblePaths)
    {
        this.action = action;
        this.possiblePaths = possiblePaths;
    }

    public Action GetAction()
    {
        return this.action;
    }

    public List<List<Vector3>> GetPossiblePaths()
    {
        return this.possiblePaths;
    }

    public List<Vector3> GetChosenPath()
    {
        return this.chosenPath;
    }

}
