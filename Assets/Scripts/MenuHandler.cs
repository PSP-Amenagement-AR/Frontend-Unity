using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public Animator animator;
    public void OpenMenu()
    {
        if (animator != null)
        {
            animator.SetBool("open", !animator.GetBool("open"));
        }
    }
}
