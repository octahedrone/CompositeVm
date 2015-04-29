using System;
using System.Collections.Generic;
using Composite.Core.Tests.StructTargetTests.Target;
using Composite.Core.Validation;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Composite.Core.Tests.StructTargetTests
{
    [TestFixture]
    public class DataEditorsManagerTests
    {
        private DataEditorsManager<EditableStruct, ValidationState> _sut;
        private readonly IEqualityComparer<EditableStruct> _comparer = new TextEqualityComparer();

        [Test]
        public void EditorsTargetIsInitializedAtInsertion()
        {
            var target = new EditableStruct {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableStruct>>();
            var editorB = Substitute.For<IDataEditor<EditableStruct>>();

            var validator = CreateValidator();

            // given
            _comparer.Equals(editorA.EditableTarget, target).Should().BeFalse();
            _comparer.Equals(editorB.EditableTarget, target).Should().BeFalse();

            // act
            _sut = new DataEditorsManager<EditableStruct, ValidationState>(validator)
            {
                EditableTarget = target
            };

            _sut.Add(editorA);
            _sut.Add(editorB);

            // assert
            _comparer.Equals(editorA.EditableTarget, target).Should().BeTrue();
            _comparer.Equals(editorB.EditableTarget, target).Should().BeTrue();
        }

        [Test]
        public void EditorsValidityStateIsInitializedAtInsertion()
        {
            var target = new EditableStruct {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableStruct>>();
            var editorB = Substitute.For<IValidatedDataEditor<EditableStruct, ValidationState>>();

            var state = ValidationState.Valid;
            var validator = CreateValidator(state);

            // given
            _comparer.Equals(editorA.EditableTarget, target).Should().BeFalse();
            _comparer.Equals(editorB.EditableTarget, target).Should().BeFalse();

            // act
            _sut = new DataEditorsManager<EditableStruct, ValidationState>(validator)
            {
                EditableTarget = target
            };

            _sut.Add(editorA);
            _sut.Add(editorB);

            // assert
            editorB.Received().UpdateValidationState(state);
        }

        [Test]
        public void EditorsTargetIsUpdatedWhenManagersIsUpdated()
        {
            var target = new EditableStruct {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableStruct>>();
            var editorB = Substitute.For<IDataEditor<EditableStruct>>();

            var validator = CreateValidator();

            _sut = new DataEditorsManager<EditableStruct, ValidationState>(validator)
            {
                editorA,
                editorB
            };

            // given
            _comparer.Equals(editorA.EditableTarget, target).Should().BeFalse();
            _comparer.Equals(editorB.EditableTarget, target).Should().BeFalse();

            // act
            _sut.EditableTarget = target;

            // assert
            _comparer.Equals(editorA.EditableTarget, target).Should().BeTrue();
            _comparer.Equals(editorB.EditableTarget, target).Should().BeTrue();
        }

        [Test]
        public void EditorsValidityStateIsUpdatedWhenManagersTargetIsUpdated()
        {
            var target = new EditableStruct {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableStruct>>();
            var editorB = Substitute.For<IValidatedDataEditor<EditableStruct, ValidationState>>();

            var state = ValidationState.Valid;
            var validator = CreateValidator(state);

            _sut = new DataEditorsManager<EditableStruct, ValidationState>(validator)
            {
                editorA,
                editorB
            };

            // act
            _sut.EditableTarget = target;

            // assert
            editorB.Received().UpdateValidationState(state);
        }

        [Test]
        public void ManagersValidityStateIsUpdatedOnTargetUpdate()
        {
            var target = new EditableStruct {Text = "Initial"};

            var state = ValidationState.Valid;
            var validator = CreateValidator(state);

            _sut = new DataEditorsManager<EditableStruct, ValidationState>(validator);

            // act
            _sut.MonitorEvents();
            _sut.EditableTarget = target;

            // assert
            _sut.ShouldRaise("ValidationStateUpdated")
                .WithSender(_sut)
                .WithArgs<EventArgs>(args => args == EventArgs.Empty);
        }

        [Test]
        public void AllEditorsTargetsAreSynchronizedOnSingleEditorUpdate()
        {
            var target = new EditableStruct {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableStruct>>();
            var editorB = Substitute.For<IDataEditor<EditableStruct>>();

            var validator = CreateValidator();

            _sut = new DataEditorsManager<EditableStruct, ValidationState>(validator)
            {
                editorA,
                editorB
            };

            _sut.EditableTarget = target;

            // act
            var updatedTarget = new EditableStruct { Text = "Updated" };
            editorB.EditableTarget = updatedTarget;

            editorB.TargetUpdated += Raise.EventWith(EventArgs.Empty);

            // assert
            _comparer.Equals(editorA.EditableTarget, updatedTarget).Should().BeTrue();
            _comparer.Equals(_sut.EditableTarget, updatedTarget).Should().BeTrue();
        }


        [Test]
        public void ManagersValidityStateIsUpdatedOnSingleEditorUpdate()
        {
            var target = new EditableStruct { Text = "Initial" };

            var editorB = Substitute.For<IDataEditor<EditableStruct>>();

            var validator = CreateValidator();

            _sut = new DataEditorsManager<EditableStruct, ValidationState>(validator)
            {
                editorB
            };

            _sut.EditableTarget = target;

            // act
            _sut.MonitorEvents();

            var updatedTarget = new EditableStruct { Text = "Updated" };
            editorB.EditableTarget = updatedTarget;

            editorB.TargetUpdated += Raise.EventWith(EventArgs.Empty);

            // assert
            _sut.ShouldRaise("ValidationStateUpdated")
                .WithSender(_sut)
                .WithArgs<EventArgs>(a => a == EventArgs.Empty);
        }


        [Test]
        public void ValidatedEditorsValidationStateIsUpdatedOnSingleEditorUpdate()
        {
            var target = new EditableStruct {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableStruct>>();
            var editorB = Substitute.For<IValidatedDataEditor<EditableStruct, ValidationState>>();

            var state = ValidationState.Valid;
            var validator = CreateValidator(state);

            _sut = new DataEditorsManager<EditableStruct, ValidationState>(validator)
            {
                editorA,
                editorB
            };

            _sut.EditableTarget = target;

            // act
            editorB.ClearReceivedCalls();

            var updatedTarget = new EditableStruct { Text = "Updated" };
            editorB.EditableTarget = updatedTarget;

            var args = EventArgs.Empty;
            editorB.TargetUpdated += Raise.EventWith(args);

            // assert
            editorB.Received().UpdateValidationState(state);
        }

        [Test]
        public void RemovedEditorIsManagedNoMore()
        {
            var target = new EditableStruct {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableStruct>>();
            var editorB = Substitute.For<IValidatedDataEditor<EditableStruct, ValidationState>>();

            var state = ValidationState.Valid;
            var validator = CreateValidator(state);

            _sut = new DataEditorsManager<EditableStruct, ValidationState>(validator)
            {
                editorA,
                editorB
            };

            _sut.EditableTarget = target;

            // act
            var updatedTarget = new EditableStruct { Text = "Updated" };

            _sut.Remove(editorB);
            editorB.ClearReceivedCalls();

            _sut.EditableTarget = updatedTarget;

            editorB.TargetUpdated += Raise.EventWith(EventArgs.Empty);

            // assert
            _comparer.Equals(editorB.EditableTarget, target).Should().BeTrue();
            _comparer.Equals(_sut.EditableTarget, updatedTarget).Should().BeTrue();

            editorB.DidNotReceiveWithAnyArgs().UpdateValidationState(state);
        }

        private static IValidator<EditableStruct, ValidationState> CreateValidator()
        {
            return CreateValidator(ValidationState.Valid);
        }

        private static IValidator<EditableStruct, ValidationState> CreateValidator(ValidationState state)
        {
            var validator = Substitute.For<IValidator<EditableStruct, ValidationState>>();
            validator.Validate(Arg.Any<EditableStruct>()).Returns(state);

            return validator;
        }
    }
}