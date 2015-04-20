using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Composite.Core.Validation;

namespace Composite.Core
{
    public class DataEditorsManager<TData, TValidationResult> : IEnumerable<IDataEditor<TData>>
    {
        private readonly LinkedList<IDataEditor<TData>> _editors = new LinkedList<IDataEditor<TData>>();
        private readonly IValidator<TData, TValidationResult> _validator;

        private TData _editableTarget;
        private TValidationResult _validationState;

        public DataEditorsManager(IValidator<TData, TValidationResult> validator)
        {
            if (validator == null) throw new ArgumentNullException("validator");

            _validator = validator;
        }
        
        public void Add(IDataEditor<TData> editor)
        {
            if (_editors.Contains(editor))
            {
                return;
            }

            _editors.AddLast(editor);

            editor.TargetUpdated += OnEditorUpdatesTarget;

            // update target
            editor.EditableTarget = _editableTarget;

            // update validation state
            var validatedEditor = editor as IValidatedDataEditor<TData, TValidationResult>;

            if (validatedEditor != null)
            {
                validatedEditor.UpdateValidationState(_validationState);
            }
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
                UpdateEditableTargets(value, null);

                UpdateValidityState();
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

            UpdateEditableTargets(updater.EditableTarget, updater);

            UpdateValidityState();
        }

        private void UpdateEditableTargets(TData target, IDataEditor<TData> exception)
        {
            _editableTarget = target;
            foreach (var editor in _editors)
            {
                if (editor != exception)
                {
                    editor.EditableTarget = _editableTarget;
                }
            }
        }

        private void UpdateValidityState()
        {
            _validationState = _validator.Validate(_editableTarget);

            foreach (var validatedEditor in _editors.OfType<IValidatedDataEditor<TData, TValidationResult>>())
            {
                validatedEditor.UpdateValidationState(_validationState);
            }
        }
    }
}