using JvLib.Services;
using JvLib.UI;
using UnityEngine;

namespace Project.Audio
{
    [RequireComponent(typeof(UIButton))]
    public class ButtonClickAudio : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<UIButton>().OnClick.AddListener(Svc.Audio.Click);
        }
    }
}
