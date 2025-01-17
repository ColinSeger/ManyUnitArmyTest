using DSA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class UnitManager : MonoBehaviour
    {
        public static UnitManager   Instance;
        public List<Unit>           AllUnits = new List<Unit>();
        kDTree                      unitTree;
        void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            //StartCoroutine(TreeCreation());
            StartCoroutine(UnitUpdateLogic());
        }
        IEnumerator TreeCreation(){
            while(true){
                if(AllUnits.Count <= 0) yield return null;
                // first create kD Tree
                unitTree = new kDTree(AllUnits.ConvertAll(u => u.Position2D).ToArray());
                yield return new WaitForSeconds(0.02f);                
            }
        }

        IEnumerator UnitUpdateLogic()
        {
            while (true){
                if(AllUnits.Count <= 0) yield return null;
                unitTree = new kDTree(AllUnits.ConvertAll(u => u.Position2D).ToArray());

                for (int i = 0; i < AllUnits.Count; ++i)
                {
                    // get nodes in range
                    Unit unit = AllUnits[i];
                    List<kDTree.kDNode> nodesInRange = new List<kDTree.kDNode>();
                    unitTree.FindNodesInRange(unit.Position2D, Unit.INFLUENCE_RADIUS, nodesInRange);

                    // assign neighbors
                    unit.NeighborUnits = nodesInRange.ConvertAll(n => AllUnits[n.m_iIndex]);

                    // sleep 1 frame?
                    if (i % 100 == 0)
                    {
                        yield return null;
                    }
                }
            }
        }
    }
}