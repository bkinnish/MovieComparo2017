namespace MovieComparo.Config
{
    public interface IConfig
    {
        string ApiAddress { get; }
        string AccessToken { get; }
        int ApiMaxRetries { get; }
        int ApiRetryInterval { get; }
    }
}