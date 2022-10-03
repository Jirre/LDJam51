using System.Collections.Generic;
using DG.Tweening;
using Project.Generation;
using UnityEngine;

namespace Project.Buildings
{
    public class BuildingBehaviour : MonoBehaviour
    {
        [SerializeField] protected BuildingConfig _Config;
        protected Transform TurretTransform;
        protected Vector3 TurretPosition;

        private const float DROP_HEIGHT = 2f;
        private const float DROP_DELAY = 0.5f;

        private List<Transform> _objects;

        public virtual void SetConfig(BuildingConfig pConfig)
        {
            Dispose();
            _Config = pConfig;
            _objects ??= new List<Transform>();
            
            _objects.Clear();
            TurretPosition = transform.position + Vector3.up * BuildingConstants.WEAPON_ADDED_HEIGHT;

            if (pConfig.Base == null) return;
            GameObject baseObject = Instantiate(pConfig.Base, transform, true);
            baseObject.transform.localPosition = Vector3.up * DROP_HEIGHT;
            baseObject.transform.DOLocalMoveY(BuildingConstants.BASE_HEIGHT, DROP_DELAY);
            TurretPosition = transform.position + Vector3.up * (BuildingConstants.BASE_HEIGHT + BuildingConstants.WEAPON_ADDED_HEIGHT);
            _objects.Add(baseObject.transform);
            if (pConfig.Middle == null)
            {
                if (pConfig.Weapon == null) return;
                SetWeapon(pConfig.Weapon, BuildingConstants.BASE_HEIGHT + BuildingConstants.WEAPON_ADDED_HEIGHT);
                return;
            }
            
            GameObject middleObject = Instantiate(pConfig.Middle, transform, true);
            middleObject.transform.localPosition = Vector3.up * DROP_HEIGHT * 2f;
            middleObject.transform.DOLocalMoveY(BuildingConstants.MIDDLE_HEIGHT, DROP_DELAY * 2f);
            TurretPosition = transform.position + Vector3.up * (BuildingConstants.MIDDLE_HEIGHT + BuildingConstants.WEAPON_ADDED_HEIGHT);
            _objects.Add(middleObject.transform);
            if (pConfig.Top == null)
            {
                if (pConfig.Weapon == null) return;
                SetWeapon(pConfig.Weapon, BuildingConstants.MIDDLE_HEIGHT + BuildingConstants.WEAPON_ADDED_HEIGHT);
                return;
            }
            
            GameObject topObject = Instantiate(pConfig.Top, transform, true);
            topObject.transform.localPosition = Vector3.up * DROP_HEIGHT * 3f;
            topObject.transform.DOLocalMoveY(BuildingConstants.TOP_HEIGHT, DROP_DELAY * 3f);
            TurretPosition = transform.position + Vector3.up * (BuildingConstants.TOP_HEIGHT + BuildingConstants.WEAPON_ADDED_HEIGHT);
            _objects.Add(topObject.transform);
            if (pConfig.Weapon == null) return;
            SetWeapon(pConfig.Weapon, BuildingConstants.TOP_HEIGHT + BuildingConstants.WEAPON_ADDED_HEIGHT);
        }

        public void Dispose()
        {
            _objects ??= new List<Transform>();
            
            foreach (Transform o in _objects)
            {
                o.DOMoveY(-DROP_HEIGHT, DROP_DELAY * DROP_HEIGHT * o.transform.position.y);
            }

            OnDemolish();
        }


        public virtual void OnBuild(WorldCell pCell)
        {
            pCell.SetContent(EWorldCellContent.Building);
        }
        public virtual void OnDemolish() { }

        private void SetWeapon(GameObject pObj, float pHeight)
        {
            GameObject weaponObject = Instantiate(pObj, transform, true);
            weaponObject.transform.localPosition = Vector3.up * (DROP_HEIGHT + pHeight) * 2f ;
            weaponObject.transform.DOLocalMoveY(pHeight, DROP_DELAY * 3f);
            TurretTransform = weaponObject.transform;
            _objects.Add(weaponObject.transform);
        }
    }
}
