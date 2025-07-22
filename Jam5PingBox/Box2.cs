using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jam5PingBox {
    public class Box2 : MonoBehaviour {
        Transform _door;
        Transform _doorOpen;
        Transform _doorClose;
        BoxButton _doorButton;
        List<TractorBeamFluid> _beamFluids;
        BoxButton _beamButton;
        BoxButton _beamButton2;

        bool _isDoorOpen = false;
        bool _isBeamReversed = false;
        bool _isBeamReversed1 = false;
        bool _isBeamReversed2 = false;

        class Data : DioramaMachine.BaseData {
            public bool _isDoorButtonPushed;
            public bool _isBeamButtonPushed;
            public bool _isBeamButton2Pushed;
        }
        static List<Data> _prevRecords;
        List<Data> _currentRecords;
        Coroutine _record;

        public void Initialize() {
            _beamFluids = new List<TractorBeamFluid> ();
            foreach (Transform child in GetComponentsInChildren<Transform>(true)) {
                if (child.name == "BoxButtonDoor") {
                    _doorButton = child.gameObject.AddComponent<BoxButton>();
                }
                else if (child.name == "Door") {
                    _door = child.Find("DoorBody");
                    _doorOpen = child.Find("Open");
                    _doorClose = child.Find("Close");
                }
                else if(child.name == "BoxButtonBeam") {
                    _beamButton = child.gameObject.AddComponent<BoxButton>();
                }
                else if(child.name == "BoxButtonBeam2") {
                    _beamButton2 = child.gameObject.AddComponent<BoxButton>();
                }
                else if(child.name.Contains("Tractor Beam")) {
                    _beamFluids.Add(child.GetComponentInChildren<TractorBeamFluid>());
                }
                else if(child.name == "text_diorama_box2_end_pc") {
                    DioramaMachine.ActivateComputer(child.GetComponent<NomaiComputer>());
                }
                else if(child.name == "Clock") {
                    if(DioramaMachine._clocks == null) {
                        DioramaMachine._clocks = new List<Transform>();
                    }
                    DioramaMachine._clocks.Add(child);
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

            _beamButton._onAction = () => {
                _isBeamReversed1 = true;
                _isBeamReversed = true;
            };
            _beamButton._offAction = () => {
                _isBeamReversed1 = false;
                if(!_isBeamReversed2) {
                    _isBeamReversed = false;
                }
            };
            _beamButton.Initialize();
            _beamButton2._onAction = () => {
                _isBeamReversed2 = true;
                _isBeamReversed = true;
            };
            _beamButton2._offAction = () => {
                _isBeamReversed2 = false;
                if(!_isBeamReversed1) {
                    _isBeamReversed = false;
                }
            };
            _beamButton2.Initialize();

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
                data._isBeamButtonPushed = _beamButton._pushed;
                data._isBeamButton2Pushed = _beamButton2._pushed;
            }, data => {
                _doorButton._pushedOnRecord = data._isDoorButtonPushed;
                _doorButton.ChangeState();
                _beamButton._pushedOnRecord = data._isBeamButtonPushed;
                _beamButton.ChangeState();
                _beamButton2._pushedOnRecord = data._isBeamButton2Pushed;
                _beamButton2.ChangeState();
            }));
        }

        void Update() {
            if (_door) {
                var pos = _isDoorOpen ? _doorOpen : _doorClose;
                _door.transform.localPosition = Vector3.Lerp(_door.transform.localPosition, pos.localPosition, 0.1f);
            }
            if (_beamFluids != null) {
                ReverseBeam();
            }
        }

        void ReverseBeam() {
            foreach(var beam in _beamFluids) {
                if(beam._reversed ^ _isBeamReversed) {
                    beam.SetFluidReversed(_isBeamReversed);
                }
            }
        }

        void OnDestroy() {
            if (_currentRecords != null) {
                _prevRecords = _currentRecords;
            }
        }
    }
}
