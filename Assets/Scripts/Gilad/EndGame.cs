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
            onLeave.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        private void OnEnable()
        {
            onEnter.Invoke();
            PauseManager.Paused = true;
            if(pauseGameObject != null)
            {
                pauseGameObject.SetActive(false);
            }
            Time.timeScale = 0;

        }
    }
}
