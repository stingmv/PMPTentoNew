using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
namespace UI
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] private RectTransform _container;
        [SerializeField] private UnityEvent _onOutsidePopup;
        [SerializeField] private bool _detectTouchOutside = true;

        private void OnEnable()
        {
            if (_detectTouchOutside)
            {
                EnhancedTouchSupport.Enable();

                Touch.onFingerDown += TouchOnonFingerDown;
            }
        
        }

        private void TouchOnonFingerDown(Finger obj)
        {
            var pos = obj.currentTouch.screenPosition;
            Debug.Log("touch pressed " + pos + " - " + _container.gameObject.activeSelf + " - " + RectTransformUtility.RectangleContainsScreenPoint(_container, pos));

            if (_container.gameObject.activeSelf &&
                !RectTransformUtility.RectangleContainsScreenPoint(_container, pos))
            {
                _onOutsidePopup?.Invoke();
            }
        }

        private void OnDisable()
        {
            if (_detectTouchOutside)
            {
                Touch.onFingerDown -= TouchOnonFingerDown;
                EnhancedTouchSupport.Disable();
            }
        }
    }
}
