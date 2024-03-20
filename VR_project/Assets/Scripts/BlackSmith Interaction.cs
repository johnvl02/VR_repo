using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BlackSmithInteraction : MonoBehaviour
{
    [SerializeField] private Animator animator;

    //private bool isOut = false;


    public BlacksmithNavigation AgentPatrol;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")//&& !isOut)
        {
            //animator.SetTrigger("GoOut");
            AgentPatrol.walk = true;
            AgentPatrol.GoNext();
            //isOut = true;
            animator.SetBool("Idle", true);
            //transform.rotation *= Quaternion.Euler(0, 90, 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")// && isOut)
        {
            //animator.SetTrigger("GoOut");
            AgentPatrol.walk = true;
            AgentPatrol.GoNext();
            //isOut = false;
            animator.SetBool("Idle", false);
            //transform.rotation *= Quaternion.Euler(0, 270, 0);
            //transform.position += new Vector3(-0.15f, -0.1f, 0);

        }
    }


}
