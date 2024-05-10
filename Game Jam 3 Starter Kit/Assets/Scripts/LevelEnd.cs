using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public LevelLoader levelloader;
    // Start is called before the first frame update
    void Start()
    {
        levelloader = GameObject.FindObjectOfType<LevelLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            levelloader.levelnumber += 1;
            levelloader.LoadMap();
        }
    }
}
