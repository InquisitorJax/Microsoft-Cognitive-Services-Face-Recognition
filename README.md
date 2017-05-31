# Microsoft-Cognitive-Services-Face-Recognition

Mutant Registration Program for upload to sentinals using facial recognition to identify mutants.

Xamarin Forms & Microsoft Cognitive Services

http://inquisitorjax.blogspot.co.za/2017/01/sentinel-mutant-registration-facial.html

You will need to provide your own implementation of ApiKeyProvider that looks something like this:

```C#
public class ApiKeyProvider : IApiKeyProvider
    {
        public string GetApiKey(ApiKeyType keyType)
        {
            string key = null;

            switch (keyType)
            {
                case ApiKeyType.FaceApi:
                    key = <YOUR_STRING_KEY_HERE>;
                    break;

                default:
                    throw new NotSupportedException("Cognitive service not supported!");
            }

            return key;
        }
    }
```

On details on how to get a key - head on over to Pierce's blog post here

https://blog.xamarin.com/adding-facial-recognition-to-your-mobile-apps/

