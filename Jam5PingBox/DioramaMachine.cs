using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

namespace Jam5PingBox {
    public class DioramaMachine : MonoBehaviour {
        public const float INTERVAL = 1f;

        public GameObject _dioramaMachine;
        public GameObject _box1;
        public GameObject _box2;
        public GameObject _box3;
        public GameObject _boxTriStar;
        public List<GameObject> _boxTriStarObjs;

        static GameObject _hhearthian;

        public enum BoxType {
            BOX1,
            BOX2,
            BOX3,
            BOX_TRISTAR,
        }

        public class BaseData {
            public Vector3 _pos;
            public Quaternion _rot;
        }

        public static Vector3 RecordPlayerPos(Transform box) {
            return box.InverseTransformPoint(Locator._playerBody.transform.position);
        }

        public static Quaternion RecordPlayerRot(Transform box) {
            return box.InverseTransformRotation(Locator._playerBody.transform.rotation);
        }

        public static IEnumerator Record<Data>(Transform box, List<Data> currentRecords, List<Data> prevRecords, Action<Data> onRecord, Action<Data> onLoad) where Data : BaseData, new() {
            float time = INTERVAL + 1;
            int idx = -1;
            while (true) {
                if(time >= INTERVAL) {
                    time = 0;
                    var newData = new Data { _pos = RecordPlayerPos(box), _rot = RecordPlayerRot(box) };
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
                    onLoad(prevRecords[idx]);
                }
                time += Time.deltaTime;
                yield return null;
            }
        }

        public void Initialize() {
            _dioramaMachine = gameObject;

            _hhearthian = transform.Find("hhearthian").gameObject;
            _hhearthian.SetActive(false);

            _box1.SetActive(false);
            _box2.SetActive(false);
            _box3.SetActive(false);

            foreach (var obj in _boxTriStarObjs) {
                obj.SetActive(false);
            }
        }

        public void Load(BoxType boxType) {
            GameObject box = null;
            if (boxType == BoxType.BOX1) {
                box = _box1;
                box.SetActive(true);
                box.AddComponent<Box1>().Initialize();
            }
            else if (boxType == BoxType.BOX2) {
                box = _box2;
                box.SetActive(true);
                box.AddComponent<Box2>().Initialize();
            }
            else if (boxType == BoxType.BOX3) {
                box = _box3;
                box.SetActive(true);
                box.AddComponent<Box3>().Initialize();
            }
            else if(boxType == BoxType.BOX_TRISTAR) {
                box = _boxTriStar;
                foreach(var obj in _boxTriStarObjs) {
                    obj.SetActive(true);
                }
                box.AddComponent<BoxTriStar>().Initialize();
            }
            _dioramaMachine.SetActive(false);

            foreach (Transform child in box.GetComponentsInChildren<Transform>()) {
                if(child.name == "Spawn") {
                    var playerRigidbody = Locator.GetPlayerBody();
                    playerRigidbody.WarpToPositionRotation(child.position, child.rotation);
                    playerRigidbody.SetVelocity(PointVelocity(child));
                    break;
                }
            }

            Locator.GetPlayerSuit().RemoveSuit(true);
        }

        Vector3 PointVelocity(Transform point) {
            var parentOWRigidbody = point.GetComponentInParent<OWRigidbody>();
            return parentOWRigidbody.GetVelocity() + parentOWRigidbody.GetPointTangentialVelocity(point.position);
        }
    }
}
