using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHitEvent : MonoBehaviour {

    public float duration;
    public Color perfect;
    public Color fail;
    public Color hit;
    public Color miss;

    public void Initialize(string content)
    {
            GameObject canvas = GameObject.Find("HitDisplay").gameObject;
            transform.SetParent(canvas.transform);
            transform.localPosition = new Vector3(0, 0, 0);

        GetComponent<Text>().text = content;
        GetComponent<Text>().resizeTextForBestFit = true;
        transform.Translate(Random.insideUnitCircle * 40);

        switch (content)
        {
            case "PERFECT":
                GetComponent<Text>().color = perfect;
                GetComponent<Text>().fontStyle = FontStyle.Bold;
                break;
            case "FAIL":
                GetComponent<Text>().color = fail;
                GetComponent<Text>().fontStyle = FontStyle.BoldAndItalic;
                break;
            case "miss":
                GetComponent<Text>().color = miss;
                GetComponent<Text>().fontStyle = FontStyle.Italic;
                break;
            case "hit":
                GetComponent<Text>().color = hit;
                GetComponent<Text>().fontStyle = FontStyle.Normal;
                break;
            default:
                break;
        }

                GetComponent<Text>().CrossFadeAlpha(0.0f, duration, false);

                Destroy(gameObject, duration);
        }
    }


