using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheChessBoard
{
    public class MyChess
    {
        public Label label;
        public char name;
        public char x;
        public int y;

        public void Initialization(Label l, char c, char j, char k)
        {
            label = l;
            name = c;
            x = j;
            y = k;
        }
    }
    public partial class Form1 : Form
    {
        
        MyChess chesses = new MyChess();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
