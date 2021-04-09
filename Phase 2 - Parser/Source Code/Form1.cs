using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CodeBox.AcceptsTab = true;
        }

        private void UploadCodeButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                CodeBox.Clear();
                SyntaxTreeView.Nodes.Clear();
                SyntaxErrorsTextBox.Clear();
                CodeBox.Text = File.ReadAllText(Dialog.FileName);
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            CodeBox.Clear(); //  Clear the code box
            SyntaxTreeView.Nodes.Clear();
            SyntaxErrorsTextBox.Clear();
        }

        private void ParseButton_Click(object sender, EventArgs e)
        {
            SyntaxTreeView.Nodes.Clear();
            SyntaxErrorsTextBox.Clear();
            ScanLogic.ScanCode(CodeBox.Text); // Run the scanner on the code first
            TreeNode root_node = ParseLogic.Parse(); // Run the parser and get the syntax tree root node
            if (ParseLogic.GetSyntaxErrors().Count == 0) // No syntax errors
            {
                addToTeeView(root_node, SyntaxTreeView.Nodes);
                SyntaxTreeView.ExpandAll();
            }

            // Syntax Errors
            foreach (string s in ParseLogic.GetSyntaxErrors())
            {
                SyntaxErrorsTextBox.Text += s;
                SyntaxErrorsTextBox.Text += System.Environment.NewLine;
            }
        }

        private void addToTeeView(TreeNode tree, TreeNodeCollection target)
        {
            while (tree != null)
            {
                target.Add(ParseLogic.getPrintValue(tree));
                int index = target.Count - 1;
                //                Console.WriteLine("Node :" + ParseLogic.getPrintValue(tree));
                for (int i = 0; i < tree.GetChildNodes().Length; i++) // Child nodes
                {
                    if (tree.GetChildNodes()[i] != null)
                    {
                        //                        Console.WriteLine("Children"+i+" :" + index);
                        addToTeeView(tree.GetChildNodes()[i], target[index].Nodes);
                    }
                    else
                    {
                        continue;
                    }
                }

                tree = tree.GetSiblingNode();
            }
        }
    }
}
