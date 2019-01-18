using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using CloudPlus.QueueModels.EmailNotification.Commands;

namespace CloudPlus.AppServices.Notification.Consumers
{
    public interface ISendEmailUserConsumer : IConsumer<ISendEmailCommand>
    {
    }
}
