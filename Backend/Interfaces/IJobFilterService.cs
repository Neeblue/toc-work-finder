using Backend.Models;

namespace Backend.Interfaces;

public interface IJobFilterService
{
    List<Job> FilterJobs(List<Job> jobs);
}