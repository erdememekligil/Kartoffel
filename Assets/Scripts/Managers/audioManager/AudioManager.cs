using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonComponent<AudioManager> {
	public AudioSource musicSource;
	public AudioSource soundSource;

	public void PlaySound(string name){
		soundSource.Stop ();
		soundSource.clip = Resources.Load<AudioClip> ("Sounds/" + name);
		soundSource.Play ();
	}

	public void PlayMusic(string name){
		musicSource.Stop ();
		musicSource.clip = Resources.Load<AudioClip> ("Musics/" + name);
		musicSource.Play ();
	}
}
