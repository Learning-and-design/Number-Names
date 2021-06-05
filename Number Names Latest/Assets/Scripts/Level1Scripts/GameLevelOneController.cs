using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameLevelOneController : MonoBehaviour
{
    public GameObject HandIcon;
    public GameObject HandIconDestina;
    public GameObject MasCot;
    public AudioClip[] AudioClips;
    public AudioClip[] TensNameClips;
    public Sprite[] TensLabelNames;
    public GameObject TrollyMen;

    public GameObject ThirtyNoBlock;
    public GameObject ThirtyNoBlockDesti;
    public GameObject AudioReplayIcon;
    //[HideInInspector]
    //public List<int> checkRandomNo = new List<int>();
    public Slider LevelOneSlider;
    public GameObject SliderFill;
    public GameObject BottomTrollyBlock;
    public AudioClip TryAgain;
    public static bool isPlayAgain = false;
    public bool isLevel1MascotisSpeech = false;
    public AudioClip[] GlAudioList;
    public GameObject PreviousPanel;
    public GameObject PrevoiusMascot;
    public GameObject QueHandIcon;
    Coroutine awayfromKeyBoardcoroutine;
    public static bool isSkipEnable = false;

    public static GameLevelOneController instance;

    private void Awake()
    {
        if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "isPlayAgain", 0) == 0)
        {
            isPlayAgain = false;
        }
        else
        {
            isPlayAgain = true;

            LevelCompCount = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "LevelCompCount", 0);

            if (LevelCompCount > 6)
            {
                LevelCompCountSequence = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "LevelCompCountSequence", 0);
                SliderFill.SetActive(true);
                LevelOneSlider.value = 0.75f + (float)(LevelCompCountSequence) * (0.25f / 3f);                  
            }
            else
            {
                if (LevelCompCount > 0)
                {
                    SliderFill.SetActive(true);
                    LevelOneSlider.value = (float)(LevelCompCount) * (0.75f / 6f);                    
                }
                else
                {
                    SliderFill.SetActive(false);
                }               
            }
        }
        print("-------------------------->>>>>>" + PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "LevelComplete", 0));
        if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "LevelComplete", 0) == 1)
        {
            SliderFill.SetActive(true);
            LevelOneSlider.value = 1f;
            print("-------------------------->>>>>>" + LevelOneSlider.value);
        }
        instance = this;
    }

    public int currentLevelOne
    {
        get
        {
            return PlayerPrefs.GetInt("LevelOne", 0);
        }
        set
        {
            PlayerPrefs.SetInt("LevelOne", value);
        }
    }

    private void Start()
    {
      

        Time.timeScale = 1;
        if (PlayerPrefs.GetInt("FirstHintLevelOne", 0) == 0)
        {
            HintBtn();
            PlayerPrefs.SetInt("FirstHintLevelOne", 1);
            if (isSkipEnable)
            {
                CanvasManager.instance.SkipBtn.SetActive(true);
            }
            CanvasManager.instance.SetfalseHomehintback(false);
        }
        else
        {
            isSkipEnable = false;
            CanvasManager.instance.SkipBtn.SetActive(false);
            StartCoroutine(ChangeNextLevel());
            CanvasManager.instance.SetfalseHomehintback(true);

        }
     
        CanvasManager.instance.ScoreText.text = CanvasManager.instance.TotalScore.ToString();
    }

    int RandomLevelNo;
   public static int CurrentRandomNo;
    IEnumerator ChangeNextLevel()
    {
        isLevel1MascotisSpeech = true;
        AudioManager.instance.PauseAudio(true);

        for (int i = 0; i < 9; i++)
        {
            int random = Random.Range(0, 9);
            Vector3 position = BottomTrollyBlock.transform.GetChild(i).transform.position;
            BottomTrollyBlock.transform.GetChild(i).transform.position = BottomTrollyBlock.transform.GetChild(random).transform.position;
            BottomTrollyBlock.transform.GetChild(random).transform.position = position;

            BottomTrollyBlock.transform.GetChild(i).GetComponent<DragTrollyBlock>().Start();
            BottomTrollyBlock.transform.GetChild(random).GetComponent<DragTrollyBlock>().Start();
        }
        

        if (isPlayAgain)
        {
            //if (LevelCompCount > 0)
            //{
            //    SliderFill.SetActive(true);
            //}
            //LevelOneSlider.value = (float)(LevelCompCount) * (1f / 3f);
        }
        else
        {
            //if (LevelCompCount > 0)
            //{
            //    SliderFill.SetActive(true);
            //}
            //LevelOneSlider.value = (float)(LevelCompCount) * (1f / 9f);
        }

        //DOTween.To(() => LevelOneSlider.value, x => LevelOneSlider.value = x, (float)(LevelCompCount) * (1f / 3f), 0.1f).OnStart(() =>
        //{
        //    SliderFill.SetActive(true);
        //});
        if (CurrentRandomNo == 0)
        {
            //RandomLevelNo = Random.Range(0, DontDestroyRandomNoList.instance.checkRandomNo.Count - 1);
            //yield return new WaitForSeconds(0.1f);
            //CurrentRandomNo = DontDestroyRandomNoList.instance.checkRandomNo[RandomLevelNo];
            ChangerandomNo();
        }else if (isPlayAgain)
        {
            ChangerandomNo();
        }
        PlayerPrefs.GetInt("CurrentRandomNo", CurrentRandomNo);
        DontDestroyRandomNoList.instance.checkRandomNo.Remove(CurrentRandomNo);
        PlayAudio(5);
        TrollyMen.GetComponent<MinerTrolleyMovement>().enabled = true;
        yield return new WaitForSeconds(AudioClips[5].length);
        PlayTensAudio(CurrentRandomNo - 1);
        TrollyMen.transform.Find("number_labels").gameObject.SetActive(true);
        TrollyMen.transform.Find("number_labels").GetComponent<SpriteRenderer>().sprite = TensLabelNames[CurrentRandomNo - 1];
        yield return new WaitForSeconds(TensNameClips[CurrentRandomNo - 1].length);
        AudioReplayIcon.SetActive(true);
        AudioReplayIcon.GetComponent<Animator>().enabled = false;
        isLevel1MascotisSpeech = false;
        AudioManager.instance.PauseAudio(false);

        if (awayfromKeyBoardcoroutine != null)
        {
            StopCoroutine(awayfromKeyBoardcoroutine);
        }
        awayfromKeyBoardcoroutine = StartCoroutine(checkawayfromKeyBoard());
      
    }

    public void ChangerandomNo()
    {
        //if (DontDestroyRandomNoList.instance.checkRandomNo.Count != 0)
        //{
            RandomLevelNo = Random.Range(0, DontDestroyRandomNoList.instance.checkRandomNo.Count - 1);
            //yield return new WaitForSeconds(0.1f);
            CurrentRandomNo = DontDestroyRandomNoList.instance.checkRandomNo[RandomLevelNo];
        //}
        //else
        //{
        //    DontDestroyRandomNoList.instance.Refill();

        //    RandomLevelNo = Random.Range(0, DontDestroyRandomNoList.instance.checkRandomNo.Count - 1);
        //    //yield return new WaitForSeconds(0.1f);
        //    CurrentRandomNo = DontDestroyRandomNoList.instance.checkRandomNo[RandomLevelNo];
        //}
    }

    public void HintBtn()
    {
        StartCoroutine(HIntBtnLevelOne());
    }

    IEnumerator HIntBtnLevelOne()
    {
        //AudioManager.instance.PauseAudio(true);
        isLevel1MascotisSpeech = true;
        TrollyMen.GetComponent<MinerTrolleyMovement>().enabled = false;
        PlayAudio(0);
        TrollyMen.SetActive(false);
        BottomTrollyBlock.SetActive(false);
        MasCot.SetActive(true);
        yield return new WaitForSeconds(AudioClips[0].length);
        PlayAudio(1);
        yield return new WaitForSeconds(AudioClips[1].length);
        MasCot.SetActive(false);
        TrollyMen.SetActive(true);
        BottomTrollyBlock.SetActive(true);
        TrollyMen.transform.DOMove(new Vector3(0, 0.135f, 0), 2);
        PlayAudio(2);
        TrollyMen.transform.Find("number_labels").gameObject.SetActive(true);
        TrollyMen.transform.Find("number_labels").GetComponent<SpriteRenderer>().sprite = TensLabelNames[2];
        yield return new WaitForSeconds(AudioClips[2].length);
        HandIcon.SetActive(true);
        HandIcon.transform.DOMove(HandIconDestina.transform.position, 3);
        ThirtyNoBlock.transform.DOMove(ThirtyNoBlockDesti.transform.position, 3).OnComplete(()=>
        {
            ThirtyNoBlock.transform.parent = TrollyMen.transform;
            HandIcon.SetActive(false);
        });
        yield return new WaitForSeconds(3);
        PlayAudio(2);
        TrollyMen.GetComponent<MinerTrolleyMovement>().enabled = true;
        yield return new WaitForSeconds(AudioClips[2].length);
        PlayAudio(3);
        AudioReplayIcon.SetActive(true);
        yield return new WaitForSeconds(AudioClips[3].length);
        PlayAudio(4);
        AudioReplayIcon.GetComponent<Animator>().enabled = true;
        QueHandIcon.SetActive(true);
        yield return new WaitForSeconds(AudioClips[4].length);
        QueHandIcon.SetActive(false);
        AudioReplayIcon.GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(2);
        TrollyMen.transform.DOLocalMove(new Vector3(7f, 0.135f, 0), 3);
        yield return new WaitForSeconds(3);
        CanvasManager.instance.HintCompletePanel.SetActive(true);
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = false;
        PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[2]);
        yield return new WaitForSeconds(AudioManager.instance.GL_Audio_Clips[2].length + 1);
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;
        isLevel1MascotisSpeech = false;
        AudioManager.instance.PauseAudio(false);
        //AudioManager.instance.PauseAudio(false);
        CanvasManager.instance.SetfalseHomehintback(true);
    }

    public void PlayAudio(int value)
    {
        //if (CanvasManager.instance.PlaySoundCount == 0)
        //{
            GetComponent<AudioSource>().clip = AudioClips[value];
            GetComponent<AudioSource>().Play();
            //GetComponent<AudioSource>().PlayOneShot(AudioClips[value]);
        //}
    }

    public void PlayTensAudio(int value)
    {
        //if (CanvasManager.instance.PlaySoundCount == 0)
        //{
            GetComponent<AudioSource>().clip = TensNameClips[value];
            GetComponent<AudioSource>().Play();
            //GetComponent<AudioSource>().PlayOneShot(TensNameClips[value]);
        //}
    }


    public void GameCompleteSound()
    {
        //if (CanvasManager.instance.PlaySoundCount == 0)
        //{
            GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.GameCompleteSound);
        //}

    }

    public void LevelCompleteSound(AudioClip clip)
    {
        //if (CanvasManager.instance.PlaySoundCount == 0)
        //{
            GetComponent<AudioSource>().PlayOneShot(clip);
        //}
    }

    static int LevelCompCount = 0;
    static int LevelCompCountSequence = 0;

    public IEnumerator CheckLevelComplete()
    {
        //print("Currentno--------->>" + CurrentRandomNo + "-----Name---->" + int.Parse(TrollyMen.transform.GetChild(1).gameObject.name));
        if (CurrentRandomNo == int.Parse(TrollyMen.transform.GetChild(1).gameObject.name))
        {
            PlayTensAudio(CurrentRandomNo - 1);
            yield return new WaitForSeconds(3);
            TrollyMen.transform.DOLocalMove(new Vector3(7f, 0.135f, 0), 3);
            yield return new WaitForSeconds(3);
            //yield return new WaitForSeconds(TensNameClips[CurrentRandomNo - 1].length + 1);

            //int soundRandom = Random.Range(0, 3);
            //if (soundRandom == 0)
            //{
            //    LevelCompleteSound(AudioManager.instance.LevelComplete);
            //    yield return new WaitForSeconds(AudioManager.instance.LevelComplete.length + 1);
            //}

            //print("ispLayAgain---------------->>" + isPlayAgain);
            if (isPlayAgain)
            {
                //print("SecondSenario---------->>" + LevelCompCount);
                //Second Senario----------------
                //if (DontDestroyRandomNoList.instance.checkRandomNo.Count != 0)
                //{

                //print("LeveCOmCount --------->>" + LevelCompCount);
                //if (LevelCompCount < 3)
                //{
                LevelCompCount++;

                if (LevelCompCount > 6)
                {
                    LevelCompCountSequence++;

                    if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "LevelComplete", 0) == 0)
                    {
                        if (LevelOneSlider.value < 0.75f + (float)(LevelCompCountSequence) * (0.25f / 3f))
                        {
                            DOTween.To(() => LevelOneSlider.value, x => LevelOneSlider.value = x, 0.75f + (float)(LevelCompCountSequence) * (0.25f / 3f), 0.5f).OnStart(() =>
                            {
                                SliderFill.SetActive(true);
                            });
                        }
                    }

                    PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCountSequence", LevelCompCountSequence);
                }
                else
                {
                    if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "LevelComplete", 0) == 0)
                    {
                        DOTween.To(() => LevelOneSlider.value, x => LevelOneSlider.value = x, (float)(LevelCompCount) * (0.75f / 6f), 0.5f).OnStart(() =>
                        {
                            SliderFill.SetActive(true);
                        });
                    }
                }
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCount", LevelCompCount);

                if (FindObjectOfType<DontDestroyRandomNoList>().checkRandomNo.Count == 0)
                {
                    Destroy(FindObjectOfType<DontDestroyRandomNoList>().gameObject);
                }

                if (LevelCompCountSequence == 3)
                {
                    CanvasManager.instance.RewardPanel.SetActive(true);
                    yield return new WaitForSeconds(3);
                    CanvasManager.instance.RewardPanel.SetActive(false);
                    CanvasManager.instance.TotalScore += 10;
                    CanvasManager.instance.ScoreText.text = CanvasManager.instance.TotalScore.ToString();
                    CanvasManager.instance.LevelCompleteAnimaton.SetActive(true);
                    GameCompleteSound();
                    yield return new WaitForSeconds(AudioManager.instance.GameCompleteSound.length + 3);
                    CanvasManager.instance.LevelComPanel.SetActive(true);

                    AudioManager.instance.PauseAudio(true);
                    LevelCompleteSound(GlAudioList[8]);
                    CanvasManager.instance.LevelComPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
                    CanvasManager.instance.LevelComPanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = false;
                    yield return new WaitForSecondsRealtime(GlAudioList[8].length);
                    CanvasManager.instance.LevelComPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
                    CanvasManager.instance.LevelComPanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;
                    AudioManager.instance.PauseAudio(false);

                    PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelComplete", 1);
                }
                else
                {
                    yield return new WaitForSeconds(1);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                //}

                //else
                //{
                //    yield return new WaitForSeconds(2);
                //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //}
                //}
            }
            else
            {

                //First Senario----------------
                if (DontDestroyRandomNoList.instance.checkRandomNo.Count == 0)
                {
                    //DOTween.To(() => LevelOneSlider.value, x => LevelOneSlider.value = x, 1, 1).OnStart(() =>
                    //{
                    //    SliderFill.SetActive(true);
                    //});
                    CanvasManager.instance.PlayAgainPanel.SetActive(true);

                    AudioManager.instance.PauseAudio(true);
                    PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[3]);
                    CanvasManager.instance.PlayAgainPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
                    yield return new WaitForSeconds(AudioManager.instance.GL_Audio_Clips[3].length);
                    CanvasManager.instance.PlayAgainPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;                  
                    AudioManager.instance.PauseAudio(false);

                   
                   
                }
                else
                {
                    if (LevelCompCount < 9)
                    {
                        LevelCompCount++;
                        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCount", LevelCompCount);
                        //DOTween.To(() => LevelOneSlider.value, x => LevelOneSlider.value = x, (float)(LevelCompCount) * (1f / 9f), 1).OnStart(() =>
                        //{
                        //    SliderFill.SetActive(true);
                        //});
                        yield return new WaitForSeconds(1);
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                    ChangerandomNo();
                    yield return new WaitForSeconds(1);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }
        else
        {

            if (isPlayAgain)
            {
                LevelCompleteSound(AudioManager.instance.WrongAns);
                TrollyMen.transform.GetChild(1).transform.DOMove(TrollyMen.transform.GetChild(1).GetComponent<DragTrollyBlock>().startPosition, 1).OnComplete(() =>
                {
                    TrollyMen.transform.GetChild(1).GetComponent<DragTrollyBlock>().isDoNotMove = false;
                    TrollyMen.transform.GetChild(1).gameObject.transform.parent = BottomTrollyBlock.transform;
                });


                LevelCompCountSequence = -1;
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCountSequence", LevelCompCountSequence);
               
               
                yield return new WaitForSeconds(AudioManager.instance.WrongAns.length);
                LevelCompleteSound(TryAgain);
                yield return new WaitForSeconds(TryAgain.length);
                //DontDestroyRandomNoList.instance.checkRandomNo.Add(CurrentRandomNo);
                //DontDestroyRandomNoList.instance.checkRandomNo.Clear();
                //for (int i = 1; i <= 9; i++)
                //{
                //    DontDestroyRandomNoList.instance.checkRandomNo.Add(i);
                //}
                //DontDestroyRandomNoList.instance.checkRandomNo.Remove(CurrentRandomNo);
                //yield return new WaitForSeconds(2);
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {               
                if (LevelCompCount >= 7)
                {
                    LevelCompleteSound(AudioManager.instance.WrongAns);
                    TrollyMen.transform.GetChild(1).transform.DOMove(TrollyMen.transform.GetChild(1).GetComponent<DragTrollyBlock>().startPosition, 1).OnComplete(() =>
                    {
                        TrollyMen.transform.GetChild(1).GetComponent<DragTrollyBlock>().isDoNotMove = false;
                        TrollyMen.transform.GetChild(1).gameObject.transform.parent = BottomTrollyBlock.transform;
                    });
                    yield return new WaitForSeconds(AudioManager.instance.WrongAns.length);
                    LevelCompleteSound(TryAgain);
                    yield return new WaitForSeconds(TryAgain.length);
                    //DontDestroyRandomNoList.instance.checkRandomNo.Add(CurrentRandomNo);
                    //DontDestroyRandomNoList.instance.checkRandomNo.Clear();
                    //for (int i = 1; i <= 9; i++)
                    //{
                    //    DontDestroyRandomNoList.instance.checkRandomNo.Add(i);
                    //}
                    //DontDestroyRandomNoList.instance.checkRandomNo.Remove(CurrentRandomNo);
                    //ChangerandomNo();
                    //yield return new WaitForSeconds(2);
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                else
                {
                    LevelCompleteSound(AudioManager.instance.WrongAns);
                    TrollyMen.transform.GetChild(1).transform.DOMove(TrollyMen.transform.GetChild(1).GetComponent<DragTrollyBlock>().startPosition, 0.2f).OnComplete(() =>
                    {
                        TrollyMen.transform.GetChild(1).GetComponent<DragTrollyBlock>().isDoNotMove = false;
                        TrollyMen.transform.GetChild(1).gameObject.transform.parent = BottomTrollyBlock.transform;
                    });
                    yield return new WaitForSeconds(AudioManager.instance.WrongAns.length);
                    LevelCompleteSound(TryAgain);
                    yield return new WaitForSeconds(TryAgain.length);
                    //DontDestroyRandomNoList.instance.checkRandomNo.Add(CurrentRandomNo);
                    //yield return new WaitForSeconds(2);
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }
    }

    public void LevelComPlayAgain()
    {

        StartCoroutine(GameLevel1PlayAgain());
    }


    IEnumerator GameLevel1PlayAgain()
    {
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "isPlayAgain", 1);
        isPlayAgain = true;
        LevelOneSlider.value = 0;
        LevelCompCount = 0;
        LevelCompCountSequence = 0;
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCount", LevelCompCount);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCountSequence", LevelCompCountSequence);
        DestroyObject(FindObjectOfType<DontDestroyRandomNoList>().gameObject);
        yield return new WaitForSeconds(1);
       
        isSkipEnable = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayAgain()
    {
        isPlayAgain = true;
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "isPlayAgain", 1);
        LevelCompCount = 0;
        LevelCompCountSequence = 0;
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCount", LevelCompCount);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCountSequence", LevelCompCountSequence);
        DestroyObject(FindObjectOfType<DontDestroyRandomNoList>().gameObject);
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

    public void HintPlayAgainClick()
    {
        PlayerPrefs.SetInt("FirstHintLevelOne", 0);
        isSkipEnable = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void HintNewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PrevoiusClick()
    {
        if (!isLevel1MascotisSpeech)
            StartCoroutine(previousBtnClick());
    }

    IEnumerator previousBtnClick()
    {
        //PrevoiusMascot.SetActive(true);
        CanvasManager.instance.PreviousPanel.SetActive(true);
        PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[1]);
        CanvasManager.instance.PreviousPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
        CanvasManager.instance.PreviousPanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = false;
        yield return new WaitForSecondsRealtime(AudioManager.instance.GL_Audio_Clips[1].length + 1);
        CanvasManager.instance.PreviousPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
        CanvasManager.instance.PreviousPanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;
    }

    public void PreviousPlayAgain()
    {
        PrevoiusMascot.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PreviousPlayGame()
    {
        Time.timeScale = 1;
        PrevoiusMascot.SetActive(false);
        PreviousPanel.SetActive(false);
    }

    public void LevelCompleteNewGame()
    {
        if (FindObjectOfType<DontDestroyRandomNoList>() != null)
        {
            Destroy(FindObjectOfType<DontDestroyRandomNoList>().gameObject);
        }

        SceneManager.LoadScene("Level2");
    }

    public void HomeBtnClick()
    {
        if (!isLevel1MascotisSpeech)
            StartCoroutine(HomeButtonClick());


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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AwayfromKeyboard(false);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (awayfromKeyBoardcoroutine != null)
            {
                StopCoroutine(awayfromKeyBoardcoroutine);
            }
            awayfromKeyBoardcoroutine = StartCoroutine(checkawayfromKeyBoard());
        }
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown)
        {
            AwayfromKeyboard(false);
        }
        if (e.type == EventType.KeyUp)
        {
            if (awayfromKeyBoardcoroutine != null)
            {
                StopCoroutine(awayfromKeyBoardcoroutine);
            }
            awayfromKeyBoardcoroutine = StartCoroutine(checkawayfromKeyBoard());
        }
    }

    IEnumerator checkawayfromKeyBoard()
    {
        yield return new WaitForSeconds(15);
        AwayfromKeyboard(true);
    }

    public void AwayfromKeyboard(bool status)
    {
        if (status)
        {
            //Complete 15 Second          
            if (!CanvasManager.instance.HintCompletePanel.activeSelf && !CanvasManager.instance.LevelComPanel.activeSelf && !CanvasManager.instance.PlayAgainPanel.activeSelf &&
                !CanvasManager.instance.PreviousPanel.activeSelf && !CanvasManager.instance.HomePanel.activeSelf)
                StartCoroutine(Second15Complete());
        }
        else
        {
            //
        }
    }

    IEnumerator Second15Complete()
    {
        isLevel1MascotisSpeech = true;
        AudioManager.instance.PauseAudio(true);


        if (!GetComponent<AudioSource>().isPlaying)
        {
            PlayAudio(1);
            yield return new WaitForSeconds(AudioClips[1].length + 1);
            PlayTensAudio(CurrentRandomNo - 1);
            //PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[CurrentRandomNo - 1]);
            yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[CurrentRandomNo - 1].length + 1);
        }
        AudioManager.instance.PauseAudio(false);
        isLevel1MascotisSpeech = false;
        if (awayfromKeyBoardcoroutine != null)
        {
            StopCoroutine(awayfromKeyBoardcoroutine);
        }
        awayfromKeyBoardcoroutine = StartCoroutine(checkawayfromKeyBoard());
    }


    public void SkipButtonClick()
    {
       
        StartCoroutine(SkipBtnClick());
    }

    IEnumerator SkipBtnClick()
    {
        AudioManager.instance.PauseAudio(true);
        GetComponent<AudioSource>().Stop();
        CanvasManager.instance.SkipPannels.SetActive(true);
        Time.timeScale = 0;

        LevelCompleteSound(GlAudioList[6]);
        CanvasManager.instance.SkipPannels.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
        CanvasManager.instance.SkipPannels.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = false;
        yield return new WaitForSecondsRealtime(GlAudioList[6].length);
        CanvasManager.instance.SkipPannels.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
        CanvasManager.instance.SkipPannels.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ContinueGame()
    {
        AudioManager.instance.PauseAudio(false);
        GetComponent<AudioSource>().Play();
        Time.timeScale = 1;
        CanvasManager.instance.SkipPannels.SetActive(false);
    }
}
