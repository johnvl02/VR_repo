using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private KeyCode pasuekey = KeyCode.Escape;
    [SerializeField] private GameObject pauseScreen;
    
    [Space]
    [SerializeField] private ThirdPersonController thirdPersonController;

    private bool isPause;
    private bool cursorVisibility;
    private CursorLockMode cursorState;

    // Start is called before the first frame update
    void Start()
    {
        isPause = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.P))
        {
            
            if (!isPause)
            {
                //pause the game
                Pause();
            }
            else
            {
                //resume the game
                Resume();
            }
        }
    }
    public void Pause()
    {
        cursorVisibility = Cursor.visible;
        cursorState = Cursor.lockState;

        pauseScreen.SetActive(true);
        isPause = true;
        Time.timeScale = 0f;
        thirdPersonController.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void Resume()
    {
        pauseScreen.SetActive(false);
        isPause = false;
        Time.timeScale = 1.0f;
        thirdPersonController.enabled = true;
        Cursor.visible = cursorVisibility;
        Cursor.lockState = cursorState;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
