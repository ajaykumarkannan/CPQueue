using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Input;
using System.Collections;
using System.Runtime.InteropServices;


// Clipboard Monitor Code adapted from: http://www.codeguru.com/columns/dotnettips/article.php/c7315/Monitoring-Clipboard-Activity-in-C.htm#overview
// Hotkey from http://www.codeproject.com/Articles/5914/Simple-steps-to-enable-Hotkey-and-ShortcutInput-us

namespace CPQueue
{
    public partial class Form1 : Form
    {

        string mstring;

        IntPtr nextClipboardViewer;
        int selectedItem = 0;
        
        public Form1()
        {
            InitializeComponent();
            nextClipboardViewer = (IntPtr)SetClipboardViewer((int)this.Handle);
            this.listView1.Clear();
            this.listView1.Columns.Add("Text", 450);
            this.listView1.Columns.Add("Added", 90);
            this.listView1.HideSelection = false;
            

            // Alt = 1, Ctrl = 2, Shift = 4, Win = 8
            RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 2, (int)'B');
            RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 2, (int)'E');
            RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 2, (int)'D');
            // UnregisterHotKey(this.Handle, this.GetType().GetHashCode());
            label2.Text = "Shortcuts: Ctrl-B: Delete selected, Ctrl-E: Move up, Ctrl-D: Move Down";
            checkBox2.Checked = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        /*
        // Add button
        private void button2_Click(object sender, EventArgs e)
        {
            mstring = Clipboard.GetText();

            // Insert ListView Code here
            ListViewItem lvi = new ListViewItem(mstring);
            lvi.SubItems.Add(DateTime.Now.ToString("HH:mm:ss tt"));
            listView1.Items.Add(lvi);

        }
        */

        void myListview_MouseDown(Object sender, MouseEventArgs e)
        {
            // Make sure it was a single left click, like the normal Click event
            if ((e.Button == MouseButtons.Left) && (e.Clicks == 1))
            {
                try
                {
                    ListViewHitTestInfo htInfo = listView1.HitTest(e.X, e.Y);
                    mstring = htInfo.Item.Text;
                    Clipboard.SetData(DataFormats.Text, (Object)mstring);
                    if (checkBox1.Checked && listView1.Items.Count > 0)
                    {
                        listView1.Items.RemoveAt(htInfo.Item.Index);
                    }
                    else
                    {
                        selectedItem = htInfo.Item.Index;
                    }
                }
                catch (NullReferenceException ex)
                {
                    ; // Do nothing
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        // Clear Button
        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            this.listView1.Columns.Add("Text", 450);
            this.listView1.Columns.Add("Added", 80);
            selectedItem = 0;

        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            // defined in winuser.h
            const int WM_DRAWCLIPBOARD = 0x308;
            const int WM_CHANGECBCHAIN = 0x030D;
            const int WM_HOTKEY = 0x0312; 

            switch (m.Msg)
            {
                case WM_HOTKEY:
                    Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                    if (key == Keys.B)
                    {
                        if (listView1.Items.Count > 0)
                        {
                            listView1.Items.RemoveAt(selectedItem);
                            if (listView1.Items.Count > 0)
                            {
                                if (!checkBox2.Checked)
                                {
                                    if (selectedItem < listView1.Items.Count - 1) ; // Stays the same
                                    else selectedItem--;
                                }
                                else
                                {
                                    if (selectedItem > 0) selectedItem--;
                                    // else stays the same
                                }

                                loadItem();
                            }
                        }
                    }
                    else if (key == Keys.E)
                    {
                        if (selectedItem > 0 && selectedItem < listView1.Items.Count)
                        {
                            listView1.Items[selectedItem].Selected = false;
                            selectedItem--;
                            loadItem();
                        }
                    }
                    else if (key == Keys.D)
                    {
                        if (selectedItem >=0 && selectedItem < listView1.Items.Count - 1)
                        {
                            listView1.Items[selectedItem].Selected = false;
                            selectedItem++;

                            loadItem();
                        }
                    }
                    base.WndProc(ref m);
                    break;
                case WM_DRAWCLIPBOARD:
                    DisplayClipboardData();
                    SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;
                case WM_CHANGECBCHAIN:
                    if (m.WParam == nextClipboardViewer)
                        nextClipboardViewer = m.LParam;
                    else
                        SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        void DisplayClipboardData()
        {
            try
            {
                IDataObject iData = new DataObject();
                iData = Clipboard.GetDataObject();

                if (iData.GetDataPresent(DataFormats.Text))
                {
                    if (mstring != (string)iData.GetData(DataFormats.Text))
                    {
                        mstring = (string)iData.GetData(DataFormats.Text);
                        ListViewItem tLVI = listView1.FindItemWithText(mstring);
                        if (tLVI == null || tLVI.Text != mstring)
                        {
                            ListViewItem lvi = new ListViewItem(mstring);
                            lvi.SubItems.Add(DateTime.Now.ToString("HH:mm:ss tt"));
                            listView1.Items.Add(lvi);
                            listView1.Items[selectedItem].Selected = false;
                            if (!checkBox2.Checked)
                            {
                                selectedItem = 0;
                                loadItem();
                            }
                            else
                            {
                                selectedItem = listView1.Items.Count - 1;
                                loadItem();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Remove selected button
        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0)
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (i >= listView1.Items.Count) break;
                    
                    if (listView1.Items[i].Selected)
                    {
                        listView1.Items.RemoveAt(selectedItem);
                        i--;
                    }
                }
                if (listView1.Items.Count > 0)
                {
                    if (!checkBox2.Checked) selectedItem = 0;
                    else selectedItem = listView1.Items.Count - 1;
                    loadItem();
                }
                else selectedItem = 0;
            }
        }

        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(listView1.Items.Count > 0) listView1.Items[selectedItem].Selected = false; 

            if (checkBox2.Checked) selectedItem = listView1.Items.Count - 1;
            else selectedItem = 0;
            loadItem();

        }

        private void loadItem()
        {
            if (listView1.Items.Count > 0)
            {
                if (selectedItem < 0) selectedItem = 0;
                else if (selectedItem >= listView1.Items.Count) selectedItem = listView1.Items.Count - 1;
                mstring = listView1.Items[selectedItem].Text;
                Clipboard.SetData(DataFormats.Text, (Object)mstring);
                listView1.Items[selectedItem].Selected = true;
                listView1.EnsureVisible(selectedItem);
            }
            else selectedItem = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mstring = "";
            if (checkBox2.Checked)
            {
                for (int i = listView1.Items.Count - 1; i >= 0; i--)
                {
                    if (listView1.Items[i].Selected)
                    {
                        mstring += listView1.Items[i].Text;
                        if (checkBox3.Checked) mstring += " ";
                        if (checkBox4.Checked) mstring += "\n";
                    }
                }
            }
            else
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].Selected)
                    {
                        mstring += listView1.Items[i].Text;
                        if (checkBox3.Checked) mstring += " ";
                        if (checkBox4.Checked) mstring += "\n";
                    }
                }
            }
            
            Clipboard.SetData(DataFormats.Text, (Object)mstring);
            
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked) checkBox4.Checked = false;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked) checkBox3.Checked = false;
        }
    }
}
