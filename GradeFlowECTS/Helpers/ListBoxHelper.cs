using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace GradeFlowECTS.Helpers
{
    public static class ListBoxHelper
    {
        public static readonly DependencyProperty BindableSelectedItemsProperty =
            DependencyProperty.RegisterAttached(
                "BindableSelectedItems",
                typeof(IList),
                typeof(ListBoxHelper),
                new PropertyMetadata(null, OnBindableSelectedItemsChanged));

        private static readonly Dictionary<ListBox, NotifyCollectionChangedEventHandler> handlers = new();

        public static IList GetBindableSelectedItems(DependencyObject obj) =>
            (IList)obj.GetValue(BindableSelectedItemsProperty);

        public static void SetBindableSelectedItems(DependencyObject obj, IList value) =>
            obj.SetValue(BindableSelectedItemsProperty, value);

        private static void OnBindableSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListBox listBox)
            {
                listBox.SelectionChanged -= ListBox_SelectionChanged;
                listBox.SelectionChanged += ListBox_SelectionChanged;

                if (e.OldValue is INotifyCollectionChanged oldCollection && handlers.TryGetValue(listBox, out var oldHandler))
                {
                    oldCollection.CollectionChanged -= oldHandler;
                    handlers.Remove(listBox);
                }

                if (e.NewValue is INotifyCollectionChanged newCollection)
                {
                    NotifyCollectionChangedEventHandler newHandler = (_, _) => SyncToListBox(listBox);
                    newCollection.CollectionChanged += newHandler;
                    handlers[listBox] = newHandler;
                }

                SyncToListBox(listBox);
            }
        }

        private static void SyncToListBox(ListBox listBox)
        {
            var boundCollection = GetBindableSelectedItems(listBox);
            if (boundCollection == null) return;

            listBox.SelectionChanged -= ListBox_SelectionChanged;

            try
            {
                // Удаление лишних.
                for (int i = listBox.SelectedItems.Count - 1; i >= 0; i--)
                {
                    var item = listBox.SelectedItems[i];
                    if (!boundCollection.Contains(item))
                        listBox.SelectedItems.Remove(item);
                }

                // Добавление недостающих.
                foreach (var item in boundCollection)
                {
                    if (!listBox.SelectedItems.Contains(item))
                        listBox.SelectedItems.Add(item);
                }
            }
            finally
            {
                listBox.SelectionChanged += ListBox_SelectionChanged;
            }
        }

        private static void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ListBox listBox) return;

            var boundCollection = GetBindableSelectedItems(listBox);
            if (boundCollection == null) return;

            foreach (var removedItem in e.RemovedItems)
            {
                if (boundCollection.Contains(removedItem))
                    boundCollection.Remove(removedItem);
            }

            foreach (var addedItem in e.AddedItems)
            {
                if (!boundCollection.Contains(addedItem))
                    boundCollection.Add(addedItem);
            }
        }
    }
}
