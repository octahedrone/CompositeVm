using System.Collections.Generic;

namespace Composite.Core.Tests.StructTargetTests.Target
{
    public sealed class TextEqualityComparer : IEqualityComparer<EditableStruct>
    {
        public bool Equals(EditableStruct x, EditableStruct y)
        {
            return string.Equals(x.Text, y.Text);
        }

        public int GetHashCode(EditableStruct obj)
        {
            return (obj.Text != null ? obj.Text.GetHashCode() : 0);
        }
    }
}