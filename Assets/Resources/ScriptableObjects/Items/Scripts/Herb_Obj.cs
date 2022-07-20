using UnityEngine;

public class Herb_Obj : itemData_Object
{
    
    public string _name;

    public int hungry;
    public int Maxhungry;
    public int hp;
    public Buff_Obj effect;
    public string description;


    public void addEffect(Buff_Obj ef)
    {
        effect = ef;
    }
}
