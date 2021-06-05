using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DragCardLvlFour : MonoBehaviour
{
    private bool isDragging;
    bool isChangePosi = false;
    public Vector3 startPosition;
    Vector2 mousePosition;
    public Vector3 SlotCrrectPosi;
    public bool isDoNotMove = false;

    private void Start()
    {
        startPosition = transform.position;
    }

    public void OnMouseDown()
    {
        if(!GameLevelFourController.instance.isLevel4MascotSpeech)
        isDragging = true;
    }

    public void OnMouseUp()
    {
        if (!GameLevelFourController.instance.isLevel4MascotSpeech)
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
                        GameLevelFourController.instance.SelectCard = gameObject;
                        GameLevelFourController.instance.CardDestinationHandler.GetComponent<BoxCollider2D>().enabled = false;
                        gameObject.GetComponent<BoxCollider2D>().enabled = false;
                        GameLevelFourController.instance.DoneButton.SetActive(true);
                        GameLevelFourController.instance.DoneButton.GetComponent<CircleCollider2D>().enabled = true;
                    });
                }
            }
        }
    }

    void Update()
    {
        if (isDragging && !GameLevelFourController.instance.isLevel4MascotSpeech)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (!isDoNotMove)
                transform.Translate(mousePosition);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "OponantImage")
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
