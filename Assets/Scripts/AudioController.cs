using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioMixerGroup group;
    [SerializeField] AudioClip clip;

    public void PlayOneShot()
    {
        AudioManager.Manager.PlayOneShot(group, clip);
    }

    public void PlayOneShot(AudioClip clip)
    {
        AudioManager.Manager.PlayOneShot(group, clip);
    }

    public void Play()
    {
        AudioManager.Manager.Play(group, clip);
    }

    public void Toggle(AudioMixerGroup group)
    {
        AudioManager.Manager.Toggle(group);
    }
}
