using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class HighScoreManager : MonoBehaviour
{
    List<ScoreData> m_data = new List<ScoreData>();

    public ScoreData[] finalScores = new ScoreData[10];

    public string path = "MyPath.json";

    [ContextMenu("Sort")]
    public void Sort()
    {

        m_data.Sort(SortByScore);
        finalScores = m_data.ToArray();
        SaveData();
    }

    [ContextMenu("Fill With Testers")]
    void Testers()
    {
        m_data.Clear();
        for (int i = 0; i < 10; i++)
        {
            ScoreData newHero = new ScoreData();
            newHero.name = "Mario";
            newHero.score = UnityEngine.Random.Range(0, 1000);

            m_data.Add(newHero);

        }
        SaveData();
    }

    public void AddItem(string newItemName, int newItemScore)
    {
        ScoreData newHero = new ScoreData();
        newHero.name = newItemName;
        newHero.score = newItemScore;
        m_data.Add(newHero);

        m_data.Sort(SortByScore);

        m_data.RemoveRange(10,m_data.Count-10);

        SaveData();

    }

    [ContextMenu("Save Data")]
    public void SaveData()
    {
        finalScores = m_data.ToArray();

        if (!File.Exists(path))
        {
            for (int i = 0; i < finalScores.Length; i++)
            {
                StreamWriter writer = new StreamWriter(path, true);

                writer.WriteLine(finalScores[i].name + " " + finalScores[i].score);
                writer.Close();
            }
        }
        else
        {

            File.Delete(path);
            //Debug.LogError("DELETED");
            for (int i = 0; i < finalScores.Length; i++)
            {
                StreamWriter writer = new StreamWriter(path, true);

                writer.WriteLine(finalScores[i].name + " " + finalScores[i].score);
                writer.Close();
            }
        }
    }

    [ContextMenu("Get Data")]
    public void GetData()
    {

        if (File.Exists(path))
        {
            StreamReader reader = new StreamReader(path);
            string fileData = reader.ReadLine();


            string[] lines = File.ReadAllLines(path);
            m_data.Clear();

            for (int i = 0; i < 10; i++)
            {
                m_data.Add(new ScoreData());
                string[] temp = lines[i].Split(' ');
                m_data[i].name = temp[0];
                m_data[i].score = Convert.ToInt32(temp[1]);
            }
            finalScores = m_data.ToArray();

        }
        else
        {
            Debug.Log("File doesn't Exists");

        }


    }

    int SortByScore(ScoreData p1, ScoreData p2)
    {
        return p2.score.CompareTo(p1.score);
    }

    [ContextMenu("Add Item")]
    void AddItemTest()
    {
        AddItem("LUIGI", 300000);
    }

    [System.Serializable]
    public class ScoreData
    {
        public int score;
        public string name;
    }
}
