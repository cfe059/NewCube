using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Newtonsoft.Json;

public class PlayfabManager : MonoBehaviour
{
    [SerializeField] private WorldGenerate _worldData;
    [SerializeField] private PlayerBase _player;
    // Start is called before the first frame update
    void Awake()
    {
        LoginPlayfab();
    }

    public void LoginPlayfab()
    {
        
        var request = new LoginWithCustomIDRequest { CustomId = "0", CreateAccount = true};
        PlayFabClientAPI.LoginWithCustomID(request, r =>
        {
            Debug.Log("Login Done");
        }, e =>
        {
            Debug.Log(e.Error);
        });

    }

    
    public void LoadWorld()
    {
        PlayFabClientAPI.GetUserData( new GetUserDataRequest(), r =>
        {
            if (r.Data != null && r.Data.ContainsKey("World"))
            {
                var rotate =  JsonConvert.DeserializeObject<WorldRotate>(r.Data["World Rotate"].Value);
                GManager.Instance.WorldData = JsonConvert.DeserializeObject<World_Data>(r.Data["World"].Value);
                _worldData.GenerateFormSave();
                _worldData.gameObject.transform.eulerAngles = new Vector3(rotate.x,rotate.y,rotate.z);
                _worldData.gameObject.GetComponent<rotate_world>()._target.transform.eulerAngles = new Vector3(rotate.x,rotate.y,rotate.z);
                
            }
            if (r.Data != null && r.Data.ContainsKey("Inventory"))
            {
                var display = GameObject.Find("InventorySystem").GetComponent<DisplayInventory>();
              //  Debug.Log(display._display.Count);
               // Debug.Log(JsonUtility.FromJson<Inventory>(r.Data["Inventory"].Value).Items.Count);
                _player.LoadInventory(JsonUtility.FromJson<Inventory>(r.Data["Inventory"].Value));
            
            }
            if (r.Data != null && r.Data.ContainsKey("PlayerInfo"))
            {
                var display = GameObject.Find("InventorySystem").GetComponent<DisplayInventory>();
            
                //_player.LoadData(JsonConvert.DeserializeObject<Player>(r.Data["PlayerInfo"].Value));
                _player.LoadData(JsonUtility.FromJson<Player>(r.Data["PlayerInfo"].Value));
            
                
            }

           
        }, e =>
        {
            
        });
    }
   
    public void SaveWorld()
    {
        //LoginPlayfab();
        var jsonData = GManager.Instance.WorldData;
        WorldRotate rotate = new WorldRotate(_worldData.gameObject.transform.eulerAngles.x
            ,_worldData.gameObject.transform.eulerAngles.y,
            _worldData.gameObject.transform.eulerAngles.z);
        var World = JsonUtility.ToJson(jsonData);// Get World Data to Json
        var World_rotate = JsonUtility.ToJson(rotate);// Get World rotate to json
        var _playerData =  JsonUtility.ToJson(_player._PLayerData); //Get PlayerData to Json
        var _player_inventory = JsonUtility.ToJson(_player._inventory.Container); // get inventory to json
        var request = new UpdateUserDataRequest　//data post
        {
            Data = new Dictionary<string, string>
            {
                {"World" , World},
                {"World Rotate", World_rotate},
                {"PlayerInfo",_playerData},
                {"Inventory",_player_inventory},
                
            }
            
        };
        PlayFabClientAPI.UpdateUserData(request, r =>
        {
            Debug.Log(r.ToString());
        }, e =>
        {
            Debug.Log(e.Error);
        });

    
    }
}

[Serializable]
public class WorldRotate 
{
    public float x, y, z;

    public WorldRotate(float _x,float _y,float _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }
}
