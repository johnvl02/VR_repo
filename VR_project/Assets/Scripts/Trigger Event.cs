using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public List<string> tags = new List<string>() { "Player" };
    public UnityEvent events;
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
        if (tags.Contains(other.tag))
        {
            events.Invoke();
        }
    }
}
