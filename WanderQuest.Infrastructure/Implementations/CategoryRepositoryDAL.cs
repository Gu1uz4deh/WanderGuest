using System;
using WanderQuest.Core.EFRepository.EFEntityRepositoryBase;
using WanderQuest.Infrastructure.Abstracts;
using WanderQuest.Infrastructure.DAL;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Infrastructure.Implementations;

public class CategoryRepositoryDAL : EFEntityRepositoryBase<Category, AppDbContext>, ICategoryDAL
{
    public CategoryRepositoryDAL(AppDbContext context) : base(context) { }
    
}