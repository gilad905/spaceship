using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    protected WalkRoute route = new WalkRoute();

    int currentPathIndex;
    int currentStep;
    float inverseMoveTime;

    delegate void moveStepEndedHandler();
    event moveStepEndedHandler moveStepEnded;

    protected override void Start()
    {
        base.Start();

        inverseMoveTime = Speed / StepLength;
        moveStepEnded += nextStep;

        moveOnRoute();
    }

    private void moveOnRoute()
    {
        if (StepLength <= float.Epsilon || route.Paths.Count == 0)
            return;

        currentPathIndex = 0;
        currentStep = 0;
        nextStep();
    }

    private void nextStep()
    {
        if (currentPathIndex == route.Paths.Count)
        {
            if (route.IsLoop)
                currentPathIndex = 0;
            else
            {
                MovementActions(Direction.None);
                return;
            }
        }

        WalkPath currentPath = route.Paths[currentPathIndex];
        Vector3 curDirVector = GetDirectionVector(currentPath.Direction);

        StartCoroutine(moveStep(curDirVector));

        currentStep++;
        if (currentStep == currentPath.Amount)
        {
            currentStep = 0;
            currentPathIndex++;
        }
    }

    protected IEnumerator moveStep(Vector3 dirVector)
    {
        float sqrRemainingDistance;

        if (dirVector.sqrMagnitude == 0)
            yield break;

        Vector3 endPosition = transform.position + (dirVector * StepLength);

        MovementActions(dirVector);

        do
        {
            Vector3 newPostion = Vector3.MoveTowards(Rb2d.position, endPosition, inverseMoveTime * Time.deltaTime);
            Rb2d.MovePosition(newPostion);

            sqrRemainingDistance = (transform.position - endPosition).sqrMagnitude;

            yield return null;
        }
        while (sqrRemainingDistance > float.Epsilon);

        moveStepEnded();
    }
}
