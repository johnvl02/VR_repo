using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] PrisonDoorInteraction door;
    [SerializeField] private GameObject indicator;
    private float timer;
    private float waitTime;
    private bool interact = false;
    
    void Start()
    {
        timer = 0;
        waitTime = 5f;
    }
    void FixedUpdate()
    {
        if (interact)
        {
            timer = timer + Time.deltaTime;
            if (timer > waitTime)
            {
                gameObject.SetActive(false);
            }
        }
        
    }

    public void OnAbortInteract()
    {
        indicator.SetActive(false);
    }

    public void OnEndInteract()
    {
        
    }

    public void OnInteract(Interactor interactor)
    {
        door.haveKey = true;
        interact = true;
        indicator.SetActive(false);
        interactor.ReceiveInteract("You have found the key! Now you can escape the prison");
        
    }

    public void OnReadyInteract()
    {
        indicator.SetActive(true);
    }
    
}
