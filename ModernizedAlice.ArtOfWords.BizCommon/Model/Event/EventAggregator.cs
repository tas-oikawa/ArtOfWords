using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Event
{
    public static class EventAggregator
    {
        //デリゲートの宣言
        //TimeEventArgs型のオブジェクトを返すようにする
        public delegate void ShowWindowEventHandler(object sender, ShowWindowEventArgs e);

        //イベントデリゲートの宣言
        public static event ShowWindowEventHandler ShowEventRised;

        public static void OnShowWindow(object sender, ShowWindowEventArgs e)
        {
            if (ShowEventRised != null)
            {
                ShowEventRised(sender, e);
            }
        }

        public delegate void MoveDocumentIndexEventHandler(object sender, MoveDocumentIndexEventArgs e);

        public static event MoveDocumentIndexEventHandler MoveDocumentIndexEventRised;

        public static void OnMoveDocumentIndex(object sender, MoveDocumentIndexEventArgs e)
        {
            if(MoveDocumentIndexEventRised != null)
            {
                MoveDocumentIndexEventRised(sender, e);
            }
        }

        public delegate void ReplaceWordEventHandler(object sender, ReplaceWordEventArgs e);

        public static event ReplaceWordEventHandler ReplaceEventRised;

        public static void OnReplaceWordEvent(object sender, ReplaceWordEventArgs e)
        {
            if (ReplaceEventRised != null)
            {
                ReplaceEventRised(sender, e);
            }
            EventAggregator.OnModelDataChanged(sender, new ModelValueChangedEventArgs());
        }


        public delegate void AdsLoadedEventHandler(object sender, AdsLoadedEventArgs e);

        public static event AdsLoadedEventHandler AdsLoaded;

        public static void OnAdsLoadedEvent(object sender, AdsLoadedEventArgs e)
        {
            if (AdsLoaded != null)
            {
                AdsLoaded(sender, e);
            }
        }


        public delegate void DataReloadedHandler(object sender, DataReloadedEventArgs e);

        public static event DataReloadedHandler DataReloaded;

        public static void OnDataReloaded(object sender, DataReloadedEventArgs e)
        {
            if (DataReloaded != null)
            {
                DataReloaded(sender, e);
            }
        }

        public delegate void ModelValueChangedHandler(object sender, ModelValueChangedEventArgs e);

        public static event ModelValueChangedHandler ModelValueChanged;

        public static void OnModelDataChanged(object sender, ModelValueChangedEventArgs e)
        {
            if (ModelValueChanged != null)
            {
                ModelValueChanged(sender, e);
            }
        }


        public delegate void SaveEventOccured(object sender, SaveEventArgs e);

        public static event SaveEventOccured SaveSucceeded;

        public static void OnSaved(object sender, SaveEventArgs e)
        {
            if (SaveSucceeded != null)
            {
                SaveSucceeded(sender, e);
            }
        }


        public delegate void TryCloseOccured(object sender, TryCloseEventArgs e);

        public static event TryCloseOccured TryClose;

        public static void OnTryClose(object sender, TryCloseEventArgs e)
        {
            if (TryClose != null)
            {
                TryClose(sender, e);
            }
        }

        public delegate void TrySaveOccured(object sender, TrySaveOccuredEventArgs e);

        public static event TrySaveOccured TrySave;

        public static void OnTrySave(object sender, TrySaveOccuredEventArgs e)
        {
            if (TrySave != null)
            {
                TrySave(sender, e);
            }
        }

        public delegate void TryOpenFileOccured(object sender, TryOpenEventArgs e);

        public static event TryOpenFileOccured TryOpen;

        public static void OnTryOpen(object sender, TryOpenEventArgs e)
        {
            if (TryOpen != null)
            {
                TryOpen(sender, e);
            }
        }

        public delegate void TryCreateNewOccured(object sender, TryCreateEventArgs e);

        public static event TryCreateNewOccured TryCreateNew;

        public static void OnTryCreateNew(object sender, TryCreateEventArgs e)
        {
            if (TryCreateNew != null)
            {
                TryCreateNew(sender, e);
            }
        }

        public delegate void TryCreateNewPlusOccured(object sender, TryCreateEventArgs e);

        public static event TryCreateNewPlusOccured TryCreateNewPlus;

        public static void OnTryCreateNewPlus(object sender, TryCreateEventArgs e)
        {
            if (TryCreateNewPlus != null)
            {
                TryCreateNewPlus(sender, e);
            }
        }


        public delegate void AddIMarkableOcccured(object sender, AddIMarkableModelEventArgs e);

        public static event AddIMarkableOcccured AddIMarkableHandler;

        public static void OnAddIMarkable(object sender, AddIMarkableModelEventArgs e)
        {
            if (AddIMarkableHandler != null)
            {
                AddIMarkableHandler(sender, e);
            }
        }

        public delegate void DeleteIMarkableOcccured(object sender, DeleteIMarkableModelEventArgs e);

        public static event DeleteIMarkableOcccured DeleteIMarkableHandler;

        public static void OnDeleteIMarkable(object sender, DeleteIMarkableModelEventArgs e)
        {
            if (DeleteIMarkableHandler != null)
            {
                DeleteIMarkableHandler(sender, e);
            }
        }

        public delegate void ChangeTabOccured(object sender, ChangeTabEventArg e);

        public static event ChangeTabOccured ChangeTabOccuredHandler;

        public static void OnChangeTabOccured(object sender, ChangeTabEventArg e)
        {
            if (ChangeTabOccuredHandler != null)
            {
                ChangeTabOccuredHandler(sender, e);
            }
        }

        public delegate void SelectObjectForce(object sender, SelectObjectForceEventArgs e);

        public static event SelectObjectForce SelectObjectForceHandler;

        public static void OnSelectObjectForceOccured(object sender, SelectObjectForceEventArgs e)
        {
            if (SelectObjectForceHandler != null)
            {
                SelectObjectForceHandler(sender, e);
            }
        }

        public delegate void FontSettingChanged(object sender, int dummy);
        public static event FontSettingChanged FontSettingChangedHandler;

        public static void OnFontSettingChanged(object sender, int dummy)
        {
            if (FontSettingChangedHandler != null)
            {
                FontSettingChangedHandler(sender, dummy);
            }
        }

        public delegate void TagModelModified(object sender, TagModelModifiedEventArgs arg);
        public static event TagModelModified TagModelModifiedHandler;

        public static void OnTagModelModified(object sender, TagModelModifiedEventArgs arg)
        {
            if (TagModelModifiedHandler != null)
            {
                TagModelModifiedHandler(sender, arg);
            }
        }
    }
}
