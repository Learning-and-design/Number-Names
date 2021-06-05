using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class GameLevelThreeController : MonoBehaviour
{
    public GameObject HandIcon;
    public GameObject HandIconDestina;
    public GameObject HandIconDone;
    public GameObject HandIconQuestinoReply;

    public GameObject MasCot;
    public AudioClip[] AudioClips;
    //public AudioClip[] TensNameClips;
    //public AudioClip[] FindTensAudioClip;
    public List<Sprite> TensNameImageList;
    public GameObject LevelBackGround;
    public GameObject TensImage;
    public GameObject TensImageDestina;
    public GameObject Shutter;
    public GameObject number_label;
    public GameObject Truck;
    public GameObject DoneButton;
    public GameObject QueReplayBtn;

    public static int CurrentRandomNo;
    public static bool isPlayAgain = false;
    static int LevelComCount;
    static int LevelCompCountSequence = 0;

    public bool isLevel3MascotSpeech = false;
    public GameObject[] AllSlots;
    public int Count = 0;
    public List<GameObject> LevelCompleObjList;
    public Slider Level3Slider;
    public GameObject SliderFill;
    public Coroutine awayfromKeyBoardcoroutine;
    static int wrongCount = 0;
    public GameObject PreviousMascotPanel;
    public static GameLevelThreeController instance;
    public static bool isSkipEnable = false;

    private void Awake()
    {

        //if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "isPlayAgain", 0) == 0)
        //{
            isPlayAgain = true;
        //}
        //else
        //{
            //isPlayAgain = true;

            LevelComCount = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "LevelCompCount", 0);

        if (LevelComCount > 6)
        {
            print("======LevelCompCountSequence=====>>>>>>>" + LevelCompCountSequence);
            LevelCompCountSequence = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "LevelCompCountSequence", 0);
            SliderFill.SetActive(true);
            print("======SliderFill=====>>>>>>>" + (0.75f + (float)(LevelCompCountSequence) * (0.25f / 3f)));
            Level3Slider.value = 0.75f + (float)(LevelCompCountSequence) * (0.25f / 3f);
        }
        else
        {
            if (LevelComCount > 0)
            {
                SliderFill.SetActive(true);
                Level3Slider.value = (float)(LevelComCount) * (0.75f / 6f);
            }
            else
            {
                SliderFill.SetActive(false);
            }
        }
        //}

        if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "LevelComplete", 0) == 1)
        {
            SliderFill.SetActive(true);
            Level3Slider.value = 1f;
        }


        instance = this;
    }

    public int HintprefLevel3
    {
        get
        {
            return PlayerPrefs.GetInt("Level3HintPref", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Level3HintPref", value);
        }
    }

    private void Start()
    {
     
        Time.timeScale = 1;
        if (HintprefLevel3 == 0)
        {
            StartCoroutine(Level3Hint());
            HintprefLevel3 = 1;
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
            StartCoroutine(GenerateRandomLevel());
            CanvasManager.instance.SetfalseHomehintback(true);
        }
        CanvasManager.instance.ScoreText.text = CanvasManager.instance.TotalScore.ToString();
    }


    IEnumerator Level3Hint()
    {
        AudioManager.instance.PauseAudio(true);
        isLevel3MascotSpeech = true;
        LevelBackGround.SetActive(false);
        MasCot.SetActive(true);
        PlayRandomAudioClip(AudioClips[0]);
        yield return new WaitForSeconds(AudioClips[0].length );
        PlayRandomAudioClip(AudioClips[1]);
        yield return new WaitForSeconds(AudioClips[1].length);
        LevelBackGround.SetActive(true);
        MasCot.SetActive(false);
        number_label.SetActive(true);
        number_label.GetComponent<SpriteRenderer>().sprite = TensNameImageList[0];
        PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[0]);
        yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[0].length + 1);
        HandIcon.SetActive(true);
        HandIcon.transform.DOMove(HandIconDestina.transform.position, 2).OnComplete(() =>
         {
             HandIcon.SetActive(false);
         });
        TensImage.transform.DOMove(TensImageDestina.transform.position, 2).OnComplete(() =>
         {
             DoneButton.SetActive(true);
         });
        yield return new WaitForSeconds(3);
        PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[0]);
        yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[0].length + 1);
      
        QueReplayBtn.SetActive(true);
        HandIconQuestinoReply.SetActive(true);
        QueReplayBtn.GetComponent<Animator>().enabled = true;
        PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[11]);
        yield return new WaitForSeconds(AudioManager.instance.GL_Audio_Clips[11].length + 1);
        QueReplayBtn.GetComponent<Animator>().enabled = false;
        HandIconQuestinoReply.SetActive(false);
        HandIconDone.SetActive(true);
        yield return new WaitForSeconds(2);
        HandIconDone.SetActive(false);
        Shutter.SetActive(true);
        Shutter.transform.GetChild(0).gameObject.SetActive(true);
        TensImage.SetActive(false);
        yield return new WaitForSeconds(1);
        Truck.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(2);
        QueReplayBtn.SetActive(false);
        CanvasManager.instance.HintCompletePanel.SetActive(true);
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = false;
        PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[2]);
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;
        yield return new WaitForSeconds(AudioManager.instance.GL_Audio_Clips[2].length + 1);
        AudioManager.instance.PauseAudio(false);
        isLevel3MascotSpeech = false;
        CanvasManager.instance.SetfalseHomehintback(true);
    }

    int RandomLevelNo;
    public void ChangerandomNo()
    {
        RandomLevelNo = Random.Range(0, DontDestroyRandomNoList.instance.checkRandomNo.Count);
        //print("RandomNNo--------------->>" + RandomLevelNo);
        CurrentRandomNo = DontDestroyRandomNoList.instance.checkRandomNo[RandomLevelNo];
        //print("Random---------->>" + (CurrentRandomNo) +"----RanValue----" + RandomLevelNo);
        DontDestroyRandomNoList.instance.checkRandomNo.RemoveAt(RandomLevelNo);

    }

    public void HintPlayAgain()
    {
        HintprefLevel3 = 0;
        isSkipEnable = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void HintPlayGame()
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

    IEnumerator GenerateRandomLevel()
    {
        AudioManager.instance.PauseAudio(true);
        isLevel3MascotSpeech = true;
        if (isPlayAgain)
        {
            //if (LevelComCount > 0)
            //{
            //    SliderFill.SetActive(true);
            //}
            //Level3Slider.value = (float)(LevelComCount) * (1f / 3f);
        }
        else
        {
            //if (LevelComCount > 0)
            //{
            //    SliderFill.SetActive(true);
            //}
            //Level3Slider.value = (float)(LevelComCount) * (1f / 9f);
        }

        if (CurrentRandomNo == 0)
        {
            ChangerandomNo();
        }
        else if (isPlayAgain)
        {
            ChangerandomNo();
        }

        if (wrongCount == 3)
        {
            HighLightPlate();
            wrongCount = 0;
        }
        //DontDestroyRandomNoList.instance.checkRandomNo.RemoveAt(RandomLevelNo);
        PlayRandomAudioClip(AudioClips[4]);
        yield return new WaitForSeconds(AudioClips[4].length);
        number_label.SetActive(true);
        number_label.GetComponent<SpriteRenderer>().sprite = TensNameImageList[CurrentRandomNo - 1];
        PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[CurrentRandomNo - 1]);
        yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[CurrentRandomNo - 1].length);
        QueReplayBtn.SetActive(true);
        AudioManager.instance.PauseAudio(false);
        isLevel3MascotSpeech = false;

        if (awayfromKeyBoardcoroutine != null)
        {
            StopCoroutine(awayfromKeyBoardcoroutine);
        }
        awayfromKeyBoardcoroutine = StartCoroutine(checkawayfromKeyBoard());        
    }


    public IEnumerator CheckLevelCompleteOrNot()
    {
        print("TotalTruckObj------------>>" + (LevelCompleObjList.Count - 1) + "------CurrentRanNo---->" + (CurrentRandomNo - 1));
        //DoneButton.SetActive(false);
        if ((LevelCompleObjList.Count - 1) == (CurrentRandomNo - 1))
        {
            isLevel3MascotSpeech = true;
            PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[CurrentRandomNo - 1]);
            yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[CurrentRandomNo - 1].length + 1);
            Shutter.SetActive(true);
            Shutter.transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            for (int i = 0; i < LevelCompleObjList.Count; i++)
            {
                LevelCompleObjList[i].SetActive(false);
            }
            //int soundRandom = Random.Range(0, 3);
            //if (soundRandom == 0)
            //{
            //    PlayRandomAudioClip(AudioManager.instance.LevelComplete);
            //    yield return new WaitForSeconds(AudioManager.instance.LevelComplete.length + 2);
            //}
            Truck.GetComponent<Animator>().enabled = true;
            wrongCount = 0;
            //Right
            if (isPlayAgain)
            {
                LevelComCount++;

                if (LevelComCount > 6)
                {
                    LevelCompCountSequence++;
                    if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "LevelComplete", 0) == 0)
                    {
                        if (Level3Slider.value < 0.75f + (float)(LevelCompCountSequence) * (0.25f / 3f))
                        {
                            DOTween.To(() => Level3Slider.value, x => Level3Slider.value = x, 0.75f + (float)(LevelCompCountSequence) * (0.25f / 3f), 0.5f).OnStart(() =>
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
                        DOTween.To(() => Level3Slider.value, x => Level3Slider.value = x, (float)(LevelComCount) * (0.75f / 6f), 0.5f).OnStart(() =>
                        {
                            SliderFill.SetActive(true);
                        });
                    }
                }

                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCount", LevelComCount);
                //DOTween.To(() => Level3Slider.value, x => Level3Slider.value = x, (float)(LevelComCount) * (1f / 3f), 1).OnStart(() =>
                //{
                //    SliderFill.SetActive(true);
                //});

                if (FindObjectOfType<DontDestroyRandomNoList>().checkRandomNo.Count == 0)
                {
                    Destroy(FindObjectOfType<DontDestroyRandomNoList>().gameObject);
                }

                if (LevelCompCountSequence == 3)
                {
                    //DOTween.To(() => Level3Slider.value, x => Level3Slider.value = x, 1, 1).OnStart(() =>
                    //{
                    //    SliderFill.SetActive(true);
                    //});

                    CanvasManager.instance.RewardPanel.SetActive(true);
                    yield return new WaitForSeconds(3);
                    CanvasManager.instance.RewardPanel.SetActive(false);
                    CanvasManager.instance.TotalScore += 10;
                    CanvasManager.instance.ScoreText.text = CanvasManager.instance.TotalScore.ToString();
                    CanvasManager.instance.LevelCompleteAnimaton.SetActive(true);
                    PlayRandomAudioClip(AudioManager.instance.GameCompleteSound);
                    isLevel3MascotSpeech = false;
                    yield return new WaitForSeconds(AudioManager.instance.GameCompleteSound.length + 3);
                    CanvasManager.instance.LevelComPanel.SetActive(true);

                    AudioManager.instance.PauseAudio(true);
                    LevelCompleteSound(AudioManager.instance.GL_Audio_Clips[8]);
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
                    isLevel3MascotSpeech = false;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
            else
            {

                if (DontDestroyRandomNoList.instance.checkRandomNo.Count == 0)
                {
                    //DOTween.To(() => Level3Slider.value, x => Level3Slider.value = x, 1, 1).OnStart(() =>
                    //{
                    //    SliderFill.SetActive(true);
                    //});
                    //PlayRandomAudioClip(AudioManager.instance.GameCompleteSound);
                    //yield return new WaitForSeconds(AudioManager.instance.GameCompleteSound.length + 2);
                    isLevel3MascotSpeech = false;
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
                    //DOTween.To(() => Level3Slider.value, x => Level3Slider.value = x, (float)(LevelComCount) * (1f / 9f), 1).OnStart(() =>
                    //{
                    //    SliderFill.SetActive(true);
                    //});
                    yield return new WaitForSeconds(1);
                    isLevel3MascotSpeech = false;
                    ChangerandomNo();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }
        else
        {
            //Wron
            if(wrongCount < 3)
            wrongCount++;
           
            //if (wrongCount == 3)
            //{
            //    for(int i=0; i< AllSlots.Length; i++)
            //    {
            //        if(i < (CurrentRandomNo))
            //        {
            //            AllSlots[i].GetComponent<SpriteRenderer>().enabled = true;
            //            AllSlots[i].GetComponent<Animator>().enabled = true;
            //        }
            //        else
            //        {
            //            AllSlots[i].GetComponent<SpriteRenderer>().enabled = false;
            //            AllSlots[i].GetComponent<Animator>().enabled = false;
            //        }
            //    }
            //}

            if (isPlayAgain)
            {
                for (int i = 0; i < LevelCompleObjList.Count; i++)
                {
                    LevelCompleObjList[i].transform.DOMove(LevelCompleObjList[i].GetComponent<DragTensTruck>().startPosition, 1);

                    LevelCompleObjList[i].GetComponent<BoxCollider2D>().enabled = true;
                    LevelCompleObjList[i].GetComponent<DragTensTruck>().isDoNotMove = false;
                }

                LevelCompCountSequence = -1;
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCountSequence", LevelCompCountSequence);

                DoneButton.SetActive(false);
                Count = 0;
                PlayRandomAudioClip(AudioManager.instance.WrongAns);
                Invoke("callnexttens", AudioManager.instance.WrongAns.length);
                //PlayRandomAudioClip(AudioManager.instance.WrongAns);
                //yield return new WaitForSeconds(AudioManager.instance.WrongAns.length + 1);
                //PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[LevelCompleObjList.Count -1]);
                //yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[LevelCompleObjList.Count-1].length + 1);
                //DontDestroyRandomNoList.instance.checkRandomNo.Add(CurrentRandomNo);
                //yield return new WaitForSeconds(2);
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {


                if (LevelComCount >= 7)
                {
                    for (int i = 0; i < LevelCompleObjList.Count; i++)
                    {
                        LevelCompleObjList[i].transform.DOMove(LevelCompleObjList[i].GetComponent<DragTensTruck>().startPosition, 1).OnComplete(() =>
                        {
                            
                        });
                        LevelCompleObjList[i].GetComponent<BoxCollider2D>().enabled = true;
                        LevelCompleObjList[i].GetComponent<DragTensTruck>().isDoNotMove = false;
                    }

                    DoneButton.SetActive(false);
                    Count = 0;
                    PlayRandomAudioClip(AudioManager.instance.WrongAns);
                    //yield return new WaitForSeconds(AudioManager.instance.WrongAns.length + 1);
                    //PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[LevelCompleObjList.Count -1]);
                    //yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[LevelCompleObjList.Count-1].length + 1);
                    //Invoke("firstcallnext", AudioManager.instance.WrongAns.length);
                    Invoke("callnexttens", AudioManager.instance.WrongAns.length);
                    //DontDestroyRandomNoList.instance.checkRandomNo.Clear();
                    //for (int i = 1; i <= 9; i++)
                    //{
                    //    DontDestroyRandomNoList.instance.checkRandomNo.Add(i);
                    //}
                    //DontDestroyRandomNoList.instance.checkRandomNo.Remove(CurrentRandomNo);
                    //ChangerandomNo();
                    ////yield return new WaitForSeconds(2);
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                else
                {
                    HighLightPlate();
                    for (int i = 0; i < LevelCompleObjList.Count; i++)
                    {
                        LevelCompleObjList[i].transform.DOMove(LevelCompleObjList[i].GetComponent<DragTensTruck>().startPosition, 1).OnComplete(() =>
                        {
                           
                        });
                        LevelCompleObjList[i].GetComponent<DragTensTruck>().isDoNotMove = false;
                        LevelCompleObjList[i].GetComponent<BoxCollider2D>().enabled = true;
                    }
                    DoneButton.SetActive(false);
                    Count = 0;
                    PlayRandomAudioClip(AudioManager.instance.WrongAns);
                    Invoke("callnexttens", AudioManager.instance.WrongAns.length);
                    
                    //yield return new WaitForSeconds(AudioManager.instance.WrongAns.length);
                    //print("Wrong-------->>");
                    //PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[1]);
                    //yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[1].length);
                    //print("Tens-------->>");
                    //LevelCompleObjList.Clear();
                    //PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[12]);
                    //yield return new WaitForSeconds(AudioManager.instance.GL_Audio_Clips[12].length + 1);
                }
            }
        }
    }


    public void HighLightPlate()
    {
        if (wrongCount == 3)
        {
            for (int i = 0; i < AllSlots.Length; i++)
            {
                if (i < (CurrentRandomNo))
                {
                    AllSlots[i].GetComponent<SpriteRenderer>().enabled = true;
                    AllSlots[i].GetComponent<Animator>().enabled = true;
                }
                else
                {
                    AllSlots[i].GetComponent<SpriteRenderer>().enabled = false;
                    AllSlots[i].GetComponent<Animator>().enabled = false;
                }
            }
        }
    }

    public void callnext()
    {
        PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[LevelCompleObjList.Count -1]);
        Invoke("callnexttens", AudioManager.instance.Tens_Audio_Clips[LevelCompleObjList.Count - 1].length);
    }

    public void callnexttens()
    {
        LevelCompleObjList.Clear();
        PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[12]);
    }

    public void firstcallnext()
    {
        PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[LevelCompleObjList.Count - 1]);
        Invoke("callnextGreterSavn", AudioManager.instance.Tens_Audio_Clips[LevelCompleObjList.Count - 1].length);
    }

    public void callnextGreterSavn()
    {
        LevelCompleObjList.Clear();
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

    public IEnumerator QueReplBtn()
    {
        if (!GetComponent<AudioSource>().isPlaying && !isLevel3MascotSpeech)
        {
            AudioManager.instance.PauseAudio(true);
            QueReplayBtn.GetComponent<CircleCollider2D>().enabled = false;
            isLevel3MascotSpeech = true;
            PlayRandomAudioClip(AudioClips[3]);
            yield return new WaitForSeconds(AudioClips[3].length);
            PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[CurrentRandomNo - 1]);
            yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[CurrentRandomNo - 1].length + 1);
            AudioManager.instance.PauseAudio(false);
            isLevel3MascotSpeech = false;
            QueReplayBtn.GetComponent<CircleCollider2D>().enabled = true;
        }
    }



    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //print("-----1111----");
            AwayfromKeyboard(false);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (awayfromKeyBoardcoroutine != null)
            {
                //print("-----2222----");
                StopCoroutine(awayfromKeyBoardcoroutine);
            }
            //print("-----3333----");
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
               !CanvasManager.instance.PreviousPanel.activeSelf && !CanvasManager.instance.HomePanel.activeSelf && HintprefLevel3 == 1)
                StartCoroutine(Second15Complete());
        }
        else
        {
            //
        }
    }

    IEnumerator Second15Complete()
    {
        isLevel3MascotSpeech = true;
        AudioManager.instance.PauseAudio(true);
        if (!GetComponent<AudioSource>().isPlaying )
        {
            PlayRandomAudioClip(AudioClips[3]);
            yield return new WaitForSeconds(AudioClips[3].length);
            PlayRandomAudioClip(AudioManager.instance.Tens_Audio_Clips[CurrentRandomNo - 1]);
            yield return new WaitForSeconds(AudioManager.instance.Tens_Audio_Clips[CurrentRandomNo - 1].length + 1);
        }
        
        AudioManager.instance.PauseAudio(false);
        isLevel3MascotSpeech = false;
        if (awayfromKeyBoardcoroutine != null)
        {
            StopCoroutine(awayfromKeyBoardcoroutine);
        }
        awayfromKeyBoardcoroutine = StartCoroutine(checkawayfromKeyBoard());
    }


    public void PlayAgain()
    {
        isPlayAgain = true;
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "isPlayAgain", 1);
        LevelComCount = 0;
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCount", LevelComCount);
        Destroy(FindObjectOfType<DontDestroyRandomNoList>().gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PrevioClick()
    {
        if(!isLevel3MascotSpeech)
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
        if (FindObjectOfType<DontDestroyRandomNoList>() != null)
        {
            Destroy(FindObjectOfType<DontDestroyRandomNoList>().gameObject);
        }

        SceneManager.LoadScene("Level4");
    }

    public void HomeBtnClick()
    {
        if (!isLevel3MascotSpeech)
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
