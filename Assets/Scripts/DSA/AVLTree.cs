using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSA
{
    public class AVLTree : BinarySearchTree
    {
        public static Node RotateRight(Node node)
        {
            Node oldLeft = node.m_left;
            Node newLeft = oldLeft.m_right;

            // right rotation
            oldLeft.m_right = node;
            node.m_left = newLeft;

            // return the new root
            return oldLeft;
        }

        public static Node RotateLeft(Node node)
        {
            Node oldRight = node.m_right;
            Node newRight = oldRight.m_left;

            // left rotation
            oldRight.m_left = node;
            node.m_right = newRight;

            // return the new root
            return oldRight;
        }

        protected static Node BalanceNode(Node node)
        {
            int iBalance = GetBalance(node);

            // Left Left
            if (iBalance > 1 && GetBalance(node.m_left) >= 0)
            {
                return RotateRight(node);
            }

            // Right Right
            if (iBalance < -1 && GetBalance(node.m_right) <= 0)
            {
                return RotateLeft(node);
            }

            // Left Right
            if (iBalance > 1 && GetBalance(node.m_left) < 0)
            {
                node.m_left = RotateLeft(node.m_left);
                return RotateRight(node);
            }

            // Right Left
            if (iBalance < -1 && GetBalance(node.m_right) > 0)
            {
                node.m_right = RotateRight(node.m_right);
                return RotateLeft(node);
            }

            return node;
        }

        protected override Node Insert(Node node, int elem)
        {
            // no longer support duplicate values
            if (node != null && node.m_value == elem)
            {
                return node;
            }

            node = base.Insert(node, elem);

            return BalanceNode(node);
        }

        protected override Node Remove(Node node, int elem)
        {
            node = base.Remove(node, elem);
            return BalanceNode(node);
        }
    }
}