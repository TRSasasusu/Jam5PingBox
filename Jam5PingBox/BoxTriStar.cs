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

        bool _isBhwhAppear = false;

        class Data : DioramaMachine.BaseData {
            public bool _isBhwhButtonPushed;
        }
        static List<Data> _prevRecords;
        List<Data> _currentRecords;

        public void Initialize() {
            foreach (Transform child in GetComponentsInChildren<Transform>(true)) {
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

            _currentRecords = new List<Data>();
            StartCoroutine(DioramaMachine.Record(transform, _currentRecords, _prevRecords, data => {
                data._isBhwhButtonPushed = _bhwhButton._pushed;
            }, data => {
                _bhwhButton._pushedOnRecord = data._isBhwhButtonPushed;
                _bhwhButton.ChangeState();
            }));
        }

        void OnDestroy() {
            if (_currentRecords != null) {
                _prevRecords = _currentRecords;
            }
        }
    }
}
