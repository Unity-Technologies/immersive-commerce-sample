using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manager for the inventory tabs
/// </summary>
public class InventoryTabManager : MonoBehaviour
{
    public enum InventoryTabState
    {
        Feed,
        Play,
        Groom
    }
    
    [SerializeField] private Button _feedTabButton;
    [SerializeField] private Button _playTabButton;
    [SerializeField] private Button _groomTabButton;

    [SerializeField] private GameObject _feedTabSelected;
    [SerializeField] private GameObject _playTabSelected;
    [SerializeField] private GameObject _groomTabSelected;

    [SerializeField] private GameObject _feedTab;
    [SerializeField] private GameObject _playTab;
    [SerializeField] private GameObject _groomTab;
        
    private InventoryTabState _currentState;
    
    private void Start()
    {
        _feedTabButton.onClick.AddListener(() => SetTabState(InventoryTabState.Feed));
        _playTabButton.onClick.AddListener(() => SetTabState(InventoryTabState.Play));
        _groomTabButton.onClick.AddListener(() => SetTabState(InventoryTabState.Groom));

        _currentState = InventoryTabState.Feed; // Set initial state
        UpdateTabDisplay();
    }

    public void SetTabState(InventoryTabState newState)
    {
        _currentState = newState;
        UpdateTabDisplay();
    }

    private void UpdateTabDisplay()
    {
        _feedTabSelected.SetActive(_currentState == InventoryTabState.Feed);
        _feedTab.SetActive(_currentState == InventoryTabState.Feed);

        _playTabSelected.SetActive(_currentState == InventoryTabState.Play);
        _playTab.SetActive(_currentState == InventoryTabState.Play);

        _groomTabSelected.SetActive(_currentState == InventoryTabState.Groom);
        _groomTab.SetActive(_currentState == InventoryTabState.Groom);
    }
}
