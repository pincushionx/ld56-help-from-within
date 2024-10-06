using Oneill;
using System.Collections.Generic;
using UnityEngine;

public class BotCell : MonoBehaviour
{
    public bool Selectable = true;


    public BotOrganism Organism { get; private set; }

    private BotCell[] _neighbourCells = null; // Maintained by parent organism
    public Transform[] RaycastTransformCenter;

    public Transform[][] RaycastTransforms;
    public Transform[] RaycastTransformUp;
    public Transform[] RaycastTransformDown;
    public Transform[] RaycastTransformLeft;
    public Transform[] RaycastTransformRight;
    public Transform DisconnectButtonContainer;


    private BotCellSelection _selection;

    public void Init(BotOrganism organism, BotCell[] neighbourCells)
    {
        Organism = organism;

        _neighbourCells = neighbourCells;

        _selection = GetComponentInChildren<BotCellSelection>(true);
        _selection.Init(this);


        RaycastTransforms = new Transform[4][];
        RaycastTransforms[0] = RaycastTransformUp;
        RaycastTransforms[1] = RaycastTransformDown;
        RaycastTransforms[2] = RaycastTransformLeft;
        RaycastTransforms[3] = RaycastTransformRight;
    }

    public bool HasNeighbour(Neighbour n)
    {
        int i = NeighbourUtil.GetNeighbourIndex(n);
        return _neighbourCells[i] != null;
    }
    public BotCell GetNeighbour(Neighbour n)
    {
        int i = NeighbourUtil.GetNeighbourIndex(n);
        return _neighbourCells[i];
    }


    public RaycastResult Raycast(Neighbour n)
    {
        return Raycast(n, 0.5f);
    }
    public RaycastResult Raycast(Neighbour n, float distance)
    {
        int i = NeighbourUtil.GetNeighbourIndex(n);
        Transform[] transforms = RaycastTransforms[i];

        RaycastResult result = new RaycastResult();
        float nearest = float.MaxValue;

        foreach (Transform transform in transforms)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction, Color.red);

            if (Physics.Raycast(ray, out hit, distance))
            {
                if (hit.distance < nearest)
                {
                    nearest = hit.distance;

                    result.hit = true;
                    result.NearestHit = hit;
                    result.ray = ray;
                }
            }
        }

        return result;
    }


    public Transform GetRaycastCenterTransform(Neighbour n)
    {
        int i = NeighbourUtil.GetNeighbourIndex(n);
        return RaycastTransformCenter[i];
    }

    public void SelectedOrganism(bool tf)
    {
        _selection.gameObject.SetActive(tf);
    }
    public void SelectedCell(bool tf)
    {
        _selection.gameObject.SetActive(tf);
    }



    public struct RaycastResult
    {
        public bool hit;
        public RaycastHit NearestHit;
        public Ray ray;
    }
}
