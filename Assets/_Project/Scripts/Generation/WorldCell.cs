using UnityEngine;

namespace Project.Generation
{
    public class WorldCell
    {
        private GameObject _ground;
        private GameObject _building;
        
        private Vector2Int _position;
        private Transform _parent;

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
            _ground = Object.Instantiate(
                pConfig == null ? GameObject.CreatePrimitive(PrimitiveType.Quad) : pConfig.GetGround(pContext), 
                _parent, 
                true);
            _ground.transform.position = new Vector3(_position.x, 0f, _position.y);
            _ground.transform.eulerAngles = Vector3.forward * (pConfig == null ? 0 : pConfig.GetRotation(pContext));
        }
    }
}
