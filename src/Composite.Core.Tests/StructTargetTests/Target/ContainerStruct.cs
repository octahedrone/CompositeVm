using System.Collections.Generic;
using Composite.Core.Validation;

namespace Composite.Core.Tests.StructTargetTests.Target
{
    public struct ContainerStruct
    {
        public ContainerStruct(EditableStruct[] items)
            : this()
        {
            Items = items;
        }

        public EditableStruct[] Items { get; private set; }
    }
}