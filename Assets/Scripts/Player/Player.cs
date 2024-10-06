using Oneill;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Player : MonoBehaviour
{
    public IPlayerMode PlayerMode;

    public Transform PlayerUnitsContainer;

    public Scene Scene;

    private BotOrganism _selectedOrganism;
    private BotCell _selectedCell;

    private Camera _camera;


    enum SelectionMode
    {
        None, 
        Cell,
        Organism
    }
    private SelectionMode _selectionMode;

    public void Init(Scene scene)
    {
        Scene = scene;
        _camera = Camera.main;
    }

    private void Start()
    {
        Scene.InputManager.MoveEvent += OnMove;
        Scene.InputManager.SelectEvent += OnSelect;
        Scene.InputManager.MergeEvent += OnMerge;
    }

    private void Update()
    {
        if (TimeKeeper.Instance.IsPaused)
        {
            return;
        }




    }

    private void OnMove(Vector2 v)
    {
        if (_selectionMode == SelectionMode.Cell)
        {
            // Deselect the previous organism
            _selectedOrganism?.Selected(false);

            // Disconnect 
            // Reconnect with neighbours in move direction
            _selectedOrganism = Scene.BotManager.DisconnectCells(_selectedCell, NeighbourUtil.GetBestNeighbourByOffset(v));

            // Select new organism
            _selectionMode = SelectionMode.Organism;
            _selectedOrganism.Selected(true);

            // Finally, move the new organism
            _selectedOrganism.Move(v);
        }
        else if (_selectedOrganism != null)
        {
            _selectedOrganism.Move(v);
        }
    }
    private void OnSelect()
    {
        // Do raycast for worm or cell


        // Reset selection
        bool somethingSelected = false;

        Ray ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("BotOrganism")))
        {
            BotDisconnectCollider disconnect = hit.transform.GetComponent<BotDisconnectCollider>();

            if (disconnect != null)
            {
                //disconnect.Cell.Organism.DisconnectCells(disconnect.Cell, disconnect.Neighbour);
            }

            else
            {

                //BotCollider collider = hit.transform.GetComponent<BotCollider>();
                //if (collider != null)
                //{

                //    if (!collider.ParentCell.Selectable)
                //    {
                //        Debug.Log("Clicked on unselectable bot");
                //        _selectedOrganism?.Selected(false);
                //        somethingSelected = true;
                //        collider.ParentCell.Organism.Selected(true);
                //        _selectedOrganism = collider.ParentCell.Organism;
                //        _selectedCell = null;
                //        _selectionMode = SelectionMode.Organism;
                //    }
                //    else if (
                //        (_selectionMode == SelectionMode.None || _selectionMode == SelectionMode.Organism)
                //     || (_selectionMode == SelectionMode.Cell && collider.ParentCell != _selectedCell))
                //    {
                //        // Do cell
                //        _selectedOrganism?.Selected(false);
                //        somethingSelected = true;
                //        collider.ParentCell.Organism.Selected(false);
                //        collider.ParentCell.SelectedCell(true);
                //        _selectedCell = collider.ParentCell;
                //        _selectedOrganism = collider.ParentCell.Organism;
                //        _selectionMode = SelectionMode.Cell;
                //    }
                //    else if (_selectionMode == SelectionMode.Cell)
                //    {
                //        // Do organism
                //        _selectedOrganism?.Selected(false);
                //        somethingSelected = true;
                //        collider.ParentCell.Organism.Selected(true);
                //        _selectedOrganism = collider.ParentCell.Organism;
                //        _selectedCell = null;
                //        _selectionMode = SelectionMode.Organism;
                //    }

                //}
                BotCell collider = hit.transform.GetComponent<BotCell>();
                if (collider != null)
                {

                    if (!collider.Selectable)
                    {
                        //Debug.Log("Clicked on unselectable bot");
                        _selectedOrganism?.Selected(false);
                        somethingSelected = true;
                        collider.Organism.Selected(true);
                        _selectedOrganism = collider.Organism;
                        _selectedCell = null;
                        _selectionMode = SelectionMode.Organism;
                    }
                    else if (
                        (_selectionMode == SelectionMode.None || _selectionMode == SelectionMode.Organism)
                     || (_selectionMode == SelectionMode.Cell && collider != _selectedCell))
                    {
                        // Do cell
                        _selectedOrganism?.Selected(false);
                        somethingSelected = true;
                        collider.Organism.Selected(false);
                        collider.SelectedCell(true);
                        _selectedCell = collider;
                        _selectedOrganism = collider.Organism;
                        _selectionMode = SelectionMode.Cell;
                    }
                    else if (_selectionMode == SelectionMode.Cell)
                    {
                        // Do organism
                        _selectedOrganism?.Selected(false);
                        somethingSelected = true;
                        collider.Organism.Selected(true);
                        _selectedOrganism = collider.Organism;
                        _selectedCell = null;
                        _selectionMode = SelectionMode.Organism;
                    }

                }
            }
        }

        if (!somethingSelected)
        {
            ClearSelection();
        }
    }

    private void OnMerge()
    {
        if (_selectedOrganism != null)
        {
            _selectedOrganism.MergeSurrounding();
            _selectionMode = SelectionMode.Organism;
            _selectedOrganism.Selected(true);
        }
        else
        {
            //TODO Give feedback saying there's nothing to merge
        }
    }



    private void ClearSelection()
    {
        _selectedOrganism?.Selected(false);
        _selectedCell = null;
        _selectedOrganism = null;
        _selectionMode = SelectionMode.None;
    }








    //public void SetMode(IPlayerMode newMode)
    //{
    //    PlayerMode?.Exit();
    //    PlayerMode = newMode;
    //    PlayerMode?.Enter();
    //}

    //private void Update()
    //{
    //    PlayerMode?.Update();
    //}
}
