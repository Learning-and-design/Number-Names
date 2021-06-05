using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyRandomNoList : MonoBehaviour
{
    public List<int> checkRandomNo = new List<int>();  
    public static DontDestroyRandomNoList instance;

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }       
        
    }

}
