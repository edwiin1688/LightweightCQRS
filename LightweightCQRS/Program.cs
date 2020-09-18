using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LightweightCQRS
{
    class Program
    {
        async static Task Main()
        {
            var services = new ServiceCollection();

            services.AddMediatR(typeof(Program).Assembly);

            var serviceProvider = services.BuildServiceProvider();

            var mediator = serviceProvider.GetService<IMediator>();


#pragma warning disable S125 // Sections of code should not be commented out
            //await mediator.Publish(new MyEvent { EventName = "event01" }).ConfigureAwait(false);

            await mediator.Send(new MyCommand { CommandName = "cmd01" }).ConfigureAwait(false);
#pragma warning restore S125 // Sections of code should not be commented out

            Console.ReadKey();
        }

#pragma warning disable CA1812 // CA1812	明顯從未具現化的內部類別。若是如此，請從組件中移除該程式碼。

        #region 
        internal class MyCommand : IRequest<long>
        {
            public string CommandName { get; set; }
        }



        internal class MyCommandHandler : IRequestHandler<MyCommand, long>
        {
            public Task<long> Handle(MyCommand request, CancellationToken cancellationToken)
            {
                Console.WriteLine($"MyCommandHandler執行命令：{request.CommandName}");
                return Task.FromResult(10L);
            }
        }
        #endregion
        #region
        internal class MyEvent : INotification
        {
            public string EventName { get; set; }
        }


        //
        internal class MyEventHandler : INotificationHandler<MyEvent>
        {
            public Task Handle(MyEvent notification, CancellationToken cancellationToken)
            {
                Console.WriteLine($"MyEventHandler執行：{notification.EventName}");
                return Task.CompletedTask;
            }
        }

        internal class MyEventHandlerV2 : INotificationHandler<MyEvent>
        {
            public Task Handle(MyEvent notification, CancellationToken cancellationToken)
            {
                Console.WriteLine($"MyEventHandlerV2執行：{notification.EventName}");
                return Task.CompletedTask;
            }
        }

        #endregion

#pragma warning restore CA1812 // CA1812	明顯從未具現化的內部類別。若是如此，請從組件中移除該程式碼。
    }
}
