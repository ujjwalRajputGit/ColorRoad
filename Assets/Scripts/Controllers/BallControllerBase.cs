using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControllerBase : MonoBehaviour
{
    [SerializeField]
    public Ball _property;
    [SerializeField]
    private Material _redMaterila;
    [SerializeField]
    private Material _greenMaterila;
    [SerializeField]
    private Material _blueMaterila;
    private Renderer _renderer;



    protected void Start()
    {
        if (!TryGetComponent<Renderer>(out _renderer))
            Debug.LogError($"error getting renderer: BallControllerBase {nameof(BallControllerBase)}");
        ChangeColor(_property.Color);
    }


    protected void ChangeColor(BallColor color)
    {
        if (!_renderer) return;

        _property.Color = color;

        switch (color)
        {
            case BallColor.Red:
                _renderer.sharedMaterial = _redMaterila;
                break;
            case BallColor.Green:
                _renderer.sharedMaterial = _greenMaterila;
                break;
            case BallColor.Blue:
                _renderer.sharedMaterial = _blueMaterila;
                break;
        }
    }
}
