using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for the home screen UI
/// </summary>
public class HomeScreenController : MonoBehaviour
{
    [SerializeField] private InventoryTabManager _inventoryTabManager;
    [SerializeField] private Button _feedTabButton;
    [SerializeField] private Button _playTabButton;
    [SerializeField] private Button _groomTabButton;

    private void Start()
    {
        _feedTabButton.onClick.AddListener(OpenInventoryWithFeed);
        _playTabButton.onClick.AddListener(OpenInventoryWithPlay);
        _groomTabButton.onClick.AddListener(OpenInventoryWithGroom);
    }

    private void OpenInventoryWithFeed()
    {
        _inventoryTabManager.SetTabState(InventoryTabManager.InventoryTabState.Feed);
    }

    private void OpenInventoryWithPlay()
    {
        _inventoryTabManager.SetTabState(InventoryTabManager.InventoryTabState.Play);
    }

    private void OpenInventoryWithGroom()
    {
        _inventoryTabManager.SetTabState(InventoryTabManager.InventoryTabState.Groom);
    }

}
