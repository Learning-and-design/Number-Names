using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DragImage : MonoBehaviour
{
    private bool isDragging;
    bool isChangePosi = false;
    Vector3 startPosition;
    Vector2 mousePosition;
    public Vector3 SlotCrrectPosi;
   public  bool isDoNotMove = false;

    private void Start()
    {
        startPosition = transform.position;
    }

    public void OnMouseDown()
    {
        if(!GameLevelZeroController.instance.isMascotSpeech)
        isDragging = true;
    }

    public void OnMouseUp()
    {
        if (!GameLevelZeroController.instance.isMascotSpeech)
        {
            isDragging = false;
            if (!isChangePosi)
            {
                transform.DOMove(startPosition, 0.5f);
            }
            else
            {
                if (!isDoNotMove)
                {
                    //0.093f, 0.509f, 0
                    transform.DOLocalMove(SlotCrrectPosi, 0.5f).OnComplete(() =>
                    {
                        isDoNotMove = true;
                        //GameLevelZeroController.instance.PlayTensAudio(GameLevelZeroController.instance.currentLevel);
                        GameLevelZeroController.instance.NumberCard0.transform.GetChild(0).gameObject.SetActive(false);
                        GameLevelZeroController.instance.NumberCard0.transform.Find(GameLevelZeroController.instance.currentLevel.ToString()).GetComponent<SpriteRenderer>().sortingOrder = 5;
                        StartCoroutine(GameLevelZeroController.instance.CheckLevelIsComlete());
                    });
                }
            }
        }
    }

    void Update()
    {
        if (isDragging && !GameLevelZeroController.instance.isMascotSpeech)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (!isDoNotMove)
                transform.Translate(mousePosition);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "OponantImage")
        {
            isChangePosi = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "OponantImage")
        {
            isChangePosi = false;
        }
    }
}
