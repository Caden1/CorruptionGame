using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggro : MonoBehaviour
{
    private float moveSpeed;

    private void Start()
    {
        moveSpeed = 1.0f;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            transform.parent.position = Vector2.MoveTowards(transform.parent.position, collision.transform.position, moveSpeed * Time.deltaTime);
        }
    }
}
