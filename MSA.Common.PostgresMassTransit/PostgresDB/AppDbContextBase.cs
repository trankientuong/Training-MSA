using System;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MSA.Common.Contracts.Settings;

namespace MSA.Common.PostgresMassTransit.PostgresDB;

public class AppDbContextBase : DbContext
{
    private readonly IConfiguration configuration;
    private readonly DbContextOptions options;

    public AppDbContextBase(
        IConfiguration configuration,
        DbContextOptions options) 
        : base(options)
    {
        this.configuration = configuration;
        this.options = options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var serviceSetting = configuration
            .GetSection(nameof(ServiceSetting))
            .Get<ServiceSetting>();

        modelBuilder.HasDefaultSchema(serviceSetting.ServiceName);
        
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity(i => {
            i.ToTable("InboxState");
        });
        modelBuilder.AddOutboxMessageEntity(o => {
            o.ToTable("OutboxMessage");
        });
        modelBuilder.AddOutboxStateEntity(o => {
            o.ToTable("OutboxState");
        });
    }
}