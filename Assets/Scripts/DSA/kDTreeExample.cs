using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DSA
{
    [ExecuteInEditMode]
    public class kDTreeExample : MonoBehaviour
    {
        public kDTree   m_tree;

        #region Properties

        #endregion

        private void OnEnable()
        {
            // create random points
            Vector2[] points = new Vector2[32];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new Vector2(Random.Range(0.2f, 9.8f), Random.Range(0.2f, 9.8f));
            }

            // create tree
            m_tree = new kDTree(points);
        }
    }
}