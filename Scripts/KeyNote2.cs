using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyNote2 : MonoBehaviour
{

    public float delayTime;
    public SpriteRenderer noteSprite;
    public UIHitEvent UI_HitEvent;
    public Transform HitZone;

    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    public int lengthOfLineRenderer = 20;
    public LineRenderer lineRenderer;
    public bool syncNote = false;

    // Use this for initialization
    void Start()
    {
        StartDelay();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = new Material((Material)Resources.Load("NewMaterial"));
        lineRenderer.widthMultiplier = 4.0f;
        lineRenderer.positionCount = lengthOfLineRenderer;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
        lineRenderer.colorGradient = gradient;
       
    }

    void StartDelay()
    {
       StartCoroutine(LerpRendererAlpha(200.0f));
       Invoke("Die", 5.0f);
    }

    void Update()
    {
        var points = new Vector3[lengthOfLineRenderer];
        var t = Time.time;
        for (int i = 0; i < lengthOfLineRenderer; i++)
        {
            points[i] = new Vector3(0, i - 0.5f * 0.5f, 0.0f);
        }
        lineRenderer.SetPositions(points);
    }

    void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator LerpRendererAlpha(float duration)
    {

            for (float time = 0.0f; time < 1.0f; time += Time.deltaTime / duration)
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, HitZone.position.x, time), Mathf.Lerp(transform.position.y, HitZone.position.y, time), 1);
                yield return null;
            }
    }

    IEnumerator LerpRendererScale(float duration, float scale, float startScale)
    {

        
        for (float time = 0.0f; time < 1.0f; time += Time.deltaTime / duration)
        {
            /*
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(scale / transform.lossyScale.x, scale / transform.lossyScale.y, scale / transform.lossyScale.z);
            */
            lineRenderer.widthMultiplier = Mathf.Lerp(startScale, scale, time);
            yield return null;
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {

        StartCoroutine(LerpRendererScale(.1f, 5.5f, 5.0f));
        if (syncNote)
        {
            GameObject.Find("MidiSyncer").GetComponent<MIDIPlayer>().SyncStart();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        StartCoroutine(LerpRendererScale(.3f, 2.5f, 5.5f));
    }

}
