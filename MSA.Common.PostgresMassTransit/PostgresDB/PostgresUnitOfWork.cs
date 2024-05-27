namespace MSA.Common.PostgresMassTransit.PostgresDB;

public class PostgresUnitOfWork<TDbContext> 
    where TDbContext : AppDbContextBase
{
    private readonly TDbContext dbcontext;

    public PostgresUnitOfWork(TDbContext dbcontext)
    {
        this.dbcontext = dbcontext;
    }

    public async Task SaveChangeAsync() => await dbcontext.SaveChangesAsync();
}