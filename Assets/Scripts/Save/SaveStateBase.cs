using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStateBase : MonoBehaviour
{
    //return JSON information about the state 
    public virtual string SaveState()
    {
        return null;
    }


    //take back in JSON and load into state 
    public virtual void LoadState(string loadedJSON)
    {

    }

    //whether or not the state shoudl be saved 
    public virtual bool ShouldSave()
    {
        return true;
    }


    public virtual string GetUId()
    {
        return (gameObject.scene.name + "_" + gameObject.name + "_" + (this.GetType()));
    }

    public virtual bool ShouldLoad()
    {
        return true;
    }
}
