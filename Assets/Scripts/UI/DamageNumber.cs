using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float fadeDuration = 1f;

    private TMP_Text damageText;
    private float startTime;

    void Awake()
    {
        damageText = GetComponent<TMP_Text>();
    }

    void Start()
    {
        startTime = Time.time;
        Destroy(gameObject, fadeDuration);
    }

    void Update()
    {
        // Move the text upwards
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // Fade out the text
        float elapsedTime = Time.time - startTime;
        float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
        Color color = damageText.color;
        color.a = alpha;
        damageText.color = color;
    }

    public void SetDamage(float damage)
    {
        damageText.text = damage.ToString();
    }
}
