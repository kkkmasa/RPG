using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] Material hitMat;
    [SerializeField] float flashDuration = 0.2f;
    [SerializeField] Material orignalMat;

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        orignalMat = sr.material;
    }

    IEnumerator FlashFX() {
        sr.material = hitMat;
        yield return new WaitForSeconds(flashDuration);
        sr.material = orignalMat;
    }
    private void RedColorBlink() {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }
    private void CancelRedBlink() {
        CancelInvoke();
        sr.color = Color.white;
    }

}
