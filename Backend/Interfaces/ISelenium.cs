using Backend.Models;

namespace Backend.Interfaces;

public interface ISelenium
{
    Task<List<Job>> GetJobs();
    Task<bool> AcceptJob(string jobId);
    void Dispose();
}