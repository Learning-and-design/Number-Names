using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameLevelFourController : MonoBehaviour
{

    public bool isLevel4MascotSpeech;
    public GameObject HandIcon;
    public GameObject HandIconDestina;
    public GameObject HandIconDone;
    public GameObject CardHandIcon;
    public GameObject CardHandIconDestina;

    public GameObject MasCot;
    public AudioClip[] AudioClips;
    public List<Sprite> TensNameImageList;
    public List<Sprite> TensImgList;
    public GameObject DoneButton;
    public GameObject QueReplayBtn;
    public static int CurrentRandomNo;
    public static bool isPlayAgain = false;
    static int LevelComCount;
    static int LevelCompCountSequence = 0;
    public GameObject number_label;
    public GameObject TensImage;
    public GameObject TensImageDestina;
    public static GameLevelFourController instance;
    public List<GameObject> fivePosition;
    public GameObject Card;
    public GameObject CardDestination;
    public GameObject BackGround;
    public GameObject PreviousMascotPanel;
    public Slider LevelFourSlider;
    public List<GameObject> LevelCompleteObj;
    public GameObject SelectCard;
    public GameObject SliderFill;
    static int wrongCount = 0;
    public int Count = 0;
    public List<GameObject> LevelFourAllSlots;
    Coroutine awayfromKeyBoardcoroutine;
    public static bool isSkipEnable = false;
    public GameObject CardDestinationHandler;
    public List<GameObject> TempFivePosi;
    public GameObject QueHandIcon;
    private void Awake()
    {
        //if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "isPlayAgain", 0) == 0)
        //{
        //    isPlayAgain = false;
        //}
        //else
        //{
        isPlayAgain = true;

        LevelComCount = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "LevelCompCount", 0);

        if (LevelComCount > 6)
        {
            LevelCompCountSequence = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "LevelCompCountSequence", 0);
            SliderFill.SetActive(true);
            LevelFourSlider.value = 0.75f + (float)(LevelCompCountSequence) * (0.25f / 3f);
        }
        else
        {
            if (LevelComCount > 0)
            {
                SliderFill.SetActive(true);
                LevelFourSlider.value = (float)(LevelComCount) * (0.75f / 6f);
            }
            else
            {
                SliderFill.SetActive(false);
            }
        }     

        if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "LevelComplete", 0) == 1)
        {
            SliderFill.SetActive(true);
            LevelFourSlider.value = 1f;
        }

        instance = this;
    }

    public int HintprefLevel4
    {
        get
        {
            return PlayerPrefs.GetInt("Level4HintPref", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Level4HintPref", value);
        }
    }

    private void Start()
    {
        
        Time.timeScale = 1;
        if (HintprefLevel4 == 0)
        {
            StartCoroutine(LevelFourHint());
            HintprefLevel4 = 1;
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
            StartCoroutine(GenerateRandomNo());
            CanvasManager.instance.SetfalseHomehintback(true);
        }
        CanvasManager.instance.ScoreText.text = CanvasManager.instance.TotalScore.ToString();
    }


    IEnumerator LevelFourHint()
    {
        AudioManager.instance.PauseAudio(true);
        isLevel4MascotSpeech = true;
        MasCot.SetActive(true);
        BackGround.SetActive(false);
        PlayRandomAudioClip(AudioClips[0]);
        yield return new WaitForSeconds(AudioClips[0].length + 1);
        PlayRandomAudioClip(AudioClips[1]);
        yield return new WaitForSeconds(AudioClips[1].length + 1);
        BackGround.SetActive(true);
        MasCot.SetActive(false);
        number_label.SetActive(true);
        number_label.GetComponent<SpriteRenderer>().sprite = TensImgList[0];
        PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[0]);
        yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[0].length + 1);
        HandIcon.SetActive(true);
        HandIcon.transform.DOMove(HandIconDestina.transform.position, 2).OnComplete(() =>
        {
            HandIcon.SetActive(false);
        });
        TensImage.transform.DOMove(TensImageDestina.transform.position, 2);
        yield return new WaitForSeconds(3);
        CardHandIcon.SetActive(true);
        CardHandIcon.transform.DOMove(CardHandIconDestina.transform.position, 2).OnComplete(() =>
        {
            CardHandIcon.SetActive(false);
        });
        Card.transform.DOMove(CardDestination.transform.position, 2).OnComplete(() =>
        {
            DoneButton.SetActive(true);
        });
        yield return new WaitForSeconds(3);
        HandIconDone.SetActive(true);
        yield return new WaitForSeconds(1);
        HandIconDone.SetActive(false);
        PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[0]);
        yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[0].length + 1);
        QueHandIcon.SetActive(true);
        QueReplayBtn.SetActive(true);
        QueReplayBtn.GetComponent<Animator>().enabled = true;
        PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[11]);
        yield return new WaitForSeconds(AudioManager.instance.GL_Audio_Clips[11].length + 1);
        QueReplayBtn.GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(1);      
        QueReplayBtn.SetActive(false);
        QueHandIcon.SetActive(false);
        CanvasManager.instance.HintCompletePanel.SetActive(true);
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = false;
        PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[2]);
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;
        yield return new WaitForSeconds(AudioManager.instance.GL_Audio_Clips[2].length + 1);
        AudioManager.instance.PauseAudio(false);
        isLevel4MascotSpeech = false;
        CanvasManager.instance.SetfalseHomehintback(true);
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

    IEnumerator GenerateRandomNo()
    {
        AudioManager.instance.PauseAudio(true);
        isLevel4MascotSpeech = true;
        yield return new WaitForSeconds(0.1f);
        BackGround.SetActive(true);
        int randNo = Random.Range(0, fivePosition.Count);
        if (isPlayAgain)
        {
            //if (LevelComCount > 0)
            //{
            //    SliderFill.SetActive(true);
            //}
            //LevelFourSlider.value = (float)(LevelComCount) * (1f / 3f);
        }
        else
        {
            //if (LevelComCount > 0)
            //{
            //    SliderFill.SetActive(true);
            //}
            //LevelFourSlider.value = (float)(LevelComCount) * (1f / 9f);
        }

        if (CurrentRandomNo == 0)
        {
            ChangerandomNo();
        }
        else if (isPlayAgain)
        {
            ChangerandomNo();
        }
        //DontDestroyRandomNoList.instance.checkRandomNo.Remove(CurrentRandomNo);


        for (int i = 0; i < 5; i++)
        {
            if (i == 0)
            {
                fivePosition[randNo].GetComponent<SpriteRenderer>().sprite = TensNameImageList[CurrentRandomNo - 1];
                TensNameImageList.Remove(TensNameImageList[CurrentRandomNo - 1]);
                fivePosition.Remove(fivePosition[randNo]);
            }
            else
            {
                int removeNo = Random.Range(0, TensNameImageList.Count);
                int posiRandom = Random.Range(0, fivePosition.Count);
                fivePosition[posiRandom].GetComponent<SpriteRenderer>().sprite = TensNameImageList[removeNo];
                TensNameImageList.Remove(TensNameImageList[removeNo]);
                fivePosition.Remove(fivePosition[posiRandom]);
            }
        }
        PlayRandomAudioClip(AudioClips[3]);
        yield return new WaitForSeconds(AudioClips[3].length);
        number_label.SetActive(true);
        number_label.GetComponent<SpriteRenderer>().sprite = TensImgList[CurrentRandomNo - 1];
        if (wrongCount == 3)
        {
            HighLightPlate();
            wrongCount = 0;
        }
        PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[CurrentRandomNo - 1]);
        yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[CurrentRandomNo - 1].length + 1);
        AudioManager.instance.PauseAudio(false);
        isLevel4MascotSpeech = false;
        if (awayfromKeyBoardcoroutine != null)
        {
            StopCoroutine(awayfromKeyBoardcoroutine);
        }
        awayfromKeyBoardcoroutine = StartCoroutine(checkawayfromKeyBoard());

       
    }

    int RandomLevelNo;
    public void ChangerandomNo()
    {
        RandomLevelNo = Random.Range(0, DontDestroyRandomNoList.instance.checkRandomNo.Count - 1);
        CurrentRandomNo = DontDestroyRandomNoList.instance.checkRandomNo[RandomLevelNo];
        DontDestroyRandomNoList.instance.checkRandomNo.RemoveAt(RandomLevelNo);
    }

    public void HintPlayAgain()
    {
        HintprefLevel4 = 0;
        isSkipEnable = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void HintPlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayAgain()
    {
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "isPlayAgain", 1);
        isPlayAgain = true;
        LevelComCount = 0;
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCount", LevelComCount);
        Destroy(FindObjectOfType<DontDestroyRandomNoList>().gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PrevioClick()
    {
        if (!isLevel4MascotSpeech)
            StartCoroutine(previousBtnClick());
    }

    IEnumerator previousBtnClick()
    {
        //PreviousMascotPanel.SetActive(true);
        CanvasManager.instance.PreviousPanel.SetActive(true);
        PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[1]);
        CanvasManager.instance.PreviousPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
        CanvasManager.instance.PreviousPanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(AudioManager.instance.GL_Audio_Clips[1].length + 1);
        CanvasManager.instance.PreviousPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
        CanvasManager.instance.PreviousPanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;
    }

    public void previousLevelClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void LevelComPlayAgain()
    {
        //HintPrefCount = 0;
        LevelComCount = 0;
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCount", LevelComCount);
        Destroy(FindObjectOfType<DontDestroyRandomNoList>().gameObject);

        LevelCompCountSequence = 0;       
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCountSequence", LevelCompCountSequence);  
        isSkipEnable = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PickNewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator DoneButtonClick()
    {
        if (SelectCard != null)
        {
            print("Currentvalue----------->>" + int.Parse(SelectCard.GetComponent<SpriteRenderer>().sprite.name) + "-----SelectedCard----->" + (LevelCompleteObj.Count - 1) + "----Current---" + (CurrentRandomNo - 1));
            if ((int.Parse(SelectCard.GetComponent<SpriteRenderer>().sprite.name) - 1) == (CurrentRandomNo - 1) && (LevelCompleteObj.Count - 1) == (CurrentRandomNo - 1))
            {
                isLevel4MascotSpeech = true;
                PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[CurrentRandomNo - 1]);
                yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[CurrentRandomNo - 1].length + 1);
                //int soundRandom = Random.Range(0, 3);
                //if (soundRandom == 0)
                //{
                //    PlayRandomAudioClip(AudioManager.instance.LevelComplete);
                //    yield return new WaitForSeconds(AudioManager.instance.LevelComplete.length + 2);
                //}
                wrongCount = 0;
                if (isPlayAgain)
                {
                    LevelComCount++;
                    PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCount", LevelComCount);

                    if (LevelComCount > 6)
                    {
                        LevelCompCountSequence++;
                        if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "LevelComplete", 0) == 0)
                        {
                            if (LevelFourSlider.value < 0.75f + (float)(LevelCompCountSequence) * (0.25f / 3f))
                            {
                                DOTween.To(() => LevelFourSlider.value, x => LevelFourSlider.value = x, 0.75f + (float)(LevelCompCountSequence) * (0.25f / 3f), 0.5f).OnStart(() =>
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
                            DOTween.To(() => LevelFourSlider.value, x => LevelFourSlider.value = x, (float)(LevelComCount) * (0.75f / 6f), 0.5f).OnStart(() =>
                            {
                                SliderFill.SetActive(true);
                            });
                        }
                    }

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
                        PlayRandomAudioClip(AudioManager.instance.GameCompleteSound);
                        yield return new WaitForSeconds(AudioManager.instance.GameCompleteSound.length + 3);
                        CanvasManager.instance.LevelComPanel.SetActive(true);

                        AudioManager.instance.PauseAudio(true);
                        LevelCompleteSound(AudioManager.instance.GL_Audio_Clips[5]);
                        CanvasManager.instance.LevelComPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
                        CanvasManager.instance.LevelComPanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = false;
                        yield return new WaitForSecondsRealtime(AudioManager.instance.GL_Audio_Clips[8].length);
                        CanvasManager.instance.LevelComPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
                        CanvasManager.instance.LevelComPanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;
                        AudioManager.instance.PauseAudio(false);

                        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelComplete", 1);
                    }
                    else
                    {
                        yield return new WaitForSeconds(1);
                        isLevel4MascotSpeech = false;
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                }
                else
                {
                    if (DontDestroyRandomNoList.instance.checkRandomNo.Count == 0)
                    {
                        //DOTween.To(() => LevelFourSlider.value, x => LevelFourSlider.value = x, 1, 1).OnStart(() =>
                        //{
                        //    SliderFill.SetActive(true);
                        //});
                        //PlayRandomAudioClip(AudioManager.instance.GameCompleteSound);
                        //CanvasManager.instance.LevelCompleteAnimaton.SetActive(true);
                        //yield return new WaitForSeconds(AudioManager.instance.GameCompleteSound.length + 1);
                        CanvasManager.instance.PlayAgainPanel.SetActive(true);

                        AudioManager.instance.PauseAudio(true);
                        PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[3]);
                        CanvasManager.instance.PlayAgainPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
                        yield return new WaitForSeconds(AudioManager.instance.GL_Audio_Clips[3].length);
                        CanvasManager.instance.PlayAgainPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
                        AudioManager.instance.PauseAudio(false);

                        //PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[3]);
                        //yield return new WaitForSeconds(AudioManager.instance.GL_Audio_Clips[3].length);
                    }
                    else
                    {
                        LevelComCount++;
                        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCount", LevelComCount);
                        //DOTween.To(() => LevelFourSlider.value, x => LevelFourSlider.value = x, (float)(LevelComCount) * (1f / 9f), 0.5f).OnStart(() =>
                        //{
                        //    SliderFill.SetActive(true);
                        //});
                        yield return new WaitForSeconds(1);
                        isLevel4MascotSpeech = false;
                        ChangerandomNo();
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                }
            }
            else
            {
                if (wrongCount < 3)
                    wrongCount++;


                if (isPlayAgain)
                {
                    HighLightPlate();
                    for (int i = 0; i < LevelCompleteObj.Count; i++)
                    {
                        LevelCompleteObj[i].transform.DOLocalMove(LevelCompleteObj[i].GetComponent<DragLevelFTens>().startPosition, 0.5f);
                        LevelCompleteObj[i].GetComponent<BoxCollider2D>().enabled = true;
                        LevelCompleteObj[i].GetComponent<DragLevelFTens>().isDoNotMove = false;
                    }
                    for (int j = 0; j < LevelFourAllSlots.Count; j++)
                    {
                        LevelFourAllSlots[j].GetComponent<BoxCollider2D>().enabled = true;
                    }

                    LevelCompCountSequence = -1;
                    PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCountSequence", LevelCompCountSequence);


                    SelectCard.transform.DOMove(SelectCard.transform.GetComponent<DragCardLvlFour>().startPosition, 1);
                    SelectCard.GetComponent<DragCardLvlFour>().isDoNotMove = false;
                    DoneButton.SetActive(false);
                    Count = 0;
                    //yield return new WaitForSeconds(2);
                    PlayRandomAudioClip(AudioManager.instance.WrongAns);
                    Invoke("callnexttens", AudioManager.instance.WrongAns.length);

                    //PlayRandomAudioClip(AudioManager.instance.WrongAns);
                    //yield return new WaitForSeconds(AudioManager.instance.WrongAns.length);
                    //PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[LevelCompleteObj.Count - 1]);
                    //yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[LevelCompleteObj.Count - 1].length + 1);
                    //DontDestroyRandomNoList.instance.checkRandomNo.Add(CurrentRandomNo);
                    //yield return new WaitForSeconds(2);
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                else
                {
                  
                    if (LevelComCount >= 7)
                    {
                        for (int i = 0; i < LevelCompleteObj.Count; i++)
                        {
                            LevelCompleteObj[i].transform.DOLocalMove(LevelCompleteObj[i].GetComponent<DragLevelFTens>().startPosition, 0.5f);
                            LevelCompleteObj[i].GetComponent<BoxCollider2D>().enabled = true;
                            LevelCompleteObj[i].GetComponent<DragLevelFTens>().isDoNotMove = false;
                        }

                        for (int j = 0; j < LevelFourAllSlots.Count; j++)
                        {
                            LevelFourAllSlots[j].GetComponent<BoxCollider2D>().enabled = true;
                        }
                        SelectCard.transform.DOMove(SelectCard.transform.GetComponent<DragCardLvlFour>().startPosition, 1);
                        SelectCard.GetComponent<DragCardLvlFour>().isDoNotMove = false;
                        DoneButton.SetActive(false);
                        Count = 0;
                        //yield return new WaitForSeconds(2);
                        PlayRandomAudioClip(AudioManager.instance.WrongAns);
                        Invoke("callnexttens", AudioManager.instance.WrongAns.length);
                        //yield return new WaitForSeconds(AudioManager.instance.WrongAns.length + 1);
                        //PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[LevelCompleteObj.Count - 1]);
                        //yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[LevelCompleteObj.Count - 1].length + 1);
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
                        HighLightPlate();
                        for (int i = 0; i < LevelCompleteObj.Count; i++)
                        {
                            LevelCompleteObj[i].transform.DOLocalMove(LevelCompleteObj[i].GetComponent<DragLevelFTens>().startPosition, 1).OnComplete(() =>
                            {

                            });
                            LevelCompleteObj[i].GetComponent<DragLevelFTens>().isDoNotMove = false;
                            LevelCompleteObj[i].GetComponent<BoxCollider2D>().enabled = true;
                        }
                        for (int j = 0; j < LevelFourAllSlots.Count; j++)
                        {
                            LevelFourAllSlots[j].GetComponent<BoxCollider2D>().enabled = true;
                        }

                        SelectCard.transform.DOMove(SelectCard.transform.GetComponent<DragCardLvlFour>().startPosition, 1);
                        SelectCard.GetComponent<DragCardLvlFour>().isDoNotMove = false;
                        DoneButton.SetActive(false);
                        Count = 0;
                        //yield return new WaitForSeconds(2);
                        PlayRandomAudioClip(AudioManager.instance.WrongAns);
                        Invoke("callnexttens", AudioManager.instance.WrongAns.length);
                        //PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[LevelCompleteObj.Count - 1]);
                        //yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[LevelCompleteObj.Count - 1].length);
                        //LevelCompleteObj.Clear();
                        //PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[12]);
                    }
                }
            }
        }
        else
        {
            DoneButton.GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    public void callnext()
    {
        PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[LevelCompleteObj.Count - 1]);
        Invoke("callnexttens", AudioManager.instance.Tens_Audio_Clips[LevelCompleteObj.Count - 1].length);
    }

    public void callnexttens()
    {
        LevelCompleteObj.Clear();
        CardDestinationHandler.GetComponent<BoxCollider2D>().enabled = true;
        for(int j=0; j< TempFivePosi.Count; j++)
        {
            TempFivePosi[j].GetComponent<BoxCollider2D>().enabled = true;
        }

        PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[12]);
    }

    public void firstcallnext()
    {
        PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[LevelCompleteObj.Count - 1]);
        Invoke("callnextGreterSavn", AudioManager.instance.Tens_Audio_Clips[LevelCompleteObj.Count - 1].length);
    }

    public void callnextGreterSavn()
    {
        LevelCompleteObj.Clear();
        PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[12]);
        DontDestroyRandomNoList.instance.checkRandomNo.Clear();
        for (int i = 1; i <= 9; i++)
        {
            DontDestroyRandomNoList.instance.checkRandomNo.Add(i);
        }
        DontDestroyRandomNoList.instance.checkRandomNo.Remove(CurrentRandomNo);
        ChangerandomNo();
        //yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void HighLightPlate()
    {
        if (wrongCount == 3)
        {
            for (int i = 0; i < LevelFourAllSlots.Count; i++)
            {
                if (i < (CurrentRandomNo))
                {
                    LevelFourAllSlots[i].GetComponent<SpriteRenderer>().enabled = true;
                    LevelFourAllSlots[i].GetComponent<Animator>().enabled = true;
                }
                else
                {
                    LevelFourAllSlots[i].GetComponent<SpriteRenderer>().enabled = false;
                    LevelFourAllSlots[i].GetComponent<Animator>().enabled = false;
                }
            }
        }
    }


    public bool CheckSlotIsFalseOrNot()
    {
        for (int i = 0; i < LevelCompleteObj.Count; i++)
        {
            if (!LevelCompleteObj[i].GetComponent<DragLevelFTens>().isDoNotMove)
            {
                return false;
            }
        }
        return true;
    }


    public IEnumerator QueRplayBtnClickMethod()
    {
        if (!GetComponent<AudioSource>().isPlaying && !isLevel4MascotSpeech)
        {
            AudioManager.instance.PauseAudio(true);
            QueReplayBtn.GetComponent<CircleCollider2D>().enabled = false;
            isLevel4MascotSpeech = true;
            PlayRandomAudioClip(AudioClips[2]);
            yield return new WaitForSeconds(AudioClips[2].length + 1);
            PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[CurrentRandomNo - 1]);
            yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[CurrentRandomNo - 1].length + 1);
            AudioManager.instance.PauseAudio(false);
            isLevel4MascotSpeech = false;
            QueReplayBtn.GetComponent<CircleCollider2D>().enabled = true;
        }
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
            print("--------15->>");
            if (!CanvasManager.instance.HintCompletePanel.activeSelf && !CanvasManager.instance.LevelComPanel.activeSelf && !CanvasManager.instance.PlayAgainPanel.activeSelf &&
                !CanvasManager.instance.PreviousPanel.activeSelf && !CanvasManager.instance.HomePanel.activeSelf && HintprefLevel4 == 1)
                StartCoroutine(Second15Complete());
        }
        else
        {
            //
        }
    }

    IEnumerator Second15Complete()
    {
        isLevel4MascotSpeech = true;
        AudioManager.instance.PauseAudio(true);
        if (!GetComponent<AudioSource>().isPlaying )
        {
            PlayRandomAudioClip(AudioClips[2]);
            yield return new WaitForSeconds(AudioClips[2].length + 1);
            PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[CurrentRandomNo - 1]);
            yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[CurrentRandomNo - 1].length + 1);
        }
        AudioManager.instance.PauseAudio(false);
        isLevel4MascotSpeech = false;
        if (awayfromKeyBoardcoroutine != null)
        {
            StopCoroutine(awayfromKeyBoardcoroutine);
        }
        awayfromKeyBoardcoroutine = StartCoroutine(checkawayfromKeyBoard());
    }

    public void HomeBtnClick()
    {
        if (!isLevel4MascotSpeech)
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

        LevelCompleteSound(AudioManager.instance.GL_Audio_Clips[6]);
        CanvasManager.instance.SkipPannels.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
        CanvasManager.instance.SkipPannels.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = false;
        yield return new WaitForSecondsRealtime(AudioManager.instance.GL_Audio_Clips[6].length);
        CanvasManager.instance.SkipPannels.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
        CanvasManager.instance.SkipPannels.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;
    }
    public void LevelCompleteSound(AudioClip clip)
    {
        //if (CanvasManager.instance.PlaySoundCount == 0)
        //{
        GetComponent<AudioSource>().PlayOneShot(clip);
        //}
    }
    public void ExitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ContinueGame()
    {
        GetComponent<AudioSource>().Play();
        Time.timeScale = 1;
        CanvasManager.instance.SkipPannels.SetActive(false);
    }
}
