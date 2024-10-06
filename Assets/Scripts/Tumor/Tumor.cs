using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Tumor : MonoBehaviour
{
    public event Action<Tumor> CureEvent;

    public bool IsCured {get; private set;}

    private Material _defaultMaterial;
    public Material _curedMaterial;

    public TumorManager TumorManager;
    private MeshRenderer _meshRenderer;

    public void Init(TumorManager tumorManager)
    {
        TumorManager = tumorManager;


        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _defaultMaterial = _meshRenderer.material;
    }

    public void SetCured(bool tf)
    {
        if (tf)
        {
            _meshRenderer.material = _curedMaterial;
        }
        else
        {
            _meshRenderer.material = _defaultMaterial;
        }

        IsCured = tf;
        CureEvent?.Invoke(this);
    }
}
