using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneBtnClickLevelF : MonoBehaviour
{
    private void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().color = new Color32(200, 200, 200, 255);
        print("=======OnMouseDown=======>>>>>>");
        StartCoroutine(waitDoneClick());
    }

    private void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        print("=======OnMouseUp=======>>>>>>");
    }

    IEnumerator waitDoneClick()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(GameLevelFourController.instance.DoneButtonClick());
    }
}
