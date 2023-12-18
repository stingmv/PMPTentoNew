using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Question
{
    public class LerpColorBackground : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Color _initColor;
        [SerializeField] private Color _lastColorCorrect;
        [SerializeField] private Color _lastColorIncorrect;
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private float _duration;
        
        private Color _lastColor;
        private float _currentTime;
        #endregion

        #region Unity Methods

        // Start is called before the first frame update
        // void Start()
        // {
        //     StartAnimation();
        // }

        // Update is called once per frame
        void Update()
        {
            
        }

        #endregion

        #region Methods

        [ContextMenu("start animation")]
        public void StartAnimation(bool isCorrect)
        {
            _lastColor = isCorrect ? _lastColorCorrect : _lastColorIncorrect;
            StartCoroutine(IAnimation());
        }
        public void StartInverseAnimation()
        {
            StartCoroutine(IInverseAnimation());
        }

        IEnumerator IAnimation()
        {
            do
            {
                _camera.backgroundColor = Color.Lerp(_initColor, _lastColor, _currentTime);
                _currentTime += Time.deltaTime / _duration;
                yield return null;
            } while (_currentTime <= 1);

            _currentTime = 0;
        }

        IEnumerator IInverseAnimation()
        {
            _currentTime = 0;
            do
            {
                _camera.backgroundColor = Color.Lerp(_lastColor, _initColor, _currentTime);
                _currentTime += Time.deltaTime / _duration;
                yield return null;
            } while (_currentTime <= 1);
        }
        #endregion

    }

}