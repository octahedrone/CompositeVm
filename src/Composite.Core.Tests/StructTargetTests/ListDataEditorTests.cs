﻿using System;
using System.Linq;
using Composite.Core.Tests.StructTargetTests.Target;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Composite.Core.Tests.StructTargetTests
{
    [TestFixture]
    public class ListDataEditorTests
    {
        [Test]
        public void EditorsCollectionContainsEditorForEachOfCollectionItems()
        {
            var sut = new ListDataEditor<ContainerStruct, ContainerStructValiditiState, EditableStruct, ValidationState>
                (ItemEditorFactory,
                    ContainerStructMetadata.ItemsProperty,
                    GetItemValidityState);

            var implicitSut = sut as IDataEditor<ContainerStruct>;

            var target = new ContainerStruct(new[]
            {
                new EditableStruct {Text = "A"},
                new EditableStruct {Text = "B"},
            });

            // act
            implicitSut.EditableTarget = target;

            // assert
            sut.ItemEditors.Count.Should().Be(target.Items.Count());
            sut.ItemEditors.Any(e => e.EditableTarget.Text == "A").Should().BeTrue();
            sut.ItemEditors.Any(e => e.EditableTarget.Text == "B").Should().BeTrue();
        }

        [Test]
        public void TargetUpdateMakesEditorsCollectionToUpdate()
        {
            var sut = new ListDataEditor<ContainerStruct, ContainerStructValiditiState, EditableStruct, ValidationState>
                (ItemEditorFactory,
                    ContainerStructMetadata.ItemsProperty,
                    GetItemValidityState);

            var implicitSut = sut as IDataEditor<ContainerStruct>;

            var target = new ContainerStruct(new[]
            {
                new EditableStruct {Text = "A"},
                new EditableStruct {Text = "B"},
            });

            // act
            implicitSut.EditableTarget = target;

            target = new ContainerStruct(new[]
            {
                new EditableStruct {Text = "A"},
                new EditableStruct {Text = "C"},
            });

            implicitSut.EditableTarget = target;

            // assert
            sut.ItemEditors.Count.Should().Be(target.Items.Count());
            sut.ItemEditors.Any(e => e.EditableTarget.Text == "A").Should().BeTrue();
            sut.ItemEditors.Any(e => e.EditableTarget.Text == "C").Should().BeTrue();
        }

        [Test]
        public void TargetUpdateMakesEditorsCollectionToClear()
        {
            var sut = new ListDataEditor<ContainerStruct, ContainerStructValiditiState, EditableStruct, ValidationState>
                (ItemEditorFactory,
                    ContainerStructMetadata.ItemsProperty,
                    GetItemValidityState);

            var implicitSut = sut as IDataEditor<ContainerStruct>;

            var target = new ContainerStruct(new[]
            {
                new EditableStruct {Text = "A"},
                new EditableStruct {Text = "B"},
            });

            // act
            implicitSut.EditableTarget = target;

            target = new ContainerStruct(new EditableStruct[0]);

            implicitSut.EditableTarget = target;

            // assert
            sut.ItemEditors.Count.Should().Be(target.Items.Count());
        }

        [Test]
        public void ItemEditorUpdateIsReportedAsTargetUpdatedEvent()
        {
            var sut = new ListDataEditor<ContainerStruct, ContainerStructValiditiState, EditableStruct, ValidationState>
                (ItemEditorFactory,
                    ContainerStructMetadata.ItemsProperty,
                    GetItemValidityState);

            var implicitSut = sut as IDataEditor<ContainerStruct>;

            var target = new ContainerStruct(new[]
            {
                new EditableStruct {Text = "A"},
            });

            // act
            implicitSut.EditableTarget = target;

            implicitSut.MonitorEvents();

            sut.ItemEditors[0].EditableTarget = new EditableStruct {Text = "B"};
            sut.ItemEditors[0].TargetUpdated += Raise.EventWith(EventArgs.Empty);

            // assert
            implicitSut.ShouldRaise("TargetUpdated")
                .WithSender(sut);

            target.Items[0].Text.Should().Be("B");
        }

        [Test]
        public void ItemEditorUpdateTriggersValidityCheck()
        {
            var sut = new ListDataEditor<ContainerStruct, ContainerStructValiditiState, EditableStruct, ValidationState>
                (ItemValidatedEditorFactory,
                    ContainerStructMetadata.ItemsProperty,
                    GetItemValidityState);

            var containerEditor = sut as IDataEditor<ContainerStruct>;
            var implicitSut = sut as IValidatedComponent<ContainerStructValiditiState>;

            var target = new ContainerStruct(new[]
            {
                new EditableStruct {Text = "A"},
            });

            // act
            containerEditor.EditableTarget = target;

            var itemEditor = sut.ItemEditors[0] as IValidatedComponent<ValidationState>;
            itemEditor.Should().NotBeNull();

            var structEditor = itemEditor as IDataEditor<EditableStruct>;
            structEditor.Should().NotBeNull();

            structEditor.EditableTarget = new EditableStruct {Text = "B"};
            structEditor.TargetUpdated += Raise.EventWith(EventArgs.Empty);

            // assert
            target.Items[0].Text.Should().Be("B");

            // act
            implicitSut.UpdateValidationState(new ContainerStructValiditiState());

            // assert
            itemEditor.Received().UpdateValidationState(ValidationState.Valid);
        }

        private static ValidationState GetItemValidityState(EditableStruct item, ContainerStructValiditiState state)
        {
            return state.GetStateFor(item);
        }

        private static IDataEditor<EditableStruct> ItemEditorFactory(EditableStruct arg)
        {
            var pseudoEditor = Substitute.For<IDataEditor<EditableStruct>>();
            pseudoEditor.EditableTarget = arg;

            return pseudoEditor;
        }

        private static IDataEditor<EditableStruct> ItemValidatedEditorFactory(EditableStruct arg)
        {
            var pseudoEditor = Substitute.For<IDataEditor<EditableStruct>, IValidatedComponent<ValidationState>>();
            pseudoEditor.EditableTarget = arg;

            return pseudoEditor;
        }
    }
}