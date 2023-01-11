using System;
using System.Collections;
using BitStrap;
using Eflatun.SceneReference;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Logger = Nemesh.Logger;

namespace Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private SceneReference sceneToLoad;  // TODO: scene selector drawer?

        [SerializeField]
        private LoadSceneMode loadMode = LoadSceneMode.Single;
        
        [ReadOnly]
        [SerializeField]
        [InspectorName("Received load next massage")]
        private bool loadNext;

        [ReadOnly]
        [SerializeField]
        private float currentLoadingProgress;
        
        [ReadOnly]
        [SerializeField]
        [InspectorName("Scene ready to load")]
        private bool sentReadyMassage;

        [SerializeField]
        public UnityEvent<float> onProgressBarUpdate;
        
        [SerializeField]
        public UnityEvent onReadyState;
        
        [SerializeField]
        public UnityEvent onStartSwitch;
        
        [SerializeField]
        [InspectorName("On Load Complete - WARNING! USE WITH CAUTION")]
        public UnityEvent onLoadComplete;

        private void Start()
        {
            if (sceneToLoad.IsSafeToUse)
            {
                StartCoroutine(LoadScene(sceneToLoad));
            }
            else
            {
                Logger.LogWarning("Not a valid scene!");
            }
        }

        public void GoToNext()
        {
            loadNext = true;
        }

        private IEnumerator LoadScene(SceneReference scene)
        {
            yield return null;

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene.BuildIndex, LoadSceneMode.Single);
            asyncOperation.allowSceneActivation = false;
            Logger.Log($"Start async load scene {scene.Name}", this);
            
            while (!asyncOperation.isDone)
            {
                onProgressBarUpdate.Invoke(asyncOperation.progress);
                currentLoadingProgress = asyncOperation.progress;
                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    if (!sentReadyMassage)
                    {
                        Logger.Log($"Ready to switch to scene {scene.Name}", this);
                        onReadyState.Invoke();
                        sentReadyMassage = true;
                    }
                    if (loadNext)
                    {
                        Logger.Log($"Allowing scene {scene.Name} activation");
                        onStartSwitch.Invoke();
                        asyncOperation.allowSceneActivation = true;
                    }
                }

                yield return null;
            }

            if (loadMode == LoadSceneMode.Additive)
            {
                Logger.Log("This is where you would unload current, for example.", this);
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            }
            Logger.Log("Finished Load");
            onLoadComplete.Invoke();
        }
        
    }
}
