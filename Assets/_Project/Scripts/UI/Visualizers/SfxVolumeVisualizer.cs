using JvLib.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Visualizers
{
    [RequireComponent(typeof(Slider))]
    public class SfxVolumeVisualizer : UIBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            GetComponent<Slider>().onValueChanged.AddListener(SetVolume);
        }

        protected override void Start()
        {
            base.Start();
            GetComponent<Slider>().value = Svc.Audio.SfxVolume;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (Svc.Audio.IsServiceReady)
                GetComponent<Slider>().value = Svc.Audio.SfxVolume;
        }

        private static void SetVolume(float pVolume)
        {
            Svc.Audio.SetSfxVolume(pVolume);
        }
    }
}
