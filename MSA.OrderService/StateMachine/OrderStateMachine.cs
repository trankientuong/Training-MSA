using MassTransit;
using MSA.Common.Contracts.Settings;
using MSA.Common.Contracts.Domain.Events.Order;
using MSA.Common.Contracts.Domain.Events.Product;

namespace MSA.OrderService.StateMachine;

public class OrderStateMachine 
    : MassTransitStateMachine<OrderState>
{
    private readonly IConfiguration configuration;

    public OrderStateMachine(
        IConfiguration configuration
    )
    {
        this.configuration = configuration;

        var serviceEndPoints = configuration
            .GetSection(nameof(ServiceEndpointSettings))
            .Get<ServiceEndpointSettings>();

        InstanceState(
            x => x.CurrentState
        );

        Event(() => OrderSubmitted,
            x => x.CorrelateById(context => context.Message.OrderId)
        );

        Event(() => ProductValidatedSucceeded,
            x => x.CorrelateById(context => context.Message.OrderId)
        );

        Event(() => ProductValidatedFailed,
            x => x.CorrelateById(context => context.Message.OrderId)
        );

        Initially(
            When(OrderSubmitted)
                .Then(x => Console.WriteLine($"Receiving Order {x.Message.OrderId}"))
                .Then(x => {
                    x.Saga.OrderId = x.Message.OrderId;
                })
                .TransitionTo(Submitted)
        );

        During(Submitted,
            When(ProductValidatedSucceeded)
                .Then(x => Console.WriteLine($"Validate Product Succeeded for OrderId {x.Message.OrderId}"))
                .Then(x => {
                    x.Saga.ProductValidationId = x.Message.ProductId;
                })
                .TransitionTo(ValidatedSucceeded)
                .Finalize(),
            When(ProductValidatedFailed)
                .Then(x => Console.WriteLine($"Validate Product Failed for OrderId {x.Message.OrderId}"))
                .Then(x => {
                    x.Saga.ProductValidationId = x.Message.ProductId;
                    x.Saga.Reason = x.Message.Reason;
                })
                .TransitionTo(ValidatedSucceeded)
                .Finalize()
        );
 
    }

    public Event<OrderSubmitted> OrderSubmitted { get; private set; }
    public Event<ProductValidatedSucceeded> ProductValidatedSucceeded { get; private set; }
    public Event<ProductValidatedFailed> ProductValidatedFailed { get; private set; }

    public State Submitted { get; private set; }
    public State ValidatedSucceeded { get; private set; }
    public State ValidatedFailed { get; private set; }
}