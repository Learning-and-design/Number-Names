using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DragSlotTens : MonoBehaviour
{
    private bool isDragging;
    bool isChangePosi = false;
    Vector3 startPosition;
    Vector2 mousePosition;
    Vector3 destination;
    public bool isDoNotMove = false;
    GameObject disbleObject;


    private void Start()
    {
        startPosition = transform.position;
    }

    public void OnMouseDown()
    {
        isDragging = true;
    }

    public void OnMouseUp()
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
                transform.DOLocalMove(destination, 0.5f).OnComplete(() =>
                {
                    isDoNotMove = true;

                    for (int i = 0; i < CheckLevelComplete.instance.Slot_tens.Length; i++)
                    {
                        if(CheckLevelComplete.instance.Slot_tens[i].name != "Occupies")
                        {
                            destination = new Vector3(CheckLevelComplete.instance.Slot_tens[i].transform.localPosition.x - 0.03f, (CheckLevelComplete.instance.Slot_tens[i].transform.localPosition.y + 0.07f), CheckLevelComplete.instance.Slot_tens[i].transform.localPosition.z);
                            transform.DOLocalMove(destination, 0.5f);
                            transform.GetComponent<BoxCollider2D>().enabled = false;
                            CheckLevelComplete.instance.Slot_tens[i].GetComponent<BoxCollider2D>().enabled = false;
                            CheckLevelComplete.instance.Slot_tens[i].name = "Occupies";
                            break;
                        }
                    }

                   

                    StartCoroutine(GameLevelZeroController.instance.CheckLevelIsComlete());
                });
            }
        }
    }

    void Update()
    {
        if (isDragging  && !GameLevelZeroController.instance.isMascotSpeech)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if(!isDoNotMove)
            transform.Translate(mousePosition);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "OponantSlotImg")
        {
            isChangePosi = true;
            destination =new Vector3 (collision.gameObject.transform.localPosition.x - 0.03f, (collision.gameObject.transform.localPosition.y + 0.07f), collision.gameObject.transform.localPosition.z);
            disbleObject = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "OponantSlotImg")
        {
            isChangePosi = false;
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "OponantSlotImg")
        {
            if (isDoNotMove)
            {
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                print("Disable------>>");
            }
        }
    }

}
