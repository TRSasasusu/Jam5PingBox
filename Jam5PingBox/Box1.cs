using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jam5PingBox {
    public class Box1 : MonoBehaviour {
        Transform _door;
        Transform _doorOpen;
        Transform _doorClose;
        BoxButton _doorButton;
        Transform _floor;
        Transform _floorUp;
        Transform _floorDown;
        BoxButton _floorButton;

        bool _isDoorOpen = false;
        bool _isFloorUp = false;

        class Data : DioramaMachine.BaseData {
            public bool _isDoorOpen;
            public bool _isFloorUp;
        }
        static List<Data> _prevRecords;
        List<Data> _currentRecords;

        public void Initialize() {
            foreach (Transform child in GetComponentsInChildren<Transform>(true)) {
                if(child.name == "BoxButtonDoor") {
                    _doorButton = child.gameObject.AddComponent<BoxButton>();
                }
                else if(child.name == "Door") {
                    _door = child.Find("DoorBody");
                    _doorOpen = child.Find("Open");
                    _doorClose = child.Find("Close");
                }
                else if(child.name == "BoxButtonElevator") {
                    _floorButton = child.gameObject.AddComponent<BoxButton>();
                }
                else if(child.name == "Elevator") {
                    _floor = child.Find("Floor");
                    _floorUp = child.Find("Positions/1");
                    _floorDown = child.Find("Positions/0");
                }
            }

            _doorButton._onAction = () => {
                _isDoorOpen = true;
            };
            _doorButton._offAction = () => {
                _isDoorOpen = false;
            };
            _doorButton.Initialize();

            _floorButton._onAction = () => {
                _isFloorUp = true;
            };
            _floorButton._offAction = () => {
                _isFloorUp = false;
            };
            _floorButton.Initialize();

            _currentRecords = new List<Data>();
            StartCoroutine(DioramaMachine.Record(transform, _currentRecords, _prevRecords, data => {
                data._isDoorOpen = _isDoorOpen;
                data._isFloorUp = _isFloorUp;
            }, data => {
                _doorButton._on |= data._isDoorOpen;
                _doorButton.ChangeState();
                _floorButton._on |= data._isFloorUp;
                _floorButton.ChangeState();
            }));
        }

        void Update() {
            if(_door) {
                var pos = _isDoorOpen ? _doorOpen : _doorClose;
                _door.transform.localPosition = Vector3.Lerp(_door.transform.localPosition, pos.localPosition, 0.1f);
            }
            if (_floor) {
                var pos = _isFloorUp ? _floorUp : _floorDown;
                _floor.transform.localPosition = Vector3.Lerp(_floor.transform.localPosition, pos.localPosition, 0.01f);
            }

            //if(_time >= DioramaMachine.INTERVAL) {
            //    _time = 0;
            //    if(_currentRecords == null) {
            //        _currentRecords = new List<Data>();
            //    }
            //    _currentRecords.Add(new Data {
            //        _pos = DioramaMachine.RecordPlayerPos(transform),
            //        _rot = DioramaMachine.RecordPlayerRot(transform),
            //        _isDoorOpen = _isDoorOpen,
            //        _isFloorUp = _isFloorUp,
            //    });

            //    if (_records != null) {
            //        if(!_hhearthian.activeSelf) {
            //            _hhearthian.SetActive(true);
            //        }
            //        var record = _records[_idx++];
            //        if (_idx >= _records.Count) {
            //            _idx = _records.Count - 1;
            //        }
            //        _hhearthian.transform.localPosition = record._pos;
            //        _hhearthian.transform.localRotation = record._rot;
            //        _isDoorOpen |= record._isDoorOpen;
            //        _isFloorUp |= record._isFloorUp;
            //    }
            //}
            //_time += Time.deltaTime;
        }

        void OnDestroy() {
            _prevRecords = _currentRecords;
        }
    }
}
