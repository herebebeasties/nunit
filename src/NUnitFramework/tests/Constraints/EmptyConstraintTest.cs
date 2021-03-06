// Copyright (c) Charlie Poole, Rob Prouse and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections;
using System.IO;
using NUnit.Framework.Assertions;
using NUnit.TestUtilities;

namespace NUnit.Framework.Constraints
{
    [TestFixture]
    public class EmptyConstraintTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            TheConstraint = new EmptyConstraint();
            ExpectedDescription = "<empty>";
            StringRepresentation = "<empty>";
        }

        static object[] SuccessData = new object[]
        {
            string.Empty,
            new object[0],
            new ArrayList(),
            new System.Collections.Generic.List<int>(),
            Guid.Empty,
        };

        static object[] FailureData = new object[]
        {
            new TestCaseData( "Hello", "\"Hello\"" ),
            new TestCaseData( new object[] { 1, 2, 3 }, "< 1, 2, 3 >" ),
            new TestCaseData(new Guid("12345678-1234-1234-1234-123456789012"), "12345678-1234-1234-1234-123456789012"),
        };

        [TestCase(null)]
        [TestCase(5)]
        public void InvalidDataThrowsArgumentException(object data)
        {
            Assert.Throws<ArgumentException>(() => TheConstraint.ApplyTo(data));
        }

        [Test]
        public void NullStringGivesFailureResult()
        {
            string actual = null;
            var result = TheConstraint.ApplyTo(actual);
            Assert.That(result.Status, Is.EqualTo(ConstraintStatus.Failure));
        }

        [Test]
        public void NullNullableGuidGivesFailureResult()
        {
            Guid? actual = null;
            var result = TheConstraint.ApplyTo(actual);
            Assert.That(result.Status, Is.EqualTo(ConstraintStatus.Failure));
        }

        [Test]
        public void NullArgumentExceptionMessageContainsTypeName()
        {
            int? testInput = null;
            Assert.That(() => TheConstraint.ApplyTo(testInput),
               Throws.ArgumentException.With.Message.Contains("System.Int32"));
        }
    }

    [TestFixture]
    public class EmptyStringConstraintTest : StringConstraintTests
    {
        [SetUp]
        public void SetUp()
        {
            TheConstraint = new EmptyStringConstraint();
            ExpectedDescription = "<empty>";
            StringRepresentation = "<emptystring>";
        }

        static object[] SuccessData = new object[]
        {
            string.Empty
        };

        static object[] FailureData = new object[]
        {
            new TestCaseData( "Hello", "\"Hello\"" ),
            new TestCaseData( null, "null")
        };
    }

    [TestFixture]
    public class EmptyDirectoryConstraintTest
    {
        [Test]
        public void EmptyDirectory()
        {
            using (var testDir = new TestDirectory())
            {
                Assert.That(testDir.Directory, Is.Empty);
            }
        }

        [Test]
        public void NotEmptyDirectory_ContainsFile()
        {
            using (var testDir = new TestDirectory())
            {
                File.Create(Path.Combine(testDir.Directory.FullName, "DUMMY.FILE")).Dispose();

                Assert.That(testDir.Directory, Is.Not.Empty);
            }
        }

        [Test]
        public void NotEmptyDirectory_ContainsDirectory()
        {
            using (var testDir = new TestDirectory())
            {
                Directory.CreateDirectory(Path.Combine(testDir.Directory.FullName, "DUMMY_DIR"));

                Assert.That(testDir.Directory, Is.Not.Empty);
            }
        }
    }

    [TestFixture]
    public class EmptyGuidConstraintTest
    {
        [Test]
        public void EmptyGuid()
        {
            Assert.That(Guid.Empty, Is.Empty);
        }

        [Test]
        public void EmptyNullableGuid()
        {
            Guid? empty = Guid.Empty;
            Assert.That(empty, Is.Empty);
        }

        [Test]
        public void NonEmptyGuid()
        {
            Guid nonEmpty = new Guid("10000000-0000-0000-0000-000000000000");
            Assert.That(nonEmpty, Is.Not.Empty);
        }

        [Test]
        public void NonEmptyNullableGuid()
        {
            Guid? nonEmpty = new Guid("10000000-0000-0000-0000-000000000000");
            Assert.That(nonEmpty, Is.Not.Empty);
        }

        [Test]
        public void NullNullableGuid()
        {
            Guid? nonEmpty = null;
            Assert.That(nonEmpty, Is.Not.Empty);
        }
    }
}
