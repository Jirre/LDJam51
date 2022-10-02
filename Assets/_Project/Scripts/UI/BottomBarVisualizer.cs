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
            if (Svc.Ref.GameplayServiceManager.IsReady && Svc.GameplayServiceManager.IsServiceReady)
                Svc.GameplayServiceManager.OnBuildConfigChanged += SetContext;
        }

        protected override void Start()
        {
            base.Start();
            Svc.GameplayServiceManager.OnBuildConfigChanged += SetContext;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Svc.GameplayServiceManager.OnBuildConfigChanged -= SetContext;
        }

        protected override void OnContextUpdate(BuildingConfig pContext)
        {
            if (_ImageVisualizer != null)
                _ImageVisualizer.sprite = _TargetConfig == pContext ? _OnSelected : _OnDefault;
            GetComponent<UIButton>().interactable = _TargetConfig != pContext;
        }

        public void OnClick()
        {
            Svc.GameplayServiceManager.SelectBuildConfig(_TargetConfig);
        }
    }
}
