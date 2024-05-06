using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Vector3 spawnpoint;
    public Collider2D boxcollider;
    public GameObject coin;
    public float raycastLength = .1f;
    // Start is called before the first frame update
    void Start()
    {
        boxcollider = GetComponent<Collider2D>();
        spawnpoint = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 rayStartl = new Vector2(boxcollider.bounds.min.x, boxcollider.bounds.min.y - .001f);
        Vector2 rayStartr = new Vector2(boxcollider.bounds.max.x, boxcollider.bounds.min.y - .001f);
        Vector2 rayStartm = new Vector2(boxcollider.bounds.center.x, boxcollider.bounds.min.y - .001f);

        //Cast a ray downwards to check if the player is grounded
        RaycastHit2D hitm = Physics2D.Raycast(rayStartm, Vector2.down, raycastLength);
        RaycastHit2D hitr = Physics2D.Raycast(rayStartr, Vector2.down, raycastLength);
        RaycastHit2D hitl = Physics2D.Raycast(rayStartl, Vector2.down, raycastLength);
        if (hitr.collider != null && hitr.collider.CompareTag("Player")||hitl.collider != null && hitl.collider.CompareTag("Player")||hitm.collider != null && hitm.collider.CompareTag("Player"))
        {
            Instantiate(coin, spawnpoint, Quaternion.identity);
            //darken box
            enabled = false;
        }
    }
}
