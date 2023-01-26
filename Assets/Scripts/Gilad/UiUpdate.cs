using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gilad
{
    public class UiUpdate : MonoBehaviour
    {

        [SerializeField] private float updateDur = 0.5f;

        [SerializeField] private TextMeshProUGUI burnedTxt;

        [SerializeField] private Image fireImage;

        [SerializeField] private Slider fireSlider;

        [SerializeField] private float gameTime;

        [SerializeField] private GameObject endGameObject;

        [SerializeField] private TextMeshProUGUI timeText;


        private float overAllTime;

        private float _timeCount = 0f;

        private Color _fireImageColor;
        // Start is called before the first frame update
        void Start()
        {
            overAllTime = gameTime;
            _fireImageColor = fireImage.color;
            UpdateUi();
        }

        // Update is called once per frame
        void Update()
        {
            _timeCount += Time.deltaTime;
            if (_timeCount >= updateDur)
            {
                _timeCount = 0f;
                UpdateUi();
            }

            overAllTime -= Time.deltaTime;
            if (overAllTime <= 0f)
            {
                fireImage.gameObject.SetActive(false);
                burnedTxt.gameObject.SetActive(false);
                fireSlider.gameObject.SetActive(false);
                timeText.gameObject.SetActive(false);
                endGameObject.SetActive(true);
            }
        }

        private void UpdateUi()
        {
            var burnedRatio = Flammable.BurningRatio();
            burnedTxt.text = $"burned {(int) Math.Ceiling(burnedRatio * 100)}%";
            _fireImageColor.a = burnedRatio;
            fireSlider.value = burnedRatio;
            fireImage.color = _fireImageColor;
            timeText.text = GetTime();
        }

        private string GetTime()
        {
            int allSec = (int) Math.Ceiling(overAllTime);
            int sec = allSec % 60;
            int min = allSec / 60;
            return $"{min:00}:{sec:00}";
        }
    }
}
