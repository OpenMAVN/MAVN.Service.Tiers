﻿using System;
using System.Linq;
using System.Threading.Tasks;
using MAVN.Persistence.PostgreSQL.Legacy;
using MAVN.Service.Tiers.Domain.Repositories;
using MAVN.Service.Tiers.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.Tiers.MsSqlRepositories.Repositories
{
    public class CustomerTiersRepository : ICustomerTiersRepository
    {
        private readonly PostgreSQLContextFactory<DataContext> _contextFactory;

        public CustomerTiersRepository(PostgreSQLContextFactory<DataContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Guid?> GetTierByCustomerIdAsync(Guid customerId)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = await context.CustomerTiers.FirstOrDefaultAsync(o => o.CustomerId == customerId);

                return entity?.TierId;
            }
        }

        public async Task<int> GetCustomerCountByTierId(Guid tierId)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var count = await context.CustomerTiers.Where(x => x.TierId == tierId).CountAsync();

                return count;
            }
        }

        public async Task InsertAsync(Guid customerId, Guid tierId)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                context.CustomerTiers.Add(new CustomerTierEntity {CustomerId = customerId, TierId = tierId});

                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Guid customerId, Guid tierId)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = await context.CustomerTiers.SingleOrDefaultAsync(o => o.CustomerId == customerId);

                if (entity == null)
                    return;

                entity.TierId = tierId;

                await context.SaveChangesAsync();
            }
        }
    }
}
