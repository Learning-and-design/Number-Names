using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class GameLevelZeroController : MonoBehaviour
{
    public GameObject[] Level0MiniLvls;
    public GameObject HandIcon;
    public GameObject HandIconDestina;

    public GameObject HandIconQuentity;
    public GameObject HandIconQuentityDestina;

    public GameObject MasCot;
    public GameObject TenNumberOneImg;
    public GameObject TenNumberOneImgDestination;
    public GameObject QuentityImg;
    public GameObject QuentityImgDestination;
    public GameObject NumberCard0;
    public GameObject ten;
    public GameObject Speech;
    public GameObject LastCardMoveDesti;
    public GameObject[] TensImages;
    public GameObject HighLightLevels;
    public GameObject LevelComPanel;
    public GameObject LevelCompleteAnimaton;
    public AudioClip[] AudioClips;
    public AudioClip[] TensNameClips;
    public bool isMascotSpeech = false;
    public GameObject BackGround;

    Vector3 TenOriginalPosi;
    Vector3 QuentityOriginalPosi;
    public static GameLevelZeroController instance;    

    private void Awake()
    {
        instance = this;

    }

    

    private void Start()
    {
        TenOriginalPosi = TenNumberOneImg.transform.position;
        QuentityOriginalPosi = QuentityImg.transform.position;
        if (PlayerPrefs.GetInt("FirstHint", 0) == 0)
        {
          
            StartCoroutine(HintBtnClick());
            PlayerPrefs.SetInt("FirstHint", 1);
            AudioManager.instance.PlayBackGroundMusic(0);

            CanvasManager.instance.HomeButton.sprite = CanvasManager.instance.Homeoff;        
            CanvasManager.instance.HomeButton.GetComponent<Button>().interactable = false;
        }
        else
        {            
            StartCoroutine(ChangeNextLevel());

            CanvasManager.instance.HomeButton.sprite = CanvasManager.instance.HomeOn;
            CanvasManager.instance.HomeButton.GetComponent<Button>().interactable = true;
        }
       
    }

    IEnumerator ChangeNextLevel()
    {
        isMascotSpeech = true;
        AudioManager.instance.PauseAudio(true);

        for (int i = 0; i < Level0MiniLvls.Length; i++)
        {
            Level0MiniLvls[i].SetActive(false);
        }
        Level0MiniLvls[currentLevel].SetActive(true);
        for(int j=0; j < currentLevel; j++)
        {
            TensImages[j].SetActive(true);
        }
        //AudioManager.instance.PauseAudio(true);
        MasCot.SetActive(false);
        PlayAudio(7);
        yield return new WaitForSeconds(AudioClips[7].length);
        AudioManager.instance.PauseAudio(false);
        
        isMascotSpeech = false;
    }

    public int currentLevel
    {
        get
        {
            return PlayerPrefs.GetInt("LevelZero", 0);
        }
        set
        {
            //if (PlayerPrefs.GetInt("LevelZero", 0) >= 9)
            //{
            //    PlayerPrefs.GetInt("LevelZero", 0);
            //}
            //else
            //{
                PlayerPrefs.SetInt("LevelZero", value);
            //}
        }
    }

    IEnumerator HintBtnClick()
    {
        for(int i=0; i< Level0MiniLvls.Length; i++)
        {
            Level0MiniLvls[i].SetActive(false);
        }
        isMascotSpeech = true;
        Level0MiniLvls[0].SetActive(true);
        MasCot.gameObject.SetActive(true);
        TenNumberOneImg.GetComponent<DragImage>().enabled = false;
        TenNumberOneImg.GetComponent<BoxCollider2D>().enabled = false;
        QuentityImg.GetComponent<DragSlotTens>().enabled = false;
        QuentityImg.GetComponent<BoxCollider2D>().enabled = false;
        AudioManager.instance.PauseAudio(true);
        PlayAudio(0);
        yield return new WaitForSeconds(AudioClips[0].length);

        PlayAudio(1);
        yield return new WaitForSeconds(AudioClips[1].length);

        PlayAudio(2);
        MasCot.SetActive(false);
        HandIcon.SetActive(true);
        HandIcon.transform.DOMove(HandIconDestina.transform.position, 2);
        TenNumberOneImg.transform.DOMove(TenNumberOneImgDestination.transform.position,2).OnComplete(()=>
        {
            HandIcon.SetActive(false);
        });
        yield return new WaitForSeconds(AudioClips[2].length +2);

        PlayAudio(3);
        HandIconQuentity.SetActive(true);
        HandIconQuentity.transform.DOMove(HandIconQuentityDestina.transform.position, 2);
        QuentityImg.transform.DOMove(QuentityImgDestination.transform.position, 2).OnComplete(()=>
        {
            HandIconQuentity.SetActive(false);
        });
        yield return new WaitForSeconds(AudioClips[3].length + 2);

        PlayAudio(4);
        ten.SetActive(true);
        yield return new WaitForSeconds(AudioClips[4].length + 2);

        //PlayAudio(5);
        //TenNumberOneImg.transform.position = TenOriginalPosi;
        //QuentityImg.transform.position = QuentityOriginalPosi;
        //TenNumberOneImg.GetComponent<DragImage>().enabled = true;
        //TenNumberOneImg.GetComponent<BoxCollider2D>().enabled = true;
        //QuentityImg.GetComponent<DragSlotTens>().enabled = true;
        //QuentityImg.GetComponent<BoxCollider2D>().enabled = true;
        //ten.SetActive(false);
        //yield return new WaitForSeconds(AudioClips[5].length);

        //PlayAudio(6);
        //NumberCard0.transform.GetChild(0).gameObject.SetActive(true);

        CanvasManager.instance.HintCompletePanel.SetActive(true);
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = false;
        PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[2]);
        yield return new WaitForSeconds(AudioManager.instance.GL_Audio_Clips[2].length + 1);
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;
        isMascotSpeech = false;
        AudioManager.instance.PauseAudio(false);


        CanvasManager.instance.HomeButton.sprite = CanvasManager.instance.HomeOn;
        CanvasManager.instance.HomeButton.GetComponent<Button>().interactable = true;
    }

    public IEnumerator CheckLevelIsComlete()
    {
        if (Level0MiniLvls[currentLevel].GetComponent<CheckLevelComplete>().CheckTensAndCardDone())
        {
            Level0MiniLvls[currentLevel].transform.Find("number_labels").gameObject.SetActive(true);
            PlayTensAudio(currentLevel);
            yield return new WaitForSeconds(TensNameClips[currentLevel].length + 2);

            //NumberCard0.transform.Find(currentLevel.ToString()).GetComponent<SpriteRenderer>().sortingOrder = 5;
            NumberCard0.transform.Find(currentLevel.ToString()).gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            NumberCard0.transform.Find(currentLevel.ToString()).transform.DOMove(TensImages[currentLevel].transform.position, 1).OnStart(() =>
            {
                NumberCard0.transform.Find(currentLevel.ToString()).gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            }).OnComplete(() =>
            {
                NumberCard0.transform.Find(currentLevel.ToString()).gameObject.SetActive(false);
                TensImages[currentLevel].SetActive(true);
            });
            //int soundRandom = Random.Range(0, 3);
            //if (soundRandom == 0)
            //{
            //    GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.LevelComplete);
            //    yield return new WaitForSeconds(AudioManager.instance.LevelComplete.length + 1);
            //}
            if (currentLevel < 8)
            {
                currentLevel++;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                StartCoroutine(highLightTenslevels());
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (PlayerPrefs.GetInt("FirstHint", 0) == 0)
            {
                StartCoroutine(HintBtnClick());
                PlayerPrefs.SetInt("FirstHint", 1);
                AudioManager.instance.PlayBackGroundMusic(0);
            }
        }
    }


    int HighLightCount = 0;
    IEnumerator highLightTenslevels()
    {
        NumberCard0.SetActive(false);
        Level0MiniLvls[currentLevel].SetActive(false);
        BackGround.GetComponent<SpriteRenderer>().enabled = false;
        for(int j=0;j< HighLightLevels.transform.childCount; j++)
        {
            TensImages[j].transform.position = new Vector3(0, TensImages[j].transform.position.y, 0);
            HighLightLevels.transform.GetChild(j).gameObject.SetActive(false);
            TensImages[j].transform.GetChild(0).gameObject.SetActive(false);
            TensImages[j].transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        }

        TensImages[HighLightCount].transform.GetChild(0).gameObject.SetActive(true);
        //TensImages[HighLightCount].transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        //HighLightLevels.transform.GetChild(HighLightCount).gameObject.SetActive(true);
        //HighLightLevels.transform.GetChild(HighLightCount).Find("number_labels").gameObject.SetActive(true);
        PlayTensAudio(HighLightCount);
        yield return new WaitForSeconds(TensNameClips[HighLightCount].length + 2);
        if (HighLightCount < 8)
        {
            HighLightCount++;
            StartCoroutine(highLightTenslevels());
        }
        else
        {
            //GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.GameCompleteSound);
            //LevelCompleteAnimaton.SetActive(true);
            yield return new WaitForSeconds(1);
            //yield return new WaitForSeconds(2);
            LevelComPanel.SetActive(true);
        }
    }


    public void PlayAgainBtn()
    {
        currentLevel = 0;
        LevelComPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayGame()
    {
        LevelComPanel.SetActive(false);
        SceneManager.LoadScene("Level1");
    }

    public void PlayAudio(int value)
    {
        //GetComponent<AudioSource>().clip = AudioClips[value];
        //GetComponent<AudioSource>().Play();
        //if (CanvasManager.instance.PlaySoundCount == 0)
        //{
            GetComponent<AudioSource>().PlayOneShot(AudioClips[value]);
        //}
    }

    public void PlayTensAudio(int value)
    {
        //GetComponent<AudioSource>().clip = TensNameClips[value];
        //if (CanvasManager.instance.PlaySoundCount == 0)
        //{
            GetComponent<AudioSource>().PlayOneShot(TensNameClips[value]);
        //}
    }

    public void HintPlayAgainClick()
    {
        PlayerPrefs.SetInt("FirstHint", 0);
        //isSkipEnable = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void HintNewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayRandomAudioClip(AudioClip clip)
    {
        //if (CanvasManager.instance.PlaySoundCount == 0)
        //{
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
        //GetComponent<AudioSource>().PlayOneShot(clip);
        //}
    }

    public void PrevioClick()
    {
        if (!isMascotSpeech)
        {
            StartCoroutine(previousBtnClick());
        }
    }

    IEnumerator previousBtnClick()
    {
        //PreviousMascotPanel.SetActive(true);
        //Time.timeScale = 0;
        CanvasManager.instance.PreviousPanel.SetActive(true);
        PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[1]);
        CanvasManager.instance.PreviousPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
        CanvasManager.instance.PreviousPanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = false;
        yield return new WaitForSecondsRealtime(AudioManager.instance.GL_Audio_Clips[1].length + 1);
        CanvasManager.instance.PreviousPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
        CanvasManager.instance.PreviousPanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;
    }

    public void NewGameBtn()
    {
        SceneManager.LoadScene("Level1");
    }

    public void HomeBtnClick()
    {
        if(!isMascotSpeech)
        {
            StartCoroutine(HomeButtonClick());

        }        
    }
    IEnumerator HomeButtonClick()
    {      
        CanvasManager.instance.HomePanel.SetActive(true);
        PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[0]);
        CanvasManager.instance.HomePanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
        CanvasManager.instance.HomePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = false;
        yield return new WaitForSecondsRealtime(AudioManager.instance.GL_Audio_Clips[0].length + 1);
        CanvasManager.instance.HomePanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
        CanvasManager.instance.HomePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;
    }

}
