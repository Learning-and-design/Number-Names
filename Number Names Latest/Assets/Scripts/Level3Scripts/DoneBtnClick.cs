using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneBtnClick : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (!GameLevelThreeController.instance.isLevel3MascotSpeech)
        {
            GetComponent<SpriteRenderer>().color = new Color32(200, 200, 200, 255);
            StartCoroutine(ClickWait());
        }
    }

    private void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }

    IEnumerator ClickWait()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(GameLevelThreeController.instance.CheckLevelCompleteOrNot());
    }
}
