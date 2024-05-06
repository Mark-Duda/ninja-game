using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coincollect : MonoBehaviour
{
    // Start is called before the first frame update
    public HUD hud;
   
    // Start is called before the first frame update
    void Start()
    {
        hud = GameObject.FindObjectOfType<HUD>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            CollectCoin(1);
            Destroy(other.gameObject);
        }
    }

    void CollectCoin(int amount)
    {
        hud.gold+=amount;
        Debug.Log("Gold: "+hud.gold);
    }
}
