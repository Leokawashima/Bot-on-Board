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

    private void Start()
    {
        DeckList.Initialize();
        DeckEdit.Initialize();
    }
}