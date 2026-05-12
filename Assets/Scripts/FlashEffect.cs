using System.Collections;
using UnityEngine;

public sealed class FlashEffect : MonoBehaviour
{
    [Tooltip("Flash Type")]
    [SerializeField] private bool isUsingPlayer;

    private SpriteRenderer spriteRenderer;
    private Material flashMaterial;
    private Material originalMaterial;

    private Coroutine flashRoutine;
    private float effectDuration;

    private void Awake()
    {
        flashMaterial = Resources.Load("Materials/Flash Material") as Material;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        effectDuration = isUsingPlayer
            ? GetComponent<Player>().DamageCooldown
            : GetComponent<Enemy>().DamageCooldown;

        originalMaterial = spriteRenderer.material;
    }

    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        CancelInvoke(nameof(StopFlash));

        flashRoutine = StartCoroutine(StartFlashEffect());
        Invoke(nameof(StopFlash), effectDuration);
    }

    private void StopFlash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
            flashRoutine = null;
        }

        spriteRenderer.material = originalMaterial;
    }

    private IEnumerator StartFlashEffect()
    {
        const float flashDuration = 0.25f;

        while (true)
        {
            spriteRenderer.material = flashMaterial;
            yield return new WaitForSeconds(flashDuration);

            spriteRenderer.material = originalMaterial;
            yield return new WaitForSeconds(flashDuration);
        }
    }
}