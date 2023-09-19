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

	public void PlayMainTrack(float volume = 1f) {
		musicSource.clip = activeConfig.mainTrack;
		PlayTrackHelper(volume);
	}

	public void PlayBossFightTrack(float volume = 1f) {
		musicSource.clip = activeConfig.bossFightTrack;
		PlayTrackHelper(volume);
	}

	private void PlayTrackHelper(float volume) {
		musicSource.loop = true;
		musicSource.volume = volume;
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

	public void PlayEnemyTakeDamageSound(float volume = 1f) {
		soundSource.clip = activeConfig.enemyTakeDamage;
		PlaySoundHelper(volume);
	}

	public void PlayEnemyAttackSound(float volume = 1f) {
		soundSource.clip = activeConfig.enemyAttack;
		PlaySoundHelper(volume);
	}

	public void PlayEnemyDyingSound(float volume = 1f) {
		soundSource.clip = activeConfig.enemyDying;
		PlaySoundHelper(volume);
	}

	private void PlaySoundHelper(float volume) {
		soundSource.loop = false;
		soundSource.volume = volume;
		soundSource.Play();
	}
}
