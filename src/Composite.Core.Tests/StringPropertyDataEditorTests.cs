using System.Collections.Generic;
using Composite.Core.Tests.EditrableTargets;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Composite.Core.Tests
{
    [TestFixture]
    public class StringPropertyDataEditorTests
    {
        [Test]
        public void ValueUpdateChangeReportedOnTargetUpdate()
        {
            var target = new EditableStruct {Text = "Initial"};
            var editorComponent = new EditorComponent<string>();
            var propertyAdapter = new TextPropertyAdapter();
            var sut = new ScalarPropertyDataEditor<EditableStruct, string, EditorComponent<string>>(propertyAdapter, editorComponent);

            // act
            sut.Component.MonitorEvents();

            sut.EditableTarget = target;

            // assert
            sut.Component.ShouldRaisePropertyChangeFor(x => x.Value);
            sut.Component.Value.Should().Be(target.Text);
        }

        [Test]
        public void ComponentErrorIsUpdatedWhenValidationFails()
        {
            const string message = "Error";

            var target = new EditableStruct {Text = "Invalid"};

            var editorComponent = Substitute.For<IEditorComponent<string>>();
            var propertyAdapter = new TextPropertyAdapter();
            var sut = new PropertyDataEditor<EditableStruct, string>(propertyAdapter, editorComponent)
            {
                EditableTarget = target
            };

            // act
            sut.UpdateValidationState(new ValidationState(new Dictionary<string, string>
            {
                {propertyAdapter.PropertyName, message}
            }));

            // assert
            editorComponent.Received().SetError(message);
        }

        [Test]
        public void ComponentErrorIsUpdatedOnlyWithItsPropertyErrors()
        {
            const string message = "Error";

            var target = new EditableStruct {Text = "Invalid"};

            var editorComponent = Substitute.For<IEditorComponent<string>>();
            var propertyAdapter = new TextPropertyAdapter();
            var sut = new PropertyDataEditor<EditableStruct, string>(propertyAdapter, editorComponent)
            {
                EditableTarget = target
            };

            // act
            sut.UpdateValidationState(new ValidationState(new Dictionary<string, string>
            {
                {"Some property", message}
            }));

            // assert
            editorComponent.DidNotReceive().SetError(message);
        }

        [Test]
        public void ValueChangeUpdatesTargetProperty()
        {
            const string updatedValue = "Updated";

            var target = new EditableStruct {Text = "Initial"};
            var editorComponent = new EditorComponent<string>();
            var propertyAdapter = new TextPropertyAdapter();
            var sut = new ScalarPropertyDataEditor<EditableStruct, string, EditorComponent<string>>(propertyAdapter, editorComponent);

            // act
            sut.MonitorEvents();

            sut.EditableTarget = target;

            sut.Component.Value = updatedValue;

            // assert
            sut.ShouldRaise("TargetUpdated")
                .WithSender(sut)
                .WithArgs<PropertyUpdatedEventArgs>(args => args.PropertyName == "Text");

            sut.Component.Value.Should().Be(updatedValue);
        }
    }
}