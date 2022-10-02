using JvLib.Services;
using JvLib.UI;
using JvLib.UI.Visualizers;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Buildings.UI
{
    [RequireComponent(typeof(UIButton))]
    public class BottomBarVisualizer : UIVisualizer<BuildingConfig>
    {
        [SerializeField] private BuildingConfig _TargetConfig;

        [SerializeField] private Image _ImageVisualizer;
        
        [SerializeField] private Sprite _OnDefault;
        [SerializeField] private Sprite _OnSelected;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (Svc.Ref.Gameplay.IsReady && Svc.Gameplay.IsServiceReady)
                Svc.Gameplay.OnBuildConfigChanged += SetContext;
            SetContext(null);
        }

        protected override void Start()
        {
            base.Start();
            Svc.Gameplay.OnBuildConfigChanged += SetContext;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Svc.Gameplay.OnBuildConfigChanged -= SetContext;
        }

        protected override void OnContextUpdate(BuildingConfig pContext)
        {
            if (_ImageVisualizer != null)
                _ImageVisualizer.sprite = _TargetConfig == pContext ? _OnSelected : _OnDefault;
            GetComponent<UIButton>().interactable = _TargetConfig != pContext;
        }

        public void OnClick()
        {
            Svc.Gameplay.SelectBuildConfig(_TargetConfig);
        }
    }
}
