using System;
using UnityEngine;

public class TumorManager : MonoBehaviour
{
    public event Action CureEvent;

    public Scene Scene {get; private set;}

    public Transform TumorContainer;
    public Tumor[] Tumors { get; private set; }

    public void Init(Scene scene)
    {
        Scene = scene;
        Tumors = TumorContainer.GetComponentsInChildren<Tumor>();

        foreach (var tumor in Tumors)
        {
            tumor.Init(this);
            tumor.CureEvent += Tumor_CureEvent;
        }
    }

    private void Tumor_CureEvent(Tumor obj)
    {
        // notify UI of change?
        CureEvent?.Invoke();
    }

    public int NumTumorsRemaining()
    {
        int remaining = 0;

        for (int i = 0; i < Tumors.Length; i++)
        {
            if (!Tumors[i].IsCured)
            {
                remaining++;
            }
        }

        return remaining;
    }
}
