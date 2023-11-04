using Backend.Models;

namespace Backend.Interfaces;

public interface IJobService
{
    Task<List<Job>> GetJobs();
    void Dispose();
}