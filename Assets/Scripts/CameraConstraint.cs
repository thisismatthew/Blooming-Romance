using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConstraint : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float contraintRadius;
    private Vector3 startingPos;

    private void Start()
    {
        startingPos = transform.position;
    }

    private void Update()
    {
        //get direction to player
        Vector2 dir = player.transform.position - startingPos;
        float length = dir.magnitude;
        dir.Normalize();

        if (length <= contraintRadius)
        {
            transform.position = player.transform.position;
        }
        else
        {
            transform.position = (Vector2)startingPos + dir * contraintRadius;
        }


    }


}
