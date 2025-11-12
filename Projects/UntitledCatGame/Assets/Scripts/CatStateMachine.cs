using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The different states the cat can be in
/// </summary>
public enum CatState
{
    Idle,
    Eat,
    Play,
    Groom
}

/// <summary>
/// The available stats for the cat
/// </summary>
public enum StatType
{
    Health,
    Happiness,
    Tidiness
}

/// <summary>
/// State machine that controls the cat
/// </summary>
public class CatStateMachine : MonoBehaviour
{
    [SerializeField] private GameObject _idleGameObject;
    [SerializeField] private GameObject _eatGameObject;
    [SerializeField] private GameObject _playGameObject;
    [SerializeField] private GameObject _groomGameObject;
    [SerializeField] private CatState _currentState;
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _happiness = 100f;
    [SerializeField] private float _tidiness = 100f;
    [SerializeField] private float _decayRate = 10f;
    [SerializeField] private float _recoveryRate = 40f;

    [SerializeField] private Image _healthBarFillImage;
    [SerializeField] private Image happinessBarFillImage;
    [SerializeField] private Image _tidinessBarFillImage;

    [SerializeField] private GameObject _happyFace;
    [SerializeField] private GameObject _sadFace;
    
    public static CatStateMachine instance;

    private const float HAPPINESS_THRESHOLD = 60f;

    private void Awake()
    {
        instance = this;
        TransitionToState(CatState.Idle);
    }

    private void Update()
    {
        if (_health <= HAPPINESS_THRESHOLD || _happiness <= HAPPINESS_THRESHOLD || _tidiness <= HAPPINESS_THRESHOLD)
        {
            _happyFace.SetActive(false);
            _sadFace.SetActive(true);
        }
        else
        {
            _happyFace.SetActive(true);
            _sadFace.SetActive(false);
        }

        switch (_currentState)
        {
            case CatState.Idle:
                HandleIdleState();
                break;
            case CatState.Eat:
                HandleEatState();
                break;
            case CatState.Play:
                HandlePlayState();
                break;
            case CatState.Groom:
                HandleGroomState();
                break;
            default:
                Debug.LogWarning($"{_currentState} unhandled.");
                break;
        }
    }
    
    /// <summary>
    /// Function to handle the state transitions
    /// </summary>
    public void TransitionToState(CatState newState)
    {
        _currentState = newState;

        switch (newState)
        {
            case CatState.Idle:
                _idleGameObject.SetActive(true);
                _eatGameObject.SetActive(false);
                _playGameObject.SetActive(false);
                _groomGameObject.SetActive(false);
                break;
            case CatState.Eat:
                _idleGameObject.SetActive(false);
                _eatGameObject.SetActive(true);
                _playGameObject.SetActive(false);
                _groomGameObject.SetActive(false);
                break;
            case CatState.Play:
                _idleGameObject.SetActive(false);
                _eatGameObject.SetActive(false);
                _playGameObject.SetActive(true);
                _groomGameObject.SetActive(false);
                break;
            case CatState.Groom:
                _idleGameObject.SetActive(false);
                _eatGameObject.SetActive(false);
                _playGameObject.SetActive(false);
                _groomGameObject.SetActive(true);
                break;
        }
    }
    
    /// <summary>
    /// Handle stats changing for different states, and resume to idle when stat full
    /// </summary>
    private void HandleIdleState()
    {
        //all stats decay
        UpdateStat(StatType.Health, -_decayRate * Time.deltaTime);
        UpdateStat(StatType.Happiness, -_decayRate * Time.deltaTime);
        UpdateStat(StatType.Tidiness, -_decayRate * Time.deltaTime);
    }

    private void HandleEatState()
    {
        //all stats decay except health
        UpdateStat(StatType.Health, _recoveryRate * Time.deltaTime);
        UpdateStat(StatType.Happiness, -_decayRate * Time.deltaTime);
        UpdateStat(StatType.Tidiness, -_decayRate * Time.deltaTime);

        if (_health >= 100f)
        {
            TransitionToState(CatState.Idle);
        }
    }

    private void HandlePlayState()
    {
        //all stats decay except happiness
        UpdateStat(StatType.Health, -_decayRate * Time.deltaTime);
        UpdateStat(StatType.Happiness, _recoveryRate * Time.deltaTime);
        UpdateStat(StatType.Tidiness, -_decayRate * Time.deltaTime);

        if (_happiness >= 100f)
        {
            TransitionToState(CatState.Idle);
        }
    }

    private void HandleGroomState()
    {
        //all stats decay except tidiness
        UpdateStat(StatType.Health, -_decayRate * Time.deltaTime);
        UpdateStat(StatType.Happiness, -_decayRate * Time.deltaTime);
        UpdateStat(StatType.Tidiness, _recoveryRate * Time.deltaTime);

        if (_tidiness >= 100f)
        {
            TransitionToState(CatState.Idle);
        }
    }
    
    /// <summary>
    /// Handles how stats are updated in each frame, and how the fill bar behaves
    /// </summary>
    private void UpdateStat(StatType statType, float changeAmount)
    {
        switch (statType)
        {
            case StatType.Health:
                _health += changeAmount;
                _health = Mathf.Clamp(_health, 0f, 100f);
                _healthBarFillImage.fillAmount = _health / 100f;
                break;
            case StatType.Happiness:
                _happiness += changeAmount;
                _happiness = Mathf.Clamp(_happiness, 0f, 100f);
                happinessBarFillImage.fillAmount = _happiness / 100f;
                break;
            case StatType.Tidiness:
                _tidiness += changeAmount;
                _tidiness = Mathf.Clamp(_tidiness, 0f, 100f);
                _tidinessBarFillImage.fillAmount = _tidiness / 100f;
                break;
            default:
                Debug.LogError("Invalid stat type:" + statType);
                break;
        }
    }
}
