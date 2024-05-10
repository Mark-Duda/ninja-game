using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : MonoBehaviour
{
    public HUD hud;
    // Start is called before the first frame update
    void Start()
    {
        hud = GameObject.FindObjectOfType<HUD>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("untaged");
        if (other.gameObject.CompareTag("Player"))
        {
            hud.Death();
        }
    }
}
