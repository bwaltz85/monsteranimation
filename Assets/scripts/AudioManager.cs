using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public List<AudioClip> battleMusicClips = new List<AudioClip>();
    public AudioClip victoryClip;
    public AudioClip lossClip;
    public AudioClip postVictoryClip;
    public AudioClip attackClip;

    private AudioClip previousBattleTrack;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void PlayRandomBattleTrack()
    {
        if (battleMusicClips.Count == 0) return;

        int index = Random.Range(0, battleMusicClips.Count);
        previousBattleTrack = battleMusicClips[index];
        musicSource.clip = previousBattleTrack;
        musicSource.Play();
    }

    public void ResumePreviousBattleMusic()
    {
        if (previousBattleTrack != null)
        {
            musicSource.clip = previousBattleTrack;
            musicSource.Play();
        }
    }

    public void PlayVictorySoundThenPostTrack()
    {
        StartCoroutine(PlayVictoryAndContinue());
    }

    private IEnumerator PlayVictoryAndContinue()
    {
        musicSource.Stop();
        sfxSource.PlayOneShot(victoryClip);
        yield return new WaitForSeconds(victoryClip.length);
        musicSource.clip = postVictoryClip;
        musicSource.Play();
    }

    public void PlayLossSound()
    {
        musicSource.Stop();
        sfxSource.PlayOneShot(lossClip);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null) sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(string clipName)
    {
        if (clipName == "attack" && attackClip != null)
        {
            sfxSource.PlayOneShot(attackClip);
        }
        else
        {
            Debug.LogWarning($"SFX '{clipName}' not found.");
        }
    }
}
