using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTextureTiling : MonoBehaviour
{

    [SerializeField] private Renderer _renderer;

    private void Start() {
        _renderer = GetComponent<Renderer>();
    }

    [ContextMenu("UpdateTiling")]
    private void UpdateTiling(){
        var scaleX = transform.localScale.x;
        var scaleY = transform.localScale.z;
        _renderer.material.mainTextureScale = new Vector2(scaleX, scaleY);
    }

}
