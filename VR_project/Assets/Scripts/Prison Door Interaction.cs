using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonDoorInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private GameObject indicator;
    public bool haveKey = false;

    public void OnAbortInteract()
    {
        indicator.SetActive(false); //hide
        //throw new System.NotImplementedException();
    }

    public void OnEndInteract()
    {
      
        //throw new System.NotImplementedException();
    }

    public void OnInteract(Interactor interactor)
    {
        indicator.SetActive(false); //hide
        //call interactor's public method ReceiveInteract
        //...with override method that gets a string as a parameter       
        if (haveKey)
        {
            m_Animator.SetTrigger("Run");
            if (m_Animator.GetBool("isOpen"))
            {
                m_Animator.SetBool("isOpen", false);
            }
            else
            {
                m_Animator.SetBool("isOpen", true);
            }
        }
        else
        {
            interactor.ReceiveInteract("You have to find the key first");
        }               
    }

    public void OnReadyInteract()
    {
        indicator.SetActive(true); //show
    }
   
}
