using System;
using BitStrap;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Logger = Nemesh.Logger;

namespace Gilad
{
    public class UiUpdate : MonoBehaviour
    {

        [SerializeField]
        private float updateDur = 0.5f;

        [SerializeField]
        private TextMeshProUGUI burnedTxt;

        [SerializeField]
        private string percentageFormat = "burned {0}%";

        [SerializeField]
        private Image fireImage;

        [SerializeField]
        private Slider fireSlider;

        [SerializeField]
        private float gameTime;

        [SerializeField]
        private GameObject endGameObject;

        [SerializeField]
        private TextMeshProUGUI timeText;

        [SerializeField]
        private BoolAnimationParameter onGameEndAnimationParameter;

        [SerializeField]
        public UnityEvent<float> onPercentageUpdate;

        private float _overAllTime;

        private float _timeCount = 0f;

        private Color _fireImageColor;
        private Animator _animator;
        private bool _hasAnimator;

        // Start is called before the first frame update
        void Start()
        {
            _hasAnimator = TryGetComponent(out _animator);
            _overAllTime = gameTime;
            _fireImageColor = fireImage.color;
            UpdateUi();
            if (_hasAnimator)
            {
                onGameEndAnimationParameter.Set(_animator, false);
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            _timeCount += Time.deltaTime;
            if (_timeCount >= updateDur)
            {
                _timeCount = 0f;
                UpdateUi();
            }

            _overAllTime -= Time.deltaTime;
            if (_overAllTime <= 0f)
            {
                // fireImage.gameObject.SetActive(false);
                // burnedTxt.gameObject.SetActive(false);
                // fireSlider.gameObject.SetActive(false);
                timeText.gameObject.SetActive(false);
                endGameObject.SetActive(true);
                if (_hasAnimator)
                {
                    onGameEndAnimationParameter.Set(_animator, true);
                }
            }
        }

        private void UpdateUi()
        {
            var burnedRatio = Flammable.BurningRatio();
            burnedTxt.text = string.Format(percentageFormat, (int) Math.Ceiling(burnedRatio * 100));
            onPercentageUpdate.Invoke(burnedRatio);
            _fireImageColor.a = burnedRatio;
            fireSlider.value = burnedRatio;
            fireImage.color = _fireImageColor;
            timeText.text = GetTime();
        }

        private string GetTime()
        {
            int allSec = (int) Math.Ceiling(_overAllTime);
            int sec = allSec % 60;
            int min = allSec / 60;
            return $"{min:00}:{sec:00}";
        }
    }
}
