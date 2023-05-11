using UnityEngine;

public class GroundCheck : MonoBehaviour
{
	[SerializeField] private Transform groundCheckTransform;
	[SerializeField] private LayerMask groundLayers;

	private Vector2 groundCheckSize;

	private bool isGrounded;

	private void Start() {
		groundCheckSize = groundCheckTransform.GetComponent<BoxCollider2D>().size;
	}

	private void FixedUpdate() {
		isGrounded = Physics2D.OverlapBox(groundCheckTransform.position, groundCheckSize, 0f, groundLayers);
	}

	public bool IsGrounded() {
		return isGrounded;
	}
}
