using FastDoc.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastDoc.Visualizer
{
    public partial class TreeForm : Form
    {
        public TreeForm()
        {
            InitializeComponent();
        }

        public void LoadNode(Node n, TreeNode parent = null)
        {
            var text = string.Format("{0}", n);

            //if(n is ItemNode)
            //{
            //    ItemNode item = (ItemNode)n;
            //    text += string.Format(" [{0}, {1}]", item.ItemType, item.Type.FullName);
            //}

            //if(n is MemberNode)
            //{
            //    MemberNode member = (MemberNode)n;
            //    text += string.Format(" [{0}, {1}]", member.MemberType, member.MemberInfo.Name);
            //}

            var tn = new TreeNode
            {
                Text = text,
                Tag = n
            };

            if (parent == null)
                treeViewStructure.Nodes.Add(tn);
            else
                parent.Nodes.Add(tn);

            if (n.Children != null)
                foreach (var node in n.Children)
                    LoadNode(node, tn);


        }

        private void treeViewStructure_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }
    }
}
