using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour {

    public GameObject String_1;
    public GameObject String_2;
    public GameObject String_3;
    public GameObject String_4;



    public GameObject KeyNote;
    public float rate;

    private GameObject[] strings;

    // Use this for initialization
    void Start () {

        strings = new GameObject[] { String_1, String_2, String_3, String_4 };
    }


	
    void spawnNote()
    { 

        int randString = Random.Range(0, 3);
        int randFret = Random.Range(8, 8);


        if (randFret < 7)
        {
            Transform fret = strings[randString].GetComponent<Transform>().GetChild(randFret);
            GameObject key = Instantiate(KeyNote, fret.position, Quaternion.identity);
            key.transform.SetParent(fret.transform.GetChild(0));
        }
        else
        {
            Transform fret = strings[randString].GetComponent<Transform>();
            GameObject key = Instantiate(KeyNote, fret.position, Quaternion.identity);
            key.transform.SetParent(fret.transform);
            key.GetComponent<KeyNote>().stringNote = true;
        }
        Invoke("spawnNote", rate);

	}

    public void spawnNote(int violinString, int note)
    {
        int randString = violinString;
        int randFret = note;


        if (randFret > 0)
        {
            Transform fret = strings[randString].GetComponent<Transform>().GetChild(randFret-1);
            GameObject key = Instantiate(KeyNote, fret.position, Quaternion.identity);
            key.transform.SetParent(fret.transform.GetChild(0));
        }
        else
        {
            Transform fret = strings[randString].GetComponent<Transform>();
            GameObject key = Instantiate(KeyNote, fret.position, Quaternion.identity);
            key.transform.SetParent(fret.transform);
            key.GetComponent<KeyNote>().stringNote = true;
        }

    }

}
