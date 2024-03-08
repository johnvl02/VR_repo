using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Slider Slider;    
    [SerializeField] private TextMeshProUGUI startText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartApp()
    {        
        Debug.Log("Start");
        StartCoroutine(LoadYourAsyncScene());

    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("SampleScene");
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            Slider.value = async.progress;
            Slider.maxValue = 0.9f;
            yield return null;
            if (Slider.value == 0.9f)
            {                
                startText.gameObject.SetActive(true);                
            }
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                async.allowSceneActivation = true;
            }
        }
    }

    public void QuitApp()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
