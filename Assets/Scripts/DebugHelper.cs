using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHelper : MonoBehaviour
{

    public bool DebugMode = false;
    public List<GameObject> EnableOnStart;

    // Start is called before the first frame update
    void Start()
    {
        if (DebugMode) return;
        foreach (GameObject g in EnableOnStart) g.SetActive(true);
    }

}
