using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarmanInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private GameObject indicator;

    public void OnAbortInteract()
    {
        indicator.SetActive(false);
                
    }

    public void OnEndInteract()
    {        
    }

    public void OnInteract(Interactor interactor)
    {
        m_Animator.SetBool("Change",false);
        indicator.SetActive(false); //hide
    }

    public void OnReadyInteract()
    {
        m_Animator.SetBool("Change", true);
        indicator.SetActive(true);
    }
}
