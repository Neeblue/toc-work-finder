namespace Backend.Interfaces;

public interface IAcceptJobService
{
    Task<bool> AcceptJob(string jobId);
}