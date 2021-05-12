using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public void PlayAudio(AudioClip clip)
    {
        Game.manager.GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
