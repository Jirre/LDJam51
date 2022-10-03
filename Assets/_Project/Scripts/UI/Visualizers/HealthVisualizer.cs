using System.Collections;
using System.Collections.Generic;
using JvLib.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.UI
{
    public class HealthVisualizer : UIBehaviour
    {
        [SerializeField] private Image _ImageField;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (!Svc.Ref.Gameplay.IsReady || !Svc.Gameplay.IsServiceReady) return;
            Svc.Gameplay.OnDamageTaken += SetContext;
            SetContext();
        }

        protected override void Start()
        {
            base.Start();
            Svc.Gameplay.OnDamageTaken += SetContext;
            SetContext();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Svc.Gameplay.OnDamageTaken -= SetContext;
        }

        private void SetContext()
        {
            if (_ImageField != null) _ImageField.fillAmount = Svc.Gameplay.GetScaledHealth();
        }
    }
}

