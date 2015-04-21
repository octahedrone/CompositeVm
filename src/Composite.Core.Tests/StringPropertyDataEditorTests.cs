using Composite.Core.Tests.EditrableTargets;
using Composite.Core.Validation;
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
            var editorComponent = new StringEditorComponent();
            var sut = new ScalarPropertyDataEditor<EditableStruct, StringEditorComponent>(new TextPropertyAdapter(), editorComponent);

            var validator = CreateValidator();

            var manager = new DataEditorsManager<EditableStruct, ValidationState>(validator)
            {
                sut
            };

            // act
            sut.Component.MonitorEvents();

            manager.EditableTarget = target;

            // assert
            sut.Component.ShouldRaisePropertyChangeFor(x => x.Value);
        }

        [Test]
        public void ValueChangeUpdatesTargetProperty()
        {
            const string updatedValue = "Updated";

            var target = new EditableStruct {Text = "Initial"};
            var editorComponent = new StringEditorComponent();
            var sut = new ScalarPropertyDataEditor<EditableStruct, StringEditorComponent>(new TextPropertyAdapter(), editorComponent);

            var validator = CreateValidator();

            var manager = new DataEditorsManager<EditableStruct, ValidationState>(validator)
            {
                sut
            };

            // act
            sut.MonitorEvents();

            manager.EditableTarget = target;

            sut.Component.Value = updatedValue;

            // assert
            sut.ShouldRaise("TargetUpdated")
                .WithSender(sut)
                .WithArgs<PropertyUpdatedEventArgs>(args => args.PropertyName == "Text");

            manager.EditableTarget.Text.Should().Be(updatedValue);
        }

        private static IValidator<EditableStruct, ValidationState> CreateValidator()
        {
            var validator = Substitute.For<IValidator<EditableStruct, ValidationState>>();
            validator.Validate(Arg.Any<EditableStruct>()).Returns(ValidationState.Valid);

            return validator;
        }
    }
}