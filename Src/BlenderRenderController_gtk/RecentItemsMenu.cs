﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace BlenderRenderController
{
    class RecentItemsMenu : Menu, IRecentChooser
    {
        const string RECENT_ITEM_NAME = "recent";
        RecentChooserWidget _RecentChooser;
        RecentManager _Manager;

        ImageMenuItem miClearRecents = new ImageMenuItem("gtk-clear", null);
        MenuItem miEmptyPH = new MenuItem("Empty") { Sensitive = false };


        public RecentItemsMenu() : this(null)
        {
        }

        public RecentItemsMenu(RecentFilter filter)
        {
            _Manager = RecentManager.Default;

            _RecentChooser = new RecentChooserWidget();
            _RecentChooser.Filter = filter;
            _RecentChooser.SortType = RecentSortType.Mru;

            miClearRecents.Activated += (s, e) => OnClearRecentsClicked();

            Add(miClearRecents);
            Add(new SeparatorMenuItem());
            Add(miEmptyPH);

            _Manager.Changed += RecentManager_ItemsChanged;
        }



        public event EventHandler<RecentInfo> RecentItem_Clicked;

        public event CancelHandler Clear_Clicked;


        protected override void OnShowAll()
        {
            PlaceholderVisiblity();
            base.OnShowAll();
        }

        protected override void OnShown()
        {
            base.OnShown();
            UpdateRecentItems();
        }


        private new void Add(Widget widget) => base.Add(widget);
        private new void Remove(Widget widget) => base.Remove(widget);

        private void RecentManager_ItemsChanged(object sender, EventArgs e)
        {
            //UpdateRecentItems();
        }

        void UpdateRecentItems()
        {
            ClearRecentMenuItems();

            if (PlaceholderVisiblity())
                return;

            foreach (var item in Items)
            {
                var mi = new ImageMenuItem(item.DisplayName)
                {
                    TooltipText = item.UriDisplay,
                    Name = RECENT_ITEM_NAME,
                    Image = new Image(item.GetIcon(16))
                };
                mi.Activated += delegate { OnRecentItemClicked(item); };
                Append(mi);
                mi.ShowAll();
            }

        }

        void ClearRecentMenuItems()
        {
            foreach (var item in GetRecentMenuItems())
            {
                Remove(item);
            }
        }

        bool PlaceholderVisiblity()
        {
            if (Items.Length == 0)
            {
                miEmptyPH.Show();
            }
            else
            {
                miEmptyPH.Hide();
            }

            return miEmptyPH.Visible;
        }

        private void OnRecentItemClicked(RecentInfo info)
        {
            RecentItem_Clicked?.Invoke(this, info);
        }

        private void OnClearRecentsClicked()
        {
            var args = new CancelArgs();

            if (Clear_Clicked == null)
                return;

            Clear_Clicked(this, args);
            Console.WriteLine("Should clear? " + args.RetVal);

            if ((bool)args.RetVal)
            {
                return;
            }

            _Manager.PurgeItems();
            //ClearRecentMenuItems();
        }

        IEnumerable<MenuItem> GetRecentMenuItems() =>
            AllChildren.Cast<MenuItem>().Where(m => m.Name == RECENT_ITEM_NAME);


        class RecentMenuItem : MenuItem
        {
            public RecentMenuItem()
            {
            }

            public RecentMenuItem(string label) : base(label)
            {
            }

            public RecentInfo Info { get; set; }
        }

        #region IRecentChooser Impl
        //
        // Public
        //

        public RecentInfo CurrentItem => ((IRecentChooser)_RecentChooser).CurrentItem;

        public string CurrentUri => _RecentChooser.CurrentItem.UriDisplay;

        public RecentFilter Filter
        {
            get => ((IRecentChooser)_RecentChooser).Filter;
            set => ((IRecentChooser)_RecentChooser).Filter = value;
        }

        public RecentFilter[] Filters => ((IRecentChooser)_RecentChooser).Filters;

        public void AddFilter(RecentFilter filter)
        {
            ((IRecentChooser)_RecentChooser).AddFilter(filter);
        }

        public void RemoveFilter(RecentFilter filter)
        {
            ((IRecentChooser)_RecentChooser).RemoveFilter(filter);
        }

        public RecentInfo[] Items => ((IRecentChooser)_RecentChooser).Items;
        public RecentSortType SortType { get => ((IRecentChooser)_RecentChooser).SortType; set => ((IRecentChooser)_RecentChooser).SortType = value; }
        public bool ShowNotFound { get => ((IRecentChooser)_RecentChooser).ShowNotFound; set => ((IRecentChooser)_RecentChooser).ShowNotFound = value; }
        public int Limit { get => ((IRecentChooser)_RecentChooser).Limit; set => ((IRecentChooser)_RecentChooser).Limit = value; }

        //
        // Explicit
        //

        bool IRecentChooser.ShowIcons { get => ((IRecentChooser)_RecentChooser).ShowIcons; set => ((IRecentChooser)_RecentChooser).ShowIcons = value; }
        bool IRecentChooser.ShowTips { get => ((IRecentChooser)_RecentChooser).ShowTips; set => ((IRecentChooser)_RecentChooser).ShowTips = value; }
        bool IRecentChooser.LocalOnly { get => ((IRecentChooser)_RecentChooser).LocalOnly; set => ((IRecentChooser)_RecentChooser).LocalOnly = value; }
        bool IRecentChooser.SelectMultiple { get => ((IRecentChooser)_RecentChooser).SelectMultiple; set => ((IRecentChooser)_RecentChooser).SelectMultiple = value; }
        bool IRecentChooser.ShowPrivate { get => ((IRecentChooser)_RecentChooser).ShowPrivate; set => ((IRecentChooser)_RecentChooser).ShowPrivate = value; }
        RecentSortFunc IRecentChooser.SortFunc { set => ((IRecentChooser)_RecentChooser).SortFunc = value; }

        string[] IRecentChooser.GetUris(out ulong length)
        {
            return ((IRecentChooser)_RecentChooser).GetUris(out length);
        }

        void IRecentChooser.SelectAll()
        {
            ((IRecentChooser)_RecentChooser).SelectAll();
        }

        bool IRecentChooser.SelectUri(string uri)
        {
            return ((IRecentChooser)_RecentChooser).SelectUri(uri);
        }

        bool IRecentChooser.SetCurrentUri(string uri)
        {
            return ((IRecentChooser)_RecentChooser).SetCurrentUri(uri);
        }

        void IRecentChooser.UnselectAll()
        {
            ((IRecentChooser)_RecentChooser).UnselectAll();
        }

        void IRecentChooser.UnselectUri(string uri)
        {
            ((IRecentChooser)_RecentChooser).UnselectUri(uri);
        } 
        #endregion
    }
    
}