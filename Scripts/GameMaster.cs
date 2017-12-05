using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static public class GameMaster {


    private static int score = 0;
    private static int hit = 0;
    private static int miss = 0;
    private static int criticalHit = 0;
    private static int criticalMiss = 0;
    private static string note = "";
    public static bool state = true;

    static public void StartGame()
    {
        UpdateScoreDisplay();
    }

    static public void PauseGame()
    {
        state = false;
        GameObject.Find("ScoreDisplay").GetComponent<Text>().text = "PAUSED";
        Time.timeScale = 0;
        var keynotes = GameObject.FindGameObjectsWithTag("KeyNote");
        foreach (GameObject go in keynotes)
        {
            go.GetComponent<KeyNote>().isEnabled = false;
        }

        GameObject.Find("PauseMenu").transform.GetChild(0).gameObject.SetActive(true);

    }

    static public void ContinueGame()
    {
        var keynotes = GameObject.FindGameObjectsWithTag("KeyNote");
        foreach (GameObject go in keynotes)
        {
            go.GetComponent<KeyNote>().isEnabled = true;
        }

        Time.timeScale = 1;
        UpdateScoreDisplay();
        state = true;
        GameObject.Find("PauseMenu").transform.GetChild(0).gameObject.SetActive(false);
    }

    static public void Hit()
    {
        hit++;
        score++;
        UpdateScoreDisplay();
    }

    static public void CriticalHit()
    {
        criticalHit++;
        score = score + 2;
        UpdateScoreDisplay();
    }

    static public void Miss()
    {
        miss++;
        UpdateScoreDisplay();
    }

    static public void CriticalMiss()
    {
        criticalMiss++;
        score--;
        UpdateScoreDisplay();
    }

    static public void DisplayNote(string Note)
    {
        note = Note;
        UpdateScoreDisplay();
    }

    static private void UpdateScoreDisplay()
    {
        //GameObject.Find("ScoreDisplay").GetComponent<Text>().text = "Score " + score.ToString() + " Perfect: " + criticalHit.ToString() + " Hit: " + hit.ToString() + " Fail: " + criticalMiss.ToString() + " Miss: " + miss.ToString();
        GameObject.Find("NoteDisplay").GetComponent<Text>().text = note;
    }
}
