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

        private float _timeCount = 0f;

        private Color _fireImageColor;
        // Start is called before the first frame update
        void Start()
        {
            _fireImageColor = fireImage.color;
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
        }

        private void UpdateUi()
        {
            var burnedRatio = Flammable.BurningRatio();
            burnedTxt.text = $"burned {(int) Math.Ceiling(burnedRatio * 100)}%";
            _fireImageColor.a = burnedRatio;
            fireImage.color = _fireImageColor;
        }
    }
}
