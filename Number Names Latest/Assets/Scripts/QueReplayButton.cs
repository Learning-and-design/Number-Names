using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueReplayButton : MonoBehaviour
{

    private void OnMouseDown()
    {
        StartCoroutine(WaitMethod());
    }

    IEnumerator WaitMethod()
    {
        print("--------------question repeat-------->>>>>>");
        if (!GameLevelOneController.instance.GetComponent<AudioSource>().isPlaying && !GameLevelOneController.instance.isLevel1MascotisSpeech && CanvasManager.instance.PlaySoundCount == 0)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            AudioManager.instance.PauseAudio(true);
            GameLevelOneController.instance.isLevel1MascotisSpeech = true;
            GameLevelOneController.instance.PlayAudio(1);
            yield return new WaitForSeconds(GameLevelOneController.instance.AudioClips[1].length + 1);
            GameLevelOneController.instance.PlayTensAudio(GameLevelOneController.CurrentRandomNo - 1);
            yield return new WaitForSeconds(GameLevelOneController.instance.TensNameClips[GameLevelOneController.CurrentRandomNo - 1].length + 1);
            AudioManager.instance.PauseAudio(true);
            GameLevelOneController.instance.isLevel1MascotisSpeech = false;
            GetComponent<CircleCollider2D>().enabled = true;
        }
    }
}
