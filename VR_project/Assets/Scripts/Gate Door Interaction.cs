using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateDoorInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private Animator r_Animator;
    [SerializeField] private Animator l_Animator;
    [SerializeField] private GameObject indicator;    

    public void OnAbortInteract()
    {
        m_Animator.SetBool("Interact", false);
        transform.position += new Vector3(0, 0.1f, 0);
        indicator.SetActive(false); //hide
        //throw new System.NotImplementedException();
    }

    public void OnEndInteract()
    {
        //transform.position += new Vector3(0, 0.2f, 0);
        m_Animator.SetBool("Interact", false);
        //throw new System.NotImplementedException();
    }

    public void OnInteract(Interactor interactor)
    {
        indicator.SetActive(false); //hide
        //call interactor's public method ReceiveInteract
        //...with override method that gets a string as a parameter               
        r_Animator.SetTrigger("Run");
        l_Animator.SetTrigger("Run");
        if (r_Animator.GetBool("isOpen") && l_Animator.GetBool("isOpen"))
        {
            interactor.ReceiveInteract("The Gate has closed");
            r_Animator.SetBool("isOpen", false);
            l_Animator.SetBool("isOpen", false);
        }
        else
        {
            interactor.ReceiveInteract("The Gate is open");
            r_Animator.SetBool("isOpen", true);
            l_Animator.SetBool("isOpen", true);
        }    
        
        //throw new System.NotImplementedException();
    }

    public void OnReadyInteract()
    {
        transform.position += new Vector3(0, - 0.1f, 0); 
        m_Animator.SetBool("Interact", true);
        indicator.SetActive(true); //show
    }
   
}
