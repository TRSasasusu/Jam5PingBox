﻿using OWML.ModHelper;
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
            public bool _isDoorButtonPushed;
            public bool _isFloorButtonPushed;
        }
        static List<Data> _prevRecords;
        List<Data> _currentRecords;
        Coroutine _record;

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
                else if(child.name == "text_diorama_box1_end_pc") {
                    DioramaMachine.ActivateComputer(child.GetComponent<NomaiComputer>());
                }
                else if(child.name == "Clock") {
                    DioramaMachine._clocks = new List<Transform> { child };
                    child.GetComponent<InteractReceiver>().OnPressInteract += Restart;
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

            Restart();
        }

        void Restart() {
            if(_record != null) {
                StopCoroutine(_record);
            }
            if (_currentRecords != null) {
                _prevRecords = _currentRecords;
            }

            _currentRecords = new List<Data>();
            _record = StartCoroutine(DioramaMachine.Record(transform, _currentRecords, _prevRecords, data => {
                data._isDoorButtonPushed = _doorButton._pushed;
                data._isFloorButtonPushed = _floorButton._pushed;
            }, data => {
                _doorButton._pushedOnRecord = data._isDoorButtonPushed;
                _doorButton.ChangeState();
                _floorButton._pushedOnRecord = data._isFloorButtonPushed;
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
        }

        void OnDestroy() {
            if (_currentRecords != null) {
                _prevRecords = _currentRecords;
            }
        }
    }
}
