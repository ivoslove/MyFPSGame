using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = this;
            }
            return instance;
        }
    }

    void Start()
    {
   
        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
