using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSA
{
    public class kDTree : BinaryTree<Vector2>
    {
        public class kDNode : Node
        {
            public int      m_iIndex;
        }

        #region Nearest Neighbor Search

        public kDNode FindNearestNeighbor(Vector2 v, List<kDNode> visitedNodes)
        {
            return FindNearestNeighbor(m_root as kDNode, v, 0, visitedNodes);
        }

        protected kDNode FindNearestNeighbor(kDNode node, Vector2 v, int iDimension, List<kDNode> visitedNodes)
        {
            if (node == null) return null;

            if (visitedNodes != null) visitedNodes.Add(node);

            // decide which branch to search
            bool bSearchLeft = v[iDimension] < node.m_value[iDimension];
            kDNode nextBranch = (bSearchLeft ? node.m_left : node.m_right) as kDNode;
            kDNode otherBranch = (bSearchLeft ? node.m_right : node.m_left) as kDNode;

            // calculate next dimension
            int iNextDimension = (iDimension + 1) % 2;

            // get the best node
            kDNode temp = FindNearestNeighbor(nextBranch, v, iNextDimension, visitedNodes);
            kDNode best = GetClosest(v, temp, node);

            // should we search the other branch?
            float fDistanceToBest = Vector2.SqrMagnitude(best.m_value - v);
            float fDistanceToPlane = Mathf.Abs(v[iDimension] - node.m_value[iDimension]);
            if (fDistanceToBest >= fDistanceToPlane)
            {
                temp = FindNearestNeighbor(otherBranch, v, iNextDimension, visitedNodes);
                best = GetClosest(v, temp, best);
            }

            return best;
        }

        public kDNode GetClosest(Vector2 v, kDNode A, kDNode B)
        {
            float fDistanceA = A != null ? Vector2.SqrMagnitude(A.m_value - v) : float.MaxValue;
            float fDistanceB = B != null ? Vector2.SqrMagnitude(B.m_value - v) : float.MaxValue;
            return fDistanceA < fDistanceB ? A : B;
        }

        #endregion

        #region Points in Range

        public void FindNodesInRange(Vector2 v, float fRange, List<kDNode> nodesInRange /*, List<kDNode> visitedNodes*/)
        {
            FindNodesInRange(m_root as kDNode, v, 0, fRange, nodesInRange);
        }

        private void FindNodesInRange(kDNode node, Vector2 vectorPos2,int dim, float fRange, List<kDNode> nodesInRange)
        {
            if (node == null || nodesInRange.Count > 6) return;
            //if(nodesInRange.Count > 5) return;

            // is node in range?
            float fDistanceToNode = Vector2.SqrMagnitude(node.m_value - vectorPos2);
            
            //if(fDistanceToNode > fRange) return;
            if (fDistanceToNode <= fRange){
                //var t = node.m_value - vectorPos2;
                //Debug.Log(fDistanceToNode + " "  + t);
                nodesInRange.Add(node);
            }
            

            // decide which branch to search
            bool bSearchLeft = vectorPos2[dim] < node.m_value[dim];
            /*
            kDNode nextBranch;
            kDNode otherBranch;
            if(vectorPos2.x < node.m_value.x){
                nextBranch = node.m_left as kDNode;
                //otherBranch = node.m_right as kDNode;
            }
            else{
                otherBranch = node.m_right as kDNode;
                //otherBranch = node.m_left as kDNode;
            }
            if(vectorPos2.y < node.m_value.y){
                //nextBranch = node.m_left as kDNode;
                otherBranch = node.m_left as kDNode;
            }
            else{
                //nextBranch = node.m_left as kDNode;
                otherBranch = node.m_right as kDNode;
            }*/
            kDNode nextBranch = (bSearchLeft ? node.m_left : node.m_right) as kDNode;
            kDNode otherBranch = (bSearchLeft ? node.m_right : node.m_left) as kDNode;

            // calculate next dimension
            int iNextDimension = (dim + 1) % 2;

            // search next branch
            FindNodesInRange(nextBranch, vectorPos2, iNextDimension, fRange, nodesInRange);

            // should we search the other branch?
            float fDistanceToPlane = Mathf.Abs(vectorPos2.x - node.m_value.x);

            if (fDistanceToPlane <= fRange)
            {
                FindNodesInRange(otherBranch, vectorPos2, dim, fRange, nodesInRange);
            }
        }

        #endregion

        #region Tree Creation

        public kDTree(Vector2[] points)
        { 
            // create index array
            int[] indices = new int[points.Length];
            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = i;
            }

            // create root
            m_root = CreateNode(points, indices, 0, 0, points.Length - 1);
        }

        private static kDNode CreateNode(Vector2[] points, int[] indices, int iDimension, int iStart, int iEnd)
        {
            // no points?
            if (iStart > iEnd) return null;
            
            // Sort points in dimension
            Sort(points, indices, iDimension, iStart, iEnd);

            // Select Median value
            int iMiddle = (iStart + iEnd) / 2;
            Vector2 vMedian = points[iMiddle];

            // calculate next dimension
            int iNextDimension = (iDimension + 1) % 2;

            // create node
            kDNode node = new kDNode
            {
                m_value = vMedian,
                m_iIndex = indices[iMiddle],
                m_left = CreateNode(points, indices, iNextDimension, iStart, iMiddle - 1),
                m_right = CreateNode(points, indices, iNextDimension, iMiddle + 1, iEnd),
            };

            return node;
        }

        private static int Partition(Vector2[] points, int[] indices, int iDimension, int iStart, int iEnd)
        {
            // select the pivot value
            Vector2 vPivotValue = points[(iStart + iEnd) / 2];
            int iLeft = iStart;
            int iRight = iEnd;

            while (iLeft <= iRight){
                // move left index until a value greater or equal to the pivot is found
                while (points[iLeft][iDimension] < vPivotValue[iDimension]){
                    iLeft++;
                }

                // move right index until a value less or equal to the pivot is found
                while (points[iRight][iDimension] > vPivotValue[iDimension]){
                    iRight--;
                }

                // should we swap?
                if (iLeft <= iRight)
                {
                    // ... otherwise swap
                    Vector2 vTemp = points[iLeft];
                    points[iLeft] = points[iRight];
                    points[iRight] = vTemp;

                    int iTemp = indices[iLeft];
                    indices[iLeft] = indices[iRight];
                    indices[iRight] = iTemp;

                    iLeft++;
                    iRight--;
                }
            }

            return iLeft;
        }

        private static void Sort(Vector2[] points, int[] indices, int iDimension, int iStart, int iEnd)
        {
            if (iStart < iEnd)
            {
                // Partition the array
                int iPivotIndex = Partition(points, indices, iDimension, iStart, iEnd);

                // Send left side off to QuickSort
                Sort(points, indices, iDimension, iStart, iPivotIndex - 1);

                // Send right side off to QuickSort
                Sort(points, indices, iDimension, iPivotIndex, iEnd);
            }
        }

        #endregion
    }
}