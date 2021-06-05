using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //private void Awake()
    //{
    //    StartCoroutine(wait());
    //}

    //IEnumerator wait()
    //{
    //    yield return new WaitForSeconds(1);
    //    SceneManager.LoadScene("Level0");
    //}

    public void changescene()
    {
        SceneManager.LoadScene("Level0");
    }

}
