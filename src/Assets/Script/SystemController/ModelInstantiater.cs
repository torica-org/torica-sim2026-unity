using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelInstantiater : MonoBehaviour
{
    public void OnEnables()
    {
        if(GameManager.instance.game.PlaneName == null){
            GameManager.instance.game.PlaneName = GameManager.instance.game.DefaultPlane;
        }

        GameObject PlaneObj = (GameObject)Resources.Load(GameManager.instance.game.PlaneName);
        var obj = Instantiate(PlaneObj, new Vector3(PlaneObj.transform.position.x,PlaneObj.transform.position.y,PlaneObj.transform.position.z), Quaternion.identity);
        obj.name = PlaneObj.name;
        
        GameManager.instance.game.Plane = GameObject.Find(GameManager.instance.game.PlaneName);
    }
}
