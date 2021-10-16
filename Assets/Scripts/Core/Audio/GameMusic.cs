using System.Collections;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    public AudioClip FirstBeep;
    public AudioClip SecondBeep;

    private AudioSource MusicAudioSource;

    private bool isMusicOn;

    [SerializeField] private float MusicTimerDelay;
    //public float MusicTimerSubtractor;

    private const float MUSIC_TIME_DELAY_DEFAULT = 1.5f;
    //private const float MUSIC_TIME_SUBTRACTOR_DEFAULT = 0;

    private int CurrentAsteroidsLeft = 0;
    private int InitialAsteroidsAmount = 0;

    /// <summary>
    /// New music volume is 0.5 + 0.5 * amountOfAsteroidsRightnow / initialAmountOfAsteroids
    /// </summary>


    private void Awake()
    {
        MusicAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Actions.OnAsteroidDestroyed += IncreaseMusicSpeed;
    }

    private void OnDisable()
    {
        Actions.OnAsteroidDestroyed -= IncreaseMusicSpeed;
    }

    public void ResetMusic()
    {
        StopAllCoroutines();
        //MusicTimerDelay = MUSIC_TIME_DELAY_DEFAULT;
    }

    public void StartMusicForNewGame()
    {
        StartCoroutine(StartMusicTimer());
        isMusicOn = true;
    }

    public void SetupMusicTimeSubtractor(int amountOfAsteroids)
    {
        ResetMusic();
        CurrentAsteroidsLeft = amountOfAsteroids;
        InitialAsteroidsAmount = amountOfAsteroids;
        MusicTimerDelay = CalculateSubtraction();
    }

    private float CalculateSubtraction()
    {
        var result = MUSIC_TIME_DELAY_DEFAULT * CurrentAsteroidsLeft / InitialAsteroidsAmount + 0.5f;
        return result;
    }

    public IEnumerator StartMusicTimer()
    {
        if (!MusicAudioSource.isPlaying)
            MusicAudioSource.PlayOneShot(FirstBeep);

        yield return new WaitForSeconds(MusicTimerDelay);

        if (!MusicAudioSource.isPlaying)
            MusicAudioSource.PlayOneShot(SecondBeep);

        yield return new WaitForSeconds(MusicTimerDelay);

        StartCoroutine(StartMusicTimer());
    }

    public void IncreaseMusicSpeed(AsteroidProjectile asteroidProjectile)
    {
        CurrentAsteroidsLeft--;
        MusicTimerDelay = CalculateSubtraction();
    }

    public void StopMusic()
    {
        isMusicOn = false;
        ResetMusic();
    }
}
