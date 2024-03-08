using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator r_Animator;
    [SerializeField] private Animator l_Animator;
    [SerializeField] private GameObject indicator;    

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
        r_Animator.SetTrigger("Run"); 
        l_Animator.SetTrigger("Run");
        if ( r_Animator.GetBool("isOpen") && l_Animator.GetBool("isOpen"))
        {            
            r_Animator.SetBool("isOpen", false);
            l_Animator.SetBool("isOpen", false);
        }
        else
        {
            r_Animator.SetBool("isOpen", true);
            l_Animator.SetBool("isOpen", true);
        }

        //throw new System.NotImplementedException();
    }

    public void OnReadyInteract()
    {
        indicator.SetActive(true); //show
    }

    /*// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
    }*/
}
