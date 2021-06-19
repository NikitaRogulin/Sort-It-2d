using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    private Dictionary<AudioMixerGroup, AudioSource> audioSources;

    public static AudioManager Manager { get; private set; }

    private void Awake()
    {
        if (Manager == null) 
        {
            Manager = this;

            var sources = GetComponents<AudioSource>();

            audioSources = new Dictionary<AudioMixerGroup, AudioSource>();
            foreach (var item in sources)
            {
                audioSources.Add(item.outputAudioMixerGroup, item);
            }

            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayOneShot(AudioMixerGroup group, AudioClip clip)
    {
        audioSources[group].PlayOneShot(clip);
    }

    public void Play(AudioMixerGroup group, AudioClip clip)
    {
        audioSources[group].clip = clip;
        audioSources[group].Play();
    }

    public void Toggle(AudioMixerGroup group)
    {
        group.audioMixer.GetFloat(group.name, out float value);
        value = value == 0 ? -80 : 0;
        group.audioMixer.SetFloat(group.name, value);
        group.audioMixer.GetFloat(group.name, out float value2);
    }
}
