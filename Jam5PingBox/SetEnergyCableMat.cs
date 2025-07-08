using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jam5PingBox {
    public class SetEnergyCableMat : MonoBehaviour {
        Material _energyCableMaterial;

        public void Initialize(Material originalEnergyCableMaterial) {
            var zSize = transform.localScale.z;
            _energyCableMaterial = new Material(originalEnergyCableMaterial);
            _energyCableMaterial.mainTextureScale = new Vector2(1, 60 * (zSize / 10));
            GetComponent<MeshRenderer>().material = _energyCableMaterial;
        }

        void OnDestroy() {
            Destroy(_energyCableMaterial);
        }
    }
}
