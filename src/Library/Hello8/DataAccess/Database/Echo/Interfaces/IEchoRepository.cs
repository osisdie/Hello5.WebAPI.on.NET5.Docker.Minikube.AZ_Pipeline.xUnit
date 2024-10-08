﻿using System.Threading.Tasks;
using CoreFX.Abstractions.Contracts.Interfaces;

namespace Hello8.Domain.DataAccess.Database.Echo.Interfaces
{
    public interface IEchoRepository
    {
        Task<ISvcResponse> SetVerision(string version);
        Task<ISvcResponse<string>> GetVerision();
    }
}
