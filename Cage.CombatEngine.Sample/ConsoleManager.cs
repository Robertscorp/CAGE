using Cage.CombatEngine.Common;
using Cage.CombatEngine.Sample.Resources;
using System.Collections.Concurrent;

namespace Cage.CombatEngine.Sample
{

    public class ConsoleManager
    {

        #region - - - - - - Fields - - - - - -

        private readonly ConcurrentDictionary<ResourcePoolViewModel, int> m_ResourcePoolDisplayOrder = new();

        private int m_NextIndex = -1;
        private int m_ResultsDisplayRow;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        private ConsoleManager() { }

        #endregion Constructors

        #region - - - - - - Properties - - - - - -

        public static ConsoleManager Instance { get; } = new();

        #endregion Properties

        #region - - - - - - Methods - - - - - -

        public void AddResourcePools(params ResourcePoolViewModel[] resourcePools)
        {
            foreach (var _ResourcePool in resourcePools)
                if (!this.m_ResourcePoolDisplayOrder.ContainsKey(_ResourcePool))
                    _ = this.m_ResourcePoolDisplayOrder.TryAdd(_ResourcePool, Interlocked.Increment(ref this.m_NextIndex));
        }

        public bool TryGetUserInput(string operation, out int userChoice, params string[] options)
        {
            this.Reset();

            Console.WriteLine(operation);

            foreach (var (_Option, _Index) in options.Select((o, i) => (o, i)))
                Console.WriteLine($"{_Index + 1}. {_Option}");

            Console.WriteLine("other. Exit");

            return int.TryParse(Console.ReadLine(), out userChoice) && userChoice > 0 && userChoice <= options.Length;
        }

        public void DisplayResourcePool(ResourcePoolViewModel resourcePool)
        {
            var _Name = resourcePool.Resource.Name.PadLeft(20, ' ');

            var _ResourceBarBlocks
                = Math.Max(
                    0,
                    resourcePool.Capacity == 0.0M
                        ? 0.0M
                        : DecimalRoundingStrategies.AlwaysRoundDown(resourcePool.RemainingResource / resourcePool.Capacity * 20));

            var _ResourceBar = new string('█', (int)_ResourceBarBlocks).PadRight(20, '_');
            var _RemainingResource = resourcePool.RemainingResource.ToString().PadLeft(6, ' ');
            var _Capacity = resourcePool.Capacity.ToString().PadLeft(6, ' ');

            Console.SetCursorPosition(Console.CursorLeft, this.m_ResourcePoolDisplayOrder[resourcePool]);
            Console.WriteLine($"{_Name} {_ResourceBar} {_RemainingResource} / {_Capacity}");
        }

        public void DisplayResourcePools()
        {
            foreach (var _ResourcePool in this.m_ResourcePoolDisplayOrder.Keys)
                this.DisplayResourcePool(_ResourcePool);
        }

        public void DisplayResult(string result)
        {
            var (_Left, _Top) = Console.GetCursorPosition();

            Console.SetCursorPosition(65, this.m_ResultsDisplayRow++);
            Console.Write(result);
            Console.SetCursorPosition(_Left, _Top);
        }

        public void Reset()
        {
            Console.SetCursorPosition(Console.CursorLeft, this.m_ResourcePoolDisplayOrder.Count + 1);

            foreach (var _ in Enumerable.Range(0, 10))
                Console.WriteLine(new string(' ', 64));

            this.DisplayResourcePools();
            Console.SetCursorPosition(Console.CursorLeft, this.m_ResourcePoolDisplayOrder.Count + 1);
        }

        public void ResetResults()
        {
            Console.SetCursorPosition(65, 0);

            foreach (var _Index in Enumerable.Range(0, this.m_ResultsDisplayRow))
            {
                Console.SetCursorPosition(65, _Index);
                Console.WriteLine(new string(' ', 45));
            }

            this.m_ResultsDisplayRow = 0;
            Console.SetCursorPosition(Console.CursorLeft, this.m_ResourcePoolDisplayOrder.Count + 1);
        }

        #endregion Methods

    }

}
