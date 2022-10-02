using DG.Tweening;
using UnityEngine;

namespace Project.Buildings
{
    public class BuildingController : MonoBehaviour
    {
        [SerializeField] private BuildingConfig _Config;

        private const float BASE_HEIGHT = .2f;
        private const float MIDDLE_HEIGHT = 0.7f;
        private const float TOP_HEIGHT = 1.2f;

        private const float DROP_HEIGHT = 2f;
        private const float DROP_DELAY = 0.5f;

        public void SetConfig(BuildingConfig pConfig)
        {
            _Config = pConfig;

            if (pConfig.Base == null) return;
            GameObject baseObject = Instantiate(pConfig.Base, transform, true);
            baseObject.transform.localPosition = Vector3.up * DROP_HEIGHT;
            baseObject.transform.DOLocalMoveY(BASE_HEIGHT, DROP_DELAY);
            
            if (pConfig.Middle == null) return;
            GameObject middleObject = Instantiate(pConfig.Middle, transform, true);
            middleObject.transform.localPosition = Vector3.up * DROP_HEIGHT * 2f;
            middleObject.transform.DOLocalMoveY(MIDDLE_HEIGHT, DROP_DELAY * 2f);
            
            if (pConfig.Top == null) return;
            GameObject topObject = Instantiate(pConfig.Top, transform, true);
            topObject.transform.localPosition = Vector3.up * DROP_HEIGHT * 3f;
            topObject.transform.DOLocalMoveY(TOP_HEIGHT, DROP_DELAY * 3f);
        }
    }
}
