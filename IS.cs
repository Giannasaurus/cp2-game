using System;
using System.Drawing;
using System.Windows.Forms;

public class InventorySystem : Form
{
    private bool isInventoryOpen = false;

    public InventorySystem()
    {
        this.FormBorderStyle = FormBorderStyle.None;
        this.WindowState = FormWindowState.Maximized;
        this.BackColor = Color.FromArgb(50, 50, 50); // Dark gray background
        this.Opacity = 0.9; // Slight transparency
        this.Visible = false;

        this.KeyDown += Inventory_KeyDown;
    }

    private void Inventory_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.B || e.KeyCode == Keys.Escape)
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        this.Visible = isInventoryOpen;
    }
}
