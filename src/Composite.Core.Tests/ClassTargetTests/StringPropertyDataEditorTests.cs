using System.Collections.Generic;
using Composite.Core.Tests.ClassTargetTests.Target;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Composite.Core.Tests.ClassTargetTests
{
    [TestFixture]
    public class StringPropertyDataEditorTests
    {
        [Test]
        public void ReadonlyStateIsAssignedToComponent()
        {
            var editorComponent = new EditorComponent<string>();
            var propertyAdapter = EditableClassMetadata.ReadonlyTextProperty;
            
            // act
            var sut = new ScalarPropertyDataEditor<EditableClass, string, EditorComponent<string>>(propertyAdapter, editorComponent);

            // assert
            sut.Component.IsReadOnly.Should().Be(EditableClassMetadata.ReadonlyTextProperty.IsReadOnly);
        }

        [Test]
        public void NullEditableTargetIsTolerated()
        {
            var editorComponent = new EditorComponent<string>();
            var propertyAdapter = EditableClassMetadata.TextProperty;
            var sut = new ScalarPropertyDataEditor<EditableClass, string, EditorComponent<string>>(propertyAdapter, editorComponent);

            // act
            sut.Component.MonitorEvents();

            sut.EditableTarget = null;

            // assert
            sut.Component.Value.Should().Be(default(string));
        }

        [Test]
        public void ValueUpdateChangeReportedOnTargetUpdate()
        {
            var target = new EditableClass {Text = "Initial"};
            var editorComponent = new EditorComponent<string>();
            var propertyAdapter = EditableClassMetadata.TextProperty;
            var sut = new ScalarPropertyDataEditor<EditableClass, string, EditorComponent<string>>(propertyAdapter, editorComponent);

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

            var target = new EditableClass {Text = "Invalid"};

            var editorComponent = Substitute.For<IEditorComponent<string>>();
            var propertyAdapter = EditableClassMetadata.TextProperty;
            var sut = new PropertyDataEditor<EditableClass, string, ValidationState>(propertyAdapter, editorComponent, ValidationStateAdapter.GetPropertyError)
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

            var target = new EditableClass {Text = "Invalid"};

            var editorComponent = Substitute.For<IEditorComponent<string>>();
            var propertyAdapter = EditableClassMetadata.TextProperty;
            var sut = new PropertyDataEditor<EditableClass, string, ValidationState>(propertyAdapter, editorComponent, ValidationStateAdapter.GetPropertyError)
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

            var target = new EditableClass {Text = "Initial"};
            var editorComponent = new EditorComponent<string>();
            var propertyAdapter = EditableClassMetadata.TextProperty;
            var sut = new ScalarPropertyDataEditor<EditableClass, string, EditorComponent<string>>(propertyAdapter, editorComponent);

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