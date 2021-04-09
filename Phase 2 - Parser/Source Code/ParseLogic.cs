using System;
using System.Collections.Generic;
using System.Linq;

namespace Parser
{
    // Syntax Tree Nodes Kinds
    enum NodeKind { STMT_KIND, EXP_KIND };
    enum StatementKind { IF_KIND, REPEAT_KIND, ASSIGN_KIND, READ_KIND, WRITE_KIND };
    enum ExpressionKind { OP_KIND, CONST_KIND, ID_KIND };

    // The tree node which can be of two kinds
    class TreeNode
    {
        private NodeKind nk;
        private StatementKind stk;
        private ExpressionKind exk;
        protected TreeNode[] child_nodes;
        protected TreeNode sibling_node;

        // For expression kind nodes
        private int int_val;
        private float float_val;
        private string Id_Name;
        // private TokenType operator; // Not used ... just to indicate that operator can be represented by it's token type
        private char op; // The real operator charachter

        // Constructors for both kinds
        public TreeNode(StatementKind k) // For statement kind
        {
            nk = NodeKind.STMT_KIND;
            child_nodes = new TreeNode[3] { null, null, null }; // Initialize the statement node child nodes with max no. of 3
            sibling_node = null; // Innitialize the node sibling node
            stk = k; // Assign the statement kind
        }

        public TreeNode(ExpressionKind k) // For expression kind
        {
            nk = NodeKind.EXP_KIND;
            child_nodes = new TreeNode[3] { null, null, null }; // Initialize the expression node child nodes
            sibling_node = null; // Innitialize the node sibling node
            exk = k; // Assign the expression kind
        }

        // Setters
        public void SetChildNode(int child_no, TreeNode cn)
        {
            child_nodes[child_no] = cn;
        }

        public void SetSiblingNode(TreeNode sn)
        {
            sibling_node = sn;
        }

        public void SetIdName(string s)
        {
            Id_Name = s;
        }

        public void SetOp(string t)
        {
            op = t.ElementAt(0);
        }

        public void SetIntVal(int i)
        {
            int_val = i;
        }

        public void SetFloatVal(float f)
        {
            float_val = f;
        }


        // Getters
        public NodeKind GetNodeKind()
        {
            return nk;
        }

        public StatementKind GetStatementKind()
        {
            return stk;
        }

        public ExpressionKind GetExpressionKind()
        {
            return exk;
        }

        public TreeNode[] GetChildNodes()
        {
            return child_nodes;
        }

        public TreeNode GetSiblingNode()
        {
            return sibling_node;
        }

        public string GetIdName()
        {
            return Id_Name;
        }

        public char GetOperatorr()
        {
            return op;
        }

        public int GetIntVal()
        {
            return int_val;
        }

        public float GetFloatVal()
        {
            return float_val;
        }
    }


    static class ParseLogic
    {
        private static Token current_token; // Holds the current token to be parsed

        // The big player: *** parse method ***
        public static TreeNode Parse()
        {
            SyntaxErrorsList.Clear();
            ScanLogic.reset_index();
            current_token = ScanLogic.GetNextToken(); // Bring up the first token from the scanner to parse
            TreeNode tn = StatementSequence();
            if (current_token.GetTokenType() != TokenType.END_OF_FILE)
            {
                SyntaxError("Code ends before file");
            }

            return tn;

        }

