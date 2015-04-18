using System.Collections.Generic;
using Composite.Core.Tests.EditrableTargets;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Composite.Core.Tests
{
    [TestFixture]
    public class DataEditorsManagerTests
    {
        private DataEditorsManager<EditableStruct> _sut;
        private readonly IEqualityComparer<EditableStruct> _comparer = new TextEqualityComparer();

        [Test]
        public void EditorsTargetIsInitializedAtInsertion()
        {
            var target = new EditableStruct {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableStruct>>();
            var editorB = Substitute.For<IDataEditor<EditableStruct>>();

            // given
            _comparer.Equals(editorA.EditableTarget, target).Should().BeFalse();
            _comparer.Equals(editorB.EditableTarget, target).Should().BeFalse();

            // act
            _sut = new DataEditorsManager<EditableStruct>
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
        public void EditorsTargetIsUpdatedWhenManagersIsUpdated()
        {
            var target = new EditableStruct {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableStruct>>();
            var editorB = Substitute.For<IDataEditor<EditableStruct>>();

            _sut = new DataEditorsManager<EditableStruct>
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
        public void AllEditorsTargetsAreSynchronizedOnSingleEditorUpdate()
        {
            var target = new EditableStruct {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableStruct>>();
            var editorB = Substitute.For<IDataEditor<EditableStruct>>();

            _sut = new DataEditorsManager<EditableStruct>
            {
                editorA,
                editorB
            };

            _sut.EditableTarget = target;

            // act
            var updatedTarget = new EditableStruct { Text = "Updated" };
            editorB.EditableTarget = updatedTarget;

            var args = new PropertyUpdatedEventArgs("Text");
            editorB.TargetUpdated += Raise.EventWith(args);

            // assert
            _comparer.Equals(editorA.EditableTarget, updatedTarget).Should().BeTrue();
            _comparer.Equals(_sut.EditableTarget, updatedTarget).Should().BeTrue();
        }

        [Test]
        public void RemovedEditorIsManagedNoMore()
        {
            var target = new EditableStruct {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableStruct>>();
            var editorB = Substitute.For<IDataEditor<EditableStruct>>();

            _sut = new DataEditorsManager<EditableStruct>
            {
                editorA,
                editorB
            };

            _sut.EditableTarget = target;

            // act
            var updatedTarget = new EditableStruct { Text = "Updated" };

            _sut.Remove(editorB);

            _sut.EditableTarget = updatedTarget;

            var args = new PropertyUpdatedEventArgs("Text");
            editorB.TargetUpdated += Raise.EventWith(args);

            // assert
            _comparer.Equals(editorB.EditableTarget, target).Should().BeTrue();
            _comparer.Equals(_sut.EditableTarget, updatedTarget).Should().BeTrue();
        }
    }
}