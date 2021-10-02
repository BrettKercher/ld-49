using UnityEngine;

public class VisionRangeVisualizer : MonoBehaviour {
    [SerializeField] private bool _enabled;
    [SerializeField] private VisionComponent _visionComponent;

    private GlobalVisionManager _visionManager;

    public void SetEnabled(bool value) {
        _enabled = value;
    }

    private void Awake() {
        _visionManager = GameObject.Find("GameManager").GetComponent<GlobalVisionManager>();
    }

    private void Update() {
        if (_enabled) {
            var visibleNodes = _visionManager.CalculateVisibleNodesInRange(_visionComponent.VisionRange, transform.position, _visionComponent.VisionAngle, transform.up);
            _visionManager.UpdateVisionList(visibleNodes);
        }
    }
}