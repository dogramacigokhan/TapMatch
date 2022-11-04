using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using TapMatch.GridSystem.Interactions;

namespace TapMatch.GridSystem.Tests.PlayMode
{
    public class GridViewModelTests
    {
        private IGridMatchFinder gridMatchFinder;
        private IInteractionProvider mainInteractionProvider;

        [SetUp]
        public void SetUp()
        {
            this.gridMatchFinder = Substitute.For<IGridMatchFinder>();
            this.mainInteractionProvider = Substitute.For<IInteractionProvider>();

            // Default mock to return selected index to destroy
            this.gridMatchFinder
                .FindMatches(default, default, default)
                .ReturnsForAnyArgs(call => new GridIndex[] {new(call.ArgAt<int>(1), call.ArgAt<int>(2))});
        }

        [TestCase(1, 1, 1)]
        [TestCase(3, 5, 3)]
        public void GridViewModel_CanBeCreatedWithoutException(int rowCount, int colCount, int colorCount)
        {
            Assert.DoesNotThrow(() => this.MakeViewModel(rowCount, colCount));
        }

        [Test]
        public void GridViewModel_ListensToInteractionsFromMultipleSources()
        {
            var extraInteractionProvider = Substitute.For<IInteractionProvider>();
            var viewModel = this.MakeViewModel(
                rowCount: 5,
                colCount: 5,
                interactionProviders: new []
                {
                    this.mainInteractionProvider,
                    extraInteractionProvider,
                });

            var handler = Substitute.For<GridViewModel.AddedOrDestroyedGridItemsEventHandler>();
            viewModel.DestroyedGridItems += handler;

            // Trigger the interactions and expect it to be handled by grid view-model
            this.mainInteractionProvider.GridItemSelected += Raise.Event<Action<(int row, int column)>>((1, 2));
            this.mainInteractionProvider.GridItemSelected += Raise.Event<Action<(int row, int column)>>((2, 3));

            Received.InOrder(() =>
            {
                handler.ReceivedWithAnyArgs().Invoke(Arg.Is<GridIndex[]>(arg => arg.Length == 1 && arg[0].Row == 1 && arg[0].Column == 2));
                handler.ReceivedWithAnyArgs().Invoke(Arg.Is<GridIndex[]>(arg => arg.Length == 1 && arg[0].Row == 2 && arg[0].Column == 3));
            });
        }

        public static readonly GridTestCase[] GridTestCases =
        {
            // 0 0 -> x 0
            // 0 0 -> 0 0
            new GridTestCase(
                rowCount: 2,
                columnCount: 2,
                expectedIndicesToDestroy: new[] {new GridIndex(0, 0)},
                expectedIndicesToShift: new Dictionary<GridIndex, int>(),
                expectedIndicesToAdd: new[] {new GridIndex(0, 0)}),

            // 0 0 -> 0 0
            // 0 0 -> x 0
            new GridTestCase(
                rowCount: 2,
                columnCount: 2,
                expectedIndicesToDestroy: new[] {new GridIndex(1, 0)},
                expectedIndicesToShift: new Dictionary<GridIndex, int>
                {
                    [new GridIndex(0, 0)] = 1,
                },
                expectedIndicesToAdd: new[] {new GridIndex(0, 0)}),
        };

        [TestCaseSource(nameof(GridTestCases))]
        public void GridViewModel_DestroysCorrectItemIndices(GridTestCase testCase)
        {
            this.gridMatchFinder.FindMatches(default, default, default).ReturnsForAnyArgs(testCase.ExpectedIndicesToDestroy);

            var viewModel = this.MakeViewModel(testCase.RowCount, testCase.ColumnCount);

            var handler = Substitute.For<GridViewModel.AddedOrDestroyedGridItemsEventHandler>();
            viewModel.DestroyedGridItems += handler;

            // Trigger the interaction
            this.mainInteractionProvider.GridItemSelected += Raise.Event<Action<(int row, int column)>>((0, 0));

            handler.Received().Invoke(testCase.ExpectedIndicesToDestroy);
        }

        private IReadOnlyList<IGridItemSetting> GenerateSettings(int colorCount) => Enumerable
            .Range(0, colorCount)
            .Select(index =>
            {
                var setting = Substitute.For<IGridItemSetting>();
                setting.TypeId.Returns(index);
                return setting;
            })
            .ToList();

        private GridViewModel MakeViewModel(
            int rowCount,
            int colCount,
            IReadOnlyList<IGridItemSetting> gridItemSettings = null,
            IEnumerable<IInteractionProvider> interactionProviders = null) => new GridViewModel(
            rowCount,
            colCount,
            gridItemSettings ?? GenerateSettings(colorCount: 3),
            interactionProviders ?? new[] {this.mainInteractionProvider},
            this.gridMatchFinder);

        public class GridTestCase
        {
            public readonly int RowCount;
            public readonly int ColumnCount;
            public readonly GridIndex[] ExpectedIndicesToDestroy;
            public readonly Dictionary<GridIndex, int> ExpectedIndicesToShift;
            public readonly GridIndex[] ExpectedIndicesToAdd;

            public GridTestCase(
                int rowCount,
                int columnCount,
                GridIndex[] expectedIndicesToDestroy,
                Dictionary<GridIndex, int> expectedIndicesToShift,
                GridIndex[] expectedIndicesToAdd)
            {
                this.RowCount = rowCount;
                this.ColumnCount = columnCount;
                this.ExpectedIndicesToDestroy = expectedIndicesToDestroy;
                this.ExpectedIndicesToShift = expectedIndicesToShift;
                this.ExpectedIndicesToAdd = expectedIndicesToAdd;
            }
        }
    }
}
