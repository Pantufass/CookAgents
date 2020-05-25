using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveRequests : MonoBehaviour
{
    public List<Image> panels;
    public FoodRequests fr;
    private bool change = false;


    private void Start()
    {
        fr = FindObjectOfType<FoodRequests>();
        StartCoroutine(wait());
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        changeThem();
        change = true;
    }

    public void changeThem()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].sprite = fr.getRecipe(i);
        }
    }

}
