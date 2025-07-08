using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniversityApp
{
    public partial class MainApp : Form
    {
        Form form;
        private void ApplyDarkMode(Control control)
        {
            if (!originalColors.ContainsKey(control))
                originalColors[control] = (control.BackColor, control.ForeColor);

            control.BackColor = Color.FromArgb(30, 30, 30);
            control.ForeColor = Color.White;

            foreach (Control child in control.Controls)
            {
                ApplyDarkMode(child);
            }
        }
        private Dictionary<Control, (Color BackColor, Color ForeColor)> originalColors = new Dictionary<Control, (Color, Color)>();
        private void RestoreOriginalColors(Control control)
        {
            if (originalColors.ContainsKey(control))
            {
                var colors = originalColors[control];
                control.BackColor = colors.BackColor;
                control.ForeColor = colors.ForeColor;
            }

            foreach (Control child in control.Controls)
            {
                RestoreOriginalColors(child);
            }
        }
        public MainApp(Login Login)
        {
            this.form = Login;
            this.FormClosed += this.closeEveryThing;
            InitializeComponent();
        }
        private void closeEveryThing(object sender, EventArgs e)
        {
            this.form.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                ApplyDarkMode(this);
            else
                RestoreOriginalColors(this);
        }
        
    }
}
