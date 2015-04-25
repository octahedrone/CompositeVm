using Composite.Core.PropertyAdapaters;

namespace Composite.Core.Tests.ClassTargetTests.Target
{
    public static class EditableClassMetadata
    {
        public static readonly IPropertyAdapter<EditableClass, string> TextProperty;

        static EditableClassMetadata()
        {
            TextProperty = new ClassTypeGenericPropertyAdapter<EditableClass, string>(_ => _.Text, (t, v) => t.Text = v);
        }
    }
}