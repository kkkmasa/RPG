using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] Material hitMat;
    [SerializeField] float flashDuration = 0.2f;
    [SerializeField] Material orignalMat;

    [Header("Aliment Colors")]
    [SerializeField] Color[] chillColors;
    [SerializeField] Color[] igniteColors;
    [SerializeField] Color[] shockColors;

    [SerializeField] float fxColorDuration = 0.15f;


    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        orignalMat = sr.material;
    }

    public void MakeTransprent(bool _transparent)
    {
        if (_transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        sr.color = currentColor;
        sr.material = orignalMat;
    }
    private void RedColorBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }
    private void CancelColorChanged()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    public void IgniteForSeconds(float _seconds)
    {
        InvokeRepeating("IgniteColorFx", 0, fxColorDuration);
        Invoke("CancelColorChanged", _seconds);
    }
    public void ChillForSeconds(float _seconds)
    {
        InvokeRepeating("ChillColorFX", 0, fxColorDuration);
        Invoke("CancelColorChanged", _seconds);
    }
    public void ShockForSeconds(float _seconds)
    {
        InvokeRepeating("ShockColorFX", 0, fxColorDuration);
        Invoke("CancelColorChanged", _seconds);
    }

    void IgniteColorFx()
    {
        if (igniteColors.Length <= 0) return;

        if (sr.color != igniteColors[0])
            sr.color = igniteColors[0];
        else
            sr.color = igniteColors[1];
    }
    void ShockColorFX()
    {
        if (shockColors.Length <= 0) return;

        if (sr.color != shockColors[0])
            sr.color = shockColors[0];
        else
            sr.color = shockColors[1];
    }
    void ChillColorFX()
    {
        if (chillColors.Length <= 0) return;

        if (sr.color != chillColors[0])
            sr.color = chillColors[0];
        else
            sr.color = chillColors[1];
    }
}
