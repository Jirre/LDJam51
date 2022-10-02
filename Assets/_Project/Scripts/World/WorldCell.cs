using DG.Tweening;
using UnityEngine;

namespace Project.Generation
{
    public class WorldCell
    {
        private GameObject _ground;
        private GameObject _building;
        
        private Vector2Int _position;
        public Vector2Int Position => _position;
        private readonly Transform _parent;

        private const float BASE_SPAWN_DISTANCE = 1f;
        private const float BASE_SPAWN_SPEED = 0.5f;
        private const float ADDED_SPAWN_DISTANCE = .5f;
        private const float ADDED_SPAWN_SPEED = 0.25f;

        public int Cost { get; }
        public EWorldCellContent Content { get; private set; }

        public WorldCell(Vector2Int pPosition, Transform pParent, int pCost)
        {
            _position = pPosition;
            _parent = pParent;

            Cost = pCost;
            Content = EWorldCellContent.Empty;
        }

        public void SetContent(EWorldCellContent pContent)
        {
            Content = pContent;
        }

        public void SetGround(GroundConfig pConfig, byte pContext)
        {
            if (_ground != null) return;
            _ground = Object.Instantiate(
                pConfig == null ? GameObject.CreatePrimitive(PrimitiveType.Quad) : pConfig.GetGround(pContext), 
                _parent, 
                true);

            float dist = Mathf.Ceil(Vector2Int.Distance(_position, Vector2Int.zero));
            
            _ground.transform.position = new Vector3(_position.x, -BASE_SPAWN_DISTANCE - ADDED_SPAWN_DISTANCE * dist, _position.y);
            _ground.transform.DOMoveY(0, BASE_SPAWN_SPEED + ADDED_SPAWN_SPEED * dist);
            _ground.transform.eulerAngles = Vector3.up * (pConfig == null ? 0 : pConfig.GetRotation(pContext));
        }
    }
}
