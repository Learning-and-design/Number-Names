using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueReplayBtnLevelF : MonoBehaviour
{
    private void OnMouseDown()
    {
        StartCoroutine(GameLevelFourController.instance.QueRplayBtnClickMethod());
    }
}