        // For test purpose only
        static int indent = 0;
        private static void PrintSpaces()
        {
            for (int i = 0; i < indent; i++)
            {
                Console.Write(" ");
            }
        }
        public static void PrintTree(TreeNode tree)
        {
            indent += 3;
            while (tree != null)
            {
                PrintSpaces();

                if (tree.GetNodeKind() == NodeKind.STMT_KIND)
                {
                    switch (tree.GetStatementKind())
                    {
                        case (StatementKind.IF_KIND):
                            {
                                Console.WriteLine("If");
                                break;
                            }

                        case (StatementKind.REPEAT_KIND):
                            {
                                Console.WriteLine("Repeat");
                                break;
                            }

                        case (StatementKind.ASSIGN_KIND):
                            {
                                Console.WriteLine("Assign (" + tree.GetIdName() + ")");
                                break;
                            }

                        case (StatementKind.READ_KIND):
                            {
                                Console.WriteLine("Read (" + tree.GetIdName() + ")");
                                break;
                            }

                        case (StatementKind.WRITE_KIND):
                            {
                                Console.WriteLine("Write");
                                break;
                            }
                    }
                }

                else if (tree.GetNodeKind() == NodeKind.EXP_KIND)
                {
                    switch (tree.GetExpressionKind())
                    {
                        case (ExpressionKind.ID_KIND):
                            {
                                Console.WriteLine("Id: (" + (tree.GetIdName()) + ")");
                                break;
                            }

                        case (ExpressionKind.OP_KIND):
                            {
                                Console.WriteLine("Op: (" + tree.GetOperatorr() + ")");
                                break;
                            }

                        case (ExpressionKind.CONST_KIND):
                            {
                                Console.WriteLine("Const: (" + tree.GetIntVal() + ")");
                                break;
                            }
                    }
                }

                for (int i = 0; i < 3; i++) // Child nodes
                {
                    if (tree.GetChildNodes()[i] != null)
                    {
                        PrintTree(tree.GetChildNodes()[i]);
                    }
                    else
                    {
                        continue;
                    }
                }

                tree = tree.GetSiblingNode();
            }
            indent -= 3;
        }
        // End of print tree test function

        // For GUI purpose
        // A print-value method simillar to and based on the print tree method
        public static string getPrintValue(TreeNode tree)
        {
            string outputValue = "";
            if (tree.GetNodeKind() == NodeKind.STMT_KIND)
            {
                switch (tree.GetStatementKind())
                {
                    case (StatementKind.IF_KIND):
                        {
                            //                            Console.WriteLine("If");
                            outputValue = "If";
                            break;
                        }

                    case (StatementKind.REPEAT_KIND):
                        {
                            //                            Console.WriteLine("Repeat");
                            outputValue = "Repeat";
                            break;
                        }

                    case (StatementKind.ASSIGN_KIND):
                        {
                            //                            Console.WriteLine("Assign (" + tree.GetIdName() + ")");
                            outputValue = "Assign (" + tree.GetIdName() + ")";
                            break;
                        }

                    case (StatementKind.READ_KIND):
                        {
                            //                            Console.WriteLine("Read (" + tree.GetIdName() + ")");
                            outputValue = "Read (" + tree.GetIdName() + ")";
                            break;
                        }

                    case (StatementKind.WRITE_KIND):
                        {
                            //                           Console.WriteLine("Write");
                            outputValue = "Write";
                            break;
                        }
                }
            }

            else if (tree.GetNodeKind() == NodeKind.EXP_KIND)
            {
                switch (tree.GetExpressionKind())
                {
                    case (ExpressionKind.ID_KIND):
                        {
                            //                            Console.WriteLine("Id: (" + (tree.GetIdName()) + ")");
                            outputValue = "Id: (" + (tree.GetIdName()) + ")";
                            break;
                        }

                    case (ExpressionKind.OP_KIND):
                        {
                            //                            Console.WriteLine("Op: (" + tree.GetOp() + ")");
                            outputValue = "Operator: (" + tree.GetOperatorr() + ")";
                            break;
                        }

                    case (ExpressionKind.CONST_KIND):
                        {
                            //                            Console.WriteLine("Const: (" + tree.GetIntVal() + ")");
                            outputValue = "Const: (" + tree.GetIntVal() + ")";
                            break;
                        }
                }
            }
            return outputValue;
        }


        // Match method
        private static void Match(TokenType expected)
        {
            if (current_token.GetTokenType() == expected)
            {
                current_token = ScanLogic.GetNextToken(); // Forrward to the next token and continue parsing
            }
            else // If token doesn't match the expected tokyn type in its context
            {
                SyntaxError("Unexpected token -> " + current_token.GetLexeme());
            }
        }

