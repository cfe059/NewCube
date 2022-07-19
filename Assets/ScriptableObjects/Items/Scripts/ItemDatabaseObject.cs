using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
#endif

public class ItemDatabaseObject : ScriptableObject,ISerializationCallbackReceiver
{
    public ItemObject[] Items;
    public Dictionary<ItemObject, int> getID = new Dictionary<ItemObject, int>();
    public Dictionary<int, ItemObject> getItem = new Dictionary<int, ItemObject>();
    public void OnAfterDeserialize()
    {
        getID = new Dictionary<ItemObject, int>();
        getItem = new Dictionary<int,ItemObject>();
        for (int i = 0; i < Items.Length; i++)
        {
            getID.Add(Items[i], i);
            getItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
     }
}
