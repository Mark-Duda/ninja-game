using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Vector2 offset;

    public GameObject player;
    private Vector3 offset3D;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(delay());
        player = GameObject.FindWithTag("Player");
        offset3D = new Vector3(offset.x, offset.y, -10f);
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(.1f);
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset3D;
    }
}