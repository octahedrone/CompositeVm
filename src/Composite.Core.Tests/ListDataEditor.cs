using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Composite.Core.TypeChecks;

namespace Composite.Core.Tests
{
    public class ListDataEditor<TData, TValidationState, TItem, TItemValidationState>
        : IValidatedDataEditor<TData, TValidationState>
    {
        private static readonly INullableCheck<TData> TargetNullCheck = ValueChecks.GetNullableCheck<TData>();
        private static readonly INullableCheck<TValidationState> ValidityNullCheck = ValueChecks.GetNullableCheck<TValidationState>();
        private static readonly INullableCheck<TItemValidationState> ItemValidityNullCheck = ValueChecks.GetNullableCheck<TItemValidationState>();

        private readonly Func<TItem, IDataEditor<TItem>> _itemEditorFactory;
        private readonly Func<TItem, TValidationState, TItemValidationState> _itemValidityRetriever;
        private readonly IPropertyAdapter<TData, IList<TItem>> _propertyAdapter;

        private TData _editableTarget;

        public ListDataEditor(Func<TItem, IDataEditor<TItem>> itemEditorFactory,
            IPropertyAdapter<TData, IList<TItem>> propertyAdapter,
            Func<TItem, TValidationState, TItemValidationState> itemValidityRetriever)
        {
            if (itemEditorFactory == null)
                throw new ArgumentNullException("itemEditorFactory");

            _itemEditorFactory = itemEditorFactory;
            _propertyAdapter = propertyAdapter;
            _itemValidityRetriever = itemValidityRetriever;

            ItemEditors = new ObservableCollection<IDataEditor<TItem>>();
        }

        public ObservableCollection<IDataEditor<TItem>> ItemEditors { get; private set; }

        TData IDataBrowser<TData>.EditableTarget
        {
            get { return _editableTarget; }
            set
            {
                _editableTarget = value;

                var newValue = TargetNullCheck.IsNull(_editableTarget)
                    ? default(IList<TItem>)
                    : _propertyAdapter.GetValue(_editableTarget);

                ResetToState(newValue);
            }
        }

        private void ResetToState(IList<TItem> items)
        {
            if (items == null || items.Count == 0)
            {
                ItemEditors.Clear();

                return;
            }

            var itemsArray = items.ToList();
            var maxLevel = items.Count > ItemEditors.Count ? ItemEditors.Count : items.Count;
            var level = 0;

            for (; level < maxLevel; level++)
            {
                ItemEditors[level].EditableTarget = itemsArray[level];
            }

            if (items.Count > ItemEditors.Count)
            {
                for (; level < itemsArray.Count; level++)
                {
                    AddItemEditor(itemsArray[level]);
                }
            }
            else
            {
                for (; level < itemsArray.Count; level++)
                {
                    ItemEditors.RemoveAt(ItemEditors.Count -1);
                }
            }
        }

        public event EventHandler<EventArgs> TargetUpdated;

        void IValidatedDataEditor<TData, TValidationState>.UpdateValidationState(TValidationState state)
        {
            if (ValidityNullCheck.IsNull(state))
            {
                ((IValidatedDataEditor<TData, TValidationState>) this).ClearValidationState();

                return;
            }

            foreach (var itemEditor in ItemEditors.OfType<IValidatedDataEditor<TItem, TItemValidationState>>())
            {
                var itemValidityState = _itemValidityRetriever(itemEditor.EditableTarget, state);

                if (ItemValidityNullCheck.IsNull(itemValidityState))
                {
                    itemEditor.ClearValidationState();
                }
                else
                {
                    itemEditor.UpdateValidationState(itemValidityState);
                }
            }
        }

        void IValidatedDataEditor<TData, TValidationState>.ClearValidationState()
        {
            foreach (var itemEditor in ItemEditors.OfType<IValidatedDataEditor<TItem, TItemValidationState>>())
            {
                itemEditor.ClearValidationState();
            }
        }

        private void AddItemEditor(TItem item)
        {
            var itemEditor = _itemEditorFactory(item);

            if (itemEditor != null)
            {
                ItemEditors.Add(itemEditor);

                itemEditor.TargetUpdated += OnItemUpdated;
            }
        }

        void OnItemUpdated(object sender, EventArgs e)
        {
            var itemEditor = (IDataEditor<TItem>)sender;

            var index = ItemEditors.IndexOf(itemEditor);

            if (index < 0)
            {
                itemEditor.TargetUpdated -= OnItemUpdated;
                return;
            }

            var items = TargetNullCheck.IsNull(_editableTarget)
                    ? default(IList<TItem>)
                    : _propertyAdapter.GetValue(_editableTarget);

            if (items != null)
            {
                items[index] = itemEditor.EditableTarget;
            }

            var handler = TargetUpdated;
            if (handler != null) 
                handler(this, EventArgs.Empty);
        }
    }
}