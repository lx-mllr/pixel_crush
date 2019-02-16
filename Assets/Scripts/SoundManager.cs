﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> {
    public AudioClip[] musicClips;
    public AudioClip[] winClips;
    public AudioClip[] loseClips;
    public AudioClip[] bonusClips;

    [Range(0, 1)]
    public float musicVolume = 0.5f;

    [Range(0, 1)]
    public float fxVolume = 1.0f;

    public float lowPitch = 0.95f;
    public float highPitch = 1.05f;

    // Start is called before the first frame update
    void Start() {
        PlayRandomMusic();
    }

    //replace unity's playclipatpoint to play audioclip at a world space position
    public AudioSource PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1f, bool randomizePitch = true) {
        if(clip != null) {
            //create new gameobject at the specified world space position
            GameObject go = new GameObject("SoundFX" + clip.name);
            go.transform.position = position;

            //add an audisource component and set the audioclip
            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = clip;

            //change the pitch of the sound randomly
            if (randomizePitch) {
                float randomPitch = Random.Range(lowPitch, highPitch);
                source.pitch = randomPitch;
            }
            //set volume
            source.volume = volume;
            //play sound
            source.Play();
            //destroy audiosource after the clip is done playing
            Destroy(go, clip.length);
            return source;
        }
        return null;
    }

    public AudioSource PlayRandom(AudioClip[] clips, Vector3 position, float volume = 1f) {
        if(clips != null) {
            if(clips.Length != 0) {
                int randomIndex = Random.Range(0, clips.Length);
                if (clips[randomIndex] != null) {
                    AudioSource source = PlayClipAtPoint(clips[randomIndex], position, volume);
                    return source;
                }
            }
        }
        return null;
    }

    public void PlayRandomMusic() {
        PlayRandom(musicClips, Vector3.zero, musicVolume);
    }

    public void PlayWinSound() {
        PlayRandom(winClips, Vector3.zero, musicVolume);
    }

    public void PlayLoseSound() {
        PlayRandom(loseClips, Vector3.zero, musicVolume);
    }

    public void PlayBonusSound() {
        PlayRandom(bonusClips, Vector3.zero, musicVolume);
    }
}
