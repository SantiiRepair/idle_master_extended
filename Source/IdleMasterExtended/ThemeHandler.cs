using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace IdleMasterExtended
{
    internal class ThemeHandler
    {
        /// <summary>
        /// This class is a static utility class to handle the applied theme coloring setting of all Windows forms.
        /// 
        /// When a theme change is applied through the settings each form will be responsible to change it's colors,
        /// preferrably using this class as it automatically handles each embedded control of the form (e.g. labels, buttons, lists).
        /// </summary>

        static readonly Color DefaultBackColor = SystemColors.Control;
        static readonly Color DefaultForeColor = SystemColors.ControlText;
        static readonly Color DefaultGreenColor = Color.Green;
        static readonly FlatStyle DefaultButtonStyle = FlatStyle.Standard;
        static readonly Color DefaultLinkColor = Color.Blue;

        static readonly Color DarkBackColor = Color.FromArgb(38, 38, 38);
        static readonly Color DarkForeColor = Color.FromArgb(196, 196, 196);
        static readonly Color DarkGreenColor = Color.FromArgb(126, 166, 75);
        static readonly FlatStyle DarkButtonStyle = FlatStyle.Flat;
        static readonly Color DarkLinkColor = Color.GhostWhite;

        /// <summary>
        /// Sets the theme (default or dark theme) of the form
        /// </summary>
        /// <param name="form">The Windows form to apply the theme to</param>
        /// <param name="darkTheme">True if dark theme, else false</param>
        public static void SetTheme(Form form, bool darkTheme)
        {
            SetThemeForm(form, darkTheme);
            SetThemeControls(form.Controls, darkTheme);
        }

        private static void SetThemeForm(Form form, bool darkTheme)
        {
            form.ForeColor = darkTheme ? DarkForeColor : DefaultForeColor;
            form.BackColor = darkTheme ? DarkBackColor : DefaultBackColor;
        }

        private static void SetThemeControls(ControlCollection collection, bool darkTheme)
        {
            foreach (Control control in collection)
            {
                control.BackColor = darkTheme ? DarkBackColor : DefaultBackColor;
                control.ForeColor = darkTheme ? DarkForeColor : DefaultForeColor;

                if (control is Button button)
                {
                    button.FlatStyle = darkTheme ? DarkButtonStyle : DefaultButtonStyle;
                }
                else if (control is LinkLabel linklabel)
                {
                    linklabel.LinkColor = darkTheme ? DarkLinkColor : DefaultLinkColor;
                }
            }
        }
    }
}
