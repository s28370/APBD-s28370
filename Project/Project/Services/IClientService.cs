using Project.DTOs;

namespace Project.Services;

public interface IClientService
{
    Task<bool> AddClient(IndividualClientPostDto clientPostDto);
    Task<bool> AddClient(CompanyClientPostDto clientPostDto);
    Task<int> RemoveClient(int id);
    Task<bool> UpdateClient(int id, IndividualClientPutDto clientPutDto);
    Task<bool> UpdateClient(int id, CompanyClientPutDto clientPutDto);
}