// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BRClib
{
    public class RecentBlendsCollection : Collection<string>
    {
        int _capacity = 10;

        public int MaxElements
        {
            get => _capacity;
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Must be a positive, non-zero value");
                }
                _capacity = value;
            }
        }


        public RecentBlendsCollection() { }

        public RecentBlendsCollection(IList<string> collection) : base(collection)
        { }


        protected override void InsertItem(int index, string item)
        {
            // check if item is already present
            int exIdx = IndexOf(item);
            if (exIdx != -1)
            {
                Items.RemoveAt(exIdx);
                Items.Insert(0, item);
                return;
            }

            // remove last if capacity is reached
            if (index == MaxElements)
            {
                Items.RemoveAt(MaxElements - 1);
            }

            // elements are inserted at the front
            base.InsertItem(0, item);

            System.Diagnostics.Debug.Assert(Count <= MaxElements);
        }

    }
}
