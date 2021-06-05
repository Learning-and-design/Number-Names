using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLevelTwoController : MonoBehaviour
{
    public GameObject HandIcon;
    public GameObject HandIconDestina;
    public GameObject MasCot;
    public AudioClip[] AudioClips;
    public AudioClip[] TensNameClips;
    public AudioClip[] FindTensAudioClip;
    public List<Sprite> TensNameImageList;
    public List<GameObject> TotalImagesPositions;
    int RandomLevelNo;
    public static int CurrentRandomNo;
    public GameObject BackGroundImage;
    public GameObject[] Hands;
    public GameObject[] All30Num;
    public GameObject AudioQueBtn;
    public bool isLevel2MascotSpeech = false;

    public GameObject HintLevel;
    public GameObject AllLevels;
    Coroutine awayfromKeyBoardcoroutine;
    public List<GameObject> TotalLevelCompleteObjects;
    public static GameLevelTwoController instance;
    public GameObject PreviousMascotPanel;
    public Slider LevelTwoSlider;
    public GameObject SliderFill;
    public static bool isPlayAgain = false;
    public static bool isSkipEnable = false;
    public GameObject QueHandIcon;
    public bool AlreadyWrongSelected = false;


    private void Awake()
    {
        if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "isPlayAgain", 0) == 0)
        {
            isPlayAgain = false;
        }
        else
        {
            isPlayAgain = true;

            levelComCount = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "LevelCompCount", 0);

            if (levelComCount > 0)
            {
                SliderFill.SetActive(true);
            }
            else
            {
                SliderFill.SetActive(false);
            }
            LevelTwoSlider.value = (float)(levelComCount) * (1f / 9f);
        }

        if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "LevelComplete", 0) == 1)
        {
            SliderFill.SetActive(true);
            LevelTwoSlider.value = 1f;

        }
        instance = this;
    }

    private void Start()
    {
      
        Time.timeScale = 1;
        if (HintPrefCount == 0)
        {
            StartCoroutine(HintBtnClick());
            HintPrefCount = 1;
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

    public int HintPrefCount
    {
        get
        {
            return PlayerPrefs.GetInt("FirstHintLevelTwo", 0);
        }
        set
        {
            PlayerPrefs.SetInt("FirstHintLevelTwo", value);
        }
    }

    IEnumerator HintBtnClick()
    {
        AudioManager.instance.PauseAudio(true);
        isLevel2MascotSpeech = true;
        MasCot.SetActive(true);
        BackGroundImage.SetActive(false);
       
        PlayAudio(0);
        yield return new WaitForSeconds(AudioClips[0].length + 2);
      
        PlayAudio(1);
        MasCot.SetActive(false);
        BackGroundImage.SetActive(true);
        HintLevel.SetActive(true);
        AllLevels.SetActive(false);
        yield return new WaitForSeconds(AudioClips[1].length + 2);
        PlayRandomAudioClip(FindTensAudioClip[2]);
        yield return new WaitForSeconds(FindTensAudioClip[2].length + 2);

        Hands[0].SetActive(true);
        All30Num[0].transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        Hands[0].transform.DOMove(Hands[1].transform.position, 0.5f).OnComplete(() =>
        {
            All30Num[1].transform.GetChild(0).gameObject.SetActive(true);
        });
        yield return new WaitForSeconds(1.2f);
        Hands[0].transform.DOMove(Hands[2].transform.position, 0.5f).OnComplete(() =>
        {
            All30Num[2].transform.GetChild(0).gameObject.SetActive(true);
        });
            yield return new WaitForSeconds(1.2f);

        //for (int i =0; i < Hands.Length; i++)
        //{
        //    Hands[i].SetActive(true);
        //}
        PlayAudio(2);
        yield return new WaitForSeconds(AudioClips[2].length);
        QueHandIcon.SetActive(true);
        AudioQueBtn.SetActive(true);
        yield return new WaitForSeconds(1);
        AudioQueBtn.GetComponent<Animator>().enabled = true;
        PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[11]);
        yield return new WaitForSeconds(AudioManager.instance.GL_Audio_Clips[11].length + 2);
        AudioQueBtn.GetComponent<Animator>().enabled = false;
        QueHandIcon.SetActive(false);
        //for (int i = 0; i < Hands.Length; i++)
        //{
        Hands[0].SetActive(false);
        //    All30Num[i].transform.GetChild(0).gameObject.SetActive(true);
        //}
       
     
        for (int i = 0; i < HintLevel.transform.childCount; i++)
        {
            if ((int.Parse(HintLevel.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite.name) - 1) == 2)
            {

            }
            else
            {
                HintLevel.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        print("-------PlayAudio(2);----->>>>>>>>");
        yield return new WaitForSeconds(AudioClips[2].length + 3);
        CanvasManager.instance.HintCompletePanel.SetActive(true);
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = false;
        PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[2]);
        yield return new WaitForSeconds(AudioManager.instance.GL_Audio_Clips[2].length +1);
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
        CanvasManager.instance.HintCompletePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;       
        isLevel2MascotSpeech = false;
        AudioManager.instance.PauseAudio(false);

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

    public void ChangerandomNo()
    {
        RandomLevelNo = Random.Range(0, DontDestroyRandomNoList.instance.checkRandomNo.Count - 1);
        CurrentRandomNo = DontDestroyRandomNoList.instance.checkRandomNo[RandomLevelNo];
    }

    public void PlayAgain()
    {
        isPlayAgain = true;
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "isPlayAgain", 1);
        levelComCount = 0;
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCount", levelComCount);
        DestroyObject(FindObjectOfType<DontDestroyRandomNoList>().gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void DisableImages()
    {
        print("DisableIMae------->>");
        for(int i=0; i< AllLevels.transform.childCount; i++)
        {
            if((int.Parse(AllLevels.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite.name) -1) == (CurrentRandomNo - 1))
            {

            }
            else
            {
                AllLevels.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    IEnumerator GenerateRandomLevel()
    {
        AudioManager.instance.PauseAudio(true);
        Time.timeScale = 1;
        isLevel2MascotSpeech = true;
        int randomvalue = Random.Range(3, 5);
        HintLevel.SetActive(false);
        AllLevels.SetActive(true);
        if (isPlayAgain)
        {
            //if (levelComCount > 0)
            //{
            //    SliderFill.SetActive(true);
            //}
            //LevelTwoSlider.value = (float)(levelComCount) * (1f / 3f);
        }
        else
        {
            //if (levelComCount > 0)
            //{
            //    SliderFill.SetActive(true);
            //}
            //LevelTwoSlider.value = (float)(levelComCount) * (1f / 9f);
        }
        if(CurrentRandomNo == 0)
        {
            ChangerandomNo();
        }
        else if(isPlayAgain)
        {
            ChangerandomNo();
        }
        DontDestroyRandomNoList.instance.checkRandomNo.Remove(CurrentRandomNo);
        for (int i = 0; i < 12; i++)
        {
            if (i < randomvalue)
            {
                int randomPosi = Random.Range(0, TotalImagesPositions.Count);
                //print("randomPosi---------------->>" + randomPosi + "----CurrentNo----" + CurrentRandomNo);
                TotalImagesPositions[randomPosi].GetComponent<SpriteRenderer>().sprite = TensNameImageList[CurrentRandomNo-1];
                TotalLevelCompleteObjects.Add(TotalImagesPositions[randomPosi]);
                TotalImagesPositions.Remove(TotalImagesPositions[randomPosi]);
                //TensNameImageList.Remove(TensNameImageList[CurrentRandomNo]);
                if (i == randomvalue-1)
                {
                    TensNameImageList.Remove(TensNameImageList[CurrentRandomNo-1]);
                }
            }
            else
            {
                int randomno = Random.Range(0, TotalImagesPositions.Count);
                int randoImageno = Random.Range(0, TensNameImageList.Count);
                TotalImagesPositions[randomno].GetComponent<SpriteRenderer>().sprite = TensNameImageList[randoImageno];
                TotalImagesPositions.Remove(TotalImagesPositions[randomno]);
            }
        }
        PlayRandomAudioClip(FindTensAudioClip[CurrentRandomNo-1]);
        yield return new WaitForSeconds(FindTensAudioClip[CurrentRandomNo-1].length);
        AudioQueBtn.SetActive(true);
        AudioManager.instance.PauseAudio(false);
        isLevel2MascotSpeech = false;

        if (awayfromKeyBoardcoroutine != null)
        {
            StopCoroutine(awayfromKeyBoardcoroutine);
        }
        awayfromKeyBoardcoroutine = StartCoroutine(checkawayfromKeyBoard());
       
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

    public void PlayRandomAudioClip(AudioClip clip)
    {
        //if (CanvasManager.instance.PlaySoundCount == 0)
        //{
            GetComponent<AudioSource>().clip = clip;
            GetComponent<AudioSource>().Play();
            //GetComponent<AudioSource>().PlayOneShot(clip);
        //}
    }

    public void HintComPlayAgain()
    {
        HintPrefCount = 0;
        isSkipEnable = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void HintComPlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    static int levelComCount;
    public IEnumerator CheckGameCompleteOrNot()
    {
        if (CheckLevelTwoGameCom())
        {
            //true

            if (isPlayAgain)
            {
                isLevel2MascotSpeech = true;
                yield return new WaitForSeconds(1);
                DisableImages();
                PlayTensAudio(CurrentRandomNo - 1);
                yield return new WaitForSeconds(TensNameClips[CurrentRandomNo - 1].length + 1);
                //int soundRandom = Random.Range(0, 3);
                //if (soundRandom == 0)
                //{
                //    PlayRandomAudioClip(AudioManager.instance.LevelComplete);
                //    yield return new WaitForSeconds(AudioManager.instance.LevelComplete.length + 1);
                //}

                //if (levelComCount < 3)
                //{
                levelComCount++;
                print("----------->>>>" + levelComCount);
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCount", levelComCount);
                if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "LevelComplete", 0) == 0)
                {
                    DOTween.To(() => LevelTwoSlider.value, x => LevelTwoSlider.value = x, (float)(levelComCount) * (1f / 9f), 1).OnStart(() =>
                    {
                        SliderFill.SetActive(true);
                    });
                }
                yield return new WaitForSeconds(3);

                if (levelComCount == 9)
                {
                    CanvasManager.instance.RewardPanel.SetActive(true);
                    yield return new WaitForSeconds(3);
                    CanvasManager.instance.RewardPanel.SetActive(false);
                    CanvasManager.instance.TotalScore += 10;
                    CanvasManager.instance.LevelCompleteAnimaton.SetActive(true);
                    GameCompleteSound();
                    yield return new WaitForSeconds(AudioManager.instance.GameCompleteSound.length + 1);
                    isLevel2MascotSpeech = false;
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
                    isLevel2MascotSpeech = false;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                //}
                //else
                //{
                //    //PlayRandomAudioClip(AudioManager.instance.LevelComplete);
                //    //yield return new WaitForSeconds(AudioManager.instance.LevelComplete.length + 1);
                //    CanvasManager.instance.RewardPanel.SetActive(true);
                //    yield return new WaitForSeconds(3);
                //    CanvasManager.instance.RewardPanel.SetActive(false);
                //    CanvasManager.instance.TotalScore += 10;
                //    CanvasManager.instance.LevelCompleteAnimaton.SetActive(true);
                //    GameCompleteSound();
                //    yield return new WaitForSeconds(AudioManager.instance.GameCompleteSound.length + 1);
                //    isLevel2MascotSpeech = false;
                //    CanvasManager.instance.LevelComPanel.SetActive(true);
                //}
            }
            else
            {
                isLevel2MascotSpeech = true;
                yield return new WaitForSeconds(1);
                DisableImages();
                PlayTensAudio(CurrentRandomNo - 1);
                yield return new WaitForSeconds(TensNameClips[CurrentRandomNo - 1].length + 3);
                //int soundRandom = Random.Range(0, 3);
                //if (soundRandom == 0)
                //{
                //    PlayRandomAudioClip(AudioManager.instance.LevelComplete);
                //    yield return new WaitForSeconds(AudioManager.instance.LevelComplete.length + 1);
                //}
                levelComCount++;
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCount", levelComCount);
                //DOTween.To(() => LevelTwoSlider.value, x => LevelTwoSlider.value = x, (float)(levelComCount) * (1f / 9f), 1).OnStart(() =>
                //{
                //    SliderFill.SetActive(true);
                //});

                if (DontDestroyRandomNoList.instance.checkRandomNo.Count > 0)
                {

                    yield return new WaitForSeconds(3);
                    isLevel2MascotSpeech = false;
                    ChangerandomNo();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                else
                {
                    //levelComCount++;
                    //DOTween.To(() => LevelTwoSlider.value, x => LevelTwoSlider.value = x, 1, 1).OnStart(() =>
                    //{
                    //    SliderFill.SetActive(true);
                    //});
                    isLevel2MascotSpeech = false;
                    CanvasManager.instance.PlayAgainPanel.SetActive(true);


                    AudioManager.instance.PauseAudio(true);
                    PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[3]);
                    CanvasManager.instance.PlayAgainPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
                    yield return new WaitForSeconds(AudioManager.instance.GL_Audio_Clips[3].length);
                    CanvasManager.instance.PlayAgainPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
                    AudioManager.instance.PauseAudio(false);
                }
            }
        }
    }

    public IEnumerator wrongAns(GameObject wrongChild)
    {
        if (isPlayAgain)
        {
            if (!AlreadyWrongSelected)
            {
                print("----------->>>>" + levelComCount);
                levelComCount--;
                //DOTween.To(() => LevelTwoSlider.value, x => LevelTwoSlider.value = x, (float)(levelComCount) * (1f / 9f), 1).OnStart(() =>
                //{
                //    SliderFill.SetActive(true);
                //});
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCount", levelComCount);                
                DontDestroyRandomNoList.instance.checkRandomNo.Add(CurrentRandomNo);
                AlreadyWrongSelected = true;
            }
        }
        AudioManager.instance.PauseAudio(true);
        isLevel2MascotSpeech = true;
        wrongChild.transform.GetChild(1).gameObject.SetActive(true);
        wrongChild.transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        wrongChild.transform.GetChild(1).gameObject.SetActive(false);
        wrongChild.transform.GetChild(0).gameObject.SetActive(false);
        int currentClickedObj =int.Parse(wrongChild.GetComponent<SpriteRenderer>().sprite.name)-1;
        //PlayTensAudio(currentClickedObj);
        //yield return new WaitForSeconds(TensNameClips[currentClickedObj].length);
        PlayRandomAudioClip(AudioManager.instance.GL_Audio_Clips[12]);
        yield return new WaitForSeconds(AudioManager.instance.GL_Audio_Clips[12].length);
        PlayRandomAudioClip(FindTensAudioClip[CurrentRandomNo-1]);
        yield return new WaitForSeconds(FindTensAudioClip[CurrentRandomNo-1].length);
        AudioManager.instance.PauseAudio(false);
        isLevel2MascotSpeech = false;
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
                 !CanvasManager.instance.PreviousPanel.activeSelf && !CanvasManager.instance.HomePanel.activeSelf && HintPrefCount == 1)
                StartCoroutine(Second15Complete());
        }
        else
        {
            //
        }
    }


    IEnumerator Second15Complete()
    {
        isLevel2MascotSpeech = true;
        AudioManager.instance.PauseAudio(true);
        if (!GetComponent<AudioSource>().isPlaying)
        {
            PlayRandomAudioClip(FindTensAudioClip[CurrentRandomNo - 1]);
            yield return new WaitForSeconds(FindTensAudioClip[CurrentRandomNo - 1].length);
        }
        AudioManager.instance.PauseAudio(false);

        isLevel2MascotSpeech = false;
        if (awayfromKeyBoardcoroutine != null)
        {
            StopCoroutine(awayfromKeyBoardcoroutine);
        }
        awayfromKeyBoardcoroutine = StartCoroutine(checkawayfromKeyBoard());
    }

    public bool CheckLevelTwoGameCom()
    {
        for (int i = 0; i < TotalLevelCompleteObjects.Count; i++)
        {
            if (TotalLevelCompleteObjects[i].GetComponent<CardClick>().isCorrectAns == false)
            {
                return false;
            }
        }
        return true;
    }


    public void PrevioClick()
    {
        if (!isLevel2MascotSpeech)
            StartCoroutine(previousBtnClick());
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

    public void previousLevelClick()
    {
        //Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void previousPlayGame()
    {
        //Time.timeScale = 1;
        CanvasManager.instance.PreviousPanel.SetActive(false);
    }

    public void LevelComPlayAgain()
    {
        //HintPrefCount = 0;
        levelComCount = 0;
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "LevelCompCount", levelComCount);
        DestroyObject(FindObjectOfType<DontDestroyRandomNoList>().gameObject);
      
        isSkipEnable = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);     
    }

    public void PickNewGame()
    {
        if (FindObjectOfType<DontDestroyRandomNoList>() != null)
        {
            Destroy(FindObjectOfType<DontDestroyRandomNoList>().gameObject);
        }
        SceneManager.LoadScene("Level3");
    }

    public void HomeBtnClick()
    {
        if (!isLevel2MascotSpeech)
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

        //GetComponent<AudioSource>().Pause();
        //Time.timeScale = 0;
        //CanvasManager.instance.SkipPannels.SetActive(true);
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

    public void ExitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ContinueGame()
    {
        //if (CanvasManager.instance.PlaySoundCount == 0)
        //{
        GetComponent<AudioSource>().Play();
        //}
        Time.timeScale = 1;
        CanvasManager.instance.SkipPannels.SetActive(false);
    }

}
