// Part of the Blender Render Controller project
// https://github.com/rehdi93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

// based on: https://www.codeproject.com/Articles/12835/XP-Style-Collapsible-GroupBox

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BlenderRenderController.Ui
{
    /// <summary>
    /// GroupBox control that provides functionality to 
    /// allow it to be collapsed.
    /// </summary>
    //[ToolboxBitmap(typeof(CollapsibleGroupBox))]
    public partial class CollapsibleGroupBox : GroupBox
    {
        #region Fields

        private Rectangle m_toggleRect = new Rectangle(8, 2, 11, 11);
        private Boolean m_collapsed = false;
        private Boolean m_bResizingFromCollapse = false;

        private const int m_collapsedHeight = 20;
        private static readonly Color m_defaultForeColor = Color.FromArgb(0, 70, 213);

        private int m_expandedHeight;
        private Size m_FullSize = Size.Empty;

        #endregion

        #region Events & Delegates

        /// <summary>
        /// Fired when the Collapse Toggle button is pressed
        /// </summary>
        public event EventHandler CollapseToggleClicked;

        #endregion

        #region Constructor

        public CollapsibleGroupBox()
        {
            //InitializeComponent();
            ForeColor = m_defaultForeColor;
        }

        #endregion

        #region Public Properties

        [DefaultValue(100), Category("Layout")]
        public int FullHeight
        {
            get { return m_expandedHeight; }
            set
            {
                if (value != m_expandedHeight && value > m_collapsedHeight)
                {
                    m_expandedHeight = value;
                    m_FullSize.Height = value;
                    if (!IsCollapsed) this.Height = value;
                }
            }
        }

        [DefaultValue(false), Category("Layout")]
        public bool IsCollapsed
        {
            get { return m_collapsed; }
            set
            {
                if (value != m_collapsed)
                {
                    m_collapsed = value;

                    if (!value)
                    {
                        // Expand
                        this.Size = m_FullSize;
                    }
                    else
                    {
                        // Collapse
                        m_bResizingFromCollapse = true;
                        this.Height = m_collapsedHeight;
                        m_bResizingFromCollapse = false;
                    }

                    foreach (Control c in Controls)
                    {
                        c.Visible = !value;
                    }

                    Invalidate();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CollapsedHeight
        {
            get { return m_collapsedHeight; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int HeightRequest
        {
            get
            {
                var hRqst = m_FullSize.Height - m_collapsedHeight;
                return m_collapsed ? -hRqst : hRqst;
            }
        }

        #endregion

        #region Overrides

        protected override Size DefaultMinimumSize => new Size(0, m_collapsedHeight);

        [DefaultValue(typeof(Color), "#0046D5")]
        public override Color ForeColor
        {
            get => base.ForeColor;
            set => base.ForeColor = value;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (m_toggleRect.Contains(e.Location))
                ToggleCollapsed();
            else
                base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            HandleResize();
            DrawGroupBox(e.Graphics);
            DrawToggleButton(e.Graphics);
        }

        protected override void OnCreateControl()
        {
            // handle case of starting collapsed
            if (m_collapsed)
            {
                m_FullSize = new Size(this.Width, m_expandedHeight);
            }

            base.OnCreateControl();
        }

        protected override void OnResize(EventArgs e)
        {
            if (m_FullSize.Width != this.Width)
                m_FullSize.Width = this.Width;

            if (DesignMode && m_collapsed)
            {
                this.Height = CollapsedHeight;
                return;
            }

            base.OnResize(e);
        }

        #endregion

        #region Implimentation

        void DrawGroupBox(Graphics g)
        {
            // Get windows to draw the GroupBox
            Rectangle bounds = new Rectangle(ClientRectangle.X, ClientRectangle.Y + 6, ClientRectangle.Width, ClientRectangle.Height - 6);
            GroupBoxRenderer.DrawGroupBox(g, bounds, Enabled ? GroupBoxState.Normal : GroupBoxState.Disabled);

            // Text Formating positioning & Size
            StringFormat sf = new StringFormat();
            int i_textPos = (bounds.X + 8) + m_toggleRect.Width + 2;
            int i_textSize = (int)g.MeasureString(Text, this.Font).Width;
            if (i_textSize < 1) i_textSize = 1;
            int i_endPos = i_textPos + i_textSize + 1;

            // Draw a line to cover the GroupBox border where the text will sit
            using (Pen p_bgPen = new Pen(BackColor, 1f))
                g.DrawLine(p_bgPen, i_textPos, bounds.Y, i_endPos, bounds.Y);

            // Draw the GroupBox text
            using (SolidBrush drawBrush = new SolidBrush(ForeColor))
                g.DrawString(Text, this.Font, drawBrush, i_textPos, 0);
        }

        void DrawToggleButton(Graphics g)
        {
            if(IsCollapsed)
                g.DrawImage(Properties.Resources.cgb_plus, m_toggleRect);
            else
                g.DrawImage(Properties.Resources.cgb_minus, m_toggleRect);
        }

        void ToggleCollapsed()
        {
            IsCollapsed = !IsCollapsed;
            CollapseToggleClicked?.Invoke(this, EventArgs.Empty);
        }

        void HandleResize()
        {
            if (!m_bResizingFromCollapse && !m_collapsed)
            {
                m_FullSize = this.Size;
            }
        }

        #endregion
    }
}