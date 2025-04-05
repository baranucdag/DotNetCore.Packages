using DotNetCore.Packages.Domain.Entities;
using DotNetCore.Packages.Domain.Repositories;
using DotNetCore.Packages.Domain.UnitOfWork;
using DotNetCore.Packages.Persistence.DbContexts;
using DotNetCore.Packages.Persistence.Repositories.EFCore;
using DotNetCore.Packages.Persistence.UnitOfWork;

namespace DotNetCore.Packages.Persistence.Repositories;

public class UserRepository(EFBaseDbContext context, IUnitOfWork unitOfWork)
    : EFEntityRepository<User, EFBaseDbContext>(context, unitOfWork), IUserRepository
{
}