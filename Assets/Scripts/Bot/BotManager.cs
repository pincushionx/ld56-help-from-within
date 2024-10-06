using Oneill;
using System.Collections.Generic;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    public Scene Scene {get; private set;}
    public Transform OrganismContainer;

    public GameObject BotOrganismPrefab;

    public void Init(Scene scene)
    {
        Scene = scene;
        InitOrganisms();
    }

    private void InitOrganisms()
    {
        BotOrganism[] orgs = OrganismContainer.GetComponentsInChildren<BotOrganism>();

        foreach (BotOrganism org in orgs)
        {
            org.Init(Scene);
        }
    }

    public BotOrganism DisconnectCells(BotCell disconnectCell, Neighbour neighbour)
    {
        // Create a new organism
        GameObject go = Instantiate(BotOrganismPrefab);
        BotOrganism newOrganism = go.GetComponent<BotOrganism>();
        newOrganism.transform.SetParent(OrganismContainer, true);

        BotOrganism oldOrganism = disconnectCell.Organism;

        // Move the cells to the new organism
        BotCell currentCell = disconnectCell;
        while (currentCell != null)
        {
            currentCell.transform.SetParent(newOrganism.CellContainer, true);
            currentCell = currentCell.GetNeighbour(neighbour);

            if(currentCell != null && !currentCell.Selectable)
            {
                // Can't move selectable cell like this?
                currentCell = null;
            }
        }

        oldOrganism.InitCells();
        newOrganism.InitCells();

        // Cleanup
        if (oldOrganism.BotCells.Length == 0)
        {
            Destroy(oldOrganism.gameObject);
        }
        else
        {
            SplitOrganismIfNecessary(oldOrganism);
        }

        return newOrganism;
    }

    private void SplitOrganismIfNecessary(BotOrganism o)
    {
        List<BotCell> cluster = new List<BotCell>();

        BotCell currentCell = o.BotCells[0];
        GetCluster(currentCell, cluster);

        if (cluster.Count < o.BotCells.Length)
        {
            Debug.Log("Needed additional organism split ");

            // There's a split

            // Find the cells that aren't in the cluster and assign them to a new one
            // Create a new organism
            GameObject go = Instantiate(BotOrganismPrefab);
            BotOrganism newOrganism = go.GetComponent<BotOrganism>();
            newOrganism.transform.SetParent(OrganismContainer, true);

            foreach (BotCell cell in o.BotCells)
            {
                if (!cluster.Contains(cell))
                {
                    cell.transform.SetParent(newOrganism.CellContainer, true);
                }
            }

            o.InitCells();
            newOrganism.InitCells();
        }
    }

    private void GetCluster(BotCell currentCell, List<BotCell> cluster)
    {
        if (currentCell != null && !cluster.Contains(currentCell))
        {
            cluster.Add(currentCell);
            foreach (BotCell neighbour in currentCell.NeighbourCells)
            {
                if (neighbour != null)
                {
                    GetCluster(neighbour, cluster);
                }
            }
        }
    }
}
