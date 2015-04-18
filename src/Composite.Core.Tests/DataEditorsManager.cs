using System.Collections;
using System.Collections.Generic;

namespace Composite.Core.Tests
{
    public class DataEditorsManager<TData> : IEnumerable<IDataEditor<TData>>
    {
        private readonly LinkedList<IDataEditor<TData>> _editors = new LinkedList<IDataEditor<TData>>();
        private TData _editableTarget;

        public void Add(IDataEditor<TData> editor)
        {
            if (_editors.Contains(editor))
            {
                return;
            }

            _editors.AddLast(editor);

            editor.TargetUpdated += OnEditorUpdatesTarget;
            
            editor.EditableTarget = _editableTarget;
        }

        public void Remove(IDataEditor<TData> editor)
        {
            editor.TargetUpdated -= OnEditorUpdatesTarget;
            
            _editors.Remove(editor);
        }

        public TData EditableTarget
        {
            get { return _editableTarget; }
            set
            {
                _editableTarget = value;

                foreach (var editor in _editors)
                {
                    editor.EditableTarget = _editableTarget;
                }
            }
        }

        public IEnumerator<IDataEditor<TData>> GetEnumerator()
        {
            return _editors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void OnEditorUpdatesTarget(object sender, PropertyUpdatedEventArgs e)
        {
            var updater = sender as IDataEditor<TData>;

            if (updater == null)
            {
                return;
            }

            _editableTarget = updater.EditableTarget;

            foreach (var editor in _editors)
            {
                if (editor != sender)
                {
                    editor.EditableTarget = _editableTarget;
                }
            }
        }
    }
}