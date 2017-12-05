using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	// Use this for initialization
	void Start () {

        GameMaster.StartGame();

	}

    public void PauseGame()
    {
        GameMaster.PauseGame();
    }

    public void ContinueGame()
    {
        GameMaster.ContinueGame();
    }
	// Update is called once per frame
    void Update()
    {

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit != false && hit.collider.tag == "KeyNote")
            {
                hit.transform.GetComponent<KeyNote>().KeyTouch();
            }
            else if (hit != false && hit.collider.tag == "PauseField")
            {
                PauseGame();
            }
        }
            foreach (var touch in Input.touches)
                if (touch.phase == TouchPhase.Began)
                {
                    // Construct a ray from the current touch coordinates
                    RaycastHit2D hit = Physics2D.Raycast(touch.position, Vector2.zero);
                    if (hit != false && hit.collider.tag == "KeyNote")
                    {
                        hit.transform.GetComponent<KeyNote>().KeyTouch();
                    }
                    else if (hit != false && hit.collider.tag == "PauseField")
                    {
                        PauseGame();
                    }
            }
        }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Screenmanager Resolution Width", 956);
        PlayerPrefs.SetInt("Screenmanager Resolution Height", 439);
        PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 0);
    }

}

