using Composite.Core.Tests.EditrableTargets;
using FluentAssertions;
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
            var sut = new StringPropertyDataEditor<EditableStruct>(new TextPropertyAdapter());

            var manager = new DataEditorsManager<EditableStruct>
            {
                sut
            };

            // act
            sut.MonitorEvents();

            manager.EditableTarget = target;

            // assert
            sut.ShouldRaisePropertyChangeFor(x => x.Value);
        }

        [Test]
        public void ValueChangeUpdatesTargetProperty()
        {
            const string updatedValue = "Updated";

            var target = new EditableStruct {Text = "Initial"};
            var sut = new StringPropertyDataEditor<EditableStruct>(new TextPropertyAdapter());

            var manager = new DataEditorsManager<EditableStruct>
            {
                sut
            };

            // act
            sut.MonitorEvents();

            manager.EditableTarget = target;

            sut.Value = updatedValue;

            // assert
            sut.ShouldRaise("TargetUpdated")
                .WithSender(sut)
                .WithArgs<PropertyUpdatedEventArgs>(args => args.PropertyName == "Text");

            manager.EditableTarget.Text.Should().Be(updatedValue);
        }
    }
}