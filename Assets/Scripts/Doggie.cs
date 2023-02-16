using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Doggie : MonoBehaviour
{
    private CharacterController player;
    private Rigidbody2D rb;
    private bool playerInZone = true;
    private SpriteRenderer renderer;
    [SerializeField] private float speed;
    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        player = FindAnyObjectByType<CharacterController>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Vector3 dir = player.transform.position - transform.position;
            float length = dir.magnitude;
            dir.Normalize();

            transform.DOMove(player.transform.position - dir * .5f, 1f); ;
            playerInZone = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = player.transform.position - transform.position;
        float length = dir.magnitude;
        dir.Normalize();

        if (dir.x > 0) renderer.flipX = true;
        else renderer.flipX = false;
    }
}
