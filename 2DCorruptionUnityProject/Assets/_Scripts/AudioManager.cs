using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;

	[Header("Music")]
	public AudioSource musicSource;
	public AudioClip[] musicTracks;

	[Header("Sounds")]
	public AudioSource sfxSource;
	public AudioClip[] soundFX;

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

	private void Start() {
		// For Testing:
		PlayMusic(0, true);
	}

	public void PlayMusic(int trackIndex, bool loop = true) {
		if (trackIndex >= 0 && trackIndex < musicTracks.Length) {
			musicSource.clip = musicTracks[trackIndex];
			musicSource.loop = loop;
			musicSource.Play();
		} else {
			Debug.LogError("Track index is out of range!");
		}
	}

	public void StopMusic() {
		musicSource.Stop();
	}

	public void PlaySFX(int sfxIndex, float volume = 1.0f) {
		if (sfxIndex >= 0 && sfxIndex < soundFX.Length) {
			sfxSource.PlayOneShot(soundFX[sfxIndex], volume);
		} else {
			Debug.LogError("SFX index is out of range!");
		}
	}
}
