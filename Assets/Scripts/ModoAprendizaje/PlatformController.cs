using System;
using System.Collections;
using System.Collections.Generic;
using CameraUI;
using Unity.Mathematics;
using UnityEngine;

namespace ModoAprendizaje
{
    public class PlatformController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private GameObject _prefabs;
        [SerializeField] private float _distanceFix;
        [SerializeField] private float _multiplierDistance;
        [SerializeField] private int _multiplier;
        [SerializeField] private int _numIntances;
        [SerializeField] private BezierCurve _bezierCurve;

        private float _currentPercentage;
        
        #endregion

        #region Unity Methods

        // Start is called before the first frame update
        void Start()
        {
            CreatePlatforms();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        #endregion

        #region Methods

        [ContextMenu("Create platforms")]
        public void CreatePlatforms()
        {
            for (int i = 0; i < _numIntances; i++)
            {
                Instantiate(_prefabs, transform.TransformPoint(_bezierCurve.GetPositionInPercentage(_distanceFix / _multiplierDistance * i  ) )* _multiplier, quaternion.identity, transform);
            }
        }

        
        #endregion

        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < _numIntances; i++)
            {
                Gizmos.DrawSphere(transform.TransformPoint(_bezierCurve.GetPositionInPercentage(_distanceFix / _multiplierDistance  * i )), 100);
            }
        }
    }

}