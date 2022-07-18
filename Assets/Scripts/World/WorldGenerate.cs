using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

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
    
    
    void Start()
    {
        AddcanSpawnItem();
        foreach (var face in _Faces)
        {
            _nodeActives.Add(face.GetComponent<nodeActive>());
        }

        foreach (var node in _nodeActives)
        {
            RandomWorld(node);
        }

        
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
        ObjectGenerator(node, floors[floor_num].cantWalkable_Floor, 
            WallMinimum, WallMaximum,nodeBase.nodeStatus.cantWalkable,node.parentFloor.transform); 
        ObjectGenerator(node, floors[floor_num].Debuff_Floor, 
            WallMinimum, WallMaximum,nodeBase.nodeStatus.Debuff,node.parentFloor.transform);      
        PlaceAllFloor(node,floor_num);
        ObjectGenerator(node, monster, MonsterMinimum, MonsterMaximum,nodeBase.nodeStatus.Enemy,node.parentMonster.transform,layername:"Unwalkable");
        ObjectGenerator(node, Items.ToArray(), (int)ItemRange.x, (int)ItemRange.y,nodeBase.nodeStatus.Item,node.parentItems.transform,layername:"Walkable");
        
        node.parentFloor.transform.localPosition += new Vector3(0f, 0f, 0.49f);      

    }
    public void PlaceAllFloor(nodeActive nodeActive,int floor_num)
    {
        
        foreach (var node in nodeActive.nodes)
        {
            if (node.transform.GetChild(0).gameObject.GetComponent<nodeBase>()._nodeStatus == nodeBase.nodeStatus.None || 
                node.transform.GetChild(0).gameObject.GetComponent<nodeBase>()._nodeStatus == nodeBase.nodeStatus.Player)
            {
                int num = Random.Range(0, floors[floor_num].normal_Floor.Length);
                GameObject obj = Instantiate(floors[floor_num].normal_Floor[num],nodeActive.parentFloor.transform);
                obj.transform.position = node.transform.position;
                nodeActive.Floors.Add(obj);
            }
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
            node.resetAllNode();
            RandomWorld(node);
        }
    }
    void ObjectGenerator(nodeActive nodeActive,GameObject[] obj,int min,int max,nodeBase.nodeStatus nodeStatus,Transform parent ,string layername = "Default")
    {
        List <GameObject> noneNodes = new List<GameObject>();
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
            noneNodes.RemoveAt(numofNode);
        }
       


    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
