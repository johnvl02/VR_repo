using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarmanInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private GameObject indicator;
    [TextArea(3, 10)]
    public string text;

    public void OnAbortInteract()
    {
        indicator.SetActive(false);

                
    }

    public void OnEndInteract()
    {        
    }

    public void OnInteract(Interactor interactor)
    {
        indicator.SetActive(false);
        m_Animator.SetBool("Change",false);
        interactor.ReceiveInteract(text);
        //indicator.SetActive(false); //hide
    }

    public void OnReadyInteract()
    {
        m_Animator.SetBool("Change", true);
        indicator.SetActive(true);        
    }
}
