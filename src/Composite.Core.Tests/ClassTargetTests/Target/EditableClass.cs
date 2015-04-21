using System.Collections.Generic;

namespace Composite.Core.Tests.ClassTargetTests.Target
{
    public class EditableClass
    {
        public string Text { get; set; }
    }

    public sealed class EditableClassEqualityComparer : IEqualityComparer<EditableClass>
    {
        public bool Equals(EditableClass x, EditableClass y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return string.Equals(x.Text, y.Text);
        }

        public int GetHashCode(EditableClass obj)
        {
            return (obj.Text != null ? obj.Text.GetHashCode() : 0);
        }
    }
}