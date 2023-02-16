using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlantData
{
    public string Name, Description, Wants, Fears;
    public List<string> Likes, Dislikes, GoodResponses, BadResponses;
    public int Yield;
    public PlantData(string _name)//string _description, string _wants, string _fears, List<string> _likes, List<string> _dislikes, List<string> _goodResponses, List<string> _badResponses
    {
        Name = _name;
        Likes = new List<string>();
        Dislikes = new List<string>();
        GoodResponses = new List<string>();
        BadResponses = new List<string>();

        //Description = _description;
        //Wants = _wants;
        //Fears = _fears;
        //Likes = _likes;
        //Dislikes = _dislikes;
        //GoodResponses = _goodResponses;
        //BadResponses = _badResponses;
    }
}
