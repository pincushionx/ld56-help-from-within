using Oneill;
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
        }

        oldOrganism.InitCells();
        newOrganism.InitCells();

        // Cleanup
        if (oldOrganism.BotCells.Length == 0)
        {
            Destroy(oldOrganism.gameObject);
        }

        return newOrganism;
    }
}
