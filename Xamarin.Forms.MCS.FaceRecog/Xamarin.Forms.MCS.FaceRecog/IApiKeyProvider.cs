namespace Xamarin.Forms.MCS.FaceRecog
{
    public enum ApiKeyType
    {
        FaceApi
    }

    public interface IApiKeyProvider
    {
        string GetApiKey(ApiKeyType keyType);
    }
}