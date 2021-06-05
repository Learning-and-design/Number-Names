using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DragTrollyBlock : MonoBehaviour
{
    private bool isDragging;
    bool isChangePosi = false;
    public  Vector3 startPosition;
    Vector2 mousePosition;
    public Vector3 SlotCrrectPosi;
    public GameObject destination;

   public  bool isDoNotMove = false;

    public void Start()
    {
        startPosition = transform.position;
    }

    public void OnMouseDown()
    {
        if(!GameLevelOneController.instance.isLevel1MascotisSpeech)
        isDragging = true;
    }

    public void OnMouseUp()
    {
        if (!GameLevelOneController.instance.isLevel1MascotisSpeech)
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
                    transform.parent = destination.transform;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    MinerTrolleyMovement.instance.ChildObject = destination;
                    transform.DOLocalMove(SlotCrrectPosi, 0.5f).OnComplete(() =>
                    {
                        isDoNotMove = true;
                        StartCoroutine(GameLevelOneController.instance.CheckLevelComplete());
                    });
                }
            }
        }
    }

    void Update()
    {
        if (isDragging && !GameLevelOneController.instance.isLevel1MascotisSpeech)
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
            destination = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "OponantImage")
        {
            isChangePosi = false;
            destination = null;
        }
    }
}
