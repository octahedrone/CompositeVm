﻿using System;
using System.ComponentModel;
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
        public void NonEmptyErrorMessageValueIsWhatErrorInterfaceReturns()
        {
            var sut = new StringEditorComponent();
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
            var sut = new StringEditorComponent();
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
            var sut = new StringEditorComponent();
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
        public void SetErrorRaisesPropertyChangeNotification()
        {
            var sut = new StringEditorComponent();

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