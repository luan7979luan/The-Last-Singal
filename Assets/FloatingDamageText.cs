using UnityEngine;
using TMPro;

public class FloatingDamageText : MonoBehaviour
{
    public float floatSpeed = 50f;
    public float duration = 1f;
    public TextMeshProUGUI damageText;

    private Color originalColor;

    void Awake()
    {
        if (damageText == null)
            damageText = GetComponent<TextMeshProUGUI>();
        originalColor = damageText.color;
    }

    public void SetDamageValue(int damage)
    {
        damageText.text = damage.ToString();
    }

    void Update()
    {
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);
        duration -= Time.deltaTime;
        if (duration > 0)
        {
            float alpha = duration / 1f;
            damageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
