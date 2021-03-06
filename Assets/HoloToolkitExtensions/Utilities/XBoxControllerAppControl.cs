﻿using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace HoloToolkitExtensions.Utilities
{
    public class XBoxControllerAppControl : MonoBehaviour, IXboxControllerHandler
    {
        public float Rotatespeed = 0.6f;
        public float MoveSpeed = 0.05f;
        public float TriggerAccerationFactor = 2f;

        private Quaternion _initialRotation;
        private Vector3 _initialPosition;

        private readonly DoubleClickPreventer _doubleClickPreventer = new DoubleClickPreventer();
        void Start()
        {
            _initialRotation = gameObject.transform.rotation;
            _initialPosition = gameObject.transform.position;
        }

        public void OnXboxInputUpdate(XboxControllerEventData eventData)
        {
            if (!UnityEngine.XR.XRDevice.isPresent)
            {
                var speed = (1.0f + TriggerAccerationFactor * eventData.XboxRightTriggerAxis) - 
                            (eventData.XboxLeftTriggerAxis * 0.9f);

                gameObject.transform.position += eventData.XboxLeftStickHorizontalAxis *
                                                 gameObject.transform.right * MoveSpeed * speed;
                gameObject.transform.position += eventData.XboxLeftStickVerticalAxis *
                                                 gameObject.transform.forward * MoveSpeed * speed;

                gameObject.transform.RotateAround(gameObject.transform.position,
                    gameObject.transform.up,
                    eventData.XboxRightStickHorizontalAxis * Rotatespeed * speed);
                gameObject.transform.RotateAround(gameObject.transform.position,
                    gameObject.transform.right,
                    -eventData.XboxRightStickVerticalAxis * Rotatespeed * speed);

                gameObject.transform.RotateAround(gameObject.transform.position,
                    gameObject.transform.forward,
                    eventData.XboxDpadHorizontalAxis * Rotatespeed * speed);

                var delta = Mathf.Sign(eventData.XboxDpadVerticalAxis) *
                            gameObject.transform.up * MoveSpeed * speed;
                if (Mathf.Abs(eventData.XboxDpadVerticalAxis) > 0.0001f)
                {
                    gameObject.transform.position += delta;
                }

                if (eventData.XboxB_Pressed)
                {
                    if (!_doubleClickPreventer.CanClick()) return;
                    gameObject.transform.position = _initialPosition;
                    gameObject.transform.rotation = _initialRotation;
                }

                HandleCustomAction(eventData);
            }
        }

        protected virtual void HandleCustomAction(XboxControllerEventData eventData)
        {
        }
    }
}
