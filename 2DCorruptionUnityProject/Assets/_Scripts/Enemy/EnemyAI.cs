using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject playerObject;
    private enum State { Roaming, ChaseTarget, AttackTarget }
    private Vector2 startingPosition;
    private Vector2 raomPosition;
    private State state;
    private float moveSpeed = 0.1f;

    private void Start()
    {
        startingPosition = transform.position;
        raomPosition = GetRoamingPosition();
        state = State.Roaming;
    }

    private void Update()
    {
        // Trigger and Reset animations in methods
        switch (state)
        {
            case State.Roaming:
                Roam();
                LookForTarget();
                break;
            case State.ChaseTarget:
                ChaseTarget();
                break;
            case State.AttackTarget:
                ChaseAndAttackTarget();
                break;
        }
    }

    private void Roam()
    {
        transform.Translate(raomPosition * moveSpeed * Time.deltaTime);
        float reachedPositionDistance = 1f;
        if (Vector2.Distance(transform.position, raomPosition) < reachedPositionDistance)
        {
            raomPosition = GetRoamingPosition();
        }
    }

    private Vector2 GetRoamingPosition()
    {
        float enemyRoamDistance = 1.5f;
        return startingPosition + UtilsClass.GetRandomDirection() * enemyRoamDistance;
    }

    private void LookForTarget()
    {
        float targetRange = 2f;
        if (Vector2.Distance(transform.position, playerObject.transform.position) < targetRange)
        {
            state = State.ChaseTarget;
        }
    }

    private void ChaseTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerObject.transform.position, moveSpeed * Time.deltaTime);
        float attackRange = 1f;
        if (Vector2.Distance(transform.position, playerObject.transform.position) < attackRange)
        {
            state = State.AttackTarget;
        }
    }

    private void ChaseAndAttackTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerObject.transform.position, moveSpeed * Time.deltaTime);
        transform.Rotate(0f, 0f, 5f);
    }
}
