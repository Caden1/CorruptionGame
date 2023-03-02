using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsClass
{
	public static bool IsBoxColliderGrounded(BoxCollider2D boxCollider, LayerMask layerMask) {
		RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, layerMask);
		return raycastHit.collider != null;
	}

	public static Vector2 GetLeftAndRightDirectionalPointLocation(BoxCollider2D boxCollider, Vector2 directionPointing, float offset, bool isFacingRight) {
		Vector2 attackOrigin = new Vector2();
		Bounds playerBounds = boxCollider.bounds;
		Vector2 attackRightPosition = new Vector2(playerBounds.max.x + offset, playerBounds.center.y);
		Vector2 attackLeftPosition = new Vector2(playerBounds.min.x - offset, playerBounds.center.y);

		if (directionPointing.x > 0f) {
			attackOrigin = attackRightPosition;
		} else if (directionPointing.x < 0f) {
			attackOrigin = attackLeftPosition;
		} else {
			if (isFacingRight) {
				attackOrigin = attackRightPosition;
			} else {
				attackOrigin = attackLeftPosition;
			}
		}

		return attackOrigin;
	}

	public static Vector2 Get8DirectionalPointLocation(BoxCollider2D boxCollider, Vector2 directionPointing, float offset, Vector2 diagonalOffset, bool isFacingRight) {
		Vector2 attackOrigin = new Vector2();
		Bounds playerBounds = boxCollider.bounds;
		Vector2 attackUpRightPosition = new Vector2(playerBounds.max.x, playerBounds.max.y) + diagonalOffset;
		Vector2 attackRightPosition = new Vector2(playerBounds.max.x + offset, playerBounds.center.y);
		Vector2 attackDownRightPosition = new Vector2(playerBounds.max.x + diagonalOffset.x, playerBounds.min.y - diagonalOffset.y);
		Vector2 attackDownPosition = new Vector2(playerBounds.center.x, playerBounds.min.y - offset);
		Vector2 attackLeftDownPosition = new Vector2(playerBounds.min.x, playerBounds.min.y) - diagonalOffset;
		Vector2 attackLeftPosition = new Vector2(playerBounds.min.x - offset, playerBounds.center.y);
		Vector2 attackUpLeftPosition = new Vector2(playerBounds.min.x - diagonalOffset.x, playerBounds.max.y + diagonalOffset.y);
		Vector2 attackUpPosition = new Vector2(playerBounds.center.x, playerBounds.max.y + offset);

		if (directionPointing == Vector2.zero) {
			if (isFacingRight) {
				attackOrigin = attackRightPosition;
			} else {
				attackOrigin = attackLeftPosition;
			}
		} else if (directionPointing.x >= 0f) { // Right
			if (directionPointing.y > 0.75f) { // Up
				attackOrigin = attackUpPosition;
			} else if (directionPointing.y > 0.25f) { // Diagonal Up
				attackOrigin = attackUpRightPosition;
			} else if (directionPointing.y > -0.25f) { // Right
				attackOrigin = attackRightPosition;
			} else if (directionPointing.y > -0.75f) { // Diagonal Down
				attackOrigin = attackDownRightPosition;
			} else if (directionPointing.y >= -1f) { // Down
				attackOrigin = attackDownPosition;
			}
		} else if (directionPointing.x < 0f) { // Left
			if (directionPointing.y > 0.75f) { // Up
				attackOrigin = attackUpPosition;
			} else if (directionPointing.y > 0.25f) { // Diagonal Up
				attackOrigin = attackUpLeftPosition;
			} else if (directionPointing.y > -0.25f) { // Left
				attackOrigin = attackLeftPosition;
			} else if (directionPointing.y > -0.75f) { // Diagonal Down
				attackOrigin = attackLeftDownPosition;
			} else if (directionPointing.y >= -1f) { // Down
				attackOrigin = attackDownPosition;
			}
		}

		return attackOrigin;
	}

	public static Quaternion GetLeftOrRightRotation(bool isFacingRight) {
		float zDegrees = 180f;
		if (isFacingRight)
			zDegrees = 0f;

		return Quaternion.Euler(0f, 0f, zDegrees);
	}

	public static Quaternion GetRotationFromDegrees(float xDegrees, float yDegrees, float zDegrees, bool isFacingRight) {
		return Quaternion.Euler(xDegrees, yDegrees, zDegrees);
	}
}
