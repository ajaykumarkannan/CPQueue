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
    public partial class OptForm : Form
    {
        public OptForm()
        {
            InitializeComponent();
            
            if ((bool)CPQueue.Properties.Settings.Default["stack"]) stackBox.Checked = true;
            else stackBox.Checked = false;


            if ((bool)CPQueue.Properties.Settings.Default["mainScreenOptions"]) showOptionsBox.Checked = true;
            else showOptionsBox.Checked = false;

            if ((bool)CPQueue.Properties.Settings.Default["mini"]) miniBox.Checked = true;
            else miniBox.Checked = false;
            
            if ((bool)CPQueue.Properties.Settings.Default["splitCopy"]) splitCopyBox.Checked = true;
            else splitCopyBox.Checked = false;

            if ((bool)CPQueue.Properties.Settings.Default["space"]) spaceBox.Checked = true;
            else spaceBox.Checked = false;

            if ((bool)CPQueue.Properties.Settings.Default["newline"]) newlineBox.Checked = true;
            else newlineBox.Checked = false; 

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (stackBox.Checked)
            {
                CPQueue.Properties.Settings.Default["stack"] = true;
            }
            else
            {
                CPQueue.Properties.Settings.Default["stack"] = false;
            }

            if (showOptionsBox.Checked)
            {
                CPQueue.Properties.Settings.Default["mainScreenOptions"] = true;
            }
            else
            {
                CPQueue.Properties.Settings.Default["mainScreenOptions"] = false;
            }

            if (miniBox.Checked)
            {
                CPQueue.Properties.Settings.Default["mini"] = true;
            }
            else
            {
                CPQueue.Properties.Settings.Default["mini"] = false;
            }

            if (splitCopyBox.Checked)
            {
                CPQueue.Properties.Settings.Default["splitCopy"] = true;
            }
            else
            {
                CPQueue.Properties.Settings.Default["splitCopy"] = false;
            }

            // Delimiter
            if (spaceBox.Checked)
            {
                CPQueue.Properties.Settings.Default["space"] = true;
            }
            else
            {
                CPQueue.Properties.Settings.Default["space"] = false;
            }
            if (newlineBox.Checked)
            {
                CPQueue.Properties.Settings.Default["newline"] = true;
            }
            else
            {
                CPQueue.Properties.Settings.Default["newline"] = false;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void spaceBox_CheckedChanged(object sender, EventArgs e)
        {
            if (spaceBox.Checked) newlineBox.Checked = false;
        }

        private void newlineBox_CheckedChanged(object sender, EventArgs e)
        {
            if (newlineBox.Checked) spaceBox.Checked = false;
        }
    }
}
