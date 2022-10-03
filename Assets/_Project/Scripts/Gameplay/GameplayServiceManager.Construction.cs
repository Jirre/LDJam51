using System;
using JvLib.Events;
using JvLib.Services;
using Project.Buildings;
using Project.Generation;
using UnityEngine;

namespace Project.Gameplay
{
    public partial class GameplayServiceManager //Construction
    {
        private BuildingConfig _selectedBuildConfig;
        private Plane _plane;

        private Transform _previewTransform;
        private const float PREVIEW_BASE_HEIGHT = 0.2f;

        private Vector2Int _previousCoordinate;

        private SafeEvent<BuildingConfig> _onBuildConfigChanged = new();
        public event Action<BuildingConfig> OnBuildConfigChanged
        {
            add => _onBuildConfigChanged += value;
            remove => _onBuildConfigChanged -= value;
        }
        
        private void InitBuildSettings()
        {
            _plane = new Plane(Vector3.up, PREVIEW_BASE_HEIGHT);
            _previewTransform = new GameObject("buildPreview").transform;
            _previousCoordinate = Vector2Int.zero;
        }
        
        public void SelectBuildConfig(BuildingConfig pConfig)
        {
            for(int i = _previewTransform.childCount - 1; i >= 0; i--)
            {
                Destroy(_previewTransform.GetChild(i).gameObject);
            }
            
            _selectedBuildConfig = pConfig;
            _previousCoordinate = Vector2Int.zero;
            _onBuildConfigChanged.Dispatch(pConfig);

            if (pConfig == null) return;

            float wHeight = BuildingConstants.WEAPON_ADDED_HEIGHT;
            if (pConfig.Base != null)
            {
                CreateSegment(pConfig.Base, BuildingConstants.BASE_HEIGHT);
                wHeight = BuildingConstants.BASE_HEIGHT + BuildingConstants.WEAPON_ADDED_HEIGHT;
            }
            if (pConfig.Middle != null)
            {
                CreateSegment(pConfig.Middle, BuildingConstants.MIDDLE_HEIGHT);
                wHeight = BuildingConstants.MIDDLE_HEIGHT + BuildingConstants.WEAPON_ADDED_HEIGHT;
            }
            if (pConfig.Top != null)
            {
                CreateSegment(pConfig.Top, BuildingConstants.TOP_HEIGHT);
                wHeight = BuildingConstants.TOP_HEIGHT + BuildingConstants.WEAPON_ADDED_HEIGHT;
            }
            if (pConfig.Weapon != null)
                CreateSegment(pConfig.Weapon, wHeight);
        }

        private void CreateSegment(GameObject pObj, float pHeight)
        {
            GameObject obj = Instantiate(pObj, _previewTransform);
            obj.transform.localPosition = Vector3.up * pHeight;
            Material[] mats = obj.GetComponent<MeshRenderer>().materials;
            foreach (Material m in mats)
            {
                m.color = Color.Lerp(m.color, Color.green, 0.75f);
            }
        }

        private void BuildMouseOver()
        {
            if (_selectedBuildConfig == null)
                return;

            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetMouseButtonDown(1))
            {
                SelectBuildConfig(null);
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (_plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector2Int iPoint = new(Mathf.RoundToInt(hitPoint.x), Mathf.RoundToInt(hitPoint.z));

                if (UnityEngine.Input.GetMouseButtonDown(0) && PlaceBuilding(iPoint))
                    return;
                
                if (iPoint == _previousCoordinate || _selectedBuildConfig == null) return;
                _previousCoordinate = iPoint;
                
                _previewTransform.position = new Vector3(iPoint.x, 0f, iPoint.y);

                if (Svc.World.TryGetCell(iPoint, out WorldCell cell))
                {
                    _previewTransform.gameObject.SetActive(_selectedBuildConfig.IsCellAllowed(cell.Content));
                    return;
                }
            }                
            _previewTransform.gameObject.SetActive(false);
        }

        private bool PlaceBuilding(Vector2Int pPosition)
        {
            if (!Svc.World.TryGetCell(pPosition, out WorldCell cell) ||
                !_selectedBuildConfig.IsCellAllowed(cell.Content)) return false;

            if (!TrySpendResource(_selectedBuildConfig.Costs))
            {
                Svc.Audio.Resources();
                SelectBuildConfig(null);
                return false;
            }
            
            GameObject obj = Instantiate(_selectedBuildConfig.Prototype.gameObject);
            obj.transform.position = new Vector3(pPosition.x, 0f, pPosition.y);
            
            BuildingBehaviour behaviour = obj.GetComponent<BuildingBehaviour>();
            behaviour.SetConfig(_selectedBuildConfig);
            behaviour.OnBuild(cell);
            cell.SetBuilding(behaviour);
            
            Svc.Audio.Place();
            
            SelectBuildConfig(null);
            return true;
        }
    }
}
