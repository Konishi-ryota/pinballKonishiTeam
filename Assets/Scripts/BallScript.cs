using UnityEngine;

public class BallScript : MonoBehaviour
{
    private BallManager _ballManager;

    private void Awake()
    {
        _ballManager = GameObject.FindObjectOfType<BallManager>();
    }

    private void OnDestroy()
    {
        if (_ballManager != null)
        {
            _ballManager.DecreaseBallCount();
        }
    }
}
