using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Team : MonoBehaviour
    {
        public const int ARMY_SIZE = 50;
        public UnitTeam unitTeam = UnitTeam.Vikings;
        void Awake()
        {
            
            //Old();
            NewOne();
        }
        void Old(){
            Debug.Log("Actual team size" + ARMY_SIZE * (ARMY_SIZE*2));
            // create army
            Unit unit = GetComponentInChildren<Unit>(true);
            for (int z = 0; z < ARMY_SIZE; ++z)
            {
                for (int x = -ARMY_SIZE; x <= ARMY_SIZE; x++)
                {
                    GameObject go = Instantiate(unit.gameObject, unit.transform.parent);
                    go.transform.localPosition = new Vector3(x, 0, z) * 1.5f;
                    go.name = unit.name + "_" + x + "_" + z;
                    go.SetActive(true);
                    //RewrittenManager.Instance.unitList.Add(go.GetComponent<UnitRewriten>());
                }
            }
        }
        void NewOne(){
            Debug.Log("Actual team size" + ARMY_SIZE * (ARMY_SIZE*2));
            // create army
            UnitRewriten unit = GetComponentInChildren<UnitRewriten>(true);
            for (int z = 0; z < ARMY_SIZE; ++z)
            {
                for (int x = -ARMY_SIZE; x <= ARMY_SIZE; x++)
                {
                    GameObject go = Instantiate(unit.gameObject, unit.transform.parent);
                    go.transform.localPosition = new Vector3(x, 0, z) * 1.5f;
                    go.name = unit.name + "_" + x + "_" + z;
                    go.SetActive(true);
                    UnitRewriten unitData = go.GetComponent<UnitRewriten>();
                    unitData.team = unitTeam;
                    RewrittenManager.Instance.unitList.Add(unitData);
                }
            }
        }
    }
}