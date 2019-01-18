using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudPlus.QueueModels.Companies.Commands;
using CloudPlus.QueueModels.Users.Commands;
using MassTransit;

namespace CloudPlus.AppServices.Company.Consumers
{
    public interface ICreateCompanyConsumer : IConsumer<ICreateCompanyCommand>
    {
    }
}
