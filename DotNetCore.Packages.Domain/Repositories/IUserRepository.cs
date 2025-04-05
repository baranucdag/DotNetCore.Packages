using DotNetCore.Packages.Domain.Entities;
using DotNetCore.Packages.Domain.Repositories.EFCore;

namespace DotNetCore.Packages.Domain.Repositories;

public interface IUserRepository: IEntityRepository<User>
{
    
}