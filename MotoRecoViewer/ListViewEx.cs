using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

public class ListViewEx : ListView
{
    public ListViewEx() : base()
    {
        this.SetStyle(ControlStyles.EnableNotifyMessage, true);

        this.EditBox = new TextBox();
        this.EditBox.Parent = this;
        this.EditBox.Visible = false;
        this.EditBox.BorderStyle = BorderStyle.FixedSingle;
        this.EditBox.Leave += EditBox_Leave;
        this.EditBox.KeyPress += EditBox_KeyPress;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.EditBox.KeyPress -= EditBox_KeyPress;
            this.EditBox.Leave -= EditBox_Leave;
            this.EditBox.Dispose();
        }
        base.Dispose(disposing);
    }

    TextBox EditBox;

    private void EditBox_Leave(object sender, EventArgs e)
    {
        CurrentColumn.Text = EditBox.Text;
        EditBox.Visible = false;
    }

    private void EditBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        switch (e.KeyChar)
        {
            case (char)Keys.Enter:
                this.Focus();
                e.Handled = true;
                break;

            case (char)Keys.Escape:
                EditBox.Text = CurrentColumn.Text;
                this.Focus();
                e.Handled = true;
                break;
        }
    }

    [Browsable(false)]
    public ListViewItem CurrentRow { get; set; }

    [Browsable(false)]
    public ListViewItem.ListViewSubItem CurrentColumn { get; set; }

    [Browsable(false)]
    public int CurrentRowIndex { get { return (CurrentRow == null) ? -1 : CurrentRow.Index; } }

    [Browsable(false)]
    public int CurrentColumnIndex { get { return (CurrentColumn == null) ? -1 : CurrentRow.SubItems.IndexOf(CurrentColumn); } }

    protected override void OnItemCheck(ItemCheckEventArgs ice)
    {
        if (CurrentColumn != null && CurrentColumnIndex != 0)
            ice.NewValue = ice.CurrentValue;

        base.OnItemCheck(ice);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Point loc = this.PointToClient(Cursor.Position);

        CurrentRow = null;
        CurrentColumn = null;

        CurrentRow = this.GetItemAt(loc.X, loc.Y);
        if (CurrentRow == null || !CurrentRow.Bounds.Contains(loc))
            CurrentRow = null;
        else
        {
            CurrentColumn = CurrentRow.GetSubItemAt(loc.X, loc.Y);
            if (CurrentColumn == null || !CurrentColumn.Bounds.Contains(loc))
                CurrentColumn = null;
        }

        base.OnMouseDown(e);
    }

    protected override void OnResize(EventArgs e)
    {
        this.Focus();
        base.OnResize(e);
    }

    protected override void OnColumnWidthChanging(ColumnWidthChangingEventArgs e)
    {
        this.Focus();
        base.OnColumnWidthChanging(e);
    }

    protected override void OnNotifyMessage(Message m)
    {
        switch (m.Msg)
        {
            case WM_HSCROLL:
            case WM_VSCROLL:
                this.Focus();
                break;
        }
        base.OnNotifyMessage(m);
    }

    const int WM_HSCROLL = 0x114;
    const int WM_VSCROLL = 0x115;

    public void EditColumn()
    {
        if (CurrentColumn == null || CurrentColumnIndex == 0) return;

        Rectangle rect = CurrentColumn.Bounds;
        rect.Intersect(this.ClientRectangle);
        rect.Y -= 1;

        EditBox.Bounds = rect;
        EditBox.Text = CurrentColumn.Text;
        EditBox.Visible = true;
        EditBox.BringToFront();
        EditBox.Focus();
    }
}