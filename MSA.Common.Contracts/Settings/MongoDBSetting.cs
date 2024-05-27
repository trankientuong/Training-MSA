namespace MSA.Common.Contracts.Settings;

public class MongoDBSetting
{
    public string Host { get; init; } = string.Empty;

    public string Port { get; init; } = string.Empty;

    public string ConnectionString => $"mongodb://{Host}:{Port}";
}