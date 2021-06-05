using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //public AudioClip[] AudioClips;
    //public AudioClip[] TensNameClips;
    public AudioClip[] BackGroundMusic;
    public static AudioManager instance;
    public AudioClip GameCompleteSound;
    public AudioClip LevelComplete;
    public AudioClip WrongAns;

    public AudioClip[] GL_Audio_Clips;
    public AudioClip[] Tens_Audio_Clips;
    //bool isPlayAudio = false;

    private void Awake()
    {

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBackGroundMusic(0);
    }



    int count = 0;
    private void Update()
    {
        //if (isPlayAudio)
        //{
            if (!GetComponent<AudioSource>().isPlaying)
            {
                if (count == 1)
                {
                    count = 0;
                }
                else
                {
                    count = 1;
                }
                PlayBackGroundMusic(count);
            }
        //}
    }

    public void PlayBackGroundMusic(int value)
    {
        //print("------PlayBackGroundMusic------->>>>>>>");
        if (PlayerPrefs.GetInt("PlaySound", 0) == 0)
        {
            GetComponent<AudioSource>().clip = BackGroundMusic[value];
            GetComponent<AudioSource>().Play();
        }
    }

    public void StopMusic()
    {
        GetComponent<AudioSource>().Stop();
    }

    public void PauseAudio(bool value)
    {
        print("------PauseAudio---------->>>>>>");
        if (value)
        {
            //GetComponent<AudioSource>().Pause();
            GetComponent<AudioSource>().mute = true;
            //print("Pause--------->>"+ GetComponent<AudioSource>().isPlaying);
        }
        else
        {
            GetComponent<AudioSource>().mute = false;
            //GetComponent<AudioSource>().Play();
            //print("Play--------->>");
        }
        //isPlayAudio = true;
    }


    private void OnApplicationFocus(bool focus)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        print("--------focus----->>>>>>>>>>>"+ focus);
        if (focus)
        {
            Time.timeScale = 1;
            AudioListener.volume = 1;
            AudioListener.pause = false;
        }
        else
        {
            Time.timeScale = 0;
            AudioListener.volume = 0;
            AudioListener.pause = true;
        }
#endif
    }
}
