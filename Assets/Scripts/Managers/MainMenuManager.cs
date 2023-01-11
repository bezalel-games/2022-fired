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
        [InspectorName("Received load next massage")]
        private bool loadNext;

        [ReadOnly]
        [SerializeField]
        [InspectorName("Scene ready to load")]
        private bool sentReadyMassage;

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

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
            asyncOperation.allowSceneActivation = false;
            Logger.Log($"Start async load scene {scene}", this);

            while (!asyncOperation.isDone)
            {

                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    if (!sentReadyMassage)
                    {
                        Logger.Log($"Ready to switch to scene {scene}", this);
                        sentReadyMassage = true;
                    }
                    if (loadNext)
                    {
                        Logger.Log($"Allowing scene {scene} activation");
                        asyncOperation.allowSceneActivation = true;
                    }
                }

                yield return null;
            }
        }
        
    }
}
