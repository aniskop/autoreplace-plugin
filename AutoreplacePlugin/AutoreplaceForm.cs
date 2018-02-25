using System;
using System.Windows.Forms;

namespace Autoreplace
{
    public partial class AutoreplaceForm : Form
    {

        private AutoreplacePlugin callingPlugin;
        private AutoreplaceList allReplaces;
        private int initialSearchStringLength = 0;

        public AutoreplaceForm()
        {
            InitializeComponent();
        }

        public void Show(AutoreplaceList list, AutoreplacePlugin plugin)
        {
            callingPlugin = plugin;
            allReplaces = list;
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            PopulateAllReplaces();
            CenterToScreen();
            Show();
        }

        private void listReplaces_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            ProcessSelectedItem();
            Close();
        }

        private void ProcessSelectedItem()
        {
            AutoreplaceEntry selectedEntry = (AutoreplaceEntry)listReplaces.SelectedItem;
            if (selectedEntry != null)
            {
                callingPlugin.DoReplace(selectedEntry);
            }
        }

        private void textSearchString_TextChanged(object sender, EventArgs e)
        {
            if (textSearchString.Text.Length > initialSearchStringLength)
            {
                // If search string is getting longer, just remove items from the list control.
                DeleteMismatchingReplaces(textSearchString.Text);
                initialSearchStringLength = textSearchString.Text.Length;
            }
            else
            {
                // If search string became shorter - the list must be longer, thus repopulate.
                // If search string changed but stayed the same length - also repopulate the list.
                PopulateMatchingReplaces(textSearchString.Text);
            }
        }

        private void PopulateAllReplaces()
        {
            listReplaces.Items.Clear();
            foreach (AutoreplaceEntry entry in allReplaces.Entries)
            {
                listReplaces.Items.Add(entry);
            }
        }

        private void PopulateMatchingReplaces(string searchString)
        {
            listReplaces.Items.Clear();
            foreach (AutoreplaceEntry entry in allReplaces.Entries)
            {
                if (entry.ContainsText(searchString, StringComparison.OrdinalIgnoreCase))
                {
                    listReplaces.Items.Add(entry);
                }
            }
        }

        private void DeleteMismatchingReplaces(string searchString)
        {
            AutoreplaceEntry entry = null;
            int i = 0;
            while (i < listReplaces.Items.Count)
            {
                entry = (AutoreplaceEntry)listReplaces.Items[i];
                if (entry.ContainsText(searchString, StringComparison.OrdinalIgnoreCase))
                {
                    i++;
                }
                else
                {
                    listReplaces.Items.RemoveAt(i);
                }
            }
        }

        private void textSearchString_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            else if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                bool success = listReplaces.Focus();
                if (success && listReplaces.Items.Count > 0)
                {
                    listReplaces.SetSelected(0, true);
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (listReplaces.Items.Count == 1)
                {
                    listReplaces.SetSelected(0, true);
                    ProcessSelectedItem();
                    Close();
                }
            }
        }

        private void listReplaces_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            else if (e.KeyCode == Keys.Tab)
            {
                bool success = textSearchString.Focus();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                ProcessSelectedItem();
                Close();
            }
        }

        private void AutoreplaceForm_Deactivate(object sender, EventArgs e)
        {
            Close();
        }
    }
}
