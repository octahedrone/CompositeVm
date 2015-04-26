using Composite.Core.PropertyAdapters;
using Composite.Core.Tests.ClassTargetTests.Target;
using FluentAssertions;
using NUnit.Framework;

namespace Composite.Core.Tests
{
    [TestFixture]
    public class ClassTypeGenericPropertyAdapterTests
    {
        [Test]
        public void PropertyNameIsRetrievedCorrectly()
        {
            var sut = new ClassTypeGenericPropertyAdapter<EditableClass, string>(_ => _.Text);

            // assert
            sut.PropertyName.Should().Be("Text");
            sut.IsReadOnly.Should().BeTrue();
        }

        [Test]
        public void PropertyValueIsRetrievedCorrectly()
        {
            var sut = new ClassTypeGenericPropertyAdapter<EditableClass, string>(_ => _.Text);

            // act
            const string text = "Value";
            var value = sut.GetValue(new EditableClass {Text = text});

            // assert
            value.Should().Be(text);
        }

        [Test]
        public void PropertyValueIsUpdatedCorrectly()
        {
            var sut = new ClassTypeGenericPropertyAdapter<EditableClass, string>(_ => _.Text, (t, v) => t.Text = v);

            const string initialText = "Initial";
            const string updatedText = "Updated";

            // act
            var target = new EditableClass {Text = initialText};
            sut.SetValue(target, updatedText);

            // assert
            sut.IsReadOnly.Should().BeFalse();
            target.Text.Should().Be(updatedText);
        }
    }
}