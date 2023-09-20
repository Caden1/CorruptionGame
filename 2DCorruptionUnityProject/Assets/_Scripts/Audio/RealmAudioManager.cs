using UnityEngine;

public class RealmAudioManager : MonoBehaviour
{
	[Header("Config (Scriptable Object)")]
	public RealmAudioConfig activeConfig;

	[Header("Realm Music Audio Source")]
	public AudioSource musicSource;

	[Header("Realm Sounds Audio Source")]
	public AudioSource soundSource;

	private void Start() {
		// For testing
		PlayMainTrack();
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
		ResetAudioSource(soundSource);
	}

	public void PlayMeleeEnemyFootstepsSound() {
		ResetAudioSource(soundSource);
		soundSource.clip = activeConfig.meleeEnemyFootsteps;
		soundSource.loop = true;
		soundSource.Play();
	}

	public void PlayMeleeEnemyAttackSound() {
		ResetAudioSource(soundSource);
		soundSource.PlayOneShot(activeConfig.meleeEnemyAttack);
	}

	public void PlayMeleeEnemyTakeDamageSound() {
		ResetAudioSource(soundSource);
		soundSource.PlayOneShot(activeConfig.meleeEnemyTakeDamage);
	}

	public void PlayMeleeEnemyDyingSound() {
		ResetAudioSource(soundSource);
		soundSource.PlayOneShot(activeConfig.meleeEnemyDying);
	}

	public void PlayMeleeEnemyDizzySound() {
		ResetAudioSource(soundSource);
	}

	public void PlayMeleeEnemySuctionedSound() {
		ResetAudioSource(soundSource);
	}

	private void ResetAudioSource(AudioSource source) {
		source.loop = false;
		source.volume = 1f;
		source.pitch = 1f;
	}
}
