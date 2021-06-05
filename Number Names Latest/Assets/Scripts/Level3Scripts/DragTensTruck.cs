using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DragTensTruck : MonoBehaviour
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
        startPosition = transform.position;
    }

    public void OnMouseDown()
    {
        if (!GameLevelThreeController.instance.isLevel3MascotSpeech)
        {
            isDragging = true;
        }
    }
    //int Count;
    public void OnMouseUp()
    {
        if (!GameLevelThreeController.instance.isLevel3MascotSpeech)
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
                        GameLevelThreeController.instance.DoneButton.GetComponent<CircleCollider2D>().enabled = false;
                    //transform.parent = disbleObject.transform.parent.transform;
                    transform.GetComponent<BoxCollider2D>().enabled = false;
                        disbleObject.GetComponent<BoxCollider2D>().enabled = false;
                    //GameLevelThreeController.instance.CheckSlotPosi(gameObject);
                    GameLevelThreeController.instance.AllSlots[GameLevelThreeController.instance.Count].GetComponent<SpriteRenderer>().enabled = false;
                        GameLevelThreeController.instance.AllSlots[GameLevelThreeController.instance.Count].GetComponent<Animator>().enabled = false;

                        Vector3 des = new Vector3(GameLevelThreeController.instance.AllSlots[GameLevelThreeController.instance.Count].transform.localPosition.x, GameLevelThreeController.instance.AllSlots[GameLevelThreeController.instance.Count].transform.localPosition.y + 0.05f, GameLevelThreeController.instance.AllSlots[GameLevelThreeController.instance.Count].transform.localPosition.z);
                        transform.DOLocalMove(des, 0.2f).OnComplete(() =>
                        {
                            GameLevelThreeController.instance.Count++;
                            disbleObject.GetComponent<BoxCollider2D>().enabled = true;
                            GameLevelThreeController.instance.LevelCompleObjList.Add(gameObject);
                            GameLevelThreeController.instance.DoneButton.SetActive(true);
                            GameLevelThreeController.instance.DoneButton.GetComponent<CircleCollider2D>().enabled = true;

                        });
                    });
                }
            }
        }
    }

    void Update()
    {
        if (isDragging && !GameLevelThreeController.instance.isLevel3MascotSpeech)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (!isDoNotMove)
                transform.Translate(mousePosition);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //print("gameName---------->>" + collision.gameObject.name);
        if (collision.gameObject.tag == "OponantSlotImg")
        {
            isChangePosi = true;
            destination = new Vector3(collision.gameObject.transform.localPosition.x +0.10f, (collision.gameObject.transform.localPosition.y + 0.03f), collision.gameObject.transform.localPosition.z);
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
