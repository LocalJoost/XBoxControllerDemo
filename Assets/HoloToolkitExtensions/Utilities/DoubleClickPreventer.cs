using UnityEngine;

namespace HoloToolkitExtensions.Utilities
{
    public class DoubleClickPreventer
    {
        private readonly float _clickTimeOut;

        private float _lastClick;

        public DoubleClickPreventer(float clickTimeOut = 0.1f)
        {
            _clickTimeOut = clickTimeOut;
        }

        public bool CanClick()
        {
            if (!(Time.time - _lastClick > _clickTimeOut))
            {
                return false;
            }
            _lastClick = Time.time;
            return true;
        }
    }
}
