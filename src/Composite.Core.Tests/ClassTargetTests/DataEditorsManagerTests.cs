using System;
using System.Collections.Generic;
using Composite.Core.Tests.ClassTargetTests.Target;
using Composite.Core.Validation;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Composite.Core.Tests.ClassTargetTests
{
    [TestFixture]
    public class DataEditorsManagerTests
    {
        private DataEditorsManager<EditableClass, ValidationState> _sut;
        private readonly IEqualityComparer<EditableClass> _comparer = new EditableClassEqualityComparer();

        [Test]
        public void NullTargetIsAssignedAtInsertion()
        {
            var target = new EditableClass {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableClass>>();
            editorA.EditableTarget = target;

            var validator = CreateValidator();

            // act
            _sut = new DataEditorsManager<EditableClass, ValidationState>(validator)
            {
                EditableTarget = null
            };

            _sut.Add(editorA);

            // assert
            editorA.EditableTarget.Should().BeNull();
        }

        [Test]
        public void EditorsTargetIsInitializedAtInsertion()
        {
            var target = new EditableClass {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableClass>>();
            var editorB = Substitute.For<IDataEditor<EditableClass>>();

            var validator = CreateValidator();

            // given
            _comparer.Equals(editorA.EditableTarget, target).Should().BeFalse();
            _comparer.Equals(editorB.EditableTarget, target).Should().BeFalse();

            // act
            _sut = new DataEditorsManager<EditableClass, ValidationState>(validator)
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
            var target = new EditableClass {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableClass>>();
            var editorB = Substitute.For<IDataEditor<EditableClass>, IValidatedComponent<ValidationState>>();
            var validatedEditorB = (IValidatedComponent<ValidationState>) editorB;

            var state = ValidationState.Valid;
            var validator = CreateValidator(state);

            // given
            _comparer.Equals(editorA.EditableTarget, target).Should().BeFalse();
            _comparer.Equals(editorB.EditableTarget, target).Should().BeFalse();

            // act
            _sut = new DataEditorsManager<EditableClass, ValidationState>(validator)
            {
                EditableTarget = target
            };

            _sut.Add(editorA);
            _sut.Add(editorB);

            // assert
            validatedEditorB.Received().UpdateValidationState(state);
        }

        [Test]
        public void EditorsTargetIsUpdatedWhenManagersIsUpdated()
        {
            var target = new EditableClass {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableClass>>();
            var editorB = Substitute.For<IDataEditor<EditableClass>>();

            var validator = CreateValidator();

            _sut = new DataEditorsManager<EditableClass, ValidationState>(validator)
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
        public void ManagersValidityStateIsUpdatedOnTargetUpdate()
        {
            var target = new EditableClass {Text = "Initial"};

            var state = ValidationState.Valid;
            var validator = CreateValidator(state);

            _sut = new DataEditorsManager<EditableClass, ValidationState>(validator);

            // act
            _sut.MonitorEvents();
            _sut.EditableTarget = target;

            // assert
            _sut.ShouldRaise("ValidationStateUpdated")
                .WithSender(_sut)
                .WithArgs<EventArgs>(args => args == EventArgs.Empty);
        }

        [Test]
        public void EditorsTargetIsNulledWhenManagersIsNulled()
        {
            var target = new EditableClass {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableClass>>();
            editorA.EditableTarget = target;

            var validator = CreateValidator();

            // act
            _sut = new DataEditorsManager<EditableClass, ValidationState>(validator)
            {
                editorA
            };

            _sut.EditableTarget = target;
            _sut.EditableTarget = null;

            // assert
            editorA.EditableTarget.Should().BeNull();
        }

        [Test]
        public void EditorsValidityStateIsUpdatedWhenManagersTargetIsUpdated()
        {
            var target = new EditableClass {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableClass>>();
            var editorB = Substitute.For<IDataEditor<EditableClass>, IValidatedComponent<ValidationState>>();
            var validatedEditorB = (IValidatedComponent<ValidationState>) editorB;

            var state = ValidationState.Valid;
            var validator = CreateValidator(state);

            _sut = new DataEditorsManager<EditableClass, ValidationState>(validator)
            {
                editorA,
                editorB
            };

            // act
            _sut.EditableTarget = target;

            // assert
            validatedEditorB.Received().UpdateValidationState(state);
        }

        [Test]
        public void AnyEditorTargetUpdateIsIgnoredWhenTargetIsNull()
        {
            var editorA = Substitute.For<IDataEditor<EditableClass>>();
            var editorB = Substitute.For<IDataEditor<EditableClass>>();

            var validator = CreateValidator();

            _sut = new DataEditorsManager<EditableClass, ValidationState>(validator)
            {
                editorA,
                editorB
            };

            _sut.EditableTarget = null;

            // act
            editorB.TargetUpdated += Raise.EventWith(EventArgs.Empty);

            // assert
            editorA.EditableTarget.Should().BeNull();
        }

        [Test]
        public void AllEditorsTargetsAreSynchronizedOnSingleEditorUpdate()
        {
            var target = new EditableClass {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableClass>>();
            var editorB = Substitute.For<IDataEditor<EditableClass>>();

            var validator = CreateValidator();

            _sut = new DataEditorsManager<EditableClass, ValidationState>(validator)
            {
                editorA,
                editorB
            };

            _sut.EditableTarget = target;

            // act
            var updatedTarget = new EditableClass {Text = "Updated"};
            editorB.EditableTarget = updatedTarget;

            var args = EventArgs.Empty;
            editorB.TargetUpdated += Raise.EventWith(args);

            // assert
            _comparer.Equals(editorA.EditableTarget, updatedTarget).Should().BeTrue();
            _comparer.Equals(_sut.EditableTarget, updatedTarget).Should().BeTrue();
        }

        [Test]
        public void ManagersValidityStateIsUpdatedOnSingleEditorUpdate()
        {
            var target = new EditableClass {Text = "Initial"};

            var editorB = Substitute.For<IDataEditor<EditableClass>>();

            var validator = CreateValidator();

            _sut = new DataEditorsManager<EditableClass, ValidationState>(validator)
            {
                editorB
            };

            _sut.EditableTarget = target;

            // act
            _sut.MonitorEvents();

            var updatedTarget = new EditableClass {Text = "Updated"};
            editorB.EditableTarget = updatedTarget;

            var args = EventArgs.Empty;
            editorB.TargetUpdated += Raise.EventWith(args);

            // assert
            _sut.ShouldRaise("ValidationStateUpdated")
                .WithSender(_sut)
                .WithArgs<EventArgs>(a => a == EventArgs.Empty);
        }

        [Test]
        public void ValidatedEditorsValidationStateIsUpdatedOnSingleEditorUpdate()
        {
            var target = new EditableClass {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableClass>>();
            var editorB = Substitute.For<IDataEditor<EditableClass>, IValidatedComponent<ValidationState>>();
            var validatedEditorB = (IValidatedComponent<ValidationState>) editorB;

            var state = ValidationState.Valid;
            var validator = CreateValidator(state);

            _sut = new DataEditorsManager<EditableClass, ValidationState>(validator)
            {
                editorA,
                editorB
            };

            _sut.EditableTarget = target;

            // act
            editorB.ClearReceivedCalls();

            var updatedTarget = new EditableClass {Text = "Updated"};
            editorB.EditableTarget = updatedTarget;

            var args = EventArgs.Empty;
            editorB.TargetUpdated += Raise.EventWith(args);

            // assert
            validatedEditorB.Received().UpdateValidationState(state);
        }

        [Test]
        public void RemovedEditorIsManagedNoMore()
        {
            var target = new EditableClass {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableClass>>();
            var editorB = Substitute.For<IDataEditor<EditableClass>, IValidatedComponent<ValidationState>>();
            var validatedEditorB = (IValidatedComponent<ValidationState>) editorB;

            var state = ValidationState.Valid;
            var validator = CreateValidator(state);

            _sut = new DataEditorsManager<EditableClass, ValidationState>(validator)
            {
                editorA,
                editorB
            };

            _sut.EditableTarget = target;

            // act
            var updatedTarget = new EditableClass {Text = "Updated"};

            _sut.Remove(editorB);
            editorB.ClearReceivedCalls();

            _sut.EditableTarget = updatedTarget;

            var args = EventArgs.Empty;
            editorB.TargetUpdated += Raise.EventWith(args);

            // assert
            _comparer.Equals(editorB.EditableTarget, target).Should().BeTrue();
            _comparer.Equals(_sut.EditableTarget, updatedTarget).Should().BeTrue();

            validatedEditorB.DidNotReceiveWithAnyArgs().UpdateValidationState(state);
        }

        private static IValidator<EditableClass, ValidationState> CreateValidator()
        {
            return CreateValidator(ValidationState.Valid);
        }

        private static IValidator<EditableClass, ValidationState> CreateValidator(ValidationState state)
        {
            var validator = Substitute.For<IValidator<EditableClass, ValidationState>>();
            validator.Validate(Arg.Any<EditableClass>()).Returns(arg =>
            {
                arg[0].Should().NotBeNull();

                return state;
            });

            return validator;
        }
    }
}