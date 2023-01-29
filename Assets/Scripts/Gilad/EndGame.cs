using System;
using GreatArcStudios;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Gilad
{
    public class EndGame : MonoBehaviour
    {
        [SerializeField] private GameObject pauseGameObject;

        [SerializeField] private UnityEvent onEnter;

        [SerializeField] private UnityEvent onLeave;

        public void Restart()
        {
            PauseManager.Paused = false; // TODO: move this to the scene activations onReady state
            onLeave.Invoke();
        }


        private void OnEnable()
        {
            onEnter.Invoke();
            PauseManager.Paused = true;
            if(pauseGameObject != null)
            {
                pauseGameObject.SetActive(false);
            }
        }
    }
}
