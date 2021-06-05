using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwoQuesReplayBtn : MonoBehaviour
{
    private void OnMouseDown()
    {
        StartCoroutine(QueClick());
    }

    IEnumerator QueClick()
    {
        if (!GameLevelTwoController.instance.GetComponent<AudioSource>().isPlaying && !GameLevelTwoController.instance.isLevel2MascotSpeech && CanvasManager.instance.PlaySoundCount == 0)
        {
            AudioManager.instance.PauseAudio(true);
            GetComponent<CircleCollider2D>().enabled = false;

            GameLevelTwoController.instance.PlayRandomAudioClip(GameLevelTwoController.instance.FindTensAudioClip[GameLevelTwoController.CurrentRandomNo - 1]);
            yield return new WaitForSeconds(GameLevelTwoController.instance.FindTensAudioClip[GameLevelTwoController.CurrentRandomNo - 1].length);
            AudioManager.instance.PauseAudio(false);
            GetComponent<CircleCollider2D>().enabled = true;
        }
    }
}
