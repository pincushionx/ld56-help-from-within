using UnityEngine;

public class TumorCollider : MonoBehaviour
{
    public Tumor ParentTumor {get; private set;}

    private void Awake()
    {
        ParentTumor = GetComponentInParent<Tumor>(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        BotCollider cell = other.gameObject.GetComponent<BotCollider>();
        if (cell != null)
        {
            ParentTumor.SetCured(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        BotCollider cell = other.gameObject.GetComponent<BotCollider>();
        if (cell != null)
        {
            ParentTumor.SetCured(false);
        }
    }
}
