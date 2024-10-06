using Oneill;
using System.Collections.Generic;
using UnityEngine;

public class BotOrganism : MonoBehaviour
{

    public Scene scene {get; private set; }

    public Transform CellContainer;
    public BotCell[] BotCells { get; private set; }


    private float _moveSpeedModifier = 5.0f;
    private Vector2 _moveVector = Vector2.zero;


    private float _colliderBuffer = 0.05f;

    private bool _animatingMovement = false;
    private float _animatingMovementStartTime = -1f;
    private Vector3 _animatingMovementOrigin = Vector3.zero;
    private Vector3 _animatingMovementTarget = Vector3.zero;


    private Neighbour _climbUpDirection = Neighbour.None;

    // For reuse
    List<BotCell> _cellsFacingDirection = new List<BotCell>();

    //private Rigidbody _rigidbody;

    
    public void Init(Scene scene)
    {
        this.scene = scene;
        InitCells();
    }

    public void InitCells()
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


                if (diff.y == 0)
                {
                    // Connect left
                    if (diff.x < 0)
                    {
                        if (diff.x >= -1.5f)
                        {
                            neighbours[NeighbourUtil.GetNeighbourIndex(Neighbour.Left)] = compareCell;
                        }
                    }
                    // Connect right
                    else if (diff.x > 0)
                    {
                        if (diff.x <= 1.5f)
                        {
                            neighbours[NeighbourUtil.GetNeighbourIndex(Neighbour.Right)] = compareCell;
                        }
                    }
                }

