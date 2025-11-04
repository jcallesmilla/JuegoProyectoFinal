using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Behaviour : MonoBehaviour
{
    public Slider slider;
    public Color low;
    public Color high;
    public Vector3 offset;

    public void SetHealth(int health, int maxHealth)
    {
        slider.gameObject.SetActive(health < maxHealth);
        slider.maxValue = maxHealth;
        slider.value = health;

        slider.fillRect.GetComponentInChildren<Image>().color =
            Color.Lerp(low, high, slider.normalizedValue);
    }

    void Update()
    {
        slider.transform.position =
            Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }
}
