using System;
using System.ComponentModel;
using Composite.Core.PropertyEditors;
using FluentAssertions;
using NUnit.Framework;

namespace Composite.Core.Tests
{
    [TestFixture]
    public class EditorComponentTests
    {
        [Test]
        public void WhatWasSetIsWhatValueReturns()
        {
            var sut = new EditorComponent<string>();
            var implicitSut = (IEditorComponent<string>) sut;

            const string sample = "sample";

            // act
            implicitSut.SetValue(sample);

            // assert
            sut.Value.Should().Be(sample);
        }

        [Test]
        public void NonEmptyErrorMessageValueIsWhatErrorInterfaceReturns()
        {
            var sut = new EditorComponent<string>();
            var implicitSut = (IDataErrorInfo) sut;

            const string error = "error";

            // act
            sut.SetError(error);

            // assert
            implicitSut.Error.Should().Be(error);
            implicitSut["Value"].Should().Be(error);
        }

        [Test]
        public void NullErrorMessageValueIsWhatErrorInterfaceReturns()
        {
            var sut = new EditorComponent<string>();
            var implicitSut = (IDataErrorInfo) sut;

            const string error = "error";

            // act
            sut.SetError(error);
            sut.SetError(null);

            // assert
            implicitSut.Error.Should().Be(null);
            implicitSut["Value"].Should().Be(null);
        }

        [Test]
        public void EmptyErrorMessageValueIsWhatErrorInterfaceReturns()
        {
            var sut = new EditorComponent<string>();
            var implicitSut = (IDataErrorInfo) sut;

            const string error = "error";

            // act
            sut.SetError(error);
            sut.SetError(String.Empty);

            // assert
            implicitSut.Error.Should().Be(null);
            implicitSut["Value"].Should().Be(null);
        }

        [Test]
        public void SetValueRaisesPropertyChangeNotification()
        {
            var sut = new EditorComponent<string>();
            var implicitSut = (IEditorComponent<string>) sut;

            const string sample = "sample";

            // act
            sut.MonitorEvents();
            implicitSut.SetValue(sample);

            // assert
            sut.ShouldRaisePropertyChangeFor(x => x.Value);
        }

        [Test]
        public void SetErrorRaisesPropertyChangeNotification()
        {
            var sut = new EditorComponent<string>();

            const string error = "error";

            // act
            sut.MonitorEvents();
            sut.SetError(error);

            // assert
            sut.ShouldRaisePropertyChangeFor(x => x.Value);
        }

        [Test]
        public void WhatWasAssignedToValueIsWhatGetValueReturns()
        {
            var sut = new EditorComponent<string>();
            var implicitSut = (IEditorComponent<string>) sut;

            const string sample = "sample";

            // act
            sut.Value = sample;

            // assert
            implicitSut.GetValue().Should().Be(sample);
        }

        [Test]
        public void AssignedValueCausesEventToBeRaised()
        {
            var sut = new EditorComponent<string>();
            var implicitSut = (IEditorComponent<string>) sut;

            const string sample = "sample";

            // act
            implicitSut.MonitorEvents();
            sut.Value = sample;

            // assert
            implicitSut.ShouldRaise("ValueUpdated")
                .WithSender(implicitSut)
                .WithArgs<EventArgs>(args => args == EventArgs.Empty);
        }

        [Test]
        public void EventIsRaisedWhenReadOnlyStateIsUpdated()
        {
            var sut = new EditorComponent<string>();
            var implicitSut = (IEditorComponent<string>) sut;

            const bool isReadOnly = true;

            // act
            implicitSut.MonitorEvents();

            implicitSut.SetReadOnly(isReadOnly);

            // assert
            sut.ShouldRaisePropertyChangeFor(x => x.IsReadOnly);
            sut.IsReadOnly.Should().Be(isReadOnly);
        }
    }
}