using NUnit.Framework;

namespace TapMatch.GridSystem.Tests.PlayMode
{
    public class DepthFirstGridMatchFinderTests
    {
        [Test]
        public void ConstructingObject_DoesNotThrowException()
        {
            Assert.DoesNotThrow(() => new DepthFirstGridMatchFinder());
        }

        public static readonly TestCaseData[] SearchTestCases =
        {
            new(new SearchTestCase(
                    grid: new TestGridItem[,]
                    {
                        {0, 0, 0},
                        {0, 1, 0},
                        {0, 0, 0},
                    },
                    startRow: 1,
                    startColumn: 1,
                    expectedMatches: new[]
                    {
                        new GridIndex(1, 1),
                    }))
                {TestName = "Single match"},

            new(new SearchTestCase(
                    grid: new TestGridItem[,]
                    {
                        {1, 0, 1},
                        {0, 1, 0},
                        {1, 0, 1},
                    },
                    startRow: 1,
                    startColumn: 1,
                    expectedMatches: new[]
                    {
                        new GridIndex(1, 1),
                    }))
                {TestName = "Single match with ignored corners"},

            new(new SearchTestCase(
                    grid: new TestGridItem[,]
                    {
                        {0, 0, 0},
                        {1, 1, 1},
                        {0, 0, 0},
                    },
                    startRow: 1,
                    startColumn: 1,
                    expectedMatches: new[]
                    {
                        new GridIndex(1, 0),
                        new GridIndex(1, 1),
                        new GridIndex(1, 2),
                    }))
                {TestName = "Row match"},

            new(new SearchTestCase(
                    grid: new TestGridItem[,]
                    {
                        {1, 0, 0},
                        {1, 0, 0},
                        {1, 1, 1},
                    },
                    startRow: 2,
                    startColumn: 0,
                    expectedMatches: new[]
                    {
                        new GridIndex(0, 0),
                        new GridIndex(1, 0),
                        new GridIndex(2, 0),
                        new GridIndex(2, 1),
                        new GridIndex(2, 2),
                    }))
                {TestName = "L shape match"},

            new(new SearchTestCase(
                    grid: new TestGridItem[,]
                    {
                        {0, 1, 1, 1},
                        {0, 1, 0, 0},
                        {0, 1, 1, 1},
                        {0, 0, 0, 1},
                        {0, 1, 1, 1},
                    },
                    startRow: 3,
                    startColumn: 3,
                    expectedMatches: new[]
                    {
                        new GridIndex(0, 1),
                        new GridIndex(0, 2),
                        new GridIndex(0, 3),
                        new GridIndex(1, 1),
                        new GridIndex(2, 1),
                        new GridIndex(2, 2),
                        new GridIndex(2, 3),
                        new GridIndex(3, 3),
                        new GridIndex(4, 1),
                        new GridIndex(4, 2),
                        new GridIndex(4, 3),
                    }))
                {TestName = "S shape match"},

            new(new SearchTestCase(
                    grid: new TestGridItem[,]
                    {
                        {1, 1, 1},
                        {1, 1, 1},
                        {1, 1, 1},
                    },
                    startRow: 0,
                    startColumn: 0,
                    expectedMatches: new[]
                    {
                        new GridIndex(0, 0),
                        new GridIndex(0, 1),
                        new GridIndex(0, 2),
                        new GridIndex(1, 0),
                        new GridIndex(1, 1),
                        new GridIndex(1, 2),
                        new GridIndex(2, 0),
                        new GridIndex(2, 1),
                        new GridIndex(2, 2),
                    }))
                {TestName = "Match with all items"},
        };

        [TestCaseSource(nameof(SearchTestCases))]
        public void FinderReturnsExpectedResults(SearchTestCase testCase)
        {
            var result = new DepthFirstGridMatchFinder().FindMatches(
                testCase.Grid,
                testCase.StartRow,
                testCase.StartColumn);

            CollectionAssert.AreEquivalent(testCase.ExpectedMatches, result);
        }

        public class TestGridItem : IMatchableItem
        {
            public int TypeId { get; }

            public TestGridItem(int typeId)
            {
                this.TypeId = typeId;
            }

            public static implicit operator int(TestGridItem item) => item.TypeId;
            public static implicit operator TestGridItem(int typeId) => new TestGridItem(typeId);
        }

        public class SearchTestCase
        {
            public readonly TestGridItem[,] Grid;
            public readonly int StartRow;
            public readonly int StartColumn;
            public readonly GridIndex[] ExpectedMatches;

            public SearchTestCase(TestGridItem[,] grid, int startRow, int startColumn, GridIndex[] expectedMatches)
            {
                this.Grid = grid;
                this.StartRow = startRow;
                this.StartColumn = startColumn;
                this.ExpectedMatches = expectedMatches;
            }
        }
    }
}