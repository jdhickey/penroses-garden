using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class ButtonParent : MonoBehaviour
{

    public string loadScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual IEnumerator LoadYourAsyncScene()
    {
        if (loadScene == "Quit") {
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        else if (!(string.IsNullOrEmpty(loadScene))) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loadScene);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        } 
        
    }

    public abstract void AttemptLoad();
}
