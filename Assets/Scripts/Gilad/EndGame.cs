using System;
using BitStrap;
using GreatArcStudios;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Gilad
{
    public class EndGame : MonoBehaviour
    {
        [SerializeField] private GameObject pauseGameObject;
        
        [Header("Sentences")]
        [SerializeField]
        private TextMeshProUGUI sentenceText;

        [SerializeField]
        [InlineScriptableObject]
        private Sentences.Sentences sentences;
        
        [Space]
        [Header("Events")]
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
