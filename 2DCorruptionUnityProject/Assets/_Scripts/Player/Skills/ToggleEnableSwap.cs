using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleEnableSwap : MonoBehaviour
{
	public bool canSwap;

	private GameObject playerGO;
	private PlayerSkillController playerSkillController;

	private void Start() {
		playerGO = GameObject.FindWithTag("Player");
		if (playerGO != null) {
			playerSkillController = playerGO.GetComponent<PlayerSkillController>();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			playerSkillController.CanSwap = canSwap;
		}
	}
}
