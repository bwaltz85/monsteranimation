using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip[] battleMusicClips;
    public AudioClip victoryClip;
    public AudioClip postVictoryClip;
    public AudioClip lossClip;

    [Header("SFX Clips")]
    public AudioClip attackClip;
    public AudioClip buffClip;
    public AudioClip debuffClip;
    public AudioClip healClip;

    [Header("SFX Volumes (0 to 1)")]
    [Range(0f, 1f)] public float attackVolume = 1f;
    [Range(0f, 1f)] public float buffVolume = 0.5f;
    [Range(0f, 1f)] public float debuffVolume = 0.5f;
    [Range(0f, 1f)] public float healVolume = 0.5f;

    private Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, float> sfxVolumes = new Dictionary<string, float>();

    private AudioClip previousBattleTrack;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log($"Destroying duplicate AudioManager instance: {gameObject.name}");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log($"AudioManager instance created: {gameObject.name}");

        // Register SFX clips
        sfxClips["attack"] = attackClip;
        sfxClips["buff"] = buffClip;
        sfxClips["debuff"] = debuffClip;
        sfxClips["heal"] = healClip;

        // Register SFX volumes
        sfxVolumes["attack"] = attackVolume;
        sfxVolumes["buff"] = buffVolume;
        sfxVolumes["debuff"] = debuffVolume;
        sfxVolumes["heal"] = healVolume;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            Debug.Log("AudioManager instance destroyed.");
        }
    }

    public void PlayMainMenuMusic()
    {
        if (Instance != this)
        {
            Debug.LogWarning("PlayMainMenuMusic called on non-Instance AudioManager. Exiting.");
            return;
        }

        if (mainMenuMusic == null || musicSource == null)
        {
            Debug.LogWarning("MainMenuMusic or musicSource is null. Cannot play main menu music.");
            return;
        }

        Debug.Log("Playing main menu music.");
        if (musicSource.isPlaying)
        {
            Debug.Log("Stopping current music before playing main menu music.");
            musicSource.Stop();
        }

        musicSource.clip = mainMenuMusic;
        musicSource.enabled = true;
        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        if (Instance != this)
        {
            Debug.LogWarning("PlaySFX called on non-Instance AudioManager. Exiting.");
            return;
        }

        if (sfxClips.ContainsKey(name) && sfxClips[name] != null)
        {
            float volume = sfxVolumes.ContainsKey(name) ? sfxVolumes[name] : 1f;
            if (sfxSource != null)
            {
                sfxSource.enabled = true;
                sfxSource.PlayOneShot(sfxClips[name], volume);
            }
        }
        else
        {
            Debug.LogWarning($"SFX '{name}' not found.");
        }
    }

    public void PlayRandomBattleTrack()
    {
        if (Instance != this)
        {
            Debug.LogWarning("PlayRandomBattleTrack called on non-Instance AudioManager. Exiting.");
            return;
        }

        if (battleMusicClips.Length == 0 || musicSource == null)
        {
            Debug.LogWarning("battleMusicClips is empty or musicSource is null. Cannot play battle music.");
            return;
        }

        Debug.Log("Playing random battle track.");
        if (musicSource.isPlaying)
        {
            Debug.Log("Stopping current music before playing battle track.");
            musicSource.Stop();
        }

        previousBattleTrack = battleMusicClips[Random.Range(0, battleMusicClips.Length)];
        musicSource.clip = previousBattleTrack;
        musicSource.enabled = true;
        musicSource.Play();
        Debug.Log($"Playing battle track: {previousBattleTrack.name}");
    }

    public void ResumePreviousBattleMusic()
    {
        if (Instance != this)
        {
            Debug.LogWarning("ResumePreviousBattleMusic called on non-Instance AudioManager. Exiting.");
            return;
        }

        if (previousBattleTrack != null && musicSource != null)
        {
            Debug.Log($"Resuming previous battle track: {previousBattleTrack.name}");
            musicSource.clip = previousBattleTrack;
            musicSource.enabled = true;
            musicSource.Play();
        }
    }

    public void PlayVictorySoundThenPostTrack()
    {
        if (Instance != this)
        {
            Debug.LogWarning("PlayVictorySoundThenPostTrack called on non-Instance AudioManager. Exiting.");
            return;
        }

        StartCoroutine(PlayVictoryAndContinue());
    }

    IEnumerator PlayVictoryAndContinue()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            Debug.Log("Stopping current music before playing victory sound.");
            musicSource.Stop();
        }

        if (sfxSource != null && victoryClip != null)
        {
            sfxSource.enabled = true;
            sfxSource.PlayOneShot(victoryClip);
            Debug.Log("Playing victory sound.");
        }

        yield return new WaitForSeconds(victoryClip != null ? victoryClip.length + 0.5f : 0.5f);

        if (musicSource != null && postVictoryClip != null)
        {
            musicSource.clip = postVictoryClip;
            musicSource.enabled = true;
            musicSource.Play();
            Debug.Log("Playing post-victory track.");
        }
    }

    public void PlayLossSound()
    {
        if (Instance != this)
        {
            Debug.LogWarning("PlayLossSound called on non-Instance AudioManager. Exiting.");
            return;
        }

        if (musicSource != null && musicSource.isPlaying)
        {
            Debug.Log("Stopping current music before playing loss sound.");
            musicSource.Stop();
        }

        if (sfxSource != null && lossClip != null)
        {
            sfxSource.enabled = true;
            sfxSource.PlayOneShot(lossClip);
            Debug.Log("Playing loss sound.");
        }
    }
}