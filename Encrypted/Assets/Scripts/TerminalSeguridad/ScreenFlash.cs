using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlash : MonoBehaviour
{
    public Image flashImage;
    public float flashDuration = 0.15f;

    public Transform cameraTransform;
    public float shakeDuration = 0.15f;
    public float shakeMagnitude = 0.15f;

    private Vector3 originalCamPos;

    private void Start()
    {
        flashImage.gameObject.SetActive(false);

        if (cameraTransform != null)
            originalCamPos = cameraTransform.localPosition;
    }

    public void FlashRed()
    {
        StartCoroutine(Flash());
        StartCoroutine(CameraShake());
    }

    private IEnumerator Flash()
    {
        flashImage.gameObject.SetActive(true);
        flashImage.color = new Color(1, 0, 0, 0.4f);

        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        flashImage.color = new Color(1, 0, 0, 0);
        flashImage.gameObject.SetActive(false);
    }

    private IEnumerator CameraShake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            cameraTransform.localPosition =
                originalCamPos + new Vector3(x, y, 0);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalCamPos;
    }
}
