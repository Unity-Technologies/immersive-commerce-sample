using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// MonoBehaviour for an inventory item
/// </summary>
public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Button _useButton;
    [SerializeField] private CatState _itemType;
    [SerializeField] private UnityEvent<CatState> _onItemUsed;

    private void Start()
    {
        _useButton.onClick.AddListener(UseItem);
        _onItemUsed.AddListener(CatStateMachine.instance.TransitionToState);
    }

    private void UseItem()
    {
        _onItemUsed?.Invoke(_itemType);
    }
}
