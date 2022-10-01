using System;
using System.Collections.Generic;
using System.Linq;
using JvLib.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Generation
{
    public partial class GroundConfig : DataEntry
    {
        [SerializeField] private EWorldCellContent _ContentType;
        public EWorldCellContent ContentType => _ContentType;

        [SerializeField] private EWorldCellContent[] _AdditionalMatches;

        [SerializeField] private GameObject _Default;
        [SerializeField] private bool _RandomRotation;
        [SerializeField, TableList] private List<Variants> _Variants;

        public GameObject GetGround(byte pContext)
        {
            foreach (Variants v in _Variants)
            {
                if ((byte) v.Connections == pContext)
                    return v._Prefab;
            }

            return _Default;
        }

        public float GetRotation(byte pContext)
        {
            if (_RandomRotation) return Mathf.Round(UnityEngine.Random.value * 4f) * 90f;
            
            foreach (Variants v in _Variants)
            {
                if ((byte) v.Connections == pContext)
                    return v._Rotation;
            }
            return 0f;
        }

        public bool DoesConnect(EWorldCellContent pContent)
        {
            return pContent == ContentType || _AdditionalMatches.Contains(pContent);
        }

        [Serializable]
        private struct Variants
        {
            [TableColumnWidth(90, Resizable = false), EnumToggleButtons, HideLabel] public EGroundDirections Connections;
            [VerticalGroup("Content")] public GameObject _Prefab;
            [VerticalGroup("Content")] public float _Rotation;
        }

        [System.Flags]
        private enum EGroundDirections
        {
            Right = 0b0001,
            Up = 0b0010,
            Left = 0b0100,
            Down = 0b1000
        }
    }
}
