using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlacksmithNavigation : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    public bool walk = true;
    public Transform[] targets;
    int targetIndex;
    NavMeshAgent agent;
    float timer;
    float waitTime;
    float waitTimeMin = 4f;
    float waitTimeMax = 5f;

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
        indicator.SetActive(false); //hide
    }

    public void OnReadyInteract()
    {
        indicator.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        timer = 0;
        targetIndex = -1;
        GoNext();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("start");

        if (walk)
        {
            //animator.SetBool("Walking", true);
            if (agent.remainingDistance < agent.stoppingDistance && !agent.pathPending)
            {
                animator.SetBool("Walking", false);
                walk = false;
                timer = timer + Time.deltaTime;

                if (timer >= waitTime)
                {
                    GoNext();
                }
            }
        }
    }

    public void GoNext()
    {
        timer = 0;
        waitTime = Random.Range(waitTimeMin, waitTimeMax);
        targetIndex = (targetIndex + 1) % targets.Length;

        //animation
        animator.SetBool("Walking", true);
        //walk = true;

        agent.SetDestination(targets[targetIndex].position);
    }
}
