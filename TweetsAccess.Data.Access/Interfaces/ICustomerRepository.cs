﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace TweetsAccess.Data.Access.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<CustomerEntity>> GetAll();
        Task<CustomerEntity> GetById(int id);
        Task<CustomerEntity> Add(CustomerEntity customerEntity);
        Task Update(CustomerEntity customerEntity);
        Task Delete(int id);
    }
}
