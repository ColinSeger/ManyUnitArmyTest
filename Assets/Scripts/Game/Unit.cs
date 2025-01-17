using DSA;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game
{
    public class Unit : MonoBehaviour
    {
        private Team                m_team;
        private float               m_fSpeed;
        private float               m_fMaxSpeed;
        private float               m_fCycle;

        public const float          MAX_SPEED = 2.0f;
        public const float          REPEL_FORCE = 1.0f;
        public const float          INFLUENCE_RADIUS = 6.0f;


        #region Properties

        public Team Team => m_team;
        public Transform unitTransform;
        public Vector3 unitPosition;

        public Vector2 Position2D => new Vector2(unitTransform.position.x, unitTransform.position.z);

        public List<Unit> NeighborUnits { get; set; }
        Vector3 vForce;
        Unit closestEnemy = null;
        float timer = 0f;
        Thread thread;
        #endregion

        private void Start()
        {
            m_team = GetComponentInParent<Team>();
            //vForce = transform.forward * 0.1f;
            unitTransform = this.transform;
            unitPosition = this.transform.position;

            // get random max speed
            m_fMaxSpeed = Random.Range(MAX_SPEED * 0.8f, MAX_SPEED * 1.2f);

            // bobbing cycle
            m_fCycle = Random.Range(0.0f, Mathf.PI * 2.0f);

            UnitManager.Instance.AllUnits.Add(this);
            StartCoroutine(UpdatePos());
            //thread = new Thread(SmarterUpdate);
            //StartCoroutine(EndOfFrame());
        }
        private void FixedUpdate()
        {
            SmarterUpdate();
            timer = Time.fixedDeltaTime;
        }
        private IEnumerator UpdatePos(){
            while (true){
                //unitTransform = this.transform;
                //unitTransform.position = this.transform.position;
                this.transform.position = unitTransform.position;
                unitPosition = unitTransform.position;
                yield return  new WaitForSeconds(0.01f);
            }
        }
        private IEnumerator EndOfFrame(){
            yield return new WaitForSeconds(1);
            //Thread thread = new Thread(SmarterUpdate);
            if (!thread.IsAlive)
            {
                Debug.Log(thread.IsAlive);
                thread.Abort();
                thread.Start();
            }
        }
        private void SmarterUpdate()
        {
            if (NeighborUnits == null) return;
            //Debug.Log(NeighborUnits.Count);
            vForce = m_team.transform.forward * 0.1f;

            // default to teams forward
            
            float fClosestEnemyDistance = float.MaxValue;

            foreach (Unit unit in NeighborUnits){
                if (unit == this) continue;

                // is unit inside influence distance?
                Vector3 vToUnit = unit.unitPosition - unitPosition;
                float fDistance = vToUnit.sqrMagnitude;
                //if (fDistance > INFLUENCE_RADIUS) continue;
                

                // is closest enemy?
                //bool bIsFriend = unit.Team == Team;
                // repel away from friendlies?
                if (unit.Team == Team)
                {
                    float fAmount = Mathf.Clamp01(1.0f - (fDistance / INFLUENCE_RADIUS));
                    vForce -= vToUnit.normalized * fAmount * REPEL_FORCE;
                    continue;
                }                
                else if (fDistance < fClosestEnemyDistance)
                {
                    closestEnemy = unit;
                    fClosestEnemyDistance = fDistance;
                }
            }

            MoveAndFightLogic(vForce, closestEnemy);
        }
        
        private void MoveAndFightLogic(Vector3 vDirection, Unit closestEnemy)
        {
            // got enemy to fight?
            //Vector3 myPosition = unitTransform.position;
            if(closestEnemy)
            {
                Vector3 enemyPosition = closestEnemy.unitPosition;
                float fClosestEnemyDistance = closestEnemy != null ? Vector3.Distance(enemyPosition, unitPosition) : float.MaxValue;
                if (fClosestEnemyDistance < 1.5f)
                {
                    // TODO: do some fighting here
                    // subtract HP, blood... gore... death etc.
                    m_fSpeed = Mathf.MoveTowards(m_fSpeed, 0.0f, timer * m_fMaxSpeed * 10.0f);
                    vDirection = Vector3.Normalize(enemyPosition - unitPosition);
                }
                else
                {
                    // move towards closest enemy
                    vDirection += Vector3.Normalize(enemyPosition - unitPosition);
                }
            }
            m_fSpeed = Mathf.MoveTowards(m_fSpeed, m_fMaxSpeed, timer);
            
            // move
            this.transform.position += vDirection.normalized * m_fSpeed * timer;
            //Debug.Log(m_fSpeed);
            //unitPosition += vDirection.normalized * m_fSpeed * timer;

            // face target
            if (vDirection.magnitude > 0.09f)
            {
                //unitTransform.rotation = Quaternion.Slerp(unitTransform.rotation, Quaternion.LookRotation(vDirection), timer * 2.0f);
            }

            //MovementAnimation();
        }
        void MovementAnimation()
        {
            // 'movement' rotation bobbing
            m_fCycle += Time.deltaTime * m_fSpeed * Mathf.PI;
            m_fCycle %= Mathf.PI * 2.0f;
            Vector3 vLR = unitTransform.localEulerAngles;
            vLR.x = 0.0f;
            vLR.z = Mathf.Sin(m_fCycle) * 10.0f;
            unitTransform.localEulerAngles = vLR;

            // 'movement' position bobbing
            Vector3 vPos = unitTransform.position;
            vPos.y += Mathf.Abs(Mathf.Cos(m_fCycle) * 0.2f);
            unitTransform.position = vPos;
        }
    }
}