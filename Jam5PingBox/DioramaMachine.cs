using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

namespace Jam5PingBox {
    public class DioramaMachine : MonoBehaviour {
        public const float INTERVAL = 0.5f;

        public GameObject _dioramaMachine;
        public GameObject _box1;
        public GameObject _box2;
        public GameObject _box3;
        public GameObject _boxTriStar;
        public List<GameObject> _boxTriStarObjs;
        public BoxTriStar _boxTriStarInstance;
        public GameObject _hiddenPingShip;

        public static DioramaMachine Instance;
        public static bool IsMapRestricted { get { return Instance && Instance._boxTriStar.activeSelf; } }
        public static bool _isMeditating;
        public static List<Transform> _clocks;
        static GameObject _hhearthian;
        static GameObject _hscout;
        static float _time;

        const float BOX1_SECONDS = 60 * 5;
        const float BOX2_SECONDS = 60 * 6;
        const float BOX3_SECONDS = 60 * 6;
        const float BOXTRISTAR_SECONDS = 60 * 22;
        static float _thisSeconds;

        (BoxTriStar.ScoutOrPlayer, float) _sunV = (BoxTriStar.ScoutOrPlayer.EMPTY, 0);
        (BoxTriStar.ScoutOrPlayer, float) _sunO = (BoxTriStar.ScoutOrPlayer.EMPTY, 0);
        (BoxTriStar.ScoutOrPlayer, float) _sunX = (BoxTriStar.ScoutOrPlayer.EMPTY, 0);

        public enum BoxType {
            BOX1,
            BOX2,
            BOX3,
            BOX_TRISTAR,
        }

        public class BaseData {
            public Vector3 _pos;
            public Quaternion _rot;
            public bool _scoutActive;
            public Vector3 _scoutPos;
            public Quaternion _scoutRot;
            public (BoxTriStar.ScoutOrPlayer, float) _sunV = (BoxTriStar.ScoutOrPlayer.EMPTY, 0);
            public (BoxTriStar.ScoutOrPlayer, float) _sunO = (BoxTriStar.ScoutOrPlayer.EMPTY, 0);
            public (BoxTriStar.ScoutOrPlayer, float) _sunX = (BoxTriStar.ScoutOrPlayer.EMPTY, 0);
        }

        public static Vector3 RecordPlayerPos(Transform box) {
            return box.InverseTransformPoint(Locator._playerBody.transform.position);
        }

        public static Quaternion RecordPlayerRot(Transform box) {
            return box.InverseTransformRotation(Locator._playerBody.transform.rotation);
        }

        public static Vector3 RecordScoutPos(Transform box) {
            return box.InverseTransformPoint(Locator._probe.transform.position);
        }

        public static Quaternion RecordScoutRot(Transform box) {
            return box.InverseTransformRotation(Locator._probe.transform.rotation);
        }

        public static void DestructProbe(string name) {
            if(!Instance || !Instance._boxTriStarInstance) {
                return;
            }

            var val = (BoxTriStar.ScoutOrPlayer.SCOUT, _time);
            if(name == "Salvation_Body") {
                Instance._boxTriStarInstance._sunO = val;
                Instance._sunO = val;
            }
            else if(name == "Hope_Body") {
                Instance._boxTriStarInstance._sunV = val;
                Instance._sunV = val;
            }
            else if(name == "Faith_Body") {
                Instance._boxTriStarInstance._sunX = val;
                Instance._sunX = val;
            }
        }

        public static void DestructPlayer(string name) {
            if(!Instance || !Instance._boxTriStarInstance) {
                return;
            }

            var val = (BoxTriStar.ScoutOrPlayer.PLAYER, _time);
            if(name == "Salvation_Body") {
                Instance._boxTriStarInstance._sunO = val;
                Instance._sunO = val;
            }
            else if(name == "Hope_Body") {
                Instance._boxTriStarInstance._sunV = val;
                Instance._sunV = val;
            }
            else if(name == "Faith_Body") {
                Instance._boxTriStarInstance._sunX = val;
                Instance._sunX = val;
            }
        }

