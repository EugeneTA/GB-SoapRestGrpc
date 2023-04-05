namespace PumpService
{
    public interface IStatisticsService
    {
        int SucceesTacts { get; set; }
        int ErrorTacts { get; set; }
        int AllTacts { get; set; }
    }
}
