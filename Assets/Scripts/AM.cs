using UnityEngine;

public class AM : MonoBehaviour
{
    private AudioSource audioSource;

    public static AM instance;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Play(AudioClip clip)
    {
        instance.PlayIternal(clip);
    }

    private void PlayIternal(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
