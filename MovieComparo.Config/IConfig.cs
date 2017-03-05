namespace MovieComparo.Config
{
    public interface IConfig
    {
        string ApiAddress { get; }
        string AccessToken { get; }
        int ApiMaxRetries { get; }
        int ApiTimeout { get; }
        string Provider1Name { get; }
        string Provider2Name { get; }
    }
}