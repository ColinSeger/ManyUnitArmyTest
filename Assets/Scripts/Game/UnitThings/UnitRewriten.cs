
using System;
using System.Collections.Generic;
using UnityEngine;

public enum UnitTeam : byte{
    Vikings,
    Romans
}
// [Serializable]
// public struct UnitData{
//     public Team team;
//     public Transform transform;
//     public Vector3 position;
//     public Vector2 Position2D;
//     public float mySpeed;
//     public float maxSpeed;
// }
public struct PositionData{
    public Transform transform;
    public Vector3 position;
}
public struct MoveData{
    public float mySpeed;
    public float maxSpeed;
}
public struct Neighbors{
    public List<Vector2> Neighbor;    
}

public class UnitRewriten : MonoBehaviour
{
    //public UnitData unitData = new UnitData();
    public UnitTeam team = UnitTeam.Vikings;
    public Vector2 position2D;
    public PositionData positionData = new PositionData();
    public MoveData moveData = new MoveData();
    public Neighbors neighbors = new Neighbors();

    private void Start(){
        // unitData.position = this.transform.position;
        positionData.transform = this.transform;
        positionData.position = positionData.transform.position;
        position2D = new Vector2(positionData.position.x, positionData.position.z);
        //Debug.Log(position2D + " " + positionData.position);
    }
}
