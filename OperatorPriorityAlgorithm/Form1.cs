using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;

namespace OperatorPriorityAlgorithm
{
    public struct node
    {
        public int l;
        public int r;

        public node(int l,int r)
        {
            this.l = l;
            this.r = r;
        }

    };

    public struct Sign
    {
        public string str;
        public int pos;

        public Sign(string str,int pos)
        {
            this.str = str;
            this.pos = pos;
        }
    };

    public struct match
    {
        public int l;
        public int r;
        public int res;
    };
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            nonTerminal = new Dictionary<string, int>();
            Terminal = new Dictionary<string, int>();
            //terminal= new Dictionary<string, int>();
            terminal = new string[100];
            isTermial = new Boolean[100];
            notTerminal = new string[100];
            /*terminal[0] = "+";
            terminal[1] = "-";
            terminal[2] = "*";
            terminal[3] = "/";
            terminal[4] = "(";
            terminal[5] = ")";
            terminal[6] = "i";
            for (int i = 0; i < 7; i++)
                isTermial[i] = false;
            for (int i = 0; i < 7; i++)
                Terminal.Add(terminal[i], i);*/

            nonCount = 0;
            terminatalCount = 0;
            istarget = new Boolean[100];
            son = new Boolean[100];

            /*first = new List<List<string>>();
            last = new List<List<string>>();
            left = new List<List<string>>();
            right = new List<List<string>>();*/

            isPre = new Boolean[100, 100];
            isSuff = new Boolean[100, 100];
            Array.Clear(isPre, 0, isPre.Length);
            Array.Clear(isSuff, 0, isSuff.Length);

            first = new Boolean[100, 100];
            last = new Boolean[100, 100];
            Array.Clear(first, 0, first.Length);
            Array.Clear(last, 0, last.Length);

            stF = new Stack();
            stL = new Stack();

            com = new int[100, 100];
            Array.Clear(com, 0, com.Length);

            line = new int[100, 100];

            Operator = new Sign[100];
            Output = new string[100];

            //val = new int[100];
            father = new int[100];

