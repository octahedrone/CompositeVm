using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Composite.Core.TypeChecks;
using Composite.Core.Validation;

namespace Composite.Core
{
    public class DataEditorsManager<TData, TValidationResult> : IEnumerable<IDataEditor<TData>>
    {
        private static readonly INullableCheck<TData> TargetNullCheck = ValueChecks.GetNullableCheck<TData>();

        private static readonly INullableCheck<TValidationResult> ValidationNullCheck =
            ValueChecks.GetNullableCheck<TValidationResult>();

        private readonly LinkedList<IDataEditor<TData>> _editors = new LinkedList<IDataEditor<TData>>();
        private readonly IValidator<TData, TValidationResult> _validator;

        private TData _editableTarget;
        private TValidationResult _validationState;

        public DataEditorsManager(IValidator<TData, TValidationResult> validator)
        {
            if (validator == null) throw new ArgumentNullException("validator");

            _validator = validator;
        }

        public event EventHandler<EventArgs> ValidationStateUpdated;

        public TData EditableTarget
        {
            get { return _editableTarget; }
            set
            {
                UpdateEditableTargets(value, null);

                UpdateValidityState(_editableTarget, _editors.OfType<IValidatedComponent<TValidationResult>>());
            }
        }

        public TValidationResult ValidationState
        {
            get { return _validationState; }
            private set
            {
                _validationState = value;

                OnValidationStateUpdated();
            }
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
            var validatedEditor = editor as IValidatedComponent<TValidationResult>;

            if (validatedEditor != null)
            {
                UpdateEditorValidityState(validatedEditor);
            }
        }

        public void Remove(IDataEditor<TData> editor)
        {
            editor.TargetUpdated -= OnEditorUpdatesTarget;

            _editors.Remove(editor);
        }

        public IEnumerator<IDataEditor<TData>> GetEnumerator()
        {
            return _editors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void OnEditorUpdatesTarget(object sender, EventArgs e)
        {
            var updater = sender as IDataEditor<TData>;

            if (updater == null)
            {
                return;
            }

            UpdateEditableTargets(updater.EditableTarget, updater);

            UpdateValidityState(_editableTarget, _editors.OfType<IValidatedComponent<TValidationResult>>());
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

        private void UpdateValidityState(TData editableTarget, 
            IEnumerable<IValidatedComponent<TValidationResult>> validatedComponents)
        {
            ValidationState = TargetNullCheck.IsNull(editableTarget)
                ? default(TValidationResult)
                : _validator.Validate(editableTarget);

            if (ValidationNullCheck.IsNull(ValidationState))
            {
                foreach (var validatedEditor in validatedComponents)
                {
                    validatedEditor.ClearValidationState();
                }
            }
            else
            {
                foreach (var validatedEditor in _editors.OfType<IValidatedComponent<TValidationResult>>())
                {
                    validatedEditor.UpdateValidationState(ValidationState);
                }
            }
        }

        private void UpdateEditorValidityState(IValidatedComponent<TValidationResult> validatedEditor)
        {
            if (ValidationNullCheck.IsNull(ValidationState))
            {
                validatedEditor.ClearValidationState();
            }
            else
            {
                validatedEditor.UpdateValidationState(ValidationState);
            }
        }

        private void OnValidationStateUpdated()
        {
            var handler = ValidationStateUpdated;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}