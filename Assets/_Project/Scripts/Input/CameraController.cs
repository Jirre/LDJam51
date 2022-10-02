using JvLib.Services;
using UnityEngine;

namespace Project.Input
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _MoveSpeed = 10;
        [SerializeField] private Vector2 _BorderWidth;

        [SerializeField] private float _RotationSpeed = 10;

        [SerializeField] private float _DefaultZoom = 7.5f;
        [SerializeField] private float _MinZoom = 5f;
        [SerializeField] private float _MaxZoom = 10f;

        private Camera _camera;
        [SerializeField] private Transform _PitchAnchor;
        private Transform _cameraTransform;

        private Vector2 _minBound;
        private Vector2 _maxBound;
        [SerializeField] private float _boundBuffer;
        
        private float _zoom;

        private void Awake()
        {
            _camera = GetComponentInChildren<Camera>();
            _cameraTransform = _camera.transform;
            _zoom = _DefaultZoom;
            _cameraTransform.localPosition = Vector3.forward * -_zoom;

            _minBound = Vector2.one * -_boundBuffer;
            _maxBound = Vector2.one * _boundBuffer;
        }

        private void Start()
        {
            Svc.World.OnBuildFinish += ChangeBounds;
        }

        private void OnDestroy()
        {
            Svc.World.OnBuildFinish -= ChangeBounds;
        }

        private void Update()
        {
            Vector2 mousePos = _camera.ScreenToViewportPoint(UnityEngine.Input.mousePosition);

            bool lKey = UnityEngine.Input.GetKey(KeyCode.A);
            bool rKey = UnityEngine.Input.GetKey(KeyCode.D);
            
            bool uKey = UnityEngine.Input.GetKey(KeyCode.W);
            bool dKey = UnityEngine.Input.GetKey(KeyCode.S);

            Vector3 pos = transform.position;
            pos += 
                (transform.right * ((lKey ? -1 : 0) + (rKey ? 1 : 0)) +
                transform.forward * ((dKey ? -1 : 0) + (uKey ? 1 : 0))) * Time.deltaTime * _MoveSpeed;
            pos = new Vector3(
                Mathf.Clamp(pos.x, _minBound.x, _maxBound.x), 0,
                Mathf.Clamp(pos.z, _minBound.y, _maxBound.y));
            transform.position = pos;

            bool rLKey = UnityEngine.Input.GetKey(KeyCode.Q);
            bool rRKey = UnityEngine.Input.GetKey(KeyCode.E);

            transform.eulerAngles += Vector3.up * ((rLKey ? -1 : 0) + (rRKey ? 1 : 0)) * 
                                          Time.deltaTime * 
                                          -_RotationSpeed;

            _zoom -= UnityEngine.Input.mouseScrollDelta.y;
            _zoom = Mathf.Clamp(_zoom, _MinZoom, _MaxZoom);
            _cameraTransform.localPosition = Vector3.forward * -_zoom;
            _PitchAnchor.localEulerAngles = Vector3.right * (35f + _zoom * 2f);
        }

        private void ChangeBounds()
        {
            _minBound = Svc.World.MinCoordinate - Vector2.one * _boundBuffer;
            _maxBound = Svc.World.MaxCoordinate + Vector2.one * _boundBuffer;
        }
    }
}
