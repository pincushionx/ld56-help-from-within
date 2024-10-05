using Oneill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class BotOrganism : MonoBehaviour
{
    public Scene Scene {get; private set; }

    public Transform CellContainer;
    public BotCell[] BotCells { get; private set; }


    private float _moveSpeedModifier = 1.0f;
    private Vector2 _targetMoveVector = Vector2.zero;


    private float _colliderBuffer = 0.01f;


    // For reuse
    List<BotCell> _cellsFacingDirection = new List<BotCell>();

    //private Rigidbody _rigidbody;

    private void Awake()
    {
        //_rigidbody = GetComponent<Rigidbody>();


        // Remove once it's initialized somewherE?>
        InitCells();
    }

    public void Init(Scene scene)
    {
        Scene = scene;
        InitCells();
    }

    private void InitCells()
    {
        BotCells = CellContainer.GetComponentsInChildren<BotCell>();
        for (int i = 0; i < BotCells.Length; i++)
        {
            BotCell cell = BotCells[i];
            Vector3 cellPosition = cell.transform.position;

            BotCell[] neighbours = new BotCell[4];

            for (int j = 0; j < BotCells.Length; j++)
            {
                BotCell compareCell = BotCells[j];

                if (cell == compareCell)
                {
                    continue;
                }

                Vector3 diff = compareCell.transform.position - cellPosition;

                // Connect left
                if (diff.x < 0)
                {
                    if (diff.x >= -1.5f)
                    {
                        neighbours[NeighbourUtil.GetNeighbourIndex(Neighbour.Left)] = cell;
                    }
                }
                // Connect right
                else if (diff.x > 0)
                {
                    if (diff.x <= 1.5f)
                    {
                        neighbours[NeighbourUtil.GetNeighbourIndex(Neighbour.Right)] = cell;
                    }
                }

                //TODO connect up/down
            }

            cell.Init(this, neighbours);
        }
    }

    private void Update()
    {
        Vector3 diff = _targetMoveVector * _moveSpeedModifier * Time.deltaTime;

        Vector3 targetPosition = transform.position + diff;



        //Neighbour neighbourMask = NeighbourUtil.GetNeighbourByOffset(diff);
        //Neighbour[] neighbours = NeighbourUtil.GetNeighboursFromMask(neighbourMask);

        if (_targetMoveVector.x != 0)
        {
            float sign = (_targetMoveVector.x < 0)? -1 : 1;
            float dist = diff.x * sign;
            Neighbour neighbour = sign < 0? Neighbour.Left : Neighbour.Right;


            BotCell[] cells = GetCellsFacingDirection(neighbour);
            foreach (BotCell cell in cells)
            {
                Ray ray = new Ray(cell.GetRaycastTransform(neighbour).position, _targetMoveVector);
                RaycastHit hit = new RaycastHit();

                Debug.DrawRay(ray.origin, ray.direction, Color.red);

                if (Physics.Raycast(ray, out hit, dist, LayerMask.GetMask("Terrain")))
                {
                    targetPosition.x += (sign * ray.origin.x) - (sign*hit.point.x) - (sign * _colliderBuffer);
                }

                else if (Physics.Raycast(ray, out hit, dist, LayerMask.GetMask("BotOrganism")))
                {
                    BotCollider collider = hit.transform.GetComponent<BotCollider>();
                    if (collider != null)
                    {
                        targetPosition.x += (sign * ray.origin.x) - (sign * hit.point.x);
                        MergeOrganisms(collider.ParentCell.Organism);
                    }
                }
            }
        }



        //    if (_targetMoveVector.x < 0)
        //{
        //    float dist = -diff.x;
        //    Neighbour neighbour = Neighbour.Left;
        //    BotCell[] cells = GetCellsFacingDirection(neighbour);

        //    foreach (BotCell cell in cells)
        //    {
        //        Ray ray = new Ray(cell.GetRaycastTransform(neighbour).position, _targetMoveVector);
        //        RaycastHit hit = new RaycastHit();
        //        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        //        if (Physics.Raycast(ray, out hit, dist, LayerMask.GetMask("Terrain")))
        //        {
        //            targetPosition.x += hit.point.x - ray.origin.x + _colliderBuffer;
        //        }
        //        else if (Physics.Raycast(ray, out hit, dist, LayerMask.GetMask("BotOrganism")))
        //        {
        //            BotCollider collider = hit.transform.GetComponent<BotCollider>();
        //            if (collider != null)
        //            {
        //                targetPosition.x += ray.origin.x - hit.point.x;
        //                MergeOrganisms(collider.ParentCell.Organism);
        //            }
        //        }
        //    }
        //}
        //if (_targetMoveVector.x > 0)
        //{
        //    float dist = diff.x;
        //    Neighbour neighbour = Neighbour.Right;
        //    BotCell[] cells = GetCellsFacingDirection(neighbour);

        //    foreach (BotCell cell in cells)
        //    {
        //        Ray ray = new Ray(cell.GetRaycastTransform(neighbour).position, _targetMoveVector);
        //        RaycastHit hit = new RaycastHit();
        //        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        //        if (Physics.Raycast(ray, out hit, dist, LayerMask.GetMask("Terrain")))
        //        {
        //            targetPosition.x += ray.origin.x - hit.point.x - _colliderBuffer;
        //        }

        //        else if (Physics.Raycast(ray, out hit, dist, LayerMask.GetMask("BotOrganism")))
        //        {
        //            BotCollider collider = hit.transform.GetComponent<BotCollider>();
        //            if (collider != null)
        //            {
        //                targetPosition.x += ray.origin.x - hit.point.x;
        //                MergeOrganisms(collider.ParentCell.Organism);
        //            }
        //        }
        //    }
        //}
        if (_targetMoveVector.y < 0)
        {
           // neighbour |= Neighbour.Down;
        }
        if (_targetMoveVector.y > 0)
        {
           // neighbour |= Neighbour.Up;
        }


        



        transform.position = targetPosition;




        
    }

    // Jump? That would allow it to climb up? Maybe it just needs to touch. (probably just touch)
    // Gravity
    // CanMove




    // TODO VFX
    private void MergeOrganisms(BotOrganism absorb)
    {
        if (absorb == this)
        {
            Debug.LogError("Attempting to absorb itself.");
            return;
        }

        foreach (BotCell cell in absorb.BotCells)
        {
            cell.transform.SetParent(CellContainer, true);
        }
        InitCells();
        Destroy(absorb.gameObject);
    }


    public void Selected(bool tf)
    {
        foreach (BotCell cell in BotCells)
        {
            cell.SelectedOrganism(tf);
        }
    }











    // TODO check edges when moving up. The organism may need to center
    private bool CanMove(Vector2 dir)
    {
        Neighbour neighbour = NeighbourUtil.GetNeighbourByOffset(dir);


        // Check each cell
        // For each face without another cell
        BotCell[] cells = GetCellsFacingDirection(neighbour);

        foreach (BotCell cell in cells)
        {
            // Raycast in the direction we're going

            // If something's there, return false

            Ray ray = new Ray(cell.transform.position, dir);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 1f, LayerMask.GetMask("Terrain"))) 
            {
                return false;
            }
        }

        // If none found a blocker, return true
        return true;
    }

    private BotCell[] GetCellsFacingDirection(Neighbour dir)
    {
        _cellsFacingDirection.Clear();

        foreach (BotCell cell in BotCells)
        {
            if (!cell.HasNeighbour(dir))
            {
                _cellsFacingDirection.Add(cell);
            }
        }

        return _cellsFacingDirection.ToArray();
    }


    //private void FixedUpdate()
    //{
    //    _rigidbody.velocity = _targetMoveSpeed;
    //}

    public void Move(Vector2 v)
    {
        _targetMoveVector = v;
    }
}
