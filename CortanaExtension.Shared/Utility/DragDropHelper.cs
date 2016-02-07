using CortanaExtension.Shared.Model;
using CortanaExtension.Shared.Utility;
using CortanaExtension.Shared.Utility.Cortana;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CortanaExtension.Utility
{
    public class DragDropHelper<T> where T : CortanaCommand
    {
        private ObservableCollection<T> _reference;
        private ObservableCollection<T> _selection;
        private string _deletedItem;

        private ListView _source;
        private ListView _target;

        private const string undeleteableListviewName = "ActionPicker";

        public DragDropHelper(ListView source, ListView target)
        {
            _reference = source.ItemsSource as ObservableCollection<T>;
            _selection = target.ItemsSource as ObservableCollection<T>;

            _source = source;
            _target = target;
        }

        /// <summary>
        /// DragItemsStarting is called when the Drag and Drop operation starts
        /// We take advantage of it to set the content of the DataPackage
        /// as well as indicate which operations are supported
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void SourceListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            var obj = e.Items.First();
            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
            List<Type> knownTypes = new List<Type>();
            knownTypes.Add(obj.GetType());
            settings.KnownTypes = knownTypes;
            string json = Serializer.Serialize(obj, settings);
            e.Data.SetText(json);
            // As we want our Reference list to say intact, we only allow Copy
            e.Data.RequestedOperation = DataPackageOperation.Move;
        }

        /// <summary>
        /// DragOver is called when the dragged pointer moves over a UIElement with AllowDrop=True
        /// We need to return an AcceptedOperation != None in either DragOver or DragEnter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TargetListView_DragOver(object sender, DragEventArgs e)
        {
            // Our list only accepts text
            e.AcceptedOperation = (e.DataView.Contains(StandardDataFormats.Text)) ? DataPackageOperation.Move : DataPackageOperation.None;
        }

        /// <summary>
        /// We need to return the effective operation from Drop
        /// This is not important for our source ListView, but it might be if the user
        /// drags text from another source
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void TargetListView_Drop(object sender, DragEventArgs e)
        {
            // This test is in theory not needed as we returned DataPackageOperation.None if
            // the DataPackage did not contained text. However, it is always better if each
            // method is robust by itself
            if (e.DataView.Contains(StandardDataFormats.Text))
            {
                // We need to take a Deferral as we won't be able to confirm the end
                // of the operation synchronously
                var def = e.GetDeferral();
                var json = await e.DataView.GetTextAsync();
                T obj = Serializer.Deserialize<T>(json);
                obj = (T)obj.UnPack();

                if (!_target.Name.Equals(undeleteableListviewName))
                {
                    _selection.Add(obj);
                }
                if (!_source.Name.Equals(undeleteableListviewName)) {
                     _reference.Remove(obj); 
                }
               

                e.AcceptedOperation = DataPackageOperation.Move;
                def.Complete();
            }
        }

        /// <summary>
        /// DragtemsStarting is called for D&D and reorder as the framework does not
        /// know wherer the user will drop the items. Reorder means that the target
        /// and the source ListView are the same.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TargetListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            // The ListView is declared with selection mode set to Single.
            // But we want the code to be robust
            if (e.Items.Count == 1)
            {
                e.Data.SetText(e.Items[0] as string);
                // Reorder or move to trash are always a move
                e.Data.RequestedOperation = DataPackageOperation.Move;
                _deletedItem = null;
            }
        }

        /// <summary>
        /// Called at the end of the operation, whether it was a reorder or move to trash
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void TargetListView_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            // args.DropResult is always Move and therefore we have to rely on _deletedItem to distinguish
            // between reorder and move to trash
            // Another solution would be to listen for events in the ObservableCollection

            T obj;
            if (_deletedItem != null && !_source.Name.Equals(undeleteableListviewName))
            {
                obj = Serializer.Deserialize<T>(_deletedItem);
                obj = (T)obj.UnPack();
                //_selection.Remove(_deletedItem);
                _reference.Remove(obj);
                //_selection.Remove(obj);
                _deletedItem = null;
            }
        }

        /// <summary>
        /// Entering the Trash icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TargetTextBlock_DragEnter(object sender, DragEventArgs e)
        {
            // Trash only accepts text
            //e.Data.RequestedOperation = DataPackageOperation.Move;
            //e.AcceptedOperation = (e.DataView.Contains(StandardDataFormats.Text) ? DataPackageOperation.Move : DataPackageOperation.None);
            e.AcceptedOperation = DataPackageOperation.Move;

            // We don't want to show the Move icon
            e.DragUIOverride.IsGlyphVisible = false;
            e.DragUIOverride.Caption = "Drop item here to remove it from selection";
        }


        /// <summary>
        /// Drop on the Trash
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void TargetTextBlock_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.Text))
            {
                // We need to take the deferral as the source will read _deletedItem which
                // we cannot set synchronously
                var def = e.GetDeferral();
                string raw = await e.DataView.GetTextAsync();
                _deletedItem = raw;
                e.AcceptedOperation = DataPackageOperation.Move;
                def.Complete();
            }
        }
    }
}
