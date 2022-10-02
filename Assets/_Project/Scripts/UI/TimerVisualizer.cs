using JvLib.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class TimerVisualizer : MonoBehaviour
    {
        [SerializeField] private Image _ImageField;

        private void Update()
        {
            if (!Svc.Gameplay.IsServiceReady)
                return;

            if (_ImageField != null) _ImageField.fillAmount = Svc.Gameplay.GetScaledTime();
        }
    }
}
