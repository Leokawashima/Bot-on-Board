using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class GameObserver : MonoBehaviour
{
     private void Awake() 
    {
        GlobalMember.ResetProgress();
    }
}