                if (diff.x == 0)
                {
                    // Connect down
                    if (diff.y < 0)
                    {
                        if (diff.y >= -1.5f)
                        {
                            neighbours[NeighbourUtil.GetNeighbourIndex(Neighbour.Down)] = compareCell;
                        }
                    }
                    // Connect up
                    else if (diff.y > 0)
                    {
                        if (diff.y <= 1.5f)
                        {
                            neighbours[NeighbourUtil.GetNeighbourIndex(Neighbour.Up)] = compareCell;
                        }
                    }
                }
            }

            cell.Init(this, neighbours);
        }
    }

    private void Update()
    {
        if (TimeKeeper.Instance.IsPaused)
        {
            return;
        }


        if (_animatingMovement)
        {
            transform.position = Vector3.Lerp(_animatingMovementOrigin, _animatingMovementTarget, TimeKeeper.Instance.PlayTimeSince(_animatingMovementStartTime) * _moveSpeedModifier);

            if (Vector3.Distance(transform.position, _animatingMovementTarget) < 0.001f)
            {
                _animatingMovement = false;
            }

            return;
        }












        Vector3 diff = _moveVector * _moveSpeedModifier * Time.deltaTime;

        Vector3 targetPosition = transform.position + diff;



        //Neighbour neighbourMask = NeighbourUtil.GetNeighbourByOffset(diff);
        //Neighbour[] neighbours = NeighbourUtil.GetNeighboursFromMask(neighbourMask);

        if (_moveVector.x != 0)
        {
            float sign = (_moveVector.x < 0) ? -1 : 1;
            float dist = diff.x * sign;
            Neighbour neighbour = sign < 0 ? Neighbour.Left : Neighbour.Right;

            bool hasXDiff = false;
            float xDiff = float.MaxValue;


            BotCell[] cells = GetCellsFacingDirection(neighbour);
            foreach (BotCell cell in cells)
            {
                BotCell.RaycastResult raycastResult = cell.Raycast(neighbour, Mathf.Abs(dist));

                if (raycastResult.hit)
                {
                    Ray ray = raycastResult.ray;
                    RaycastHit hit = raycastResult.NearestHit;
                    float newxDiff = (sign * ray.origin.x) - (sign * hit.point.x) - (sign * _colliderBuffer);
                    if (Mathf.Abs(newxDiff) < Mathf.Abs(xDiff))
                    {
                        xDiff = newxDiff;
                        hasXDiff = true;
                    }
                }
            }

            if (hasXDiff)
            {
                targetPosition.x += xDiff;
            }
        }
        else
        {
            // Center on grid if there's no motion
            Vector3 pos = targetPosition;
            pos.x = Mathf.Round(pos.x);
            targetPosition = pos;
        }



        Neighbour lrCollision = GetCollisionInDirection(Neighbour.LeftRight, 0.5f);



        // check if falling
        if (_moveVector.y == 0 && lrCollision == Neighbour.None && GetCollisionInDirection(Neighbour.Down, 0.5f) == Neighbour.None)
        {
            _animatingMovement = true;
            _animatingMovementOrigin = transform.position;
            _animatingMovementTarget = _animatingMovementOrigin;
            _animatingMovementTarget.y -= 1;
            _animatingMovementTarget.x = Mathf.Round(_animatingMovementTarget.x);
            _animatingMovementTarget.y = Mathf.Round(_animatingMovementTarget.y);
            _animatingMovementStartTime = TimeKeeper.Instance.PlayTimeElapsed;
        }

        // Y movement is for climbing. It requires collision left or right
        // Can climb if 
        // 1. There's room above
        // 2. There's a wall to climb on
        else if (_moveVector.y != 0)
        {
            // complete a climb
            if (_moveVector.y > 0f
            && _climbUpDirection != Neighbour.None
            && lrCollision == Neighbour.None)
            {
                _animatingMovement = true;
                _animatingMovementOrigin = transform.position;
                _animatingMovementTarget = _animatingMovementOrigin;


                if (_moveVector.x != 0)
                {
                    _animatingMovementTarget.x += _moveVector.x > 0 ? 1 : -1;
                }
                else
                {
                    _animatingMovementTarget.x += (_climbUpDirection == Neighbour.Right) ? 1 : -1;
                }


                _animatingMovementTarget.x = Mathf.Round(_animatingMovementTarget.x);
                _animatingMovementTarget.y = Mathf.Round(_animatingMovementTarget.y);
                _animatingMovementStartTime = TimeKeeper.Instance.PlayTimeElapsed;
            }
            else if (lrCollision != Neighbour.None)
            {
                float sign = (_moveVector.y < 0) ? -1 : 1;
                float dist = diff.y * sign;
                Neighbour neighbour = sign < 0 ? Neighbour.Down : Neighbour.Up;

                bool hasYDiff = false;
                float yDiff = float.MaxValue;


                BotCell[] cells = GetCellsFacingDirection(neighbour);
                foreach (BotCell cell in cells)
                {
                    BotCell.RaycastResult raycastResult = cell.Raycast(neighbour, dist);

                    if (raycastResult.hit)
                    {
                        Ray ray = raycastResult.ray;
                        RaycastHit hit = raycastResult.NearestHit;

                        float newyDiff = (sign * ray.origin.y) - (sign * hit.point.y) - (sign * _colliderBuffer);
                        if (Mathf.Abs(newyDiff) < Mathf.Abs(yDiff))
                        {
                            yDiff = newyDiff;
                            hasYDiff = true;
                        }
                    }
                }

                if (hasYDiff)
                {
                    targetPosition.y += yDiff;
                }
            }
            else
            {
                // reset to current position
                targetPosition.y = transform.position.y;
            }
        }
        else
        {
            // Center on grid if there's no motion
            Vector3 pos = targetPosition;
            pos.y = Mathf.Round(pos.y);
            targetPosition = pos;
        }




        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * _moveSpeedModifier);
        _climbUpDirection = lrCollision;




    }


    public void MergeSurrounding()
    {
        MergeSurrounding(Neighbour.Up);
        MergeSurrounding(Neighbour.Down);
        MergeSurrounding(Neighbour.Left);
        MergeSurrounding(Neighbour.Right);
    }
    private void MergeSurrounding(Neighbour neighbour)
    {
        BotCell[] cells = GetCellsFacingDirection(neighbour);
        foreach (BotCell cell in cells)
        {
            Transform raycastTransform = cell.GetRaycastCenterTransform(neighbour);

            Ray ray = new Ray(raycastTransform.position, raycastTransform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 0.5f, LayerMask.GetMask("BotOrganism")))
            {
                BotCollider collider = hit.transform.GetComponent<BotCollider>();
                if (collider != null)
                {
                    MergeOrganisms(collider.ParentCell.Organism);

                    // Reselect this organism
                    Selected(true);
                }
            }
        }
    }


    // TODO VFX
    private void MergeOrganisms(BotOrganism absorb)
    {
        if (absorb == this)
        {
            Debug.Log("Attempting to absorb itself.");
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







    //private bool HasDiagonalCollision(Neighbour neighbourM)
    //{
    //    Neighbour[] neighbours = NeighbourUtil.GetNeighboursFromMask(Neighbour.LeftRight);
    //    foreach (Neighbour neighbour in neighbours)
    //    {
    //        BotCell[] cells = GetCellsFacingDirection(neighbour);

    //        foreach (BotCell cell in cells)
    //        {
    //            // Raycast in the direction we're going

    //            // If something's there, return false
    //            Transform raycastTransform = cell.GetRaycastTransform(neighbour);
    //            Ray ray = new Ray(raycastTransform.position, raycastTransform.forward);
    //            RaycastHit hit = new RaycastHit();
    //            if (Physics.Raycast(ray, out hit, distance))
    //            {
    //                return true;
    //            }
    //        }
    //    }

    //    // If none found a blocker, return true
    //    return false;
    //}


    // Accepts a mask of neighbours
    // TODO check edges when moving up. The organism may need to center
    private bool HasCollisionInDirection(Neighbour neighbourMask)
    {
        Neighbour mask = GetCollisionInDirection(neighbourMask, _colliderBuffer);
        return mask != Neighbour.None;
    }

    private Neighbour GetCollisionInDirection(Neighbour neighbourMask, float distance)
    {
        Neighbour outMask = Neighbour.None;

        // Check each cell
        // For each face without another cell\

        Neighbour[] neighbours = NeighbourUtil.GetNeighboursFromMask(neighbourMask);
        foreach (Neighbour neighbour in neighbours)
        {
            BotCell[] cells = GetCellsFacingDirection(neighbour);

            foreach (BotCell cell in cells)
            {
                // Raycast in the direction we're going
                BotCell.RaycastResult raycastResult = cell.Raycast(neighbour);

                if (raycastResult.hit)
                {
                    outMask |= neighbour;
                }
            }
        }

        // If none found a blocker, return true
        return outMask;
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
        _moveVector = v;
    }
}
