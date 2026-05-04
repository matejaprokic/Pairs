using UnityEngine;

public class MusicManager : MonoBehaviour 
{
    private static MusicManager instance;
    private AudioSource _audioSource;

    private void Awake()
    {
       
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        _audioSource = GetComponent<AudioSource>();
    }
}
