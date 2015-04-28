using System.Collections.Generic;
using Composite.Core.PropertyAdapters;

namespace Composite.Core.Tests.StructTargetTests.Target
{
    public class ContainerStructMetadata
    {
        public static readonly IPropertyAdapter<ContainerStruct, IList<EditableStruct>> ItemsProperty;

        static ContainerStructMetadata()
        {
            ItemsProperty = new ValueTypeGenericPropertyAdapter<ContainerStruct, IList<EditableStruct>>(_ => _.Items);
        }
    }
}