using System;
using OwnCollections;
using System.Linq;
using Xunit;

namespace MyListTests
{
    public class MyListTests
    {
        [Theory]
        [InlineData(new string[]{ "Nico", "Nino", "Adis"}, 3)]
        [InlineData(new string[]{ "Nico", "Nino", "Adis", "Sebi"}, 4)]
        [InlineData(new string[]{ "Nico", "Nino", "Adis", "Alex", "Sebi", "Premton", "Martin"}, 7)]
        public void Count_Result_Test(string[] names, int expectedCount)
        {
            // Arrange
            MyList<string> myList = new MyList<string>();
            myList.AddRange(names);

            // Act 
            var expected = expectedCount;
            // as example: Five Elements in the Linked List should return 5 as Count
            var actual = myList.Count;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Head_Should_Be_The_Same_As_Tail_With_Only_One_Element()
        {
            // Arrange
            MyList<string> myList = new MyList<string>();
            myList.Add("Nico");

            // Act 
            var expected = myList.Head;
            // Actual should return the Head-Node because if only one element is in the Linked List
            // then Head and Tail should have Pointers to each other
            // or e.g: var expected = myList.Tail   AND    var actual = myList.Head.Next
            var actual = myList.Tail;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Head_Shouldnt_Have_An_Prev_Pointer()
        {
            // Arrange
            MyList<string> myList = new MyList<string>()
            {
                "Nico",
                "Nino",
                "Alex",
                "Adis",
                "Sebi"
            };

            // Act 
            Node<string> expected = null;
            var actual = myList.Head.Prev;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Trail_Shouldnt_Have_An_Next_Pointer()
        {
            // Arrange
            MyList<string> myList = new MyList<string>()
            {
                "Nico",
                "Nino",
                "Alex",
                "Adis",
                "Sebi",
                "Premton",
                "Martin"
            };

            // Act 
            Node<string> expected = null;
            var actual = myList.Tail.Next;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Remove_With_Only_One_Element()
        {
            // Arrange
            MyList<string> myList = new MyList<string>()
            {
                "Nico"
            };

            // Act 
            Node<string> expected = myList.GetNodeAt(0);
            var actual = myList.RemoveAt(0).removedNode;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Remove_Head_Second_Element_Should_Be_Head()
        {
            // Arrange
            MyList<string> myList = new MyList<string>()
            {
                "Nico",
                "Nino",
                "Adis",
                "Alex"
            };

            // Act 
            Node<string> expected = myList.GetNodeAt(1);
            myList.RemoveAt(0);
            var actual = myList.GetNodeAt(0);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Remove_Tail_Second__Last_Element_Should_Be_Tail()
        {
            // Arrange
            MyList<string> myList = new MyList<string>()
            {
                "Nico",
                "Nino",
                "Adis",
                "Alex"
            };

            // Act 
            Node<string> expected = myList.GetNodeAt(2);
            myList.RemoveAt(3);
            var actual = myList.GetNodeAt(2);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Remove_First_Should_Be_Nico()
        {
            // Arrange
            MyList<string> myList = new MyList<string>()
            {
                "Nico"
            };

            // Act 
            Node<string> expected = myList.GetNodeAt(0);
            var actual = myList.RemoveFirst().removedNode;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Contains_Nico_Should_Be_True()
        {
            // Arrange
            MyList<string> myList = new MyList<string>()
            {
                "Nico"
            };

            // Act 
            bool expected = true;
            var actual = myList.Contains("Nico");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Contains_Nico_Should_Be_True_With_Node()
        {
            // Arrange
            MyList<string> myList = new MyList<string>()
            {
                "Nico"
            };

            // Act 
            bool expected = true;
            var actual = myList.Contains(myList[0]);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Contains_Julian_Should_Be_False()
        {
            // Arrange
            MyList<string> myList = new MyList<string>()
            {
                "Nico"
            };

            // Act 
            bool expected = true;
            var actual = myList.Contains("Julian");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Contains_New_Node_Should_Be_False()
        {
            // Arrange
            MyList<string> myList = new MyList<string>()
            {
                "Nico"
            };

            // Act 
            bool expected = true;
            var actual = myList.Contains(new Node<string>("Nico"));

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
