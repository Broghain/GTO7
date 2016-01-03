using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ObjectPooler))]
public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    [SerializeField]
    private AudioSource musicSource;

    private ObjectPooler effectsSoundPool;

    private float soundVolume = 50.0f;
    private float musicVolume = 50.0f;

    void Awake()
    {
        if (AudioManager.instance != null && AudioManager.instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

	// Use this for initialization
	void Start () 
    {
        effectsSoundPool = GetComponent<ObjectPooler>();

        soundVolume = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        musicSource.volume = musicVolume;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void PlaySound(AudioClip clip)
    {
        GameObject audioObject = effectsSoundPool.GetNextInstance(false);
        if (audioObject != null)
        {
            AudioSource effectSource = audioObject.GetComponent<AudioSource>();
            TimeToLive ttl = audioObject.GetComponent<TimeToLive>();

            audioObject.SetActive(true);
            audioObject.name = effectsSoundPool.GetPooledPrefab().name;

            ttl.TTL = clip.length;

            effectSource.clip = clip;
            effectSource.volume = soundVolume;
            effectSource.Play();
        }
    }

    public void PlaySoundWithRandomPitch(AudioClip clip, float minPitch, float maxPitch)
    {
        GameObject audioObject = effectsSoundPool.GetNextInstance(false);
        if (audioObject != null)
        {
            AudioSource effectSource = audioObject.GetComponent<AudioSource>();
            TimeToLive ttl = audioObject.GetComponent<TimeToLive>();

            audioObject.SetActive(true);
            audioObject.name = effectsSoundPool.GetPooledPrefab().name;

            ttl.TTL = clip.length;

            effectSource.clip = clip;
            effectSource.volume = soundVolume;
            effectSource.pitch = Random.Range(minPitch, maxPitch);
            effectSource.Play();
        }
    }

    public void SetFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SoundVolume", value);
        soundVolume = value;

        List<GameObject> pooledObjects = effectsSoundPool.GetPooledObjects();
        foreach (GameObject obj in pooledObjects)
        {
            obj.GetComponent<AudioSource>().volume = soundVolume;
        }
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        musicVolume = value;
        musicSource.volume = musicVolume;
    }

    public float GetSoundVolume()
    {
        return PlayerPrefs.GetFloat("SoundVolume", 0.5f);
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }
}
