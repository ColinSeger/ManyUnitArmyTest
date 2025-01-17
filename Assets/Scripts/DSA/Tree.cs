using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSA
{
    public class Tree<T>
    {
        public class Node
        {
            public T        m_value;
            public Node[]   m_children;

            #region Properties

            public IEnumerable<Node> Children
            {
                get
                {
                    if (m_children != null)
                    {
                        foreach (Node child in m_children)
                        {
                            if (child != null)
                            {
                                yield return child;
                            }
                        }
                    }
                }
            }

            #endregion
        }

        public Node         m_root;
    }
}