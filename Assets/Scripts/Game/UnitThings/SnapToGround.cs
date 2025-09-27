using System.Collections;
using UnityEngine;

public class SnapToGround : MonoBehaviour
{
    //public const float          TERRAIN_SIZE = 256.0f;
    const float DISTANCE_TO_CHECK = 2f;
    LayerMask layerMask;
    //private Terrain             m_terrain;
    //private Transform           myTransform;
    
    //protected Vector2 TerrainPosition => new Vector2(myTransform.position.x / TERRAIN_SIZE, myTransform.position.z / TERRAIN_SIZE);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //myTransform = this.transform;
        //m_terrain = GetComponentInParent<Terrain>();
        layerMask = LayerMask.GetMask("Terrain");
        float startDistance = 500f;
        RayCastToGround(startDistance);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //StartCoroutine(SnapToTerrain());
        //SnapToTerrain();
        //RayCastToGround(DISTANCE_TO_CHECK);
    }
    
    public void RayCastToGround(float distance){
        RaycastHit hit;
        Transform trsform = this.transform;
        Vector3 pos = trsform.position + Vector3.up;
        if(Physics.Raycast(pos, trsform.TransformDirection(Vector3.down), out hit, distance, layerMask)){
            transform.position = new Vector3(pos.x, hit.point.y, pos.z);
        }
    }
/*
    public void SnapToTerrain()
    {
        Vector2 v = TerrainPosition;
        float fHeight = m_terrain.terrainData.GetInterpolatedHeight(v.x, v.y);
        Vector3 vPos = myTransform.position;
        vPos.y = fHeight;
        myTransform.position = vPos;
        //yield return new WaitForSeconds(0.2f);
    }
*/
}
