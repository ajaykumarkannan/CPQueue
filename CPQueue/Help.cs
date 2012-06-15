using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPQueue
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
            string mstring = "This is a clipboard stack/queue application.\n";
            mstring += "\nItems will be copied automatically when you Control-C from within any application. It will then act as a first in last out by defaut and and as a first in last out when \"Tick to Use Stack instead of Queue\" is unchecked. ";
            mstring += "Use Control-E and Control-D to move up and down the list from any application. That item will be copied into the buffer automatically. You can also do this with the mouse with a single click. To remove the currently selected item, use Control-B. or the remove button.\n";
            mstring += "\nWhen the \"Copy clicked item to buffer and remove\" is checked, an item once clicked on is copied to buffer and is then removed. ";
            mstring += "Use the clear button to remove all items and the remove button to remove the selected items. Multiple items can be selected using the mouse and either the shift or control key. ";
            mstring += "Then use the \"Paste Multiple\" button to copy all the selected items to the buffer. When the \"Use stack\" box is checked, it will be copied from bottom to top. Otherwise, it will be copied from top to bottom.\n";
            mstring += "\nOn enabling split copy, copied text will be monitored for the default delimiter (The regular expression \\r\\n) or if the space separator box is checked, ' ' and will split the input at these points and make a separate entry for each of the split items.\n";
            label1.Text = mstring;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
