using TMPro;
using UnityEngine;

namespace CasualtiesOnOurWayToParadise;


public class AscentTimer : MonoBehaviour
{
    public static AscentTimer Instance;
    private float _timeLeft;
    private TextMeshProUGUI _timerUi;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        var mainViewTransform = PlayerCamera.main.mainCanvas.transform.Find("MainView");
        var gb = GameObject.Instantiate(mainViewTransform.Find("CurrentlyHolding").gameObject, mainViewTransform);
        gb.name = "TimeLeftText";
        _timerUi = gb.GetComponent<TextMeshProUGUI>();
        _timerUi.rectTransform.localScale = new Vector3(2f, 2f, 1f);
        var width = _timerUi.rectTransform.parent.GetComponent<RectTransform>().rect.size.x;
        var height = _timerUi.rectTransform.parent.GetComponent<RectTransform>().rect.size.x;
        _timerUi.rectTransform.anchoredPosition = new Vector2(width * 0.5f, _timerUi.rectTransform.anchoredPosition.y);
        _timerUi.color = new Color(1f, 0.1f, 0.1f, 0.8f);
    }

    private void Start()
    {
        _timeLeft = 10f * 60f;
    }

    private void Update()
    {
        var world = WorldGeneration.world;
        if (!world)
        {
            Destroy(this);
            return;
        }

        if (world.generatingWorld || world.instantiatingWorld || world.doingRegen)
            return;

        if (world.cutsceneActive)
        {
            Destroy(this);
            return;
        }

        _timeLeft -= Time.unscaledDeltaTime;

        if (_timeLeft < 0)
        {
            PlayerCamera.main.body.brainHealth = 0.0f;
            _timerUi.enabled = false;
            return;
        }

        int minutes = (int) (_timeLeft / 60f);
        int seconds = (int)(_timeLeft - minutes * 60f);
        _timerUi.SetText($"{minutes:D2}:{seconds:D2}");
    }

    private void OnDestroy()
    {
        if (this != Instance)
            return;
        Instance = null;
        Destroy(_timerUi);
    }
}