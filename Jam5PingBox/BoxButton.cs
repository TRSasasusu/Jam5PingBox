using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jam5PingBox {
    public class BoxButton : MonoBehaviour {
        public Action _onAction;
        public Action _offAction;

        bool _on = false;
        GameObject _onButton;
        GameObject _offButton;
        List<GameObject> _onCables;
        List<GameObject> _offCables;

        public void Initialize() {
            _onButton = transform.Find("On").gameObject;
            _offButton = transform.Find("Off").gameObject;
            _onButton.SetActive(false);

            _onCables = new List<GameObject>();
            _offCables = new List<GameObject>();
            foreach (Transform child in transform) {
                if(child.name.Contains("EnergyCable")) {
                    _onCables.Add(child.gameObject);
                    child.gameObject.SetActive(false);
                }
                else if(child.name.Contains("CableOff")) {
                    _offCables.Add(child.gameObject);
                }
            }
        }

        void OnTriggerEnter(Collider other) {
            if(other.gameObject == Locator._playerBody.gameObject) {
                _on = true;
                ChangeState();
            }
        }

        void OnTriggerExit(Collider other) {
            if(other.gameObject == Locator._playerBody.gameObject) {
                _on = false;
                ChangeState();
            }
        }

        void ChangeState() {
            if (_on) {
                _onButton.SetActive(true);
                _offButton.SetActive(false);
                foreach (var cable in _onCables) {
                    cable.SetActive(true);
                }
                foreach(var cable in _offCables) {
                    cable.SetActive(false);
                }
                _onAction();
            }
            else {
                _onButton.SetActive(false);
                _offButton.SetActive(true);
                foreach (var cable in _onCables) {
                    cable.SetActive(false);
                }
                foreach(var cable in _offCables) {
                    cable.SetActive(true);
                }
                _offAction();
            }
        }
    }
}
