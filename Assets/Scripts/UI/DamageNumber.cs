using System.Collections;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    private TextMeshProUGUI damageText;
    private Transform targetTransform;

    private void Awake()
    {
        damageText = GetComponent<TextMeshProUGUI>();
    }

    public void SetDamage(float damage)
    {
        if (damage > 0)
        {
            damageText.text = damage.ToString();
            StartCoroutine(FadeAndMoveEffect()); // Start the fade and move effect when the damage number is set
        }
        
    }

    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }

    private void Update()
    {
        if (targetTransform != null)
        {
            // Convert the target's position to screen space
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position + new Vector3(1f, -0.5f, 0));
            transform.position = screenPosition;
        }
    }

    private IEnumerator FadeAndMoveEffect()
    {
        float duration = 0.5f; // Duration of the effect
        float elapsedTime = 0f;
        Vector3 startPosition = transform.localPosition;

        Color startColor = damageText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0); // Fade to transparent

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Interpolate color
            damageText.color = Color.Lerp(startColor, endColor, t);

            // Update position relative to the target's screen position
            if (targetTransform != null)
            {
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position + new Vector3(1f, -0.5f, 0));
                transform.position = screenPosition + new Vector3(0, 50 * t, 0);
            }

            yield return null;
        }

        // Ensure it's fully transparent and destroy the object
        damageText.color = endColor;
        Destroy(gameObject);
    }
}
