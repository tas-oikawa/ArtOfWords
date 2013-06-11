using ModernizedAlice.ArtOfWords.BizCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TagsGrooveControls.View;

namespace TagsGrooveControls.Model
{
    public class TagSelectorViewModel
    {
        private TagsGrooveTreeViewModel _treeViewModel;
        private TagsGrooveTreeView _treeView;

        public void SetView(TagsGrooveTreeView treeView)
        {
            _treeView = treeView;

            _treeViewModel = new TagsGrooveTreeViewModel(_treeView);

            _treeViewModel.Init(ModelsComposite.TagManager);
        }
    }
}
