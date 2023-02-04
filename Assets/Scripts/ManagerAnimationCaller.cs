using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerAnimationCaller : MonoBehaviour
{
    private NotebookManager nm;
    private void Start()
    {
        nm = FindObjectOfType<NotebookManager>();
    }

    public void CallFadeUI()
    {
        nm.FadeInUI();
    }

    public void CallFadeOutUI()
    {
        nm.FadeOutUI();
    }
}
