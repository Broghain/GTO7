using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioMenuController : MonoBehaviour {

    [SerializeField]
    private Slider soundFxSlider;
    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private AudioClip testFxClip;
    private float testFxTimer = 0.0f;
    private float testFxTime = 0.0f;

    private float lastFxVolume;
    private float lastMusicVolume;

	// Use this for initialization
	void Start () 
    {
        soundFxSlider.value = AudioManager.instance.GetSoundVolume() * soundFxSlider.maxValue;
        musicSlider.value = AudioManager.instance.GetMusicVolume() * musicSlider.maxValue;
        lastFxVolume = soundFxSlider.value;
        lastMusicVolume = musicSlider.value;
	}
	
	// Update is called once per frame
	void Update () 
    {
        testFxTimer = Time.realtimeSinceStartup;
        if (lastFxVolume != soundFxSlider.value)
        {
            if(testFxTime == 0.0f)
            {
                testFxTime = Time.realtimeSinceStartup + testFxClip.length / 2;
            }
            ChangeFXVolume();
            if (testFxTimer >= testFxTime)
            {
                AudioManager.instance.PlaySound(testFxClip);
                testFxTime = 0.0f;
                lastFxVolume = soundFxSlider.value;
            }
        }

        if (lastMusicVolume != musicSlider.value)
        {
            ChangeMusicVolume();
        }
	}

    private void ChangeFXVolume()
    {
        AudioManager.instance.SetFXVolume(soundFxSlider.value / soundFxSlider.maxValue);
    }
    
    private void ChangeMusicVolume()
    {
        AudioManager.instance.SetMusicVolume(musicSlider.value / musicSlider.maxValue);
    }
}
