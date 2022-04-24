using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToHide : MonoBehaviour
{
    private bool hidden = false;
    [SerializeField] private float fadeDelay = 10f;
    private Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }


    // Update is called once per frame
    void Update()
    {
        if (hidden)
        {
            StartCoroutine(AlphaMaterialLow());
            hidden = false;
        }
        else
        {
            StartCoroutine(AlphaMaterialMax());
        }
    }

    public IEnumerator AlphaMaterialLow()
    {
        Debug.Log("hit");
        Color lowAlpha = new Color(1, 1, 1, 0.2f);
        renderer.material.SetColor("_BaseColor", Color.Lerp(renderer.material.color, lowAlpha, fadeDelay * Time.deltaTime));
        yield return null;
    }

    public IEnumerator AlphaMaterialMax()
    {
        Debug.Log("hit");
        Color maxAlpha = new Color(1, 1, 1, 1);
        renderer.material.SetColor("_BaseColor", Color.Lerp(renderer.material.color, maxAlpha, fadeDelay * Time.deltaTime));
        yield return null;
    }

    public void FadeObject()
    {
        hidden = true;
    }
}
