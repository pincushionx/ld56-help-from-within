using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCollider : MonoBehaviour
{
    public BotCell ParentCell { get; private set; }

    private void Awake()
    {
        ParentCell = GetComponentInParent<BotCell>(true);
    }
}
