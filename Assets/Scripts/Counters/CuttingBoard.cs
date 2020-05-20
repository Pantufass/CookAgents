using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuttingBoard : CounterItem
{
    float cutProgress = 0;

    float localCutProgress = 0;

    public Image progressBar;
    public GameObject progressCanvas;

    public float cutSpeed = 4f;

    float changeVelocity;


    // INIT SCRIPTS
    public override void Start()
    {
        base.Start();
        continuousAction = true;

        if (cutProgress > 0)
            InitChoppingBoard(cutProgress);

    }

    void InitChoppingBoard(float p)
    {
        progressCanvas.SetActive(true);
        progressBar.fillAmount = cutProgress / 10f;
    }


    // LIVE UPDATE SCRIPTS
    void Update()
    {
        SmoothVal();
    }


    // COMMANDS
    public override void PlaceItem(GameObject foodItem)
    {
        cutProgress = 0;
        base.PlaceItem(foodItem);
    }

    public override void UseCounter(GameObject player, bool cont, float deltaTime)
    {
        if (itemOnCounter == null)
        {
            cutProgress = 0;
            RpcUpdateProgress(cutProgress);
            return;
        }

        if (itemOnCounter != null)
        {
            if (!itemOnCounter.GetComponent<FoodItem>().choppable)
                return;
        }

        cutProgress += cutSpeed * deltaTime;

        cutProgress = Mathf.Clamp(cutProgress, 0, 10f);

        if (cutProgress >= 10f)
        {
            var obj = Instantiate(itemOnCounter.GetComponent<FoodItem>().chopsTo);
            Destroy(itemOnCounter);

            PlaceItem(obj);

        }

        RpcUpdateProgress(cutProgress);
    }

    void RpcUpdateProgress(float value)
    {
        if (value > 0)
        {
            progressCanvas.SetActive(true);
        }
        else progressCanvas.SetActive(false);

        // progressBar.fillAmount = value / 10f;
    }

    void SmoothVal()
    {
        float toVal = cutProgress / 10f;
        float curVal = Mathf.SmoothDamp(progressBar.fillAmount, toVal, ref changeVelocity, 0.2f);
        progressBar.fillAmount = curVal;

        if (cutProgress == 0 || itemOnCounter == null)
            progressCanvas.SetActive(false);
    }

}
