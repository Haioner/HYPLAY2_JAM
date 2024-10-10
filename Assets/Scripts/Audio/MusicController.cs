using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
    public static MusicController instance;

    [SerializeField] private AudioClip[] musics;

    private AudioSource audioSource;
    private int currentMusicIndex = 0;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayMusic();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            NextMusic();
        }
    }

    private void PlayMusic()
    {
        if (musics.Length == 0)
        {
            Debug.LogWarning("No music clips assigned.");
            return;
        }

        audioSource.clip = musics[currentMusicIndex];
        audioSource.Play();
    }

    private void NextMusic()
    {
        currentMusicIndex = (currentMusicIndex + 1) % musics.Length;
        PlayMusic();
    }

    public void SkipToNextMusic()
    {
        NextMusic();
    }

    public void SkipToPreviousMusic()
    {
        currentMusicIndex = (currentMusicIndex - 1 + musics.Length) % musics.Length;
        PlayMusic();
    }
}
