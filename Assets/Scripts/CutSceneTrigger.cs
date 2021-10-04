using UnityEngine;

public class CutSceneTrigger : MonoBehaviour {

    [SerializeField] private GameObject _panTarget;
    [SerializeField] private float _panSpeed;
    [SerializeField] private float _holdTime;

    [SerializeField] private StoryText _storyTextObject;
    [SerializeField] [TextArea(1,20)] private string _text;
    [SerializeField] private float _textDuration;

    private MainCamera _mainCamera;
    private vroom_vroom _move;

    private void Awake() {
        _mainCamera = Camera.main.GetComponent<MainCamera>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        _move = other.gameObject.GetComponent<vroom_vroom>();
        if (_move == null) {
            Debug.Log("null");
            Debug.Log(other.name);
        }
        else {
            _move.Pause = true;
        }
        
        _mainCamera.SubscribeToArrivedEvents(OnPanToTargetFinished, OnPanBackFinished);
        _mainCamera.PanToPositionAndBack(_panTarget.transform.position, other.gameObject, _holdTime, _panSpeed);
    }
    
    private void OnPanToTargetFinished(MainCamera dispatcher) {
        dispatcher.UnsubscribeFromArrivedEvents(OnPanToTargetFinished, OnPanBackFinished);
        StartCoroutine(_storyTextObject.ShowText(_text, _textDuration));
        _move.Pause = false;
        Destroy(gameObject);
    }

    private void OnPanBackFinished(MainCamera dispatcher) {
    }
}