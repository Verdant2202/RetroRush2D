using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : Enemy
{
    [SerializeField] private PatrolPath patrolPath;

    int currentlyMovingToPointIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = patrolPath.pathPoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 toPosition = patrolPath.pathPoints[currentlyMovingToPointIndex];
        float distanceThreshold = 0.05f;
        if (Vector2.Distance(transform.position, toPosition) < distanceThreshold)
        {
            currentlyMovingToPointIndex++;
            currentlyMovingToPointIndex %= patrolPath.pathPoints.Count;
        }

        Vector3 MoveDir = ((Vector3)toPosition - transform.position).normalized;
        transform.position += MoveDir * speed * Time.deltaTime;
    }
}
