using System.Collections.Generic;
using Composite.Core.Tests.EditrableTargets;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Composite.Core.Tests
{
    [TestFixture]
    public class CompositeVMTests
    {
        private List<IDataEditor<EditableStruct>> _editors;
        private readonly IEqualityComparer<EditableStruct> _comparer = new TextEqualityComparer();

        [SetUp]
        public void SetUp()
        {
            _editors = new List<IDataEditor<EditableStruct>>();
        }

        [Test]
        public void ScalarPropertyUpdateIsSynchronizedWellTest()
        {
            var target = new EditableStruct {Text = "Initial"};

            var editorA = Substitute.For<IDataEditor<EditableStruct>>();
            var editorB = Substitute.For<IDataEditor<EditableStruct>>();

            editorA.EditableTarget = target;
            
            target.Text = "Ipdated in B";
            editorB.EditableTarget = target;

            _editors.Add(editorB);
            _editors.Add(editorA);

            editorA.TargetUpdated += OnEditorTargetUpdated;
            editorB.TargetUpdated += OnEditorTargetUpdated;

            // act
            _comparer.Equals(editorA.EditableTarget, target).Should().BeFalse();

            var args = new PropertyUpdatedEventArgs("Text");
            editorB.TargetUpdated += Raise.EventWith(args);

            // assert
            _comparer.Equals(editorA.EditableTarget, target).Should().BeTrue();
        }

        private void OnEditorTargetUpdated(object sender, PropertyUpdatedEventArgs e)
        {
            var updater = sender as IDataEditor<EditableStruct>;

            if (updater == null)
            {
                return;
            }

            foreach (var editor in _editors)
            {
                if (editor != sender)
                {
                    editor.EditableTarget = updater.EditableTarget;
                }
            }
        }
    }
}