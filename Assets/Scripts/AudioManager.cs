﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public AudioMixerGroup MusicMixerGroup;
    public AudioMixerGroup EffectsMixerGroup;

    public Transform SoundContainer;
    public Transform MusicContainer;

    private Dictionary<string, AudioSource> sounds = new Dictionary<string, AudioSource>();

    private void Awake()
    {
        sounds.Clear();
        AudioSource[] sources = SoundContainer.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource source in sources)
        {
            sounds.Add(source.gameObject.name, source);
        }
    }

    public void Init()
    {
    }

    public void PlaySound(string soundName)
    {
        if (sounds.ContainsKey(soundName))
        {
            AudioSource sound = sounds[soundName];
            if (!sound.isPlaying)
            {
                StopSound(soundName);
                sound.Play();
            }
        }
    }

    public void StopSound(string soundName)
    {
        if (sounds.ContainsKey(soundName))
        {
            AudioSource sound = sounds[soundName];
            if (sound.isPlaying)
            {
                sound.Stop();
            }
        }
    }

    public AudioSource GetAudioSource(string soundName)
    {
        if (sounds.ContainsKey(soundName))
        {
            return sounds[soundName];
        }
        return null;
    }
}
