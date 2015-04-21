using System;
using FluentAssertions;
using NUnit.Framework;

namespace Composite.Core.Tests
{
    [TestFixture]
    public class StringEditorComponentTests
    {
        [Test]
        public void WhatWasSetIsWhatValueReturns()
        {
            var sut = new StringEditorComponent();
            var implicitSut = (IStringEditorComponent) sut;

            const string sample = "sample";

            // act
            implicitSut.SetValue(sample);

            // assert
            sut.Value.Should().Be(sample);
        }

        [Test]
        public void SetValueRaisesPropertyChangeNotification()
        {
            var sut = new StringEditorComponent();
            var implicitSut = (IStringEditorComponent) sut;

            const string sample = "sample";

            // act
            sut.MonitorEvents();
            implicitSut.SetValue(sample);

            // assert
            sut.ShouldRaisePropertyChangeFor(x => x.Value);
        }

        [Test]
        public void WhatWasAssignedToValueIsWhatGetValueReturns()
        {
            var sut = new StringEditorComponent();
            var implicitSut = (IStringEditorComponent) sut;

            const string sample = "sample";

            // act
            sut.Value = sample;

            // assert
            implicitSut.GetValue().Should().Be(sample);
        }

        [Test]
        public void AssignedValueCausesEventToBeRaised()
        {
            var sut = new StringEditorComponent();
            var implicitSut = (IStringEditorComponent) sut;

            const string sample = "sample";

            // act
            implicitSut.MonitorEvents();
            sut.Value = sample;

            // assert
            implicitSut.ShouldRaise("ValueUpdated")
                .WithSender(implicitSut)
                .WithArgs<EventArgs>(args => args == EventArgs.Empty);
        }
    }
}