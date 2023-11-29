using System;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [field: SerializeField] public DeckListManager DeckList { get; private set; }
    [field: SerializeField] public DeckEditManager DeckEdit { get; private set; }
    
    [Flags]
    public enum DeckState
    {
        Non = 0,
        isRarilyLimit = 1 << 0,
        isSameBan = 1 << 1,
    }

    public static event Action
        Event_Initialize,
        Event_Finalize;

    private void Start()
    {
        Event_Initialize?.Invoke();
    }
    private void OnDestroy()
    {
        Event_Finalize?.Invoke();
    }
}