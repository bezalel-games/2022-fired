using System;
using GreatArcStudios;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gilad
{
    public class EndGame : MonoBehaviour
    {
        [SerializeField] private GameObject pauseGameObject;
        // Start is called before the first frame update

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        private void OnEnable()
        {
            if(pauseGameObject != null)
            {
                if (TryGetComponent(out PauseManager pauseManager))
                {
                    pauseManager.onMenuEnter.Invoke(); // TODO: this is temporary solution, just make a "PauseAll" and "ResumeAll" Function.
                }
                pauseGameObject.SetActive(false);
            }
            Time.timeScale = 0;
            PauseManager.Paused = true;
        }
    }
}
