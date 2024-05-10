using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI coin;
    public int gold;
    public LevelLoader levelloader;
    public int starttime;
    public int timer;
    public float time;
    public TextMeshProUGUI gametime;
    // Start is called before the first frame update
    void Start()
    {
        starttime = 180;
    }

    // Update is called once per frame
    void Update()
    {

        coin.text = ""+gold;
        time+= Time.deltaTime;

        timer = starttime- Mathf.RoundToInt(time);
        gametime.text = timer+"s";
        if (timer < 1)
        {
            Death();
        }
    }

    public void Death()
    {
        timer = 180;
        levelloader.LoadMap(); 
    }
    
}
