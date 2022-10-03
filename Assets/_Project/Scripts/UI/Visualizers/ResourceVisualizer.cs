using JvLib.Services;
using Project.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.UI
{
    public class ResourceVisualizer : UIBehaviour
    {
        [SerializeField] private EResources _Resource;
        [SerializeField] private TMP_Text _Visualizer;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (Svc.Ref.Gameplay.IsReady && Svc.Gameplay.IsServiceReady)
            {
                Svc.Gameplay.OnResourceUpdate += SetContext;
                SetContext();
            }
        }

        protected override void Start()
        {
            base.Start();
            Svc.Gameplay.OnResourceUpdate += SetContext;
            SetContext();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Svc.Gameplay.OnResourceUpdate -= SetContext;
        }

        private void SetContext()
        {
            if (_Visualizer != null) _Visualizer.SetText(Svc.Gameplay.GetResource(_Resource).ToString());
        }
    }
}
