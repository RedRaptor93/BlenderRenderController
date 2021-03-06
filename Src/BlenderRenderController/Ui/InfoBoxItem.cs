﻿// Part of the Blender Render Controller project
// https://github.com/rehdi93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BlenderRenderController.Ui
{
    public partial class InfoBoxItem : UserControl
    {
        [Category("Info")]
        public string Title
        {
            get => titleLabel.Text;
            set => titleLabel.Text = value;
        }

        [Category("Info"), Bindable(BindableSupport.Yes)]
        public string Value
        {
            get => valueLabel.Text;
            set => valueLabel.Text = value;
        }


        public InfoBoxItem()
        {
            InitializeComponent();

            AdjustTitleFont();
        }


        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            AdjustTitleFont();
        }


        private void AdjustTitleFont()
        {
            var tSize = this.Font.SizeInPoints + 1.0f;
            titleLabel.Font = new Font(Font.FontFamily, tSize, GraphicsUnit.Point);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            var minSize = new Size
            {
                Width = titleLabel.Width,
                Height = titleLabel.Height + valueLabel.Height
            };

            this.MinimumSize = minSize;
        }

    }
}
