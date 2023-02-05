using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform FollowTarget;
    // Update is called once per frame
    void Update()
    {
        transform.position = FollowTarget.transform.position;
    }
}
