using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test_NamePlayer : MonoBehaviour
{
    public bool nameSelection;
    bool input, inputV1;
    int l1, t1;

    List<string> letter = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

    public Text[] P1L1, P1selection;
    public Color standardColor,highlightedColor;

    void Start()
    {
        P1L1[0].color = highlightedColor;
        P1L1[1].color = standardColor;
        P1L1[2].color = standardColor;
    }

    void Update()
    {
        if (nameSelection)
        {
            for (int i = 0; i < P1L1.Length; i++)
            {
                P1L1[i].gameObject.SetActive(true);
            }

            if (!inputV1 && Input.GetAxis("Vertical") < -0.5f || Input.GetKeyDown(KeyCode.Y))
            {
                inputV1 = true;
                l1++;
                if (l1 >= letter.Count) l1 = 0;
                P1L1[t1].text = letter[l1];
            }
            if (!inputV1 && Input.GetAxis("Vertical") > 0.5f || Input.GetKeyDown(KeyCode.H))
            {
                inputV1 = true;
                l1--;
                if (l1 < 0) l1 = letter.Count - 1;
                P1L1[t1].text = letter[l1];
            }

            if (!input && Input.GetAxis("Horizontal") > 0.5f || Input.GetKeyDown(KeyCode.J))
            {
                input = true;
                P1L1[t1].color = standardColor;
                t1++;
                if (t1 >= P1L1.Length) t1 = 0;
                l1 = letter.IndexOf(P1L1[t1].text);
                P1L1[t1].color = highlightedColor;
            }
            if (!input && Input.GetAxis("Horizontal") < -0.5f || Input.GetKeyDown(KeyCode.G))
            {
                input = true;
                P1L1[t1].color = standardColor;
                t1--;
                if (t1 < 0) t1 = P1L1.Length - 1;
                l1 = letter.IndexOf(P1L1[t1].text);
                P1L1[t1].color = highlightedColor;
            }

            if (Input.GetAxis("Vertical") < 0.5f && Input.GetAxis("Vertical") > -0.5f)
            {
                inputV1 = false;
            }

            if (!input && Input.GetAxis("Horizontal") > 0.5f || Input.GetKeyDown(KeyCode.RightArrow))
            {
                input = true;

            }
            else if (!input && Input.GetAxis("Horizontal") < -0.5f || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                input = true;
            }
            else if (Input.GetAxis("Horizontal") < 0.5f && Input.GetAxis("Horizontal") > -0.5f || (Input.GetKeyUp(KeyCode.RightArrow) && Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                input = false;
            }
        }
        else
        {
            for (int i = 0; i < P1L1.Length; i++)
            {
                P1L1[i].gameObject.SetActive(false);
            }
        }
    }
}
