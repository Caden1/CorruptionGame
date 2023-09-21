using System.Collections.Generic;
using UnityEngine;

public class RealmAudioManager : MonoBehaviour
{
	[Header("Config (Scriptable Object)")]
	public RealmAudioConfig activeConfig;

	[Header("Realm Music Audio Source")]
	public AudioSource musicSource;

	private List<AudioSource> audioSourcePoolSounds = new List<AudioSource>();
	private List<AudioSource> audioSourcePoolFootsteps = new List<AudioSource>();

	private void Start() {
		// For testing
		PlayMainTrack();

		int maxSoundsAudioSources = 10;
		for (int i = 0; i < maxSoundsAudioSources; i++) {
			AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
			audioSourcePoolSounds.Add(newAudioSource);
		}

		int maxFootstepsAudioSources = 4;
		for (int i = 0; i < maxFootstepsAudioSources; i++) {
			AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
			audioSourcePoolFootsteps.Add(newAudioSource);
		}
	}

	public void PlayMainTrack() {
		ResetAudioSource(musicSource);
		musicSource.clip = activeConfig.mainTrack;
		musicSource.loop = true;
		musicSource.Play();
	}

	public void PlayBossFightTrack() {
		ResetAudioSource(musicSource);
		musicSource.clip = activeConfig.bossFightTrack;
		musicSource.loop = true;
		musicSource.Play();
	}

	public void StopCurrentTrack() {
		musicSource.Stop();
	}

	public void PauseCurrentTrack() {
		musicSource.Pause();
	}

	public void UnpauseCurrentTrack() {
		musicSource.Play();
	}

	public void PlayMeleeEnemyIdleSound() {
		AudioSource availableSource = GetAvailableSoundAudioSource();
		if (availableSource != null) {

		}
	}

	public AudioSource PlayMeleeEnemyFootstepsSound() {
		AudioSource availableSource = GetAvailableFootstepAudioSource();
		if (availableSource != null) {
			availableSource.clip = activeConfig.meleeEnemyFootsteps;
			availableSource.loop = true;
			availableSource.Play();
		}
		return availableSource;
	}

	public void StopFootstepsSound(AudioSource source) {
		source.Stop();
		ResetAudioSource(source);
	}

	public void PlayMeleeEnemyAttackSound() {
		AudioSource availableSource = GetAvailableSoundAudioSource();
		if (availableSource != null) {
			availableSource.PlayOneShot(activeConfig.meleeEnemyAttack);
		}
	}

	public void PlayMeleeEnemyTakeDamageSound() {
		AudioSource availableSource = GetAvailableSoundAudioSource();
		if (availableSource != null) {
			availableSource.PlayOneShot(activeConfig.meleeEnemyTakeDamage);
		}
	}

	public void PlayMeleeEnemyDyingSound() {
		AudioSource availableSource = GetAvailableSoundAudioSource();
		if (availableSource != null) {
			availableSource.PlayOneShot(activeConfig.meleeEnemyDying);
		}
	}

	public void PlayMeleeEnemyDizzySound() {
		AudioSource availableSource = GetAvailableSoundAudioSource();
		if (availableSource != null) {

		}
	}

	public void PlayMeleeEnemySuctionedSound() {
		AudioSource availableSource = GetAvailableSoundAudioSource();
		if (availableSource != null) {

		}
	}

	private AudioSource GetAvailableSoundAudioSource() {
		foreach (AudioSource source in audioSourcePoolSounds) {
			if (!source.isPlaying) {
				ResetAudioSource(source);
				return source;
			}
		}
		return null;
	}

	private AudioSource GetAvailableFootstepAudioSource() {
		foreach (AudioSource source in audioSourcePoolFootsteps) {
			if (!source.isPlaying) {
				return source;
			}
		}
		return null;
	}

	private void ResetAudioSource(AudioSource source) {
		source.loop = false;
		source.volume = 1f;
		source.pitch = 1f;
	}
}
