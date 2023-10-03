using System.Collections.Generic;
using UnityEngine;

public class RealmAudioManager : MonoBehaviour
{
	[Header("Config (Scriptable Object)")]
	public RealmAudioConfig activeConfig;

	[Header("Realm Music Audio Source")]
	public AudioSource musicSource;

	private List<AudioSource> audioSourcePoolSounds = new List<AudioSource>();
	private int currentlyPlayingFootsteps = 0;
	private const int maxFootsteps = 4;

	private void Start() {
		// For testing
		PlayMainTrack();

		int maxSoundsAudioSources = 10;
		for (int i = 0; i < maxSoundsAudioSources; i++) {
			AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
			audioSourcePoolSounds.Add(newAudioSource);
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
		if (currentlyPlayingFootsteps >= maxFootsteps) {
			return null;
		}
		AudioSource availableSource = GetAvailableSoundAudioSource();
		if (availableSource != null) {
			currentlyPlayingFootsteps++;
			availableSource.clip = activeConfig.meleeEnemyFootsteps;
			availableSource.loop = true;
			availableSource.Play();
		}
		return availableSource;
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

	// Called from EnemyController to stop footsteps
	public void StopAndResetAudioSource(AudioSource source) {
		if (source != null && source.isPlaying) {
			if (source.clip == activeConfig.meleeEnemyFootsteps) {
				currentlyPlayingFootsteps--;
			}
			source.Stop();
			ResetAudioSource(source);
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

	private void ResetAudioSource(AudioSource source) {
		source.loop = false;
		source.volume = 1f;
		source.pitch = 1f;
	}
}
