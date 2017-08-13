using UnityEngine;

public class Enemy : Character
{
    const int SHOTS_TIME_INTERVAL = 1;

    int currentPathIndex;
    int currentStep;
    float lastShotTime = 0 - SHOTS_TIME_INTERVAL;
    static GameObject player = null;
    static Player playerCtrl = null;

    protected WalkRoute Route = new WalkRoute();
    //protected float Sight = float.PositiveInfinity;
    protected float SightDistance = 5.0f;

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

    protected override void FixedUpdate()
    {
        shootOnPlayerSight();
    }

    private void shootOnPlayerSight()
    {
        Vector3 rayStart = transform.position + (HeadingVector * ColliderHalfSize);
        RaycastHit2D[] hits = new RaycastHit2D[2];
        Physics2D.Raycast(rayStart, HeadingVector, new ContactFilter2D(), hits, SightDistance);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform != null)
            {
                if (hit.transform.tag == "Player")
                {
                    if (Time.time - lastShotTime > SHOTS_TIME_INTERVAL)
                    {
                        lastShotTime = Time.time;
                        Shoot();
                    }
                }
                else if (hit.transform.tag == "Interaction Collider")
                    continue;
                else
                    break;
            }
        }
    }
}
