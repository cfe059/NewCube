
    


using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "New Random Name Item Database", menuName = "Inventory System/Items/RandomName")]
public class RandomNameDatabase : ScriptableObject
{
    [SerializeField]
    private TextAsset _csvItem;
    public List<RandomName_Obj> weapon;
    public List<RandomName_Obj> armor;
    
    public void CreateRandomName()
    {
        List<string[]> _csvData = new List<string[]>();
        TextAsset _csvItem = Resources.Load<TextAsset>("Data/randomname_master");
        StringReader reader = new StringReader(_csvItem.text);
        
        while (reader.Peek() != -1) 
        {
            string line = reader.ReadLine();
            _csvData.Add(line.Split(','));
        }
    
        
        List<RandomName_Obj> _tmpNameW = new List<RandomName_Obj>();
        List<RandomName_Obj> _tmpNameA= new List<RandomName_Obj>();

        for (int i = 1; i < _csvData.Count; i++)
        {
            RandomName_Obj _tmp = new RandomName_Obj();
            _tmp.ID = int.Parse(_csvData[i][0]);
            _tmp.RName = _csvData[i][1];
            _tmp.Rimg = _csvData[i][2];
            if (_csvData[i][0].Substring(0,1) == "1")
            {
                _tmpNameW.Add(_tmp);

            }
            else if (_csvData[i][0].Substring(0,1) == "2")
            {
                _tmpNameA.Add(_tmp);

            }
            
        }

        armor = _tmpNameA;
        weapon = _tmpNameW;
        


    }

    public void OnEnable()
    {
        CreateRandomName();
    }
}
