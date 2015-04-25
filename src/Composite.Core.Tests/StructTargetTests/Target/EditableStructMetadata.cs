using Composite.Core.PropertyAdapaters;

namespace Composite.Core.Tests.StructTargetTests.Target
{
    public static class EditableStructMetadata
    {
        public static readonly IPropertyAdapter<EditableStruct, string> TextProperty;

        static EditableStructMetadata()
        {
            TextProperty = new ValueTypeGenericPropertyAdapter<EditableStruct, string>(_ => _.Text, (t, v) =>
            {
                t.Text = v;
                return t;
            });
        }
    }
}