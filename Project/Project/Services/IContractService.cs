using Project.DTOs;

namespace Project.Services;

public interface IContractService
{
    Task<string?> CreateContract(ContractDto contractDto);
}