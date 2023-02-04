using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONLoader : MonoBehaviour
{
    public TextAsset PlantText;
    private NotebookManager nm;
    private RawPlantList importFile = new RawPlantList();

    public List<PlantData> PlantsLoadedIn;


    [System.Serializable]
    public class RawPlantData
    {
        public string Name, Description, Wants, Fears, Likes, Dislikes, GoodResponses, BadResponses;
    }

    [System.Serializable]
    public class RawPlantList
    {
        public List<RawPlantData> _PlantData;
        public RawPlantList()
        {
            _PlantData = new List<RawPlantData>();
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        PlantsLoadedIn = new List<PlantData>();
        nm = FindObjectOfType<NotebookManager>();
        importFile = JsonUtility.FromJson<RawPlantList>(PlantText.text);

        string currentPlantName = "";
        PlantData currentPlantImport = null;
        

        foreach (RawPlantData raw in importFile._PlantData)
        {
            if (raw.Name != currentPlantName)
            {
                if (currentPlantImport != null)
                {
                    PlantsLoadedIn.Add(currentPlantImport);
                }
                currentPlantName = raw.Name;
                currentPlantImport = new PlantData(raw.Name);
            }

            if (raw.Likes != "") { currentPlantImport.Likes.Add(raw.Likes); }
            if (raw.Dislikes != "") { currentPlantImport.Dislikes.Add(raw.Dislikes); }
            if (raw.GoodResponses != "") { currentPlantImport.GoodResponses.Add(raw.GoodResponses); }
            if (raw.BadResponses != "") { currentPlantImport.BadResponses.Add(raw.BadResponses); }
            if (raw.Description != "") { currentPlantImport.Description = raw.Description; }

        }
        PlantsLoadedIn.Add(currentPlantImport);

        /*foreach (PlantData p in PlantsLoadedIn)
        {
            Debug.Log("NEW PLANT "+p.Name);
            foreach (string l in p.Likes) Debug.Log(l);
        }*/
        nm.ListOfPlants = PlantsLoadedIn;
    }

}
