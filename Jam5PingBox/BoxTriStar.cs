using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jam5PingBox {
    public class BoxTriStar : MonoBehaviour {
        GameObject _bhToVessel;
        GameObject _whToVessel;
        GameObject _bhFromVessel;
        GameObject _whFromVessel;
        BoxButton _bhwhButton;

        Transform _door1;
        Transform _door1Open;
        Transform _door1Close;
        BoxButton _door1Button;
        Transform _door2;
        Transform _door2Open;
        Transform _door2Close;
        BoxButton _door2Button;
        Transform _door3;
        Transform _door3Open;
        Transform _door3Close;
        BoxButton _door3Button;

        Transform _water;
        SphereShape _hazardVolume;

        bool _isBhwhAppear = false;
        bool _isDoor1Open = false;
        bool _isDoor2Open = false;
        bool _isDoor3Open = false;

        public enum ScoutOrPlayer { // o (Salvation) -> x (Faith) -> v (Hope)
            EMPTY,
            SCOUT,
            PLAYER,
        }
        public (ScoutOrPlayer, float) _sunV = (ScoutOrPlayer.EMPTY, 0);
        public (ScoutOrPlayer, float) _sunO = (ScoutOrPlayer.EMPTY, 0);
        public (ScoutOrPlayer, float) _sunX = (ScoutOrPlayer.EMPTY, 0);

        class Data : DioramaMachine.BaseData {
            public bool _isBhwhButtonPushed;

            public bool _isDoor1ButtonPushed;
            public bool _isDoor2ButtonPushed;
            public bool _isDoor3ButtonPushed;
        }
        static List<Data> _prevRecords;
        List<Data> _currentRecords;

        public void Initialize() {
            foreach (Transform child in transform.parent.GetComponentsInChildren<Transform>(true)) {
                if(child.name == "BoxButtonVesselWarp") {
                    _bhwhButton = child.gameObject.AddComponent<BoxButton>();
                }
                else if(child.name == "BHToVessel") {
                    _bhToVessel = child.gameObject;
                    _bhToVessel.SetActive(false);
                }
                else if(child.name == "WHToVessel") {
                    _whToVessel = child.gameObject;
                    _whToVessel.SetActive(false);
                }
                else if(child.name == "BHFromVessel") {
                    _bhFromVessel = child.gameObject;
                    _bhFromVessel.SetActive(false);
                }
                else if(child.name == "WHFromVessel") {
                    _whFromVessel = child.gameObject;
                    _whFromVessel.SetActive(false);
                }
                else if(child.name == "Door1") {
                    _door1 = child.Find("Door");
                    _door1Open = child.Find("Open");
                    _door1Close = child.Find("Close");
                }
                else if(child.name == "BoxButtonDoor1") {
                    _door1Button = child.gameObject.AddComponent<BoxButton>();
                }
                else if(child.name == "Door2") {
                    _door2 = child.Find("Door");
                    _door2Open = child.Find("Open");
                    _door2Close = child.Find("Close");
                }
                else if(child.name == "BoxButtonDoor2") {
                    _door2Button = child.gameObject.AddComponent<BoxButton>();
                }
                else if(child.name == "Door3") {
                    _door3 = child.Find("DoorBody");
                    _door3Open = child.Find("Open");
                    _door3Close = child.Find("Close");
                }
                else if(child.name == "BoxButtonDoor3") {
                    _door3Button = child.gameObject.AddComponent<BoxButton>();
                }
                else if(child.name == "Water") {
                    _water = child;
                }
                else if(child.name == "HazardVolume") {
                    _hazardVolume = child.GetComponent<SphereShape>();
                }
                else if(child.name == "text_diorama_boxt_vessel_remote") {
                    child.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                }
                else if(child.name == "text_diorama_boxt_jam") {
                    child.transform.localScale = new Vector3(2, 2, 2);
                }
            }

            _bhwhButton._onAction = () => {
                _isBhwhAppear = true;
                _bhToVessel.SetActive(true);
                _whToVessel.SetActive(true);
                _bhFromVessel.SetActive(true);
                _whFromVessel.SetActive(true);
            };
            _bhwhButton._offAction = () => {
                _isBhwhAppear = false;
                _bhToVessel.SetActive(false);
                _whToVessel.SetActive(false);
                _bhFromVessel.SetActive(false);
                _whFromVessel.SetActive(false);
            };
            _bhwhButton.Initialize();

            _door1Button._onAction = () => {
                _isDoor1Open = true;
            };
            _door1Button._offAction = () => {
                _isDoor1Open = false;
            };
            _door1Button.Initialize();
            _door2Button._onAction = () => {
                _isDoor2Open = true;
            };
            _door2Button._offAction = () => {
                _isDoor2Open = false;
            };
            _door2Button.Initialize();
            _door3Button._onAction = () => {
                _isDoor3Open = true;
            };
            _door3Button._offAction = () => {
                _isDoor3Open = false;
            };
            _door3Button.Initialize();

            _currentRecords = new List<Data>();
            StartCoroutine(DioramaMachine.Record(transform, _currentRecords, _prevRecords, data => {
                data._isBhwhButtonPushed = _bhwhButton._pushed;
                data._isDoor1ButtonPushed = _door1Button._pushed;
                data._isDoor2ButtonPushed = _door2Button._pushed;
                data._isDoor3ButtonPushed = _door3Button._pushed;
            }, data => {
                _bhwhButton._pushedOnRecord = data._isBhwhButtonPushed;
                _bhwhButton.ChangeState();
                _door1Button._pushedOnRecord = data._isDoor1ButtonPushed;
                _door1Button.ChangeState();
                _door2Button._pushedOnRecord = data._isDoor2ButtonPushed;
                _door2Button.ChangeState();
                _door3Button._pushedOnRecord = data._isDoor3ButtonPushed;
                _door3Button.ChangeState();

                if(data._sunO.Item1 != ScoutOrPlayer.EMPTY && _sunO.Item1 == ScoutOrPlayer.EMPTY) {
                    _sunO = data._sunO;
                }
                if(data._sunV.Item1 != ScoutOrPlayer.EMPTY && _sunV.Item1 == ScoutOrPlayer.EMPTY) {
                    _sunV = data._sunV;
                }
                if(data._sunX.Item1 != ScoutOrPlayer.EMPTY && _sunX.Item1 == ScoutOrPlayer.EMPTY) {
                    _sunX = data._sunX;
                }
            }, true));
        }

        void Update() {
            if(_door1) {
                var pos = _isDoor1Open ? _door1Open : _door1Close;
                _door1.transform.localPosition = Vector3.Lerp(_door1.transform.localPosition, pos.localPosition, 0.1f);
            }
            if(_door2) {
                var pos = _isDoor2Open ? _door2Open : _door2Close;
                _door2.transform.localPosition = Vector3.Lerp(_door2.transform.localPosition, pos.localPosition, 0.1f);
            }
            if(_door3) {
                var pos = _isDoor3Open ? _door3Open : _door3Close;
                _door3.transform.localPosition = Vector3.Lerp(_door3.transform.localPosition, pos.localPosition, 0.1f);
            }

            if(_sunO.Item1 == ScoutOrPlayer.SCOUT && _sunX.Item1 == ScoutOrPlayer.SCOUT && _sunV.Item1 == ScoutOrPlayer.PLAYER && _sunO.Item2 < _sunX.Item2 && _sunX.Item2 < _sunV.Item2) {
                _water.transform.localScale = Vector3.Lerp(_water.transform.localScale, new Vector3(0.01f, 0.01f, 0.01f), 10);
                _hazardVolume.radius = Mathf.Lerp(_hazardVolume.radius, 0.01f, 10);
            }
        }

        void OnDestroy() {
            if (_currentRecords != null) {
                _prevRecords = _currentRecords;
            }
        }
    }
}
