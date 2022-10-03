using JvLib.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.UI
{
    [RequireComponent(typeof(Slider))]
    public class MusicVolumeVisualizer : UIBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            GetComponent<Slider>().onValueChanged.AddListener(SetVolume);
        }

        protected override void Start()
        {
            base.Start();
            GetComponent<Slider>().value = Svc.Audio.MusicVolume;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (Svc.Audio.IsServiceReady)
                GetComponent<Slider>().value = Svc.Audio.MusicVolume;
        }

        private static void SetVolume(float pVolume)
        {
            Svc.Audio.SetMusicVolume(pVolume);
        }
    }
}
