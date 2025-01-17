using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Web;
using System.Security.Cryptography.X509Certificates;

namespace DSA
{
    [CustomEditor(typeof(kDTreeExample))]
    public class kDTreeExampleEditor : Editor
    {
        private Gradient                m_valueColors;
        private Vector2                 m_vTestPoint = Vector2.one * 5.0f;
        private List<kDTree.kDNode>     m_visitedNodes = new List<kDTree.kDNode>();
        private List<kDTree.kDNode>     m_nodesInRange = new List<kDTree.kDNode>();

        private void OnEnable()
        {
            m_valueColors = new Gradient();
            m_valueColors.colorKeys = new GradientColorKey[]{
                new GradientColorKey(Color.red, 0.0f),
                new GradientColorKey(new Color(1.0f, 0.5f, 0.0f), 0.2f),
                new GradientColorKey(Color.yellow, 0.4f),
                new GradientColorKey(Color.green, 0.6f),
                new GradientColorKey(Color.blue, 0.8f),
                new GradientColorKey(new Color(1.0f, 0.0f, 1.0f), 1.0f)
            };
        }

        private void OnSceneGUI()
        {
            kDTreeExample te = target as kDTreeExample;

            // draw dimension lines
            int iDepth = te.m_tree.Depth;
            float fSize = Mathf.Pow(iDepth, 2) * 0.5f + 0.2f;
            Handles.color = new Color(0.0f, 0.0f, 0.0f, 0.7f);
            for (int i = 0; i < iDepth; ++i)
            {
                Handles.DrawLine(new Vector3(-fSize, i * -3.0f, 0.0f), new Vector3(fSize, i * -3.0f, 0.0f), 2.0f);
                DSAEditorUtils.DrawTextAt(i % 2 == 0 ? "X" : "Y", new Vector3(-fSize - 0.5f, i * -3.0f), 18.0f, Handles.color, TextAnchor.MiddleCenter);
            }


            // draw kD tree
            Tools.current = Tool.None;
            //DSAEditorUtils.DrawBinaryTree(te.m_tree, ColorCallback, (Vector2 v) => "", NodeDrawingCallback);
            DSAEditorUtils.DrawBinaryTree(te.m_tree, (Vector2 v, int j) => Color.gray, (Vector2 v) => "", NodeDrawingCallback);

            // draw points
            Handles.matrix = Matrix4x4.Translate(new Vector3(13.0f, -10.0f, 0.0f));
            Rect rect = new Rect(0, 0, 10, 10);
            DSAEditorUtils.DrawRect(rect, 0.0f, Color.white, Color.black, 3.0f);
            DrawNode(te.m_tree.m_root as kDTree.kDNode, rect, 0);

#if true
            // test point
            m_vTestPoint = Handles.DoPositionHandle(m_vTestPoint, Quaternion.identity);
            Handles.color = new Color(1.0f, 0.5f, 0.0f);
            Handles.SphereHandleCap(0, m_vTestPoint, Quaternion.identity, 0.2f, EventType.Repaint);
#endif

#if false
            // got nearest neighbor
            m_visitedNodes.Clear();
            kDTree.kDNode nn = te.m_tree.FindNearestNeighbor(m_vTestPoint, m_visitedNodes);
            if (nn != null)
            {
                Handles.color = Color.red;
                Handles.DrawDottedLine(m_vTestPoint, nn.m_value, 5.0f);
            }
#endif

#if true
            // get neighbors in range
            m_visitedNodes.Clear();
            m_nodesInRange.Clear();
            Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.25f);
            Handles.SphereHandleCap(0, m_vTestPoint, Quaternion.identity, 1.0f, EventType.Repaint);
            te.m_tree.FindNodesInRange(m_vTestPoint, 0.5f, m_nodesInRange /*, m_visitedNodes*/);

            Handles.color = Color.red;
            foreach(kDTree.kDNode nn in m_nodesInRange)
            {
                Handles.DrawDottedLine(m_vTestPoint, nn.m_value, 5.0f);
            }

            // draw elements in range
            Handles.matrix *= Matrix4x4.Translate(new Vector3(0.0f, -1.5f, 0.0f));
            DSAEditorUtils.DrawArray<kDTree.kDNode>(m_nodesInRange.ToArray(), (kDTree.kDNode n, int i) => ColorCallback(n.m_value, i), (kDTree.kDNode n) => n.m_iIndex.ToString(), false);
#endif

            Handles.matrix = Matrix4x4.identity;
        }

        void NodeDrawingCallback(kDTree.Node node, Rect rect)
        {
            kDTreeExample te = target as kDTreeExample;
            kDTree.kDNode kdNode = node as kDTree.kDNode;

            if (m_visitedNodes.Contains(kdNode))
            {
                DSAEditorUtils.DrawRect(rect, 0.0f, new Color(1.0f, 0.5f, 0.0f), Color.black, 5.0f);
            }

            Vector3 vSP = HandleUtility.WorldToGUIPointWithDepth(new Vector3(rect.center.x, rect.yMax, 0.0f));
            if (vSP.z > 0.0f)
            {
                Vector2 v = kdNode.m_value;
                Rect sr = new Rect(vSP.x - 300.0f, vSP.y - 303.0f, 600.0f, 300.0f);
                Handles.BeginGUI();
                DSAEditorUtils.DrawText(v.x.ToString("0.0") + ", " + v.y.ToString("0.0"), sr, 10.0f, Color.white, TextAnchor.LowerCenter);
                Handles.EndGUI();
            }

            DSAEditorUtils.DrawTextAt(kdNode.m_iIndex.ToString(), new Vector3(rect.center.x, rect.center.y, 0.0f), 18.0f, Color.black, TextAnchor.MiddleCenter);
          
        }

        void DrawNode(kDTree.kDNode node, Rect rect, int iDimension)
        {
            if (node == null)
            {
                return;
            }

            // get color
            Vector2 v = node.m_value;
            float f = (v.x + v.y) / 20.0f;
            Handles.color = m_valueColors.Evaluate(f);

            // draw split line
            Rect[] splits = new Rect[2];
            switch (iDimension)
            {
                case 0:
                    Handles.DrawLine(new Vector3(v.x, rect.yMin, 0.0f), new Vector3(v.x, rect.yMax, 0.0f), 3.0f);
                    splits[0] = new Rect(rect.x, rect.y, v.x, rect.height);
                    splits[1] = new Rect(v.x, rect.y, rect.xMax - v.x, rect.height);
                    break;

                case 1:
                    Handles.DrawLine(new Vector3(rect.xMin, v.y, 0.0f), new Vector3(Mathf.Min(rect.xMax, 10.0f), v.y, 0.0f), 5.0f);
                    splits[0] = new Rect(rect.x, rect.y, rect.width, v.y);
                    splits[1] = new Rect(rect.x, v.y, rect.width, rect.yMax - v.y);
                    break;
            }

            // draw point
            Handles.SphereHandleCap(0, v, Quaternion.identity, 0.3f, EventType.Repaint);

            // draw index
            DSAEditorUtils.DrawTextAt(node.m_iIndex.ToString(), v + Vector2.one * 0.25f, 16, Color.black, TextAnchor.MiddleCenter);

            // draw next points
            int iNextDimension = (iDimension + 1) % 2;
            DrawNode(node.m_left as kDTree.kDNode, splits[0], iNextDimension);
            DrawNode(node.m_right as kDTree.kDNode, splits[1], iNextDimension);
        }

        Color ColorCallback(Vector2 v, int j) 
        {
            float f = (v.x + v.y) / 20.0f; 
            return m_valueColors.Evaluate(f); 
        }
    }
}