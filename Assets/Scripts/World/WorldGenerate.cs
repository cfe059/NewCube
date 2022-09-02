using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldGenerate : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private List<GameObject> _Faces;
    public List<nodeActive> _nodeActives;
    public int MonsterMinimum;
    public int MonsterMaximum;  
    public int WallMinimum;
    public int WallMaximum;
    public Vector2 ItemRange;
    public GameObject[] monster;
    public List<GameObject> Items;
    public Floor_Steup [] floors;
    public string json ;
    void Start()
    {
        foreach (var face in _Faces)
        {
            _nodeActives.Add(face.GetComponent<nodeActive>());
        }
        //LoadWorldData();
    
        if (json == "")
        {
            Debug.Log("new world");
            AddcanSpawnItem();
            
        
            foreach (var node in _nodeActives)
            {
                RandomWorld(node);
            }

       
            json = JsonUtility.ToJson(GManager.Instance.WorldData,true);
            string filePath = Application.dataPath + "/Resources/Json/test.json";
        
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(json); 
            streamWriter.Flush ();
            streamWriter.Close ();

        }
        else
        {
            GenerateFormSave();
        }
        
       // PlayfabManager.SaveWorld(WorldData);
    }

    GameObject FindClosestObject(GameObject gameObject)
    {
        Transform tmin = null;
        int mainDistArray = -1;
        float mint = Mathf.Infinity;
        Vector3 currentPos = gameObject.transform.position;
        foreach (var nodeActive in _nodeActives)
        {
            float dist = Vector3.Distance(nodeActive.transform.position, currentPos);
            if (dist < mint)
            {
                mainDistArray = Convert.ToInt32(nodeActive.gameObject.name);
                mint = dist;
            }
        }
        mint = Mathf.Infinity;
        foreach (var node in _nodeActives[mainDistArray].nodes)
        {
            if (node != null)
            {
                float dist = Vector3.Distance(node.transform.position, currentPos);
                if (dist < mint)
                {
                    tmin = node.transform;
                    mint = dist;
                }
            }
        }

        return tmin.gameObject;

    }
    public void DebugGenerateWorld()
    {
        _DestoryAllObj();
        foreach (var node in _nodeActives)
        {
            node.resetAllNode();

        }
        
        GameObject p = GameObject.Find("Player");
        nodeBase n = FindClosestObject(p).GetComponentInChildren<nodeBase>();
        n._nodeStatus = nodeBase.nodeStatus.Player;
        foreach (var node in _nodeActives)
        {
            RandomWorld(node);
        }

        
        
    }
    public void _DestoryAllObj()
    {
        for (int i = 0; i < _nodeActives.Count; i++)
        {
            _nodeActives[i].DestroyAllObject(_nodeActives[i].remainingObject);
            _nodeActives[i].DestroyAllObject(_nodeActives[i].Floors);
            _nodeActives[i].remainingObject = new List<GameObject>();
            _nodeActives[i].Floors = new List<GameObject>();
            
        }
        
    }

    private void FixedUpdate()
    {
       // FindAllMonster();
    }

    public void GenerateFormSave()
    {
        _DestoryAllObj();
        // foreach (var face in _Faces)
        // {
        //     _nodeActives.Add(face.GetComponent<nodeActive>());
        // }
        
        for (int i = 0; i < _nodeActives.Count; i++)        {
            List<GameObject> noneNodes = new List<GameObject>();
            List<string> items = new List<string>();
            foreach (var node in _nodeActives[i].nodes)
            {
                
                    noneNodes.Add(node);
                
            
            }
            
            for (int y = 0; y < 25; y++)
            {
                GameObject g_obj = Instantiate(Resources.Load<GameObject>(
                    $"World_Map/{GManager.Instance.WorldData.WorldGens[i].terrainType}/{GManager.Instance.WorldData.WorldGens[i].object_name[y]}"),
                    _nodeActives[i].parentFloor.transform);
//                Debug.Log(i);
                g_obj.transform.position = noneNodes[y].transform.position;
                noneNodes[y].transform.GetChild(0).gameObject.GetComponent<nodeBase>()._nodeStatus =
                    GManager.Instance.WorldData.WorldGens[i].floor_status[y];
                if (GManager.Instance.WorldData.WorldGens[i].Items[y] != "")
                {
                    GameObject i_obj = Instantiate(
                        Resources.Load<GameObject>($"Items/Prefabs/{GManager.Instance.WorldData.WorldGens[i].Items[y]}")
                        ,_nodeActives[i].parentItems.transform);
                    i_obj.transform.position = noneNodes[y].transform.position;
                    _nodeActives[i].remainingObject.Add(i_obj);


                }if (GManager.Instance.WorldData.WorldGens[i].Enemies[y] != "")
                {
                    GameObject e_obj = Instantiate(
                        Resources.Load<GameObject>($"Monster/{GManager.Instance.WorldData.WorldGens[i].Enemies[y]}")
                        ,_nodeActives[i].parentMonster.transform);
                    e_obj.transform.position = noneNodes[y].transform.position;
                    _nodeActives[i].remainingObject.Add(e_obj);


                }
                if (GManager.Instance.WorldData.WorldGens[i].floor_status[y] != nodeBase.nodeStatus.None)
                {
                    _nodeActives[i].remainingObject.Add(g_obj);
                }
                else
                {
                    _nodeActives[i].Floors.Add(g_obj);
                }
            }

            
            _nodeActives[i].parentFloor.transform.localPosition += new Vector3(0f, 0f, 0.49f);      

        }
        
        
    }
    void LoadWorldData()
    {
        json = Resources.Load<TextAsset>("Json/test").ToString();
        
        GManager.Instance.WorldData = JsonUtility.FromJson<World_Data>(json);
       
        

    }
    void AddcanSpawnItem()
    {
        List<GameObject> objs = new List<GameObject>();
        
        
        foreach (var canSpawn in GManager.Instance._canSpawns)
        {
            //Debug.Log(canSpawn.ID);
            objs.Add(Resources.Load<GameObject>($"Items/Prefabs/{canSpawn.ID}"));
            
        }

        Items = objs;
    }
    // GameObject ItemtoObject(int _ID)
    // {
    //     
    //     GameObject obj = Resources.Load<GameObject>($"Items/Prefabs/{_ID}");
    //     obj.GetComponent<Item>().RName = "";
    //     obj.GetComponent<Item>().itemImg = Resources.Load<Sprite>($"Items/item_icon_kari/{_ID}");
    //     return obj;
    // }
    void RandomWorld(nodeActive node)
    {
        AddcanSpawnItem();
        int floor_num = Random.Range(0, floors.Length);
        GManager.Instance.WorldData.WorldGens[Convert.ToInt32(node.name)].terrainType = floors[floor_num].gameObject.name;
        ObjectGenerator(node, floors[floor_num].cantWalkable_Floor, 
            WallMinimum, WallMaximum,nodeBase.nodeStatus.cantWalkable,node.parentFloor.transform); 
        ObjectGenerator(node, floors[floor_num].Debuff_Floor, 
            WallMinimum, WallMaximum,nodeBase.nodeStatus.Debuff,node.parentFloor.transform);      
        PlaceAllFloor(node,floor_num);
        EntityGenerator(node, monster, MonsterMinimum, MonsterMaximum,nodeBase.nodeStatus.Enemy,node.parentMonster.transform,layername:"Unwalkable");
        EntityGenerator(node, Items.ToArray(), (int)ItemRange.x, (int)ItemRange.y,nodeBase.nodeStatus.Item,node.parentItems.transform,layername:"Walkable");
        
        node.parentFloor.transform.localPosition += new Vector3(0f, 0f, 0.49f);      

    }
    public void PlaceAllFloor(nodeActive nodeActive,int floor_num)
    {
        List<int> dummy = new List<int>();
        List<string> dummy_obj = new List<string>();
        foreach (var node in nodeActive.nodes)
        {
            if (node.transform.GetChild(0).gameObject.GetComponent<nodeBase>()._nodeStatus == nodeBase.nodeStatus.None || 
                node.transform.GetChild(0).gameObject.GetComponent<nodeBase>()._nodeStatus == nodeBase.nodeStatus.Player)
            {
                int num = Random.Range(0, floors[floor_num].normal_Floor.Length);
                GameObject obj = Instantiate(floors[floor_num].normal_Floor[num],nodeActive.parentFloor.transform);
                obj.transform.position = node.transform.position;
                nodeActive.Floors.Add(obj);
                dummy.Add(Convert.ToInt32(node.gameObject.name));
                dummy_obj.Add(obj.name.Replace("(Clone)","").Trim());
            }
        }
        string face_name =  nodeActive.name;
        for (int i = 0; i < dummy.Count; i++)
        {
            GManager.Instance.WorldData.WorldGens[Convert.ToInt32(face_name)].node_number[dummy[i]] = dummy[i];
            GManager.Instance.WorldData.WorldGens[Convert.ToInt32(face_name)].object_name[dummy[i]] = dummy_obj[i];
            
        }



    }

    void FindAllMonster()
    {
        string[] monsters = new string[25];
        foreach (var nodeActive in _nodeActives)
        {
            for(int i = 0;i < nodeActive.nodes.Count;i++)
            {
                if (nodeActive.nodes[i].transform.GetChild(0).gameObject.GetComponent<nodeBase>()._nodeStatus == nodeBase.nodeStatus.Enemy)
                {
                    monsters[i] = nodeActive.nodes[i].gameObject.name;
                }
            }
        }

        for (int i = 0; i < GManager.Instance.WorldData.WorldGens.Length; i++)
        {
            GManager.Instance.WorldData.WorldGens[i].Enemies = monsters;
        }
    }
    public void RandomBackWorld()
    {
        List<nodeActive> notSeen = new List<nodeActive>();
        foreach (var node in _nodeActives)
        {
            if (!node.SeenFace && !node.ActivateFace)
            {
                notSeen.Add(node);
                Debug.Log(node.transform.name);
            }
        }
        
        foreach (var node in notSeen)
        {
            node.DestroyAllObject(node.remainingObject);
            node.DestroyAllObject(node.Floors);
            node.remainingObject = new List<GameObject>();
            node.Floors = new List<GameObject>();
            node.resetAllNode();
            RandomWorld(node);
        }
        
    }
    void ObjectGenerator(nodeActive nodeActive,GameObject[] obj,int min,int max,nodeBase.nodeStatus nodeStatus,Transform parent ,string layername = "Default")
    {
        List <GameObject> noneNodes = new List<GameObject>();
        List<int> dummy = new List<int>();
        List<string> dummy_obj = new List<string>();
        int numofObject = Random.Range(min, max);
        foreach (var node in nodeActive.nodes)
        {
            if (node.transform.GetChild(0).gameObject.GetComponent<nodeBase>()._nodeStatus == nodeBase.nodeStatus.None)
            {
                noneNodes.Add(node);
            }
            
        }

        for (int i = 0; i < numofObject; i++)
        {
            int numofNode = Random.Range(0, noneNodes.Count);
            GameObject g_obj = Instantiate(obj[Random.Range(0,obj.Length)], parent);
            g_obj.transform.position = noneNodes[numofNode].transform.position;
            
            nodeActive.remainingObject.Add(g_obj);
            noneNodes[numofNode].transform.GetChild(0).gameObject.GetComponent<nodeBase>()._nodeStatus = nodeStatus;
            noneNodes[numofNode].transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(layername);
            dummy.Add(Convert.ToInt32(noneNodes[numofNode].gameObject.name));
            dummy_obj.Add(g_obj.name.Replace("(Clone)","").Trim());
            noneNodes.RemoveAt(numofNode);
        }

        string face_name =  noneNodes[0].transform.parent.name;

        for (int i = 0; i < dummy.Count; i++)
        {
            GManager.Instance.WorldData.WorldGens[Convert.ToInt32(face_name)].node_number[dummy[i]] = dummy[i];
            GManager.Instance.WorldData.WorldGens[Convert.ToInt32(face_name)].object_name[dummy[i]] = dummy_obj[i];
            GManager.Instance.WorldData.WorldGens[Convert.ToInt32(face_name)].floor_status[dummy[i]] = nodeStatus;
        }


    }
    void EntityGenerator(nodeActive nodeActive,GameObject[] obj,int min,int max,nodeBase.nodeStatus nodeStatus,Transform parent ,string layername = "Default")
    {
        List <GameObject> noneNodes = new List<GameObject>();
        List<int> dummy = new List<int>();
        List<string> dummyEn = new List<string>();
        int numofObject = Random.Range(min, max);
        foreach (var node in nodeActive.nodes)
        {
            if (node.transform.GetChild(0).gameObject.GetComponent<nodeBase>()._nodeStatus == nodeBase.nodeStatus.None)
            {
                noneNodes.Add(node);
            }
            
        }

        for (int i = 0; i < numofObject; i++)
        {
            int numofNode = Random.Range(0, noneNodes.Count);
            GameObject g_obj = Instantiate(obj[Random.Range(0,obj.Length)], parent);
            g_obj.transform.position = noneNodes[numofNode].transform.position;
            nodeActive.remainingObject.Add(g_obj);
            noneNodes[numofNode].transform.GetChild(0).gameObject.GetComponent<nodeBase>()._nodeStatus = nodeStatus;
            noneNodes[numofNode].transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(layername);
            dummy.Add(Convert.ToInt32(noneNodes[numofNode].gameObject.name));
            dummyEn.Add(g_obj.name);
            noneNodes.RemoveAt(numofNode);
        }

        string face_name =  noneNodes[0].transform.parent.name;
        if (nodeStatus == nodeBase.nodeStatus.Item)
        {
            for (int i = 0; i < dummy.Count; i++)
            {
                GManager.Instance.WorldData.WorldGens[Convert.ToInt32(face_name)].Items[dummy[i]] = dummyEn[i].Replace("(Clone)","").Trim();
            }
        }else if (nodeStatus == nodeBase.nodeStatus.Enemy)
        {
            for (int i = 0; i < dummy.Count; i++)
            {
                GManager.Instance.WorldData.WorldGens[Convert.ToInt32(face_name)].Enemies[dummy[i]] = dummyEn[i].Replace("(Clone)","").Trim();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class World_Data
{
    public World_Gen[] WorldGens = new World_Gen[6];

    
    
}
[Serializable]
public class World_Gen
{
    public string terrainType;
    public int[] node_number = new int[25];
    public string[] object_name = new string[25];
    public nodeBase.nodeStatus[] floor_status = new nodeBase.nodeStatus[25];
    public string[] Items = new string[25];
    public string[] Enemies = new string[25];

   
}
