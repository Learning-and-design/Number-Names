using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLevelComplete : MonoBehaviour
{

    public GameObject[] TensComponents;
    public GameObject[] Slot_tens;
    public GameObject TensCard;
    public static CheckLevelComplete instance;

    private void Awake()
    {
        instance = this;
    }

    public bool CheckGameCom()
    {
        for(int i=0; i< TensComponents.Length; i++)
        {
            if (TensComponents[i].GetComponent<DragSlotTens>().isDoNotMove == false)
            {
                return false;
            }
        }
        return true;
    }

    public bool CheckTensAndCardDone()
    {
        if (CheckGameCom() == false || TensCard.GetComponent<DragImage>().isDoNotMove == false)
        {
            return false;
        }
        return true;
    }
}