        // Syntax Errors handling
        private static List<string> SyntaxErrorsList = new List<string>();
        private static void SyntaxError(string error_message)
        {
            SyntaxErrorsList.Add("--->>> Syntax Error: " + error_message);
        }
        public static List<string> GetSyntaxErrors()
        {
            return SyntaxErrorsList;
        }

        private static TreeNode StatementSequence()
        {
            TreeNode sstn = Statement();
            TreeNode ptn = sstn;

            while ((current_token.GetTokenType() != TokenType.END_OF_FILE) && (current_token.GetTokenType() != TokenType.RESWORD_END) &&
                (current_token.GetTokenType() != TokenType.RESWORD_ELSE) && (current_token.GetTokenType() != TokenType.RESWORD_UNTIL))
            {
                TreeNode qtn;
                Match(TokenType.SEMICOLOCN);
                qtn = Statement();

                if (qtn != null)
                {
                    if (sstn == null)
                    {
                        sstn = ptn = qtn;
                    }
                    else // The else here indicates that ptn cannot be null either in case sstn was null
                    {
                        ptn.SetSiblingNode(qtn);
                        ptn = qtn;
                    }
                }
            }

            return sstn;
        }

        private static TreeNode Statement()
        {
            TreeNode stn = null;
            // Redirect to one of the 5 statment types corresponding to the token type
            switch (current_token.GetTokenType())
            {
                case (TokenType.RESWORD_IF): stn = IfStatement(); break;
                case (TokenType.RESWORD_REPEAT): stn = RepeatStatement(); break;
                case (TokenType.IDENTIFIER): stn = AssignmentStatement(); break;
                case (TokenType.RESWORD_READ): stn = ReadStatement(); break;
                case (TokenType.RESWORD_WRITE): stn = WriteStatement(); break;
                default:
                    {
                        SyntaxError("Unexpected Statement token -> " + current_token.GetLexeme());
                        current_token = ScanLogic.GetNextToken();
                        break;
                    }
            }

            return stn;
        }

        private static TreeNode IfStatement()
        {
            TreeNode itn = new TreeNode(StatementKind.IF_KIND);
            Match(TokenType.RESWORD_IF);
            if (itn != null)
            {
                itn.SetChildNode(0, Expression());
            }
            Match(TokenType.RESWORD_THEN);

            if (itn != null)
            {
                itn.SetChildNode(1, StatementSequence());
            }

            if (current_token.GetTokenType() == TokenType.RESWORD_ELSE)
            {
                Match(TokenType.RESWORD_ELSE);
                if (itn != null)
                {
                    itn.SetChildNode(2, StatementSequence());
                }
            }

            Match(TokenType.RESWORD_END);

            return itn;
        }

        private static TreeNode RepeatStatement()
        {
            TreeNode rtn = new TreeNode(StatementKind.REPEAT_KIND);
            Match(TokenType.RESWORD_REPEAT);
            if (rtn != null)
            {
                rtn.SetChildNode(0, StatementSequence());
            }

            Match(TokenType.RESWORD_UNTIL);
            if (rtn != null)
            {
                rtn.SetChildNode(1, Expression());
            }

            return rtn;
        }

        private static TreeNode AssignmentStatement()
        {
            TreeNode atn = new TreeNode(StatementKind.ASSIGN_KIND);
            if ((atn != null) && (current_token.GetTokenType() == TokenType.IDENTIFIER))
            {
                atn.SetIdName(current_token.GetLexeme());
            }
            Match(TokenType.IDENTIFIER);
            Match(TokenType.ASSIGNMENT_OPERATOR); // The identifier is followed by an assignment operator
            if (atn != null)
            {
                atn.SetChildNode(0, Expression());
            }

            return atn;
        }

        private static TreeNode ReadStatement()
        {
            TreeNode rtn = new TreeNode(StatementKind.READ_KIND);
            Match(TokenType.RESWORD_READ);
            if ((rtn != null) && (current_token.GetTokenType() == TokenType.IDENTIFIER))
            {
                rtn.SetIdName(current_token.GetLexeme());
            }
            Match(TokenType.IDENTIFIER); // The read statement must be followed by an identifier

            return rtn;
        }