            isConversion = new match[100, 100];
            conversionNum = new int[100];
            belong = new int[100];
        }

        private void initGrammar()
        {
            nonCount = 0;
            terminatalCount = 0;
            nonTerminal.Clear();
            Terminal.Clear();
            Array.Clear(istarget, 0, istarget.Length);
            Array.Clear(son, 0, son.Length);

            Array.Clear(isPre, 0, isPre.Length);
            Array.Clear(isSuff, 0, isSuff.Length);

            stF.Clear();
            stL.Clear();

            Array.Clear(first, 0, first.Length);
            Array.Clear(last, 0, last.Length);
            Array.Clear(com, 0, com.Length);
            //Array.Clear(val, 0, val.Length);
            for (int i = 0; i < 100; i++)
                father[i] = i;
            Array.Clear(conversionNum, 0, conversionNum.Length);
            Array.Clear(belong, 0, belong.Length);
        }

        private void initInfer()
        {
            /*Operator.Clear();
            operatorPos.Clear();
            Output.Clear();*/
            textBox4.Text = "";
            textBox3.Text = "";
            operatorNum = 0;
            outputNum = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private int Find(int i)
        {
            if (father[i] == i)
                return i;
            else
            {
                father[i] = Find(father[i]);
                return father[i];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            initGrammar();
            textBox3.Text = "";
            textBox5.Text = "";
            int t = 0;
            foreach (string strLine in richTextBox1.Lines)
            {
                int llen = strLine.Length;
                string nextLine = "";
                for (int i = 0; i < llen; i++)
                {
                    if (strLine[i] != ' ')
                    {
                        nextLine += strLine[i].ToString();
                    }
                }

                int pos = nextLine.IndexOf("->");
                string str = nextLine.Substring(0, pos);
                int val = 0;
                if (!nonTerminal.TryGetValue(str, out val))
                {
                    nonTerminal.Add(str, nonCount);
                    notTerminal[nonCount] = str;
                    line[nonCount, 0] = 1;
                    line[nonCount, 1] = t;
                    nonCount++;
                }
                else
                {
                    int cur = nonTerminal[str];
                    line[cur, 0]++;
                    line[cur, line[cur, 0]] = t;
                }
                t++;

                /*pos += 2;
                for (int i = 0; i < 7; i++)
                {
                    //int pos = nextLine.IndexOf(terminal[i],pos);
                    if (nextLine.IndexOf(terminal[i], pos) >= 0)
                        isTermial[i] = true;
                }*/
            }

            //textBox3.Text += "io";

            for(int i=0;i<nonCount;i++)
            {
                father[i] = i;
            }

            foreach (string strLine in richTextBox1.Lines)
            {
                int llen = strLine.Length;
                string nextLine = "";
                for (int i = 0; i < llen; i++)
                {
                    if(strLine[i]!=' ')
                    {
                        nextLine += strLine[i].ToString();
                    }
                }
                int pos = nextLine.IndexOf("->");
                int cur = pos+2;
                pos += 2;
                int len = nextLine.Length;
                int id = nonTerminal[nextLine.Substring(0, pos-2)];
                //int id = 0;
                //textBox3.Text += nonCount;
                while(pos<len)
                {
                    int val;
                    if (!nonTerminal.TryGetValue(nextLine[pos].ToString(), out val) && nextLine[pos] != '|' && nextLine[pos] != ' ' && !Terminal.TryGetValue(nextLine[pos].ToString(), out val))
                    {
                        terminal[terminatalCount] = nextLine[pos].ToString();
                        Terminal.Add(nextLine[pos].ToString(), terminatalCount);
                        terminatalCount++;
                    }
                    else if (nonTerminal.TryGetValue(nextLine[pos].ToString(), out val))
                    {
                        istarget[nonTerminal[nextLine[pos].ToString()]] = true;
                        if (pos == len - 1 && nextLine[pos - 1] == '|')
                        {
                            son[nonTerminal[nextLine[pos].ToString()]] = true;
                            father[nonTerminal[nextLine[pos].ToString()]] = id;
                        }
                        else if(pos==len-1&&pos==cur)
                        {
                            son[nonTerminal[nextLine[pos].ToString()]] = true;
                            father[nonTerminal[nextLine[pos].ToString()]] = id;
                        }
                        else if(pos==cur&&nextLine[pos+1]=='|')
                        {
                            son[nonTerminal[nextLine[pos].ToString()]] = true;
                            father[nonTerminal[nextLine[pos].ToString()]] = id;
                        }
                        else if(nextLine[pos - 1] == '|' && nextLine[pos + 1] == '|')
                        {
                            son[nonTerminal[nextLine[pos].ToString()]] = true;
                            father[nonTerminal[nextLine[pos].ToString()]] = id;
                        }

                    }
                        

                    pos++;
                }
            }

           
            for(int i=0;i<nonCount;i++)
            {
                if(father[i]!=i)
                {
                    Find(i);
                }
            }

            //textBox3.Text = "po";
            foreach (string strLine in richTextBox1.Lines)
            {
                int llen = strLine.Length;
                string nextLine = "";
                for (int i = 0; i < llen; i++)
                {
                    if (strLine[i] != ' ')
                    {
                        nextLine += strLine[i].ToString();
                    }
                }

                int pos = nextLine.IndexOf("->");
                string str = nextLine.Substring(0, pos);
                /*int val = 0;
                if(!nonTerminal.TryGetValue(str, out val))
                {
                    nonTerminal.Add(str, nonCount);
                }*/
                pos += 2;
                int id = nonTerminal[str];
                int len = nextLine.Length;
                int l = pos;

                while (l<len)
                {
                    int tmp = nextLine.IndexOf("|", pos);
                    if (tmp == -1)
                        l = len;
                    else
                        l = tmp;

                    int nn = 0;
                    if(l-pos==1&&Terminal.TryGetValue(nextLine[pos].ToString(),out nn))
                    {
                        isConversion[Terminal[nextLine[pos].ToString()], conversionNum[Terminal[nextLine[pos].ToString()]]].l = -1;
                        isConversion[Terminal[nextLine[pos].ToString()], conversionNum[Terminal[nextLine[pos].ToString()]]].r = -1;
                        isConversion[Terminal[nextLine[pos].ToString()], conversionNum[Terminal[nextLine[pos].ToString()]]].res = id;
                        conversionNum[Terminal[nextLine[pos].ToString()]]++;
                    }
                    else if(l-pos==3&&nonTerminal.TryGetValue(nextLine[pos].ToString(),out nn)&& nonTerminal.TryGetValue(nextLine[pos+2].ToString(), out nn)&& Terminal.TryGetValue(nextLine[pos+1].ToString(), out nn))
                    {
                        int temp = Terminal[nextLine[pos + 1].ToString()];
                        isConversion[temp, conversionNum[temp]].l = father[nonTerminal[nextLine[pos].ToString()]];
                        isConversion[temp, conversionNum[temp]].r = father[nonTerminal[nextLine[pos+2].ToString()]];
                        isConversion[temp, conversionNum[temp]].res = id;
                        conversionNum[temp]++;
                    }
                    else if(l-pos==3 && Terminal.TryGetValue(nextLine[pos].ToString(), out nn) && Terminal.TryGetValue(nextLine[pos + 2].ToString(), out nn) && nonTerminal.TryGetValue(nextLine[pos + 1].ToString(), out nn))
                    {
                        int temp = Terminal[nextLine[pos].ToString()];
                        isConversion[temp, conversionNum[temp]].l = -1;
                        isConversion[temp, conversionNum[temp]].r = father[nonTerminal[nextLine[pos + 1].ToString()]];
                        isConversion[temp, conversionNum[temp]].res = id;
                        conversionNum[temp]++;
                    }
                    else if(l-pos==2)
                    {
                        if(Terminal.TryGetValue(nextLine[pos].ToString(), out nn) && nonTerminal.TryGetValue(nextLine[pos + 1].ToString(), out nn))
                        {
                            int temp = Terminal[nextLine[pos].ToString()];
                            isConversion[temp, conversionNum[temp]].l = -2;
                            isConversion[temp, conversionNum[temp]].r = father[nonTerminal[nextLine[pos + 1].ToString()]];
                            isConversion[temp, conversionNum[temp]].res = id;
                            conversionNum[temp]++;
                        }
                        else if(Terminal.TryGetValue(nextLine[pos+1].ToString(), out nn) && nonTerminal.TryGetValue(nextLine[pos].ToString(), out nn))
                        {
                            int temp = Terminal[nextLine[pos+1].ToString()];
                            isConversion[temp, conversionNum[temp]].r = -2;
                            isConversion[temp, conversionNum[temp]].l = father[nonTerminal[nextLine[pos].ToString()]];
                            isConversion[temp, conversionNum[temp]].res = id;
                            conversionNum[temp]++;
                        }
                    }

                    bool isStart = true;
                    while(pos<l)
                    {
                        int temp = pos;
                        int cnt = -1;
                        for(int i=0;i<terminatalCount;i++)
                        {
                                int cur = nextLine.IndexOf(terminal[i], pos,l-pos);
                                if(cur>-1)
                            {
                                if (cnt == -1 || cur < cnt)
                                    cnt = cur;  
                            }
                                
                            
                            
                        }

                        if (cnt == -1)
                        {
                            string ss = nextLine.Substring(pos,l-pos);
                            int val = 0;
                            if (nonTerminal.TryGetValue(ss, out val))
                            {
                                isSuff[nonTerminal[ss],id] = true;
                                if (ok(nextLine[pos - 1]))
                                {
                                    last[id, Terminal[nextLine[pos-1].ToString()]] = true;
                                    stL.Push(new node(id, Terminal[nextLine[pos-1].ToString()]));
                                }
                                else
                                    isPre[nonTerminal[ss], id] = true;
                                /*last[id, Terminal[nextLine[pos].ToString()]] = true;
                                
                                stL.Push(new node(id, Terminal[nextLine[pos].ToString()]));*/
                            }
                            else
                            {
                                textBox3.Text = "这个文法不是算符优先文法 ";
                                textBox3.Text += ss;
                                textBox3.Text += "处非终结符连续";
                                return;
                            }
                            pos = l;
                        }
                        else
                        {
                            string ss = nextLine.Substring(pos, cnt - pos);
                            if(cnt>pos)
                            {
                                int val = 0;
                                if (nonTerminal.TryGetValue(ss, out val))
                                {
                                    if(isStart)
                                    {
                                        isPre[nonTerminal[ss], id] = true;
                                    }
                                }
                                else
                                {
                                    textBox3.Text = "这个文法不是算符优先文法 ";
                                    textBox3.Text += ss;
                                    textBox3.Text += "处非终结符连续";
                                    return;
                                }
                            }

                            if(isStart)
                            {
                                first[id, Terminal[nextLine[cnt].ToString()]] = true;
                                stF.Push(new node(id, Terminal[nextLine[cnt].ToString()]));
                            }
                            isStart = false;

                            pos = cnt+1;
                            if(cnt == l-1)
                            {
                                last[id, Terminal[nextLine[cnt].ToString()]] = true;
                                stL.Push(new node(id, Terminal[nextLine[cnt].ToString()]));
                            }
                        }

                        //pos = cnt;
                    }
                    pos = l + 1;
                }
                
                
            }
            //textBox3.Text = "ert";

            while (stF.Count > 0)
            {
                node tt = (node)stF.Peek();
                stF.Pop();
                int ll = tt.l, rr = tt.r;
                for (int i = 0; i < nonCount; i++)
                {
                    if (isPre[ll, i] && !first[i, rr])
                    {
                        first[i, rr] = true;
                        stF.Push(new node(i, rr));
                    }
                }
            }
            while (stL.Count > 0)
            {
                node tt = (node)stL.Peek();
                stL.Pop();
                int ll = tt.l, rr = tt.r;
                for (int i = 0; i < nonCount; i++)
                {
                    if (isSuff[ll, i] && !last[i, rr])
                    {
                        last[i, rr] = true;
                        stL.Push(new node(i, rr));
                    }
                }
            }
            
            foreach (string strLine in richTextBox1.Lines)
            {
                int llen = strLine.Length;
                string nextLine = "";
                for (int i = 0; i < llen; i++)
                {
                    if (strLine[i] != ' ')
                    {
                        nextLine += strLine[i].ToString();
                    }
                }

                int pos = nextLine.IndexOf("->");
                pos += 2;
                int len = nextLine.Length;
                int l = pos;
                while(l<len)
                {
                    int tmp = nextLine.IndexOf("|", pos);
                    if (tmp == -1)
                        l = len;
                    else
                        l = tmp;

                    for(int i=pos;i<l-1;i++)
                    {
                        if(ok(nextLine[i])&&ok(nextLine[i+1]))
                        {
                            int ll = Terminal[nextLine[i].ToString()], rr = Terminal[nextLine[i + 1].ToString()];
                            if (com[ll, rr] > 1)
                            {
                                textBox3.Text = "这不是算法优先文法 ";
                                textBox3.Text += nextLine[i].ToString();
                                textBox3.Text += "和";
                                textBox3.Text += nextLine[i+1].ToString();
                                textBox3.Text += " 有多于一个关系";
                                return;
                            }
                            else
                                com[ll, rr] = 1;
                        }
                        else if(ok(nextLine[i]) && !ok(nextLine[i + 1]))
                        {
                            if(i<l-2&&ok(nextLine[i+2]))
                            {
                                int ll = Terminal[nextLine[i].ToString()], rr = Terminal[nextLine[i + 2].ToString()];
                                if (com[ll, rr] > 1)
                                {
                                    textBox3.Text = "这不是算法优先文法 ";
                                    textBox3.Text += nextLine[i].ToString();
                                    textBox3.Text += "和";
                                    textBox3.Text += nextLine[i+2].ToString();
                                    textBox3.Text += " 有多于一个关系";
                                    return;
                                }
                                else
                                    com[ll, rr] = 1;
                            }

                            int cnt = nonTerminal[nextLine[i + 1].ToString()];
                            int seg= Terminal[nextLine[i].ToString()];
                            for (int j=0;j<terminatalCount;j++)
                            {
                                if(first[cnt,j])
                                {
                                    if (com[seg, j]>0&&com[seg,j]!=2)
                                    {
                                        textBox3.Text = "这不是算法优先文法 ";
                                        textBox3.Text += nextLine[i].ToString();
                                        textBox3.Text += "和";
                                        textBox3.Text += terminal[j];
                                        textBox3.Text += " 有多于一个关系";
                                        return;
                                    }
                                    else
                                        com[seg, j] = 2;
                                }
                            }
                        }
                        else if(!ok(nextLine[i]) && ok(nextLine[i + 1]))
                        {
                            int ll = nonTerminal[nextLine[i].ToString()];
                            int rr = Terminal[nextLine[i + 1].ToString()];
                            for (int j = 0; j < terminatalCount; j++)
                            {
                                if (last[ll, j])
                                {
                                    if (com[j,rr] <3&& com[j, rr]>0)
                                    {
                                        textBox3.Text = "这不是算法优先文法 ";
                                        textBox3.Text += terminal[j];
                                        textBox3.Text += "和";
                                        textBox3.Text += nextLine[i+1].ToString();
                                        textBox3.Text += " 有多于一个关系";
                                        return;
                                    }
                                    else
                                        com[j, rr] = 3;
                                }
                            }
                        }
                    }

                    pos = l + 1;
                }
            }

            textBox5.Text = "";
            textBox5.Text += "UI集：";
            textBox5.Text += Environment.NewLine;
            for (int i = 0; i < terminatalCount; i++)
            {
                textBox5.Text += terminal[i];
                textBox5.Text += " ";
            }
            textBox5.Text += Environment.NewLine;
            textBox5.Text += Environment.NewLine;

            textBox5.Text += "产生式";
            textBox5.Text += Environment.NewLine;
            for(int i=0;i<nonCount;i++)
            {
                if (line[i, 0] == 1)
                    textBox5.Text += richTextBox1.Lines[line[i, 1]];
                else
                {
                    textBox5.Text += richTextBox1.Lines[line[i, 1]];
                    for(int j=2;j<=line[i,0];j++)
                    {
                        textBox5.Text += "|";
                        int cur = richTextBox1.Lines[line[i, j]].IndexOf("->");
                        textBox5.Text += richTextBox1.Lines[line[i, j]].Substring(cur + 2);
                    }
                }

                textBox5.Text += Environment.NewLine;
            }

            textBox5.Text += Environment.NewLine;
            textBox5.Text += "first集:";
            textBox5.Text += Environment.NewLine;
            for(int i=0;i<nonCount;i++)
            {
                textBox5.Text += notTerminal[i];
                textBox5.Text += ": ";
                for(int j=0;j<terminatalCount;j++)
                {
                    if(first[i,j])
                    {
                        textBox5.Text += terminal[j];
                        textBox5.Text += " ";
                    }
                }

                textBox5.Text += Environment.NewLine;
            }

            textBox5.Text += Environment.NewLine;
            textBox5.Text += "last集:";
            textBox5.Text += Environment.NewLine;
            for (int i = 0; i < nonCount; i++)
            {
                textBox5.Text += notTerminal[i];
                textBox5.Text += ": ";
                for (int j = 0; j < terminatalCount; j++)
                {
                    if (last[i, j])
                    {
                        textBox5.Text += terminal[j];
                        textBox5.Text += " ";
                    }
                }

                textBox5.Text += Environment.NewLine;
            }

            textBox5.Text += Environment.NewLine;
            textBox5.Text += "算符优先关系表:";
            textBox5.Text += Environment.NewLine;
            textBox5.Text += "  ";
            for(int i=0;i<terminatalCount;i++)
            {
                textBox5.Text += terminal[i];
                textBox5.Text += " ";
            }
            textBox5.Text += Environment.NewLine;

            for(int i=0;i<terminatalCount;i++)
            {
                textBox5.Text += terminal[i];
                textBox5.Text += " ";
                for(int j=0;j<terminatalCount;j++)
                {
                    if (com[i, j] == 0)
                        textBox5.Text += "  ";
                    else if (com[i, j] == 1)
                    {
                        textBox5.Text += "= ";
                    }
                    else if (com[i, j] == 2)
                        textBox5.Text += "< ";
                    else
                        textBox5.Text += "> ";
                }

                textBox5.Text += Environment.NewLine;
            }

            int vall = 0;
            if(!Terminal.TryGetValue("#",out vall))
            {
                Terminal.Add("#", terminatalCount);
                terminal[terminatalCount] = "#";
                terminatalCount++;
                for(int i=0;i<terminatalCount;i++)
                {
                    com[terminatalCount - 1,i] = 2;
                }
                com[terminatalCount - 1, terminatalCount - 1] = 1;
                for (int i = 0; i < terminatalCount - 1; i++)
                    com[i, terminatalCount - 1] = 3;
            }
        }

        private bool ok(char ch)
        {
            for(int i=0;i<terminatalCount;i++)
            {
                if (terminal[i] == ch.ToString())
                    return true;
            }

            return false;
        }

        private Dictionary<string,int> nonTerminal;
        private Dictionary<string, int> Terminal;
        // private Dictionary<string, int> terminal;
        private int nonCount;
        private int terminatalCount;
        /*private List<List<string>> first;
        private List<List<string>> last;
        private List<List<string>> left;
        private List<List<string>> right;*/

        private string[] terminal;
        private Boolean[] isTermial;
        private string[] notTerminal;
        private Boolean[] istarget;
        private Boolean[] son;

        private Boolean[,] isPre;
        private Boolean[,] isSuff;
        private Boolean[,] first;
        private Boolean[,] last;

        private Stack stF;
        private Stack stL;

        private int[,] com;

        private int[,] line;

        private Sign[] Operator;
        private string[] Output;
        private Stack operatorPos;
        private int operatorNum;
        private int outputNum;

        //private int[] val;
        private int[] father;

        private match[,] isConversion;
        private int[] conversionNum;
        private int[] belong;

        private void button2_Click(object sender, EventArgs e)
        {
            initInfer();
            string tmp = richTextBox2.Lines[0];

            string target = "";
            for(int i=0;i<nonCount;i++)
            {
                if(!istarget[i])
                {
                    target = notTerminal[i];
                    break;
                }
            }
            if(target.Length==0)
            {
                for(int i=0;i<nonCount;i++)
                {
                    if(!son[i])
                    {
                        target = notTerminal[i];
                        break;
                    }
                }
            }
            textBox4.Text += "步骤 栈          优先关系 当前符号 剩余符号        动作";

            textBox4.Text += Environment.NewLine;
            string str="";
            int len = 0;
            for(int i=0;i<tmp.Length;i++)
            {
                if(tmp[i]!=' ')
                {
                    str+=tmp[i].ToString();
                    len++;
                }
            }

            Operator[operatorNum].str = "#";
            Operator[operatorNum++].pos = 0;
            Output[outputNum++] = "#";
            int pos = 0;

            int tt = 1;
            while(pos<len)
            {
                textBox4.Text += tt.ToString();
                tt++;
                if (tt < 11)
                    textBox4.Text += "    ";
                else
                    textBox4.Text += "   ";
                for (int i = 0; i < outputNum; i++)
                    textBox4.Text += Output[i];
                for (int i = 0; i < 12 - outputNum; i++)
                    textBox4.Text += " ";

                string currentOperator = str[pos].ToString();
                //textBox3.Text = currentOperator;
                int v;
                if(!Terminal.TryGetValue(currentOperator,out v))
                {
                    textBox4.Text += "规约失败";
                    return;
                }
                int ll = Terminal[Operator[operatorNum - 1].str], rr = Terminal[currentOperator];
                //textBox3.Text = "rt";
                if(com[ll,rr]==0)
                {
                    textBox4.Text +="分析失败";
                    textBox4.Text += Environment.NewLine;
                    return;

                }
                else if(com[ll,rr]==1)
                {
                    textBox4.Text += "=        ";
                    textBox4.Text += currentOperator;
                    textBox4.Text += "        ";
                    for (int i = pos + 1; i < len; i++)
                        textBox4.Text += str[i].ToString();
                    for (int i = 0; i < 16 - (len - pos - 1); i++)
                        textBox4.Text += " ";

                    pos++;
                    if (operatorNum > 1)
                    {
                        int rv = father[nonTerminal[Output[outputNum - 1]]];
                        int temp = Terminal[Operator[operatorNum - 1].str];
                        int goal = -1;
                        for(int k=0;k<conversionNum[temp];k++)
                        {
                            if(isConversion[temp,k].l==-1&&isConversion[temp,k].r==rv)
                            {
                                goal = isConversion[temp, k].res;
                                break;
                            }
                        }

                        if(goal==-1)
                        {
                            textBox4.Text += "规约失败";
                            return;
                        }

                        textBox4.Text += "规约";
                        textBox4.Text += Environment.NewLine;
                        Output[Operator[operatorNum - 2].pos + 1] = notTerminal[goal];
                        outputNum = Operator[operatorNum - 2].pos + 2;
                    }
                    else
                    {
                        textBox4.Text += "规约";
                        textBox4.Text += Environment.NewLine;
                        Output[0] = target;
                        outputNum = 1;
                    }
                    operatorNum--;
                }
                else if(com[ll,rr]==2)
                {
                    textBox4.Text += "<        ";
                    textBox4.Text += currentOperator;
                    textBox4.Text += "        ";
                    for (int i = pos + 1; i < len; i++)
                        textBox4.Text += str[i].ToString();
                    for (int i = 0; i < 16 - (len - pos - 1); i++)
                        textBox4.Text += " ";
                    textBox4.Text += "移进";
                    textBox4.Text += Environment.NewLine;

                    Output[outputNum] = currentOperator;
                    outputNum++;
                    Operator[operatorNum].str = currentOperator;
                    Operator[operatorNum].pos = outputNum - 1;
                    operatorNum++;
                    pos++;
                }
                else
                {
                    textBox4.Text += ">        ";
                    textBox4.Text += currentOperator;
                    textBox4.Text += "        ";
                    for (int i = pos + 1; i < len; i++)
                        textBox4.Text += str[i].ToString();
                    for (int i = 0; i < 16 - (len - pos - 1); i++)
                        textBox4.Text += " ";
                    

                    if (operatorNum>1)
                    {
                        if(outputNum- Operator[operatorNum - 2].pos==4)
                        {
                            int lv = father[nonTerminal[Output[Operator[operatorNum - 2].pos + 1].ToString()]];
                            int rv = father[nonTerminal[Output[Operator[operatorNum - 2].pos + 3].ToString()]];
                            int cur = Terminal[Output[Operator[operatorNum - 2].pos + 2].ToString()];
                            int res = -1;

                            for(int k=0;k<conversionNum[cur];k++)
                            {
                                if(isConversion[cur,k].l==lv&&isConversion[cur,k].r==rv)
                                {
                                    res = isConversion[cur, k].res;
                                    break;
                                }
                            }

                            if (res == -1)
                            {
                                textBox4.Text += "分析失败";
                                return;
                            }

                            textBox4.Text += "规约";
                            textBox4.Text += Environment.NewLine;
                            Output[Operator[operatorNum - 2].pos + 1] = notTerminal[res];
                            outputNum = Operator[operatorNum - 2].pos + 2;
                        }
                        else if(outputNum - Operator[operatorNum - 2].pos == 3)
                        {
                            int vv = 0;
                            if(Terminal.TryGetValue(Output[Operator[operatorNum-2].pos+1],out vv))
                            {
                                int rv = father[nonTerminal[Output[Operator[operatorNum - 2].pos + 2]]];
                                int lv = -2;
                                int res = -1;
                                int temp=Terminal[Output[Operator[operatorNum - 2].pos + 1]];

                                for(int k=0;k<conversionNum[temp];k++)
                                {
                                    if(isConversion[temp,k].l==-2&&isConversion[temp,k].r==rv)
                                    {
                                        res = isConversion[temp, k].res;
                                        break;
                                    }
                                }

                                if(res==-1)
                                {
                                    textBox4.Text += "规约失败";
                                    return;
                                }

                                textBox4.Text += "规约";
                                textBox4.Text += Environment.NewLine;
                                Output[Operator[operatorNum - 2].pos + 1] = notTerminal[res];
                                outputNum = Operator[operatorNum - 2].pos + 2;
                            }
                            else
                            {
                                int lv = father[nonTerminal[Output[Operator[operatorNum - 2].pos + 1]]];
                                int rv = -2;
                                int res = -1;
                                int temp = Terminal[Output[Operator[operatorNum - 2].pos + 2]];

                                for (int k = 0; k < conversionNum[temp]; k++)
                                {
                                    if (isConversion[temp, k].r == -2 && isConversion[temp, k].l == lv)
                                    {
                                        res = isConversion[temp, k].res;
                                        break;
                                    }
                                }

                                if (res == -1)
                                {
                                    textBox4.Text += "规约失败";
                                    return;
                                }

                                textBox4.Text += "规约";
                                textBox4.Text += Environment.NewLine;
                                Output[Operator[operatorNum - 2].pos + 1] = notTerminal[res];
                                outputNum = Operator[operatorNum - 2].pos + 2;
                            }
                        }
                        else
                        {
                            int temp = Terminal[Operator[operatorNum - 1].str];
                            int res = -1;
                            for(int k=0;k<conversionNum[temp];k++)
                            {
                                if(isConversion[temp,k].l==-1&&isConversion[temp,k].r==-1)
                                {
                                    res = isConversion[temp, k].res;
                                    break;
                                }
                            }

                            if(res==-1)
                            {
                                textBox4.Text += "规约失败";
                                return;
                            }
                            
                            textBox4.Text += "规约";
                            textBox4.Text += Environment.NewLine;
                            Output[Operator[operatorNum - 2].pos + 1] = notTerminal[res];
                            outputNum = Operator[operatorNum - 2].pos + 2;
                        }
                        //int lv = father[nonTerminal[Output[Operator[operatorNum - 2].pos + 1].ToString()]];
                        /*Output[Operator[operatorNum - 2].pos + 1] = target;
                        outputNum = Operator[operatorNum - 2].pos + 2;*/
                    }
                    else
                    {
                        textBox4.Text += "规约";
                        textBox4.Text += Environment.NewLine;
                        Output[0] = target;
                        outputNum = 1;
                    }
                    
                    operatorNum--;

                }
            }

            if(operatorNum==0&&outputNum==1)
            {
                textBox4.Text += tt.ToString();
                if (tt < 10)
                    textBox4.Text += "    ";
                else
                    textBox4.Text += "   ";
                textBox4.Text += target;
            }
            else
            {
                textBox4.Text += tt.ToString();
                if (tt < 10)
                    textBox4.Text += "    ";
                else
                    textBox4.Text += "   ";
                textBox4.Text += "规约失败";
            }
        }
    }
}
