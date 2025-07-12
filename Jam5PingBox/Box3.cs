using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jam5PingBox {
    public class Box3 : MonoBehaviour {
        GameObject _gravity;
        GameObject _inverseGravity;
        BoxButton _inverseGravityButton;
        BoxButton _noGravityButton;

        bool _isInverseGravity;
        bool _isNoGravity;

        class Data : DioramaMachine.BaseData {
            public bool _isInverseGravityButtonPushed;
            public bool _isNoGravityButtonPushed;
        }
        static List<Data> _prevRecords;
        List<Data> _currentRecords;

        public void Initialize() {
            foreach (Transform child in GetComponentsInChildren<Transform>(true)) {
                if (child.name == "BoxButtonGravity") {
                    _inverseGravityButton = child.gameObject.AddComponent<BoxButton>();
                }
                else if(child.name == "BoxButtonNoGravity") {
                    _noGravityButton = child.gameObject.AddComponent<BoxButton>();
                }
                else if(child.name == "GravityVolume") {
                    _gravity = child.gameObject;
                }
                else if(child.name == "InverseGravityVolume") {
                    _inverseGravity = child.gameObject;
                    _inverseGravity.SetActive(false);
                }
            }

            _inverseGravityButton._onAction = () => {
                _isInverseGravity = true;
                if (!_isNoGravity) {
                    _inverseGravity.SetActive(true);
                }
                _gravity.SetActive(false);
            };
            _inverseGravityButton._offAction = () => {
                _isInverseGravity = false;
                _inverseGravity.SetActive(false);
                if (!_isNoGravity) {
                    _gravity.SetActive(true);
                }
            };
            _inverseGravityButton.Initialize();
            _noGravityButton._onAction = () => {
                _isNoGravity = true;
                _inverseGravity.SetActive(false);
                _gravity.SetActive(false);
            };
            _noGravityButton._offAction = () => {
                _isNoGravity = false;
                if(_isInverseGravity) {
                    _inverseGravity.SetActive(true);
                }
                else {
                    _gravity.SetActive(true);
                }
            };
            _noGravityButton.Initialize();

            _currentRecords = new List<Data>();
            StartCoroutine(DioramaMachine.Record(transform, _currentRecords, _prevRecords, data => {
                data._isInverseGravityButtonPushed = _inverseGravityButton._pushed;
                data._isNoGravityButtonPushed = _noGravityButton._pushed;
            }, data => {
                _inverseGravityButton._pushedOnRecord = data._isInverseGravityButtonPushed;
                _inverseGravityButton.ChangeState();
                _noGravityButton._pushedOnRecord = data._isNoGravityButtonPushed;
                _noGravityButton.ChangeState();
            }));
        }

        void OnDestroy() {
            if (_currentRecords != null) {
                _prevRecords = _currentRecords;
            }
        }
    }
}
