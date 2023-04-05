namespace PumpService
{
    public class StatisticsService : IStatisticsService
    {
        public int SucceesTacts { get; set; }
        public int ErrorTacts { get; set; }
        public int AllTacts
        {
            get
            {
                return SucceesTacts + ErrorTacts;
            }
            set
            {
            }
        }
    }
}