using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;

	[Header("Config (Scriptable Object)")]
	public GlobalAudioConfig activeConfig;

	[Header("Global Music Audio Source")]
	public AudioSource musicSource;

	[Header("Global Sounds Audio Source")]
	public AudioSource soundSource;

	private void Awake() {
		// Singleton pattern
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
			return;
		}
	}

	public void PlayPauseMenuTrack() {
		ResetAudioSource(musicSource);
		musicSource.clip = activeConfig.pauseMenuTrack;
		musicSource.loop = true;
		musicSource.volume = 1f;
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

	public void PlayPauseMenuNavigationSound() {
		ResetAudioSource(soundSource);
		soundSource.PlayOneShot(activeConfig.pauseMenuNavigation);
	}

	public void PlayPauseMenuButtonPressSound() {
		ResetAudioSource(soundSource);
		soundSource.PlayOneShot(activeConfig.pauseMenuButtonPress);
	}

	public void PlayPlayerFootstepsSound() {
		ResetAudioSource(soundSource);
		soundSource.clip = activeConfig.playerFootsteps;
		soundSource.loop = true;
		soundSource.pitch = 0.9f;
		soundSource.Play();
	}

	public void PlayPlayerIdleSound() {
		ResetAudioSource(soundSource);
	}

	public void PlayPlayerFallingSound() {
		ResetAudioSource(soundSource);
	}

	public void PlayPlayerJumpSound() {
		ResetAudioSource(soundSource);
		soundSource.PlayOneShot(activeConfig.playerJump);
	}

	public void PlayPlayerDashSound() {
		ResetAudioSource(soundSource);
	}

	public void PlayPlayerMeleeSound() {
		ResetAudioSource(soundSource);
	}

	public void PlayPlayerRangedAttackSound() {
		ResetAudioSource(soundSource);
		soundSource.PlayOneShot(activeConfig.playerRangedAttack);
	}

	public void PlayPlayerPushSound() {
		ResetAudioSource(soundSource);
	}

	public void PlayPlayerPullSound() {
		ResetAudioSource(soundSource);
	}

	public void PlayPlayerCorruptionSound() {
		ResetAudioSource(soundSource);
	}

	public void PlayPlayerPuritySound() {
		ResetAudioSource(soundSource);
	}

	public void PlayPlayerAirModSound() {
		ResetAudioSource(soundSource);
		soundSource.PlayOneShot(activeConfig.playerAirModSound);
	}

	public void PlayPlayerFireModSound() {
		ResetAudioSource(soundSource);
	}

	public void PlayPlayerWaterModSound() {
		ResetAudioSource(soundSource);
	}

	public void PlayPlayerEarthModSound() {
		ResetAudioSource(soundSource);
	}

	private void ResetAudioSource(AudioSource source) {
		source.loop = false;
		source.volume = 1f;
		source.pitch = 1f;
	}
}
