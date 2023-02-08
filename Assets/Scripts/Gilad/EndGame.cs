using System;
using Avrahamy.EditorGadgets;
using BitStrap;
using GreatArcStudios;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gilad
{
    public class EndGame : MonoBehaviour
    {
        [SerializeField]
        private GameObject pauseGameObject;

        [Header("Sentences")]
        [SerializeField]
        private bool useSentencesAsImages;

        [SerializeField]
        [ConditionalHide("useSentencesAsImages", true, true)]
        private TextMeshProUGUI sentenceText;

        [SerializeField]
        [ConditionalHide("useSentencesAsImages")]
        private Image sentenceImage;

        [InlineScriptableObject]
        [SerializeField]
        private Sentences.Sentences sentences;

        [Space]
        [Header("Events")]
        [SerializeField]
        private UnityEvent onEnter;

        [SerializeField]
        private UnityEvent onLeave;

        public void Restart()
        {
            PauseManager.Paused = false; // TODO: move this to the scene activations onReady state
            onLeave.Invoke();
        }


        private void OnEnable()
        {
            onEnter.Invoke();
            PauseManager.Paused = true;
            if (pauseGameObject != null)
            {
                pauseGameObject.SetActive(false);
            }

            SetSentence();
        }

        [Button]
        public void SetSentence()
        {
            var sentence = sentences.GetRandomSentence();
            sentenceText.text = sentence.sentence;
            if (useSentencesAsImages && sentence.sentenceAsImage != null)
            {
                sentenceImage.sprite = sentence.sentenceAsImage;
            }
        }
    }
}
