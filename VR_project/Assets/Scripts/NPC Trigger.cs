using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class NPCTriger : MonoBehaviour
{
    [SerializeField] private Animator animator;
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
        if (other.tag == "Player")
        {
            animator.SetTrigger("Aim");
            transform.rotation *= Quaternion.Euler(0, 90, 0);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetTrigger("Aim");
            transform.rotation *= Quaternion.Euler(0, 270, 0);
            transform.position += new Vector3(-0.15f, -0.1f, 0);

        }
    }
}
