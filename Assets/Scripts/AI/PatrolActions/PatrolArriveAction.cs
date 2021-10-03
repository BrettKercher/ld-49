using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatrolArriveAction : ScriptableObject {
    
    public delegate void ActionCompleteHandler(PatrolArriveAction dispatcher);
    public event ActionCompleteHandler ActionComplete;
    
    public abstract IEnumerator Execute(GameObject target);

    internal void OnActionComplete() {
        ActionComplete?.Invoke(this);
    }

    public void SubscribeToCompleteEvent(ActionCompleteHandler onCompleteFunction) {
        ActionComplete += onCompleteFunction;
    }
    
    public void UnsubscribeFromCompleteEvent(ActionCompleteHandler onCompleteFunction) {
        ActionComplete -= onCompleteFunction;
    }
}