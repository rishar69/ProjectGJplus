using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] EventReference menuMusic;
    [SerializeField] EventReference gameplayMusic;
    EventInstance currentMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public void PlayGameplayMusic()
{
    // Stop music yang sedang jalan (menu/music lain)
    if (currentMusic.isValid())
        currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

    // Mulai gameplay music
    currentMusic = RuntimeManager.CreateInstance(gameplayMusic);
    currentMusic.start();
}


    public void PlayMenuMusic()
    {
        if (currentMusic.isValid()) currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        currentMusic = RuntimeManager.CreateInstance(menuMusic);
        currentMusic.start();
    }


    public void PlayOneShot(EventReference sound, Vector3 pos)
    {
        RuntimeManager.PlayOneShot(sound, pos);
    }
}
