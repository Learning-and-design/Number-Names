using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueReplayBtnLevel3 : MonoBehaviour
{
    private void OnMouseDown()
    {
        StartCoroutine(GameLevelThreeController.instance.QueReplBtn());
    }
}
