using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyNote : MonoBehaviour {

    public float delayTime;
    public float criticalTimeWindow;
    public float activeTimeWindow;
    public SpriteRenderer noteSprite;
    public Color inactiveColor;
    public Color activeColor;
    public UIHitEvent UI_HitEvent;
    private bool activeState = false;
    private bool criticalActive = false;
    public bool isEnabled = true;
    public bool stringNote = false;



    // Use this for initialization
    void Start ()
    {
        StartDelay();
    }

    void StartDelay()
    {
        if (activeState)
        {
            Invoke("KeyMissed", activeTimeWindow);
        }
        else
        {
            StartCoroutine(LerpRendererAlpha(noteSprite, delayTime, inactiveColor));
            Invoke("ActiveState", delayTime);
        }
    }


    void KeyHit()
    {
        if (criticalActive)
        {
            GameMaster.CriticalHit();

            /*
            UI_HitEvent = Instantiate(UI_HitEvent);
            UI_HitEvent.GetComponent<Text>().fontSize = 25;
            UI_HitEvent.Initialize("PERFECT");
            */
        }
        else
        {
            GameMaster.Hit();
            /*
            UI_HitEvent = Instantiate(UI_HitEvent);
            UI_HitEvent.Initialize("hit");
            */
        }

        Destroy(gameObject);
    }

    void KeyMissed()
    {
        GameMaster.Miss();
        /*
        UI_HitEvent = Instantiate(UI_HitEvent);
        UI_HitEvent.Initialize("miss");
        */
        Destroy(gameObject);
    }

    void KeyCriticalMiss()
    {
        GameMaster.CriticalMiss();
        /*
        UI_HitEvent = Instantiate(UI_HitEvent);
        UI_HitEvent.GetComponent<Text>().fontSize = 25;
        UI_HitEvent.Initialize("FAIL");
        */
        Destroy(gameObject);
    }

    void ActiveState()
    {

        activeState = true;
        transform.Translate(0, 0, -5);
        ChangeCriticalState();
        StartCoroutine(LerpRendererAlpha(noteSprite, criticalTimeWindow, activeColor));
        Invoke("ChangeCriticalState", criticalTimeWindow);
        StartDelay();
        
    }

    void ChangeCriticalState()
    {

        if (criticalActive)
        {
            criticalActive = false;
            StartCoroutine(LerpRendererAlpha(noteSprite, activeTimeWindow-criticalTimeWindow, activeColor));
        }
        else
        {
            criticalActive = true;
        }
    }

    public void KeyTouch()
    {
        if (isEnabled)
        {
            if (activeState)
            {
                KeyHit();
            }
            else
            {
                KeyCriticalMiss();
            }
        }
    }

    IEnumerator LerpRendererAlpha(SpriteRenderer sprite, float duration, Color target)
    {
  

        if (!activeState)
        {
            for (float time = 0.0f; time < 1.0f; time += Time.deltaTime / duration)
            {
                if(stringNote)
                transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, 1.0f, time), Mathf.Lerp(transform.localScale.y, 0.015f, time), 1);
                else
                transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, 10.0f, time), Mathf.Lerp(transform.localScale.y, 10.0f, time), 1);
                sprite.color = new Color(Mathf.Lerp(sprite.color.r, target.r, time), Mathf.Lerp(sprite.color.g, target.g, time), Mathf.Lerp(sprite.color.b, target.b, time), Mathf.Lerp(sprite.color.a, target.a, time));


                yield return null;
            }
        }
        else if (criticalActive)
        {
            for (float time = 0.0f; time < 1.0f; time += Time.deltaTime / duration)
            {
                //transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, 1, time), Mathf.Lerp(transform.localScale.y, 1, time), 2);
                sprite.color = new Color(Mathf.Lerp(sprite.color.r, target.r, time), Mathf.Lerp(sprite.color.g, target.g, time), Mathf.Lerp(sprite.color.b, target.b, time), Mathf.Lerp(sprite.color.a, target.a, time));
                yield return null;
            }
        }
        else
        {
            for (float time = 0.0f; time < 1.0f; time += Time.deltaTime / duration)
            {
                sprite.color = new Color(Mathf.Lerp(sprite.color.r, target.r, time), Mathf.Lerp(sprite.color.g, target.g, time), Mathf.Lerp(sprite.color.b, target.b, time), Mathf.Lerp(sprite.color.a, target.a*0.5f, time));
                yield return null;
            }
        }
    }

}
