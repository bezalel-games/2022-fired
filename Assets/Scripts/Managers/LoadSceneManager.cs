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
    public class LoadSceneManager : MonoBehaviour
    {

        #region Inspector

        [HelpBox(@"The following scene will be loaded.
Notice you must enter the scene! Once loading started, changing this value is undefined.",
            HelpBoxAttribute.MessageType.Info)]
        [Header("Scene parameters")]
        [SerializeField]
        private SceneReference sceneToLoad;

        [SerializeField]
        private LoadSceneMode loadMode = LoadSceneMode.Single;

        [SerializeField]
        [Tooltip("If additive load - should we unload this scene after?")]
        private bool unloadIfAdditive = true;

        [SerializeField]
        private bool loadOnStart = true;

        [Header("Progress Logs")]
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

        [Header("Events for loading sequence")]
        [Space]
        [SerializeField]
        public UnityEvent<float> onProgressBarUpdate;

        [SerializeField]
        public UnityEvent onReadyState;

        [HelpBox("Notice - this may be just as the scene unloads, and some functions might not complete!",
            HelpBoxAttribute.MessageType.Warning)]
        [SerializeField]
        public UnityEvent onStartSwitch;

        [HelpBox("if unloaded - this will never be reached!", HelpBoxAttribute.MessageType.Warning)]
        [SerializeField]
        public UnityEvent onLoadComplete;

        #endregion

        #region MonoBehaviour

        private void Start()
        {
            if (loadOnStart)
            {
                StartLoadScene();
            }
        }

        #endregion

        #region ScenLoader

        public void StartLoadScene()
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

            if (loadMode == LoadSceneMode.Additive && unloadIfAdditive)
            {
                Logger.Log("This is where you would unload current, for example.", this);
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            }

            Logger.Log("Finished Load");
            onLoadComplete.Invoke();
        }

        #endregion

    }
}
