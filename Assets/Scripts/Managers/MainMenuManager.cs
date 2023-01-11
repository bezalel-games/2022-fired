using System;
using System.Collections;
using BitStrap;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = Nemesh.Logger;

namespace Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private int sceneToLoad = 1;
        
        [ReadOnly]
        [SerializeField]
        private bool loadNext; 

        private void Start()
        {
            StartCoroutine(LoadScene(sceneToLoad));
        }

        public void GoToNext()
        {
            loadNext = true;
        }

        private IEnumerator LoadScene(int scene)
        {
            yield return null;

            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;
            Logger.Log($"Start async load scene {scene}", this);

            while (!asyncOperation.isDone)
            {

                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    // DebugLog.Log(LogTag.Default, "Ready to load", this);
                    //Wait to you press the space key to activate the Scene
                    if (loadNext)
                    {
                        //Activate the Scene
                        asyncOperation.allowSceneActivation = true;
                    }
                }

                yield return null;
            }
        }
        
    }
}
