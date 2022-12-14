using DG.Tweening;
using Project.Buildings;
using UnityEngine;

namespace Project.Generation
{
    public class WorldCell
    {
        private GroundConfig _config;
        private byte _context;
        
        private GameObject _ground;
        private BuildingBehaviour _building;
        
        private Vector2Int _position;
        public Vector2Int Position => _position;
        private readonly Transform _parent;

        private const float BASE_SPAWN_DISTANCE = 1f;
        private const float BASE_SPAWN_SPEED = 0.5f;
        private const float ADDED_SPAWN_DISTANCE = .5f;
        private const float ADDED_SPAWN_SPEED = 0.25f;

        public int Cost { get; private set; }
        public EWorldCellContent Content { get; private set; }

        public WorldCell(Vector2Int pPosition, Transform pParent)
        {
            _position = pPosition;
            _parent = pParent;

            Cost = int.MaxValue;
            Content = EWorldCellContent.Unassigned;
        }

        public void SetContent(EWorldCellContent pContent)
        {
            Content = pContent;
        }

        public void SetGround(GroundConfig pConfig, byte pContext)
        {
            if (_config == pConfig && _context == pContext) return;
            _config = pConfig;
            _context = pContext;
            float dist = Mathf.Ceil(Vector2Int.Distance(_position, Vector2Int.zero));
            
            Dispose(dist);
            
            _ground = Object.Instantiate(
                pConfig == null ? GameObject.CreatePrimitive(PrimitiveType.Quad) : pConfig.GetGround(pContext), 
                _parent, 
                true);

            _ground.transform.position = new Vector3(_position.x, -BASE_SPAWN_DISTANCE - ADDED_SPAWN_DISTANCE * dist, _position.y);
            _ground.transform.DOMoveY(0, BASE_SPAWN_SPEED + ADDED_SPAWN_SPEED * dist);
            _ground.transform.eulerAngles = Vector3.up * (pConfig == null ? 0 : pConfig.GetRotation(pContext));
        }

        public void SetBuilding(BuildingBehaviour pBehaviour)
        {
            _building = pBehaviour;
        }

        public void Dispose(float pDist)
        {
            if (_ground == null) return;
            
            _ground.transform.DOMoveY(
                -BASE_SPAWN_DISTANCE - ADDED_SPAWN_DISTANCE * pDist, 
                BASE_SPAWN_SPEED + ADDED_SPAWN_SPEED * pDist);
            Object.Destroy(_ground, BASE_SPAWN_SPEED + ADDED_SPAWN_SPEED * pDist + 1f);

            if (_building == null)
                return;
            _building.Dispose();
            Object.Destroy(_building.gameObject, 3f);
        }

        public void SetCost(int pCost)
        {
            Cost = pCost;
        }
    }
}
