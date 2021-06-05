using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public GameObject LevelComPanel;
    public GameObject LevelCompleteAnimaton;
    public Sprite[] SoundOnOff;
    public Image Musicimage;
    public GameObject HintCompletePanel;
    public GameObject PlayAgainPanel;
    public GameObject PreviousPanel;
    public GameObject RewardPanel;
    public Text ScoreText;
    public GameObject HomePanel;
    public GameObject SkipBtn;
    public GameObject SkipPannels;

    public Image BackButton;
    public Sprite BackOn;
    public Sprite Backoff;

    public Image HomeButton;
    public Sprite HomeOn;
    public Sprite Homeoff;

    public Image HintButton;
    public Sprite HintOn;
    public Sprite Hintoff;

    public static CanvasManager instance;

    public int TotalScore
    {
        get
        {
            return PlayerPrefs.GetInt("Score", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Score", value);
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public int PlaySoundCount
    {
        get
        {
            return PlayerPrefs.GetInt("PlaySound", 0);
        }
        set
        {
            PlayerPrefs.SetInt("PlaySound", value);
        }
    }

    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        FixAudio();
#endif
        if (PlaySoundCount == 0)
        {
            Musicimage.sprite = SoundOnOff[0];
        }
        else
        {
            Musicimage.sprite = SoundOnOff[1];
        }
    }

    [DllImport("__Internal")]
    private static extern void FixAudio();

    public void OnOffMusic()
    {
        //print("PlaySount-------->>" + PlayerPrefs.GetInt("PlaySound", 0));
        if (PlaySoundCount == 0)
        {
            PlaySoundCount = 1;
            Musicimage.sprite = SoundOnOff[1];
            AudioManager.instance.StopMusic();
        }
        else
        {
            PlaySoundCount = 0;
            Musicimage.sprite = SoundOnOff[0];
            AudioManager.instance.PlayBackGroundMusic(0);
        }
    }

    public void OpenLevel1()
    {
        SceneManager.LoadScene("Level1");
        if (FindObjectOfType<DontDestroyRandomNoList>() != null)
        {
            Destroy(FindObjectOfType<DontDestroyRandomNoList>().gameObject);
        }
    }

    public void OpenLevel2()
    {
        SceneManager.LoadScene("Level2");
        if (FindObjectOfType<DontDestroyRandomNoList>() != null)
        {
            Destroy(FindObjectOfType<DontDestroyRandomNoList>().gameObject);
        }
    }

    public void OpenLevel3()
    {
        SceneManager.LoadScene("Level3");
        if (FindObjectOfType<DontDestroyRandomNoList>() != null)
        {
            Destroy(FindObjectOfType<DontDestroyRandomNoList>().gameObject);
        }
    }

    public void OpenLevel4()
    {
        SceneManager.LoadScene("Level4");
        if (FindObjectOfType<DontDestroyRandomNoList>() != null)
        {
            Destroy(FindObjectOfType<DontDestroyRandomNoList>().gameObject);
        }
    }
    public void SetfalseHomehintback(bool status)
    {
        if (status)
        {
            BackButton.sprite = BackOn;
            HintButton.sprite = HintOn;
            HomeButton.sprite = HomeOn;

            BackButton.GetComponent<Button>().interactable = true;
            HintButton.GetComponent<Button>().interactable = true;
            HomeButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            BackButton.sprite = Backoff;
            HintButton.sprite = Hintoff;
            HomeButton.sprite = Homeoff;

            BackButton.GetComponent<Button>().interactable = false;
            HintButton.GetComponent<Button>().interactable = false;
            HomeButton.GetComponent<Button>().interactable = false;


        }
    }

}
