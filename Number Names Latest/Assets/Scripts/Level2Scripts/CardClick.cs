using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClick : MonoBehaviour
{
    public bool isCorrectAns = false;

    private void OnMouseDown()
    {
        //print("Click----------->>" + int.Parse(GetComponent<SpriteRenderer>().sprite.name) + "---Curre---->>" + GameLevelTwoController.CurrentRandomNo);
        if (!GameLevelTwoController.instance.isLevel2MascotSpeech)
        {
            if ((int.Parse(GetComponent<SpriteRenderer>().sprite.name) - 1) == (GameLevelTwoController.CurrentRandomNo-1))
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
                isCorrectAns = true;
                StartCoroutine(GameLevelTwoController.instance.CheckGameCompleteOrNot());
            }
            else
            {
                //transform.GetChild(1).gameObject.SetActive(true);
                //transform.GetChild(0).gameObject.SetActive(false);
                isCorrectAns = false;
                StartCoroutine(GameLevelTwoController.instance.wrongAns(gameObject));
            }
        }
    }
}
