using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsClass
{
	public static bool IsBoxColliderGrounded(BoxCollider2D boxCollider, LayerMask layerMask) {
		RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, layerMask);
		return raycastHit.collider != null;
	}

	public static Vector2 GetRandomDirection()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
