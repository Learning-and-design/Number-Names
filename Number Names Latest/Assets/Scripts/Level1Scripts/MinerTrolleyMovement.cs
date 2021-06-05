using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MinerTrolleyMovement : MonoBehaviour
{
    int speed = 3;
    public GameObject RightPosition;
    public GameObject LeftPosition;
    public GameObject ChildObject;
    public static MinerTrolleyMovement instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        AnimationMiner();
    }

    public void AnimationMiner()
    {
        transform.DOMove(new Vector3(0, 0.135f, 0), speed);

        //transform.DOLocalMove(new Vector3(3.1f, 0.135f), speed).OnComplete(() =>
        //{
        //    transform.rotation = Quaternion.Euler(0, 180,0);
        //    for (int i = 0; i < transform.childCount; i++)
        //    {
        //        if (transform.GetChild(i).GetComponent<SpriteRenderer>().flipX)
        //        {
        //            transform.GetChild(i).GetComponent<SpriteRenderer>().flipX = false;
        //        }
        //        else
        //        {
        //            transform.GetChild(i).GetComponent<SpriteRenderer>().flipX = true;
        //        }
        //    }

        //    transform.DOLocalMove(new Vector3(-3.1f, 0.135f), speed).OnComplete(() =>
        //    {
        //        transform.rotation = Quaternion.Euler(0, 0, 0);
        //        AimationMiner();
        //        for (int i = 0; i < transform.childCount; i++)
        //        {
        //            if (transform.GetChild(i).GetComponent<SpriteRenderer>().flipX)
        //            {
        //                transform.GetChild(i).GetComponent<SpriteRenderer>().flipX = false;
        //            }
        //            else
        //            {
        //                transform.GetChild(i).GetComponent<SpriteRenderer>().flipX = true;
        //            }
        //        }
        //    });
        //});
    }
}
