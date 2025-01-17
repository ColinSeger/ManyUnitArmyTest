using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Threading;

public class RewrittenManager : MonoBehaviour
{
    public static RewrittenManager Instance;
    bool Tester = false;
    #region Static data
    const float          MAX_SPEED = 2.0f;
    const float          REPEL_FORCE = 1.0f;
    const float          INFLUENCE_RADIUS = 6.0f;
    const float          DISTANCE_TO_CHECK = 10f;
    LayerMask layerMask;
    #endregion
    public List<UnitRewriten> unitList = new List<UnitRewriten>();
    UnitTeam[] unitsTeam;
    Vector2[] position2D;
    Neighbors[] neighbors;
    DSA.kDTree      unitTree;
    #region Movement Data
    [SerializeField] Transform Target;
    public Vector3 defaultTarget;
    MoveData[] movementData;
    PositionData[] positonData;
    #endregion
    void Awake(){
        Instance = this;
    }
    void Start()
    {
        defaultTarget = Target.position;
        layerMask = LayerMask.GetMask("Terrain");
        StartCoroutine(SetupUnits());
        StartCoroutine(RecallculateNeighbors());
        // StartCoroutine(UnitMovement(defaultTarget));     
    }
    void FixedUpdate(){
        if(Tester){
            defaultTarget = Target.position;
            UnitMovement(defaultTarget);     
        }
    }
    IEnumerator SetupUnits(){
        yield return new WaitForEndOfFrame();
        //Init all data lists after all units have spawned
        movementData = new MoveData[unitList.Count];
        positonData = new PositionData[unitList.Count];
        position2D = new Vector2[unitList.Count];
        neighbors = new Neighbors[unitList.Count];
        unitsTeam = new UnitTeam[unitList.Count];
        for(int unitId = 0; unitId < unitList.Count; unitId++){
            //Assign team
            unitsTeam[unitId] = unitList[unitId].team;

            //Movement speed
            movementData[unitId].maxSpeed = Random.Range(MAX_SPEED * 0.8f, MAX_SPEED * 1.2f);

            //Position data
            positonData[unitId].transform = unitList[unitId].positionData.transform;
            positonData[unitId].position = unitList[unitId].positionData.position;
            position2D[unitId] = unitList[unitId].position2D;

            //Create list to store neigbors
            neighbors[unitId].Neighbor = new List<Vector2>();

            //Snap to ground
            RaycastHit hit;
            Vector3 pos = positonData[unitId].position + Vector3.up;
            if(Physics.Raycast(pos, positonData[unitId].transform.TransformDirection(Vector3.down), out hit, 100f, layerMask)){
                Vector3 newPos = new Vector3(pos.x, hit.point.y, pos.z);
                positonData[unitId].transform.position = newPos;
                positonData[unitId].position = newPos;
            }
        }
        Tester = true;
    }
    #region NearestNeigborCheck
        IEnumerator RecallculateNeighbors(){
            //Waits so objects exist
            yield return new WaitForEndOfFrame();
            while(true){
                //Retrive all positions in a 2D space
                for (int i = 0; i < positonData.Length; i++){
                    position2D[i].y = positonData[i].position.y;
                    position2D[i].x = positonData[i].position.x;
                }
                //Create new kdtree
                unitTree = new DSA.kDTree(position2D);

                //Do nearest neighbor check next frame
                yield return new WaitForEndOfFrame();
                for (int i = 0; i < position2D.Length; ++i){
                    //List of nodes
                    List<DSA.kDTree.kDNode> nodesInRange = new List<DSA.kDTree.kDNode>();
                    //Assign nodes in range
                    unitTree.FindNodesInRange(position2D[i], INFLUENCE_RADIUS, nodesInRange);

                    //Debug.Log(nodesInRange.Count);

                    // assign neighbors
                    neighbors[i].Neighbor.Clear();
                    foreach (DSA.kDTree.kDNode node in nodesInRange){
                        neighbors[i].Neighbor.Add(node.m_value);
                    }
                }                
            }
        }
    #endregion
    void UnitMovement(Vector3 target){
        for (int i = 0; i < positonData.Length; i++){
            // face target
            positonData[i].transform.LookAt(new Vector3(target.x, positonData[i].position.y, target.z));
            
            //Move to target I think?
            //movementData[i].mySpeed = movementData[i].maxSpeed;
            positonData[i].position += positonData[i].transform.forward * movementData[i].maxSpeed * Time.fixedDeltaTime;
            
            

            //Snap to ground and move or just move
            RaycastHit hit;
            Vector3 pos = positonData[i].position + Vector3.up;
            if(Physics.Raycast(pos, positonData[i].transform.TransformDirection(Vector3.down), out hit, DISTANCE_TO_CHECK, layerMask)){
                Vector3 newPos = new Vector3(pos.x, hit.point.y, pos.z);
                positonData[i].transform.position = newPos;
                positonData[i].position = newPos;
            }else{
                positonData[i].transform.position = positonData[i].position;                
            }
        }
    }
}
