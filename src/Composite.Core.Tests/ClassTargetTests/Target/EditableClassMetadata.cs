using Composite.Core.PropertyAdapters;

namespace Composite.Core.Tests.ClassTargetTests.Target
{
    public static class EditableClassMetadata
    {
        public static readonly IPropertyAdapter<EditableClass, string> TextProperty;
        public static readonly IPropertyAdapter<EditableClass, string> ReadonlyTextProperty;

        static EditableClassMetadata()
        {
            TextProperty = new ClassTypeGenericPropertyAdapter<EditableClass, string>(_ => _.Text, (t, v) => t.Text = v);
            
            ReadonlyTextProperty = new ClassTypeGenericPropertyAdapter<EditableClass, string>(_ => _.Text);
        }
    }
}