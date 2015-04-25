using Composite.Core.PropertyAdapaters;

namespace Composite.Core.Tests.StructTargetTests.Target
{
    public static class EditableStructMetadata
    {
        public static readonly IPropertyAdapter<EditableStruct, string> TextProperty;
        public static readonly IPropertyAdapter<EditableStruct, string> ReadonlyTextProperty;

        static EditableStructMetadata()
        {
            TextProperty = new ValueTypeGenericPropertyAdapter<EditableStruct, string>(_ => _.Text, (t, v) =>
            {
                t.Text = v;
                return t;
            });

            ReadonlyTextProperty = new ValueTypeGenericPropertyAdapter<EditableStruct, string>(_ => _.Text);
        }
    }
}