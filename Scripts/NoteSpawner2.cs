using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner2 : MonoBehaviour
{

    public GameObject String_1;
    public GameObject String_2;
    public GameObject String_3;
    public GameObject String_4;



    public GameObject KeyNote;
    public float rate;

    private GameObject[] strings;

    // Use this for initialization
    void Start()
    {
        strings = new GameObject[] { String_1, String_2, String_3, String_4 };
    }


    public void syncNote()
    {

        Transform fret = strings[1].GetComponent<Transform>().GetChild(0);
        GameObject key = Instantiate(KeyNote, fret.position, Quaternion.identity);
        key.transform.SetParent(fret.transform);
        key.GetComponent<KeyNote2>().HitZone = strings[1].GetComponent<Transform>().GetChild(1);
        key.GetComponent<KeyNote2>().syncNote = true;
        key.GetComponent<KeyNote2>().lengthOfLineRenderer = 0;
        if (Random.Range(2, 10) > 4)
        {
            key.GetComponent<KeyNote2>().c1 = Color.cyan;
            key.GetComponent<KeyNote2>().c2 = Color.white;
        }
        if (Random.Range(2, 10) > 7)
        {
            key.GetComponent<KeyNote2>().c1 = Color.green;
            key.GetComponent<KeyNote2>().c2 = Color.gray;
        }

    }


    public void spawnNote(int violinString, int note, uint length)
    {
        List<int> one = new List<int>() { 0, 1, 7, 8, 14, 15, 21, 22 };
        List<int> two = new List<int>() { 2, 3, 9, 10, 16, 17, 23, 24 };
        List<int> three = new List<int>() { 4, 5, 11, 12, 18, 19, 25, 26 };
        List<int> four = new List<int>() { 6, 13, 20, 27 };

        Transform fret = strings[violinString].GetComponent<Transform>().GetChild(0);
        GameObject key = Instantiate(KeyNote, fret.position, Quaternion.identity);
        key.transform.SetParent(fret.transform);
        key.GetComponent<KeyNote2>().HitZone = strings[violinString].GetComponent<Transform>().GetChild(1);
        key.GetComponent<SpriteRenderer>().sprite = GameObject.Find("Notes").transform.GetChild(note).GetComponent<SpriteRenderer>().sprite;
        key.GetComponent<KeyNote2>().lengthOfLineRenderer = (int)length;

        if (one.Contains(note))
        {
            key.GetComponent<KeyNote2>().c1 = Color.cyan;
            key.GetComponent<KeyNote2>().c2 = Color.white;
        }
        else if (two.Contains(note))
        {
            key.GetComponent<KeyNote2>().c1 = Color.magenta;
            key.GetComponent<KeyNote2>().c2 = Color.white;
        }
        else if (four.Contains(note))
        {
            key.GetComponent<KeyNote2>().c1 = Color.green;
            key.GetComponent<KeyNote2>().c2 = Color.gray;
        }

    }

}
