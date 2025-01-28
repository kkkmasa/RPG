using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    Entry entry;
    RectTransform rectTransform;
    CharacterStats myStats;
    Slider slider;

    void Start()
    {
        entry = GetComponentInParent<Entry>();
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();

        entry.onFilped += FlipedUI;
        myStats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }


    public void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }

    private void FlipedUI() => rectTransform.Rotate(0, 180, 0);
    private void OnDisable()
    {
        entry.onFilped -= FlipedUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
