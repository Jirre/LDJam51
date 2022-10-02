using DG.Tweening;
using UnityEngine;

namespace Project.Buildings
{
    public class BuildingBehaviour : MonoBehaviour
    {
        [SerializeField] private BuildingConfig _Config;
        protected Transform WeaponTransform;

        private const float DROP_HEIGHT = 2f;
        private const float DROP_DELAY = 0.5f;

        public void SetConfig(BuildingConfig pConfig)
        {
            _Config = pConfig;

            if (pConfig.Base == null) return;
            GameObject baseObject = Instantiate(pConfig.Base, transform, true);
            baseObject.transform.localPosition = Vector3.up * DROP_HEIGHT;
            baseObject.transform.DOLocalMoveY(BuildingConstants.BASE_HEIGHT, DROP_DELAY);
            if (pConfig.Middle == null)
            {
                if (pConfig.Weapon == null) return;
                SetWeapon(pConfig.Weapon, BuildingConstants.BASE_HEIGHT + BuildingConstants.WEAPON_ADDED_HEIGHT);
                return;
            }
            
            GameObject middleObject = Instantiate(pConfig.Middle, transform, true);
            middleObject.transform.localPosition = Vector3.up * DROP_HEIGHT * 2f;
            middleObject.transform.DOLocalMoveY(BuildingConstants.MIDDLE_HEIGHT, DROP_DELAY * 2f);
            if (pConfig.Top == null)
            {
                if (pConfig.Weapon == null) return;
                SetWeapon(pConfig.Weapon, BuildingConstants.MIDDLE_HEIGHT + BuildingConstants.WEAPON_ADDED_HEIGHT);
                return;
            }
            
            GameObject topObject = Instantiate(pConfig.Top, transform, true);
            topObject.transform.localPosition = Vector3.up * DROP_HEIGHT * 3f;
            topObject.transform.DOLocalMoveY(BuildingConstants.TOP_HEIGHT, DROP_DELAY * 3f);
            if (pConfig.Weapon == null) return;
            SetWeapon(pConfig.Weapon, BuildingConstants.TOP_HEIGHT + BuildingConstants.WEAPON_ADDED_HEIGHT);
        }

        private void SetWeapon(GameObject pObj, float pHeight)
        {
            GameObject weaponObject = Instantiate(pObj, transform, true);
            weaponObject.transform.localPosition = Vector3.up * (DROP_HEIGHT + pHeight) * 2f ;
            weaponObject.transform.DOLocalMoveY(pHeight, DROP_DELAY * 1.5f);
            WeaponTransform = weaponObject.transform;
        }
    }
}
