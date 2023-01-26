using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleSystems : MonoBehaviour
{
	private ParticleSystem corruptionJumpParticle;

    private void Start() {
		corruptionJumpParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
	}

    public void PlayCorruptionJumpParticle() {
		corruptionJumpParticle.Play();
	}
}
