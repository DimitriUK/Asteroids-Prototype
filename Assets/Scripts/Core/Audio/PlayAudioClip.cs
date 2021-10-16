using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAudioClip : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioData SoundData;

    public AudioClip[] SoundsToPlay;

    public bool isPlayingOnEnable;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnValidate()
    {
        SoundsToPlay = SoundData.ClipsToPlay;
    }

    private void OnEnable()
    {
        if (isPlayingOnEnable)
            PlayRandomClip();
    }
    public void PlayRandomClip()
    {
        AudioClip randomClip = GetRandomAudioClip();

        PlaySound(randomClip);
    }

    private AudioClip GetRandomAudioClip()
    {
        int randomClip = Random.Range(0, SoundsToPlay.Length);
        return SoundsToPlay[randomClip];
    }

    private void PlaySound(AudioClip soundToPlay)
    {
        audioSource.PlayOneShot(soundToPlay);
    }
}
