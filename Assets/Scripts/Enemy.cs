using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    int currentPathIndex;
    int currentStep;
    static GameObject player = null;
    static Player playerCtrl = null;

    protected WalkRoute Route = new WalkRoute();

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player");
        playerCtrl = player.GetComponent<Player>();

        walkOnRoute();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
            playerCtrl.OnHit();
    }

    private void walkOnRoute()
    {
        if (StepLength <= float.Epsilon || Route.Paths.Count == 0)
            return;

        currentPathIndex = 0;
        currentStep = 0;
        nextStep();
    }

    private void nextStep()
    {
        if (currentPathIndex == Route.Paths.Count)
        {
            if (Route.IsLoop)
                currentPathIndex = 0;
            else
            {
                Walk(Direction.None);
                return;
            }
        }

        WalkPath currentPath = Route.Paths[currentPathIndex];
        if (currentPath.Direction == Direction.None)
        {
            Walk(Direction.None);
            StartCoroutine(Timer(1, nextStep));
        }
        else
            WalkCR(currentPath.Direction, StepLength, Speed, nextStep);

        currentStep++;
        if (currentStep == currentPath.Amount)
        {
            currentStep = 0;
            currentPathIndex++;
        }
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }
}
