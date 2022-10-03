using JvLib.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class TimerVisualizer : MonoBehaviour
    {
        [SerializeField] private Image _ImageField;
        [SerializeField] private TMP_Text _WaveField;

        private void Update()
        {
            if (!Svc.Gameplay.IsServiceReady)
                return;

            if (_ImageField != null) _ImageField.fillAmount = Svc.Gameplay.GetScaledTime();
            if (_WaveField != null) _WaveField.SetText($"Wave: {Svc.Gameplay.Wave}");
        }
    }
}
