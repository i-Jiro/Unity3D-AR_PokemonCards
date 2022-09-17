using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TrackablesManager : MonoBehaviour
{
    public static TrackablesManager Instance;
    
    private StateManager _stateManager;
    private IEnumerable<TrackableBehaviour> _trackables;
    private bool _powerUpEventTriggered = false;

    public delegate void FoundPowerUpCardEventHandler(GameObject gameObject);
    public event FoundPowerUpCardEventHandler FoundPowerUp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _stateManager = TrackerManager.Instance.GetStateManager();
        _trackables = _stateManager.GetTrackableBehaviours();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var trackable in _trackables)
        {
            if (trackable.TrackableName == "SpiralEnergy")
            {
                if (trackable.CurrentStatus == TrackableBehaviour.Status.TRACKED && !_powerUpEventTriggered)
                {
                    OnFoundPowerUp(trackable.gameObject);
                }
            }
        }
    }

    protected virtual void OnFoundPowerUp(GameObject gameObject)
    {
        FoundPowerUp?.Invoke(gameObject);
        _powerUpEventTriggered = true;
    }
}
