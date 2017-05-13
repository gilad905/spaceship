using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    int currentPathIndex;
    int currentStep;
    float inverseMoveTime;
    static GameObject player = null;
    static Player playerCtrl = null;
    event moveEndedHandler moveStepEnded;

    protected WalkRoute Route = new WalkRoute();

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
                Standing = true;
                UpdateAnimation();
                return;
            }
        }

        WalkPath currentPath = Route.Paths[currentPathIndex];
        Vector3 curDirVector = GetDirectionVector(currentPath.Direction);

        StartCoroutine(MoveCR(curDirVector, StepLength, inverseMoveTime, moveStepEnded));
        Standing = false;
        Heading = currentPath.Direction;
        UpdateAnimation();

        currentStep++;
        if (currentStep == currentPath.Amount)
        {
            currentStep = 0;
            currentPathIndex++;
        }
    }

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player");
        playerCtrl = player.GetComponent<Player>();

        inverseMoveTime = Speed / StepLength;
        moveStepEnded += nextStep;

        walkOnRoute();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
            playerCtrl.OnHit();
    }
}