        public static IEnumerator Record<Data>(Transform box, List<Data> currentRecords, List<Data> prevRecords, Action<Data> onRecord, Action<Data> onLoad, bool recordScout = false) where Data : BaseData, new() {
            _time = 0;
            float time = INTERVAL + 1;
            int idx = -1;
            while (true) {
                if(_isMeditating) {
                    break;
                }

                if(time >= INTERVAL) {
                    time = 0;
                    var newData = new Data { _pos = RecordPlayerPos(box), _rot = RecordPlayerRot(box) };
                    if (recordScout) {
                        if(Locator._probe) {
                            if (Locator._probe.gameObject.activeSelf) {
                                newData._scoutActive = true;
                                newData._scoutPos = RecordScoutPos(box);
                                newData._scoutRot = RecordScoutRot(box);
                            }
                            else {
                                newData._scoutActive = false;
                                newData._scoutPos = RecordPlayerPos(box);
                                newData._scoutRot = RecordPlayerRot(box);
                            }
                        }
                        else {
                            newData._scoutActive = false;
                        }
                    }
                    if(Instance && Instance._boxTriStar) {
                        newData._sunO = Instance._sunO;
                        newData._sunV = Instance._sunV;
                        newData._sunX = Instance._sunX;
                    }

                    onRecord(newData);
                    currentRecords.Add(newData);

                    if (prevRecords != null) {
                        idx++;
                        if (idx >= prevRecords.Count) {
                            idx = prevRecords.Count - 1;
                        }
                    }
                }
                if(idx >= 0 && prevRecords != null) {
                    if(!_hhearthian.activeSelf) {
                        _hhearthian.SetActive(true);
                        _hhearthian.transform.parent = box;
                    }
                    if (idx < prevRecords.Count - 1) {
                        _hhearthian.transform.localPosition = Vector3.Lerp(prevRecords[idx]._pos, prevRecords[idx+1]._pos, time / INTERVAL);
                        _hhearthian.transform.localRotation = Quaternion.Lerp(prevRecords[idx]._rot, prevRecords[idx+1]._rot, time / INTERVAL);
                    }
                    else {
                        _hhearthian.transform.localPosition = prevRecords[idx]._pos;
                        _hhearthian.transform.localRotation = prevRecords[idx]._rot;
                    }

                    if(recordScout) {
                        if(_hscout.transform.parent != box) {
                            _hscout.transform.parent = box;
                        }
                        if (prevRecords[idx]._scoutActive) {
                            _hscout.SetActive(true);
                            if (idx < prevRecords.Count - 1) {
                                _hscout.transform.localPosition = Vector3.Lerp(prevRecords[idx]._scoutPos, prevRecords[idx+1]._scoutPos, time / INTERVAL);
                                _hscout.transform.localRotation = Quaternion.Lerp(prevRecords[idx]._scoutRot, prevRecords[idx+1]._scoutRot, time / INTERVAL);
                            }
                            else {
                                _hscout.transform.localPosition = prevRecords[idx]._scoutPos;
                                _hscout.transform.localRotation = prevRecords[idx]._scoutRot;
                            }
                        }
                        else {
                            _hscout.SetActive(false);
                        }
                    }
                    onLoad(prevRecords[idx]);
                }
                time += Time.deltaTime;
                _time += Time.deltaTime;

                if (_clocks != null) {
                    foreach (var clock in _clocks) {
                        if (clock) {
                            clock.transform.localEulerAngles = new Vector3(clock.transform.localEulerAngles.x, clock.transform.localEulerAngles.y, _time / (_thisSeconds + 50) * 360);
                        }
                    }
                }
                yield return null;
            }
        }

        public static void ActivateComputer(NomaiComputer computer) {
            computer.StartCoroutine(ActivateComputerBody(computer));
        }

        static IEnumerator ActivateComputerBody(NomaiComputer computer) {
            yield return null;
            computer.enabled = true;
        }

        public void EnterJam() {
            _boxTriStar.SetActive(false);
            foreach(var obj in _boxTriStarObjs) {
                obj.SetActive(false);
            }
            _hiddenPingShip.SetActive(true);
            TimeLoop.SetSecondsRemaining(60*22);

            var playerRigidbody = Locator.GetPlayerBody();
            playerRigidbody.WarpToPositionRotation(_hiddenPingShip.transform.position, _hiddenPingShip.transform.rotation);
            playerRigidbody.SetVelocity(PointVelocity(_hiddenPingShip.transform));
        }

        public void Initialize() {
            Instance = this;
            _isMeditating = false;
            _clocks = null;

            _dioramaMachine = gameObject;

            _hhearthian = transform.Find("hhearthian").gameObject;
            _hhearthian.SetActive(false);
            _hscout = transform.Find("hscout").gameObject;
            _hscout.SetActive(false);

            _box1.SetActive(false);
            _box2.SetActive(false);
            _box3.SetActive(false);
            _boxTriStar.SetActive(false);

            foreach (var obj in _boxTriStarObjs) {
                obj.SetActive(false);
            }

            _hiddenPingShip.SetActive(false);
        }

        public void Load(BoxType boxType) {
            GameObject box = null;
            if (boxType == BoxType.BOX1) {
                box = _box1;
                box.SetActive(true);
                box.AddComponent<Box1>().Initialize();
                _thisSeconds = BOX1_SECONDS;
            }
            else if (boxType == BoxType.BOX2) {
                box = _box2;
                box.SetActive(true);
                box.AddComponent<Box2>().Initialize();
                _thisSeconds = BOX2_SECONDS;
            }
            else if (boxType == BoxType.BOX3) {
                box = _box3;
                box.SetActive(true);
                box.AddComponent<Box3>().Initialize();
                _thisSeconds = BOX3_SECONDS;
            }
            else if(boxType == BoxType.BOX_TRISTAR) {
                box = _boxTriStar;
                box.SetActive(true);
                foreach(var obj in _boxTriStarObjs) {
                    obj.SetActive(true);
                }
                _boxTriStarInstance = box.AddComponent<BoxTriStar>();
                _boxTriStarInstance.Initialize();
                _thisSeconds = BOXTRISTAR_SECONDS;
            }
            _dioramaMachine.SetActive(false);
            TimeLoop.SetSecondsRemaining(_thisSeconds);

            foreach (Transform child in box.GetComponentsInChildren<Transform>()) {
                if(child.name == "Spawn") {
                    var playerRigidbody = Locator.GetPlayerBody();
                    playerRigidbody.WarpToPositionRotation(child.position, child.rotation);
                    playerRigidbody.SetVelocity(PointVelocity(child));
                }
                else if(child.name == "SpawnShip") {
                    var shipRigidbody = Locator.GetShipBody();
                    shipRigidbody.WarpToPositionRotation(child.position, child.rotation);
                    shipRigidbody.SetVelocity(PointVelocity(child));
                }
            }

            if(boxType != BoxType.BOX_TRISTAR) {
                Locator.GetPlayerSuit().RemoveSuit(true);
            }
        }

        Vector3 PointVelocity(Transform point) {
            var parentOWRigidbody = point.GetComponentInParent<OWRigidbody>();
            return parentOWRigidbody.GetVelocity() + parentOWRigidbody.GetPointTangentialVelocity(point.position);
        }
    }
}
