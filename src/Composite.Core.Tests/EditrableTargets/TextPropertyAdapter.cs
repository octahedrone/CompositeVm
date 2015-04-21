namespace Composite.Core.Tests.EditrableTargets
{
    public class TextPropertyAdapter : IPropertyAdapter<EditableStruct, string>
    {
        public string PropertyName
        {
            get { return "Text"; }
        }

        public string GetValue(EditableStruct source)
        {
            return source.Text;
        }

        public EditableStruct SetValue(EditableStruct target, string value)
        {
            target.Text = value;

            return target;
        }
    }
}