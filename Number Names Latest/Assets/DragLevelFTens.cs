using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DragLevelFTens : MonoBehaviour
{
    private bool isDragging;
    bool isChangePosi = false;
    public Vector3 startPosition;
    Vector2 mousePosition;
    Vector3 destination;
    public bool isDoNotMove = false;
    GameObject disbleObject;


    private void Start()
    {
        startPosition = transform.localPosition;
    }

    public void OnMouseDown()
    {
      if(!GameLevelFourController.instance.isLevel4MascotSpeech)
        isDragging = true;
    }
    //int Count;
    public void OnMouseUp()
    {
        if (!GameLevelFourController.instance.isLevel4MascotSpeech)
        {
            isDragging = false;
            if (!isChangePosi)
            {
                transform.DOLocalMove(startPosition, 0.5f);
            }
            else
            {

                if (!isDoNotMove)
                {
                    transform.DOLocalMove(destination, 0.5f).OnComplete(() =>
                    {
                        isDoNotMove = true;
                        GameLevelFourController.instance.DoneButton.GetComponent<CircleCollider2D>().enabled = false;
                        //transform.parent = disbleObject.transform.parent.transform;
                        transform.GetComponent<BoxCollider2D>().enabled = false;
                        //disbleObject.GetComponent<BoxCollider2D>().enabled = false;
                        //GameLevelThreeController.instance.CheckSlotPosi(gameObject);
                        GameLevelFourController.instance.LevelFourAllSlots[GameLevelFourController.instance.Count].GetComponent<SpriteRenderer>().enabled = false;
                        GameLevelFourController.instance.LevelFourAllSlots[GameLevelFourController.instance.Count].GetComponent<Animator>().enabled = false;

                        Vector3 des = new Vector3(GameLevelFourController.instance.LevelFourAllSlots[GameLevelFourController.instance.Count].transform.localPosition.x, GameLevelFourController.instance.LevelFourAllSlots[GameLevelFourController.instance.Count].transform.localPosition.y + 0.06f, GameLevelFourController.instance.LevelFourAllSlots[GameLevelFourController.instance.Count].transform.localPosition.z);
                        transform.DOLocalMove(des, 0.2f).OnComplete(() =>
                        {
                            disbleObject.GetComponent<BoxCollider2D>().enabled = true;
                            GameLevelFourController.instance.LevelFourAllSlots[GameLevelFourController.instance.Count].GetComponent<BoxCollider2D>().enabled = false;
                            GameLevelFourController.instance.Count++;
                            GameLevelFourController.instance.LevelCompleteObj.Add(gameObject);
                            GameLevelFourController.instance.DoneButton.SetActive(true);
                            GameLevelFourController.instance.DoneButton.GetComponent<CircleCollider2D>().enabled = true;
                        });
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
        if (collision.gameObject.tag == "OponantSlotImg")
        {
            isChangePosi = true;
            //print("gameName---------->>" + collision.gameObject.name);
            destination = new Vector3(collision.gameObject.transform.localPosition.x , (collision.gameObject.transform.localPosition.y + 0.09f), collision.gameObject.transform.localPosition.z);
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
            isChangePosi = true;
            if (isDoNotMove)
            {
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                print("Disable------>>");
            }
        }
    }
}
