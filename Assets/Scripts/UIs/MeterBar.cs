using UnityEngine;
using UnityEngine.UI;

public class MeterBar : MonoBehaviour
{
    [SerializeField] private Image fill;

    [Header("Low warning")]
    [SerializeField] private Color lowColor = new Color(0.85f, 0.2f, 0.2f);
    [SerializeField, Range(0f, 1f)] private float lowThreshold = 0.25f;
    [SerializeField] private bool pulseWhenLow = true;
    [SerializeField] private float pulseSpeed = 6f;

    [SerializeField] private float smoothSpeed = 2f;

    private Color normalColor;
    private float target = 1f;

    private void Awake()
    {
        normalColor = fill.color;
        target = fill.fillAmount;
    }

    public void SetValue(float current, float max)
    {
        target = max > 0f ? Mathf.Clamp01(current / max) : 0f;
    }

    public void SetInstantValue(float current, float max)
    {
        SetValue(current, max);
        fill.fillAmount = target;
    }

    // Update is called once per frame
    void Update()
    {
        fill.fillAmount = Mathf.MoveTowards(fill.fillAmount, target, smoothSpeed * Time.deltaTime);

        if (target <= lowThreshold)
        {
            float t = pulseWhenLow ? (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f : 1f;
            fill.color = Color.Lerp(normalColor, lowColor, 0.5f + 0.5f * t);
        }
        else
        {
            fill.color = normalColor;
        }
    }
}
