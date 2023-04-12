using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private GameObject virtualCamera;


	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			virtualCamera.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.tag == "Player") {
			virtualCamera.SetActive(false);
		}
	}
}