        private static TreeNode WriteStatement()
        {
            TreeNode wtn = new TreeNode(StatementKind.WRITE_KIND);
            Match(TokenType.RESWORD_WRITE);

            if (wtn != null)
            {
                wtn.SetChildNode(0, Expression());
            }

            return wtn;
        }

        private static TreeNode Expression()
        {
            TreeNode etn = SimpleExpression();

            if ((current_token.GetTokenType() == TokenType.LESS) || (current_token.GetTokenType() == TokenType.GREATER) ||
                (current_token.GetTokenType() == TokenType.ISEQUAL))
            {
                TreeNode optn = new TreeNode(ExpressionKind.OP_KIND);
                if (optn != null)
                {
                    optn.SetChildNode(0, etn);
                    optn.SetOp(current_token.GetLexeme());
                    etn = optn;
                }

                Match(current_token.GetTokenType());

                if (etn != null)
                {
                    etn.SetChildNode(1, SimpleExpression());
                }
            }

            return etn;
        }

        private static TreeNode SimpleExpression()
        {
            TreeNode setn = Term();

            while ((current_token.GetTokenType() == TokenType.PLUS) || (current_token.GetTokenType() == TokenType.MINUS))
            {
                TreeNode optn = new TreeNode(ExpressionKind.OP_KIND);
                if (optn != null)
                {
                    optn.SetChildNode(0, setn);
                    optn.SetOp(current_token.GetLexeme());
                    setn = optn;
                    Match(current_token.GetTokenType());
                    setn.SetChildNode(1, Term());
                }
            }

            return setn;
        }

        private static TreeNode Term()
        {
            TreeNode ttn = Factor();

            while ((current_token.GetTokenType() == TokenType.MULTIPLICATION) || (current_token.GetTokenType() == TokenType.DIVISION))
            {
                TreeNode optn = new TreeNode(ExpressionKind.OP_KIND);
                if (optn != null)
                {
                    optn.SetChildNode(0, ttn);
                    optn.SetOp(current_token.GetLexeme());
                    ttn = optn;
                    Match(current_token.GetTokenType());
                    optn.SetChildNode(1, Factor());
                }
            }

            return ttn;
        }

        private static TreeNode Factor()
        {
            TreeNode ftn = null;

            switch (current_token.GetTokenType())
            {
                case (TokenType.INT_NUMBER):
                    {
                        ftn = new TreeNode(ExpressionKind.CONST_KIND);
                        if ((ftn != null) && (current_token.GetTokenType() == TokenType.INT_NUMBER))
                        {
                            ftn.SetIntVal((current_token as IntNumToken).GetIntValue());
                        }
                        Match(TokenType.INT_NUMBER);
                        break;
                    }

                case (TokenType.FLOAT_NUMBER):
                    {
                        ftn = new TreeNode(ExpressionKind.CONST_KIND);
                        if ((ftn != null) && (current_token.GetTokenType() == TokenType.FLOAT_NUMBER))
                        {
                            ftn.SetFloatVal((current_token as FloatNumToken).GetFloatValue());
                        }
                        Match(TokenType.FLOAT_NUMBER);
                        break;
                    }

                case (TokenType.IDENTIFIER):
                    {
                        ftn = new TreeNode(ExpressionKind.ID_KIND);
                        if ((ftn != null) && (current_token.GetTokenType() == TokenType.IDENTIFIER))
                        {
                            ftn.SetIdName(current_token.GetLexeme());
                        }
                        Match(TokenType.IDENTIFIER);
                        break;
                    }

                case (TokenType.OPEN_PARENTHESES):
                    {
                        Match(TokenType.OPEN_PARENTHESES);
                        ftn = Expression();
                        Match(TokenType.CLOSE_PARENTHESES);
                        break;
                    }

                default:
                    {
                        SyntaxError("Unexpected factor token -> " + current_token.GetLexeme());
                        break;
                    }
            }

            return ftn;
        }
    }
}
