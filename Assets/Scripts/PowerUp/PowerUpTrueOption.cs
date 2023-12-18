using UnityEngine;
using UnityEngine.Events;

namespace PowerUp
{
    public class PowerUpTrueOption : PowerUpListener
    {
        [SerializeField] private UnityEvent _onUsePowerUp;
        public void UsePowerUp()
        {
            if (_event.amount >0)
            {
                _onUsePowerUp?.Invoke();
            }
        }
    }
}