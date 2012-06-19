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
using System.Text.RegularExpressions;
using System.IO;


// Clipboard Monitor Code adapted from: http://www.codeguru.com/columns/dotnettips/article.php/c7315/Monitoring-Clipboard-Activity-in-C.htm#overview
// Hotkey from http://www.codeproject.com/Articles/5914/Simple-steps-to-enable-Hotkey-and-ShortcutInput-us

namespace CPQueue
{
    public partial class MainForm : Form
    {

        string mstring;
        System.Drawing.Rectangle screenRectangle; 

        IntPtr nextClipboardViewer;
        int selectedItem = 0;

        // Column Sizes
        int itemSize = 400;
        int timeSize = 90;
        
        // Constructor
        public MainForm()
        {
            InitializeComponent();
            nextClipboardViewer = (IntPtr)SetClipboardViewer((int)this.Handle);
            this.listView1.Clear();
            this.listView1.Columns.Add("Text", itemSize);
            this.listView1.Columns.Add("Added", timeSize);
            this.listView1.HideSelection = false;

            // Alt = 1, Ctrl = 2, Shift = 4, Win = 8
            RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 2, (int)'B');
            RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 2, (int)'E');
            RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 2, (int)'D');
            // UnregisterHotKey(this.Handle, this.GetType().GetHashCode());
            screenRectangle = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(screenRectangle.Width - this.Width, screenRectangle.Height - this.Height);

            checkMainBox();
        }

        private void MainForm_FormClosing(Object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
            
        }


        void myListview_MouseDown(Object sender, MouseEventArgs e)
        {
            // Make sure it was a single left click, like the normal Click event
            if ((e.Button == MouseButtons.Left) && (e.Clicks == 1))
            {
                try
                {
                    ListViewHitTestInfo htInfo = listView1.HitTest(e.X, e.Y);
                    mstring = htInfo.Item.Text;
                    selectedItem = htInfo.Item.Index;
                    if (listView1.Items.Count > 0)
                    {
                        selectedItem = htInfo.Item.Index;

                        Clipboard.SetData(DataFormats.Text, (Object)mstring);
                    }
                }
                catch (NullReferenceException)
                {
                    // MessageBox.Show(ex.ToString()); 
                }
            }
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
                                if (!(bool)CPQueue.Properties.Settings.Default["stack"])
                                {
                                    if (selectedItem >= listView1.Items.Count - 1) selectedItem = listView1.Items.Count - 1;
                                }
                                else
                                {
                                    if (selectedItem > 0) selectedItem--;
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
                            listView1.Items[selectedItem].Focused = false;
                            selectedItem--;
                            loadItem();
                        }
                    }
                    else if (key == Keys.D)
                    {
                        if (selectedItem >=0 && selectedItem < listView1.Items.Count - 1)
                        {
                            listView1.Items[selectedItem].Selected = false;
                            listView1.Items[selectedItem].Focused = false;
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
                            // Default delimiter \r\n

                            if ((bool)CPQueue.Properties.Settings.Default["splitCopy"])
                            {
                                string[] substrings;

                                if (!(bool)CPQueue.Properties.Settings.Default["space"])
                                {
                                    substrings = Regex.Split(mstring, "\r\n");
                                }
                                else
                                {
                                    substrings = mstring.Split(' ');
                                }

                                foreach (string line in substrings)
                                {
                                    if (line.Length > 0)
                                    {
                                        ListViewItem lvi = new ListViewItem(line);
                                        lvi.SubItems.Add(DateTime.Now.ToString("HH:mm:ss tt"));
                                        listView1.Items.Add(lvi);
                                    }
                                }

                            }
                            else
                            {
                                ListViewItem lvi = new ListViewItem(mstring);
                                lvi.SubItems.Add(DateTime.Now.ToString("HH:mm:ss tt"));
                                listView1.Items.Add(lvi);
                            }

                            listView1.Items[selectedItem].Selected = false;
                            listView1.Items[selectedItem].Focused = false;

                            if (!(bool)CPQueue.Properties.Settings.Default["stack"])
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
                    if (!((bool)CPQueue.Properties.Settings.Default["stack"])) selectedItem = 0;
                    else selectedItem = listView1.Items.Count - 1;
                    loadItem();
                }
                else selectedItem = 0;
            }
        }

        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help helpForm = new Help();
            helpForm.ShowDialog();
        }

        // Use stack instead of queue checkbox
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0)
            {
                listView1.Items[selectedItem].Selected = false;
                listView1.Items[selectedItem].Focused = false;
            }

            if (stackBox.Checked)
            {
                selectedItem = listView1.Items.Count - 1;
                CPQueue.Properties.Settings.Default["stack"] = true;
            }
            else
            {
                selectedItem = 0;
                CPQueue.Properties.Settings.Default["stack"] = false;
            }

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
                listView1.Items[selectedItem].Focused = true;
                listView1.EnsureVisible(selectedItem);
            }
            else selectedItem = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mstring = "";
            if((bool) CPQueue.Properties.Settings.Default["stack"])
            {
                for (int i = listView1.Items.Count - 1; i >= 0; i--)
                {
                    if (listView1.Items[i].Selected)
                    {
                        mstring += listView1.Items[i].Text;
                        if ((bool)CPQueue.Properties.Settings.Default["space"]) mstring += " ";
                        if ((bool)CPQueue.Properties.Settings.Default["newline"]) mstring += "\n";
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
                        if ((bool)CPQueue.Properties.Settings.Default["space"]) mstring += " ";
                        if ((bool)CPQueue.Properties.Settings.Default["newline"]) mstring += "\n";
                    }
                }
            }
            
            Clipboard.SetData(DataFormats.Text, (Object)mstring);
            
        }

        // Space separator
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (spaceBox.Checked)
            {
                CPQueue.Properties.Settings.Default["space"] = true;
                CPQueue.Properties.Settings.Default["newline"] = false;
                newlineBox.Checked = false;
            }
            else
            {
                CPQueue.Properties.Settings.Default["space"] = false;
            }
        }

        // Newline separator
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (newlineBox.Checked)
            {
                CPQueue.Properties.Settings.Default["space"] = false;
                CPQueue.Properties.Settings.Default["newline"] = true;
                spaceBox.Checked = false;
            }
            else
            {
                CPQueue.Properties.Settings.Default["newline"] = false;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm1 = new AboutForm();
            aboutForm1.ShowDialog();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            this.listView1.Columns.Add("Text", itemSize);
            this.listView1.Columns.Add("Added", timeSize);
            selectedItem = 0;
        }

        // Write to Text
        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFD.Title = "Choose where you want to save the file to.";
            saveFD.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFD.FileName = "";
            saveFD.Filter = "Text files (*.txt)|*.txt|All files|*.*";
            saveFD.FilterIndex = 1; ;

            if (saveFD.ShowDialog() == DialogResult.OK)
            {
                try
                {
                        string chosenFile = saveFD.FileName;
                        TextWriter tw = new StreamWriter(chosenFile);
                        if (tw != null)
                        {
                            for (int i = 0; i < listView1.Items.Count; i++)
                            {
                                tw.WriteLine(listView1.Items[i].Text);
                            }
                            tw.Close();
                        }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void textToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFD.Title = "Choose the file you wish to import.";
            openFD.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFD.FileName = "";
            openFD.Filter = "Text files (*.txt)|*.txt|All files|*.*";
            openFD.FilterIndex = 1; ;

            if (openFD.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string chosenFile = openFD.FileName;
                    TextReader tr = new StreamReader(chosenFile);
                    if (tr != null)
                    {
                        while ((mstring = tr.ReadLine()) != null)
                        {
                            ListViewItem lvi = new ListViewItem(mstring);
                            lvi.SubItems.Add(DateTime.Now.ToString("HH:mm:ss tt"));
                            listView1.Items.Add(lvi);
                        }
                    }

                    if (!(bool)CPQueue.Properties.Settings.Default["stack"])
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
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void enableMiniMode()
        {
            CPQueue.Properties.Settings.Default["mini"] = true;

            stackBox.Hide();
            spaceBox.Hide();
            newlineBox.Hide();
            helpLabel.Hide();
            button2.Hide();
            button3.Hide();
            splitBox.Hide();
            menuStrip1.Hide();
            miniMode.Location = new Point(0, 0);
            listView1.Columns[0].Width = 125;
            listView1.Columns[1].Width = 0;
            listView1.Width = 150;
            listView1.Height = 150;
            listView1.Location = new Point(0, 20);
            this.Width = 150;
            this.Height = 200;
            this.Location = new Point(screenRectangle.Width - this.Width, screenRectangle.Height - this.Height);
        }

        private void disableMiniMode()
        {
            CPQueue.Properties.Settings.Default["mini"] = false;
            button2.Show();
            button3.Show();
            menuStrip1.Show();
            miniMode.Location = new Point(440, 2);
            listView1.Columns[0].Width = itemSize;
            listView1.Columns[1].Width = timeSize;
            listView1.Width = 513;
            listView1.Height = 170;
            listView1.Location = new Point(12, 27);

            if ((bool)CPQueue.Properties.Settings.Default["mainScreenOptions"])
            {
                stackBox.Show();
                spaceBox.Show();
                splitBox.Show();
                spaceBox.Show();
                newlineBox.Show();
                helpLabel.Show();

                this.Width = 551;
                this.Height = 330;
            }
            else
            {
                stackBox.Hide();
                spaceBox.Hide();
                splitBox.Hide();
                spaceBox.Hide();
                newlineBox.Hide();
                helpLabel.Hide();

                this.Width = 551;
                this.Height = 270;
            }

            this.Location = new Point(screenRectangle.Width - this.Width, screenRectangle.Height - this.Height);
        }

        private void miniMode_CheckedChanged(object sender, EventArgs e)
        {
            if (miniMode.Checked)
            {
                enableMiniMode();
            }
            else
            {
                disableMiniMode();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OptForm optionsForm = new OptForm();
            if (optionsForm.ShowDialog() == DialogResult.OK)
            {
                checkMainBox();
            }
        }

        private void checkMainBox()
        {
            if ((bool)CPQueue.Properties.Settings.Default["mini"])
            {
                miniMode.Checked = true;
                enableMiniMode();
            }
            else
            {
                miniMode.Checked = false;
                disableMiniMode();
            } 

            if ((bool)CPQueue.Properties.Settings.Default["stack"]) stackBox.Checked = true;
            else stackBox.Checked = false;


            if ((bool)CPQueue.Properties.Settings.Default["splitCopy"]) splitBox.Checked = true;
            else splitBox.Checked = false;

            if ((bool)CPQueue.Properties.Settings.Default["space"]) spaceBox.Checked = true;
            else spaceBox.Checked = false;

            if ((bool)CPQueue.Properties.Settings.Default["newline"]) newlineBox.Checked = true;
            else newlineBox.Checked = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void splitBox_CheckedChanged(object sender, EventArgs e)
        {
            if (splitBox.Checked) CPQueue.Properties.Settings.Default["splitCopy"] = true;
            else CPQueue.Properties.Settings.Default["splitCopy"] = false;
        }
    }
}
