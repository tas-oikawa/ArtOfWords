using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TagsGrooveControls.Model
{
    class BaseTagTreeViewItemModel : TagTreeViewItemModel    
    {
        public BaseTagTreeViewItemModel(int id)
            : base(id)
        {
        }

        public override bool IsBase()
        {
            return true;
        }
    }
}
