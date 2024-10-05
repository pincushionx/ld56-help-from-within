using Oneill;
using UnityEngine;

public class BotCell : MonoBehaviour
{
    public BotOrganism Organism { get; private set; }

    private BotCell[] _neighbourCells = null; // Maintained by parent organism
    public Transform[] RaycastTransform;
    public Transform DisconnectButtonContainer;


    private BotCellSelection _selection;

    public void Init(BotOrganism organism, BotCell[] neighbourCells)
    {
        Organism = organism;

        _neighbourCells = neighbourCells;

        _selection = GetComponentInChildren<BotCellSelection>(true);
        _selection.Init(this);
    }

    public bool HasNeighbour(Neighbour n)
    {
        int i = NeighbourUtil.GetNeighbourIndex(n);
        return _neighbourCells[i] != null;
    }


    public Transform GetRaycastTransform(Neighbour n)
    {
        int i = NeighbourUtil.GetNeighbourIndex(n);
        return RaycastTransform[i];
    }

    public void SelectedOrganism(bool tf)
    {
        _selection.gameObject.SetActive(tf);
        //DisconnectButtonContainer.gameObject.SetActive(tf);
    }
    public void SelectedCell(bool tf)
    {
        _selection.gameObject.SetActive(tf);
        //DisconnectButtonContainer.gameObject.SetActive(tf);
    }


    //public RaycastHit TerrainRaycast(Neighbour n)
    //{
    //    int i = NeighbourUtil.GetNeighbourIndex(n);


    //    Ray ray = new Ray(RaycastPosition[i].position, direction);
    //    RaycastHit hit = new RaycastHit();
    //    Debug.DrawRay(ray.origin, ray.direction, Color.red);
    //    if (Physics.Raycast(ray, out hit, diff.x, LayerMask.GetMask("Terrain")))
    //    {
    //        diff.x = hit.point.x + _colliderBuffer;
    //    }
    //}
}
