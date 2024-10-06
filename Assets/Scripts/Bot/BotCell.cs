using Oneill;
using System.Collections.Generic;
using UnityEngine;

public class BotCell : MonoBehaviour
{
    public bool Selectable = true;


    public BotOrganism Organism { get; private set; }

    public BotCell[] NeighbourCells = null; // Maintained by parent organism
    public Transform[] RaycastTransformCenter;

    public Transform[][] RaycastTransforms;
    public Transform[] RaycastTransformUp;
    public Transform[] RaycastTransformDown;
    public Transform[] RaycastTransformLeft;
    public Transform[] RaycastTransformRight;
    public Transform DisconnectButtonContainer;

    public GameObject[] PermanentlyConnectionIcons;
    public GameObject[] TemporarilyConnectionIcons;



    private BotCellSelection _selection;

    public void Init(BotOrganism organism, BotCell[] neighbourCells)
    {
        Organism = organism;

        NeighbourCells = neighbourCells;

        _selection = GetComponentInChildren<BotCellSelection>(true);
        _selection.Init(this);


        RaycastTransforms = new Transform[4][];
        RaycastTransforms[0] = RaycastTransformUp;
        RaycastTransforms[1] = RaycastTransformDown;
        RaycastTransforms[2] = RaycastTransformLeft;
        RaycastTransforms[3] = RaycastTransformRight;


        // Add the chain, if needed
        //if (!Selectable)
        //{
        //    for (int i = 0; i < _neighbourCells.Length; i++)
        //    {
        //        if (_neighbourCells[i] == null)
        //        {
        //            PermanentlyConnectionIcons[i].SetActive(false);
        //            continue;
        //        }
        //        PermanentlyConnectionIcons[i].SetActive(!_neighbourCells[i].Selectable);
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < _neighbourCells.Length; i++)
        //    {
        //        // Perm
        //        bool permLink = (!Selectable && _neighbourCells[i] != null && !_neighbourCells[i].Selectable);
        //        PermanentlyConnectionIcons[i].SetActive(permLink);

        //        // Temp
        //        if (_neighbourCells[i] == null)
        //        {
        //            TemporarilyConnectionIcons[i].SetActive(false);
        //            continue;
        //        }
        //        TemporarilyConnectionIcons[i].SetActive(_neighbourCells[i].Selectable);
        //    }
        //}

        for (int i = 0; i < NeighbourCells.Length; i++)
        {
            // Perm
            bool permLink = (!Selectable && NeighbourCells[i] != null && !NeighbourCells[i].Selectable);
            PermanentlyConnectionIcons[i].SetActive(permLink);

            // Temp
            bool tempLink = (!permLink && NeighbourCells[i] != null);
            TemporarilyConnectionIcons[i].SetActive(tempLink);
        }
    }

    public bool HasNeighbour(Neighbour n)
    {
        int i = NeighbourUtil.GetNeighbourIndex(n);
        return NeighbourCells[i] != null;
    }
    public BotCell GetNeighbour(Neighbour n)
    {
        int i = NeighbourUtil.GetNeighbourIndex(n);
        return NeighbourCells[i];
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

            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

            if (Physics.Raycast(ray, out hit, distance, 0xFFFF, QueryTriggerInteraction.Ignore))
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
