using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface IGoogleService
    {
        Task<double[]> CoordinatesFromLocation(string location);
        Task<string[]> DistanceDurationBetweenLocations(string location1, string location2, Transportation transportation);
        Task<string[]> DistanceDurationBetweenLocations(double[] location1, double[] location2, Transportation transportation);
    }
}
