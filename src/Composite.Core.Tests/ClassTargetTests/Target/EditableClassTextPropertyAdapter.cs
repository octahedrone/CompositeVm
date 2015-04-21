namespace Composite.Core.Tests.ClassTargetTests.Target
{
    public class EditableClassTextPropertyAdapter : IPropertyAdapter<EditableClass, string>
    {
        public string PropertyName
        {
            get { return "Text"; }
        }

        public string GetValue(EditableClass source)
        {
            return source.Text;
        }

        public EditableClass SetValue(EditableClass target, string value)
        {
            target.Text = value;

            return target;
        }
    }
}