using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicIntro : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;
    float timer;
    float waitTime =3.0f;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer = timer + Time.deltaTime;
        if (timer > waitTime)
        {
            camera.Priority = 5;
        }
        else
        {
            timer = timer + Time.deltaTime;
        }
    }
}
