using System;
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
            if(pauseGameObject != null) pauseGameObject.SetActive(false);
            Time.timeScale = 0f;
        }
    }
}
